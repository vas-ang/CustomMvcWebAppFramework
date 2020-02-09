namespace CustomFramework.Http
{
    using System;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Collections.Concurrent;

    using Common;
    using Elements;
    using Contracts;
    using Responses;

    public class HttpServer : IServerEntity
    {
        private readonly TcpListener tcpListener;
        private readonly IEnumerable<HttpRoute> routes;
        private readonly IDictionary<string, IDictionary<string, string>> sessions;

        public HttpServer(int port, IEnumerable<HttpRoute> routes)
        {
            this.routes = routes;
            this.tcpListener = new TcpListener(System.Net.IPAddress.Loopback, port);
            this.sessions = new ConcurrentDictionary<string, IDictionary<string, string>>();
        }

        public async Task StartAsync()
        {
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => ProcessClientAsync(tcpClient));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        public void Stop()
        {
            tcpListener.Stop();
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            using NetworkStream networkStream = tcpClient.GetStream();

            byte[] buffer = new byte[4096];
            List<byte> data = new List<byte>();

            do
            {
                int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

                data.AddRange(buffer.Take(bytesRead));
            }
            while (networkStream.DataAvailable);

            HttpResponse response;

            try
            {
                string stringRequest = ServerConfiguration.Encoding.GetString(data.ToArray());
                HttpRequest request = HttpRequest.Parse(stringRequest);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => Console.WriteLine($"{DateTime.UtcNow}: {request.Method} -> {request.Path}"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                HttpRoute route = routes.FirstOrDefault(r => r.Method == request.Method && string.Compare(r.Path, request.Path, true) == 0);


                if (route == null)
                {
                    response = new ErrorResponse(HttpResponseCode.NotFound);
                }
                else
                {
                    HttpCookie sessionCookie = request.Cookies.FirstOrDefault(c => c.Name == "SessionId");

                    if (sessionCookie != null)
                    {
                        if (this.sessions.ContainsKey(sessionCookie.Value))
                        {
                            request.SessionData = this.sessions[sessionCookie.Value];
                        }
                        else
                        {
                            request.SessionData = new ConcurrentDictionary<string, string>();
                            this.sessions[sessionCookie.Value] = request.SessionData;
                        }
                    }
                    else
                    {
                        request.SessionData = new ConcurrentDictionary<string, string>();
                    }

                    response = route.Action.Invoke(request);

                    if (sessionCookie == null)
                    {
                        string sessionId = Guid.NewGuid().ToString();
                        this.sessions[sessionId] = request.SessionData;
                        response.AppendCookie(new HttpCookie("SessionId", sessionId) { HttpOnly = true, MaxAge = 30 * 3600 });
                    }
                }

            }
            catch (Exception ex)
            {
                response = new ErrorResponse(HttpResponseCode.InternalServerError, ex.ToString());
            }

            byte[] actionResult = response.GetBytes();
            await networkStream.WriteAsync(actionResult, 0, actionResult.Length);
        }
    }
}