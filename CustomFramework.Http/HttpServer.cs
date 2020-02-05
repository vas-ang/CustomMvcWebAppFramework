namespace CustomFramework.Http
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Elements;
    using Contracts;

    public class HttpServer : IServerEntity
    {
        private readonly HttpVersion httpVersion = HttpVersion.Http10;

        private readonly TcpListener tcpListener;
        private readonly IEnumerable<HttpRoute> routes;

        public HttpServer(int port, IEnumerable<HttpRoute> routes)
        {
            this.tcpListener = new TcpListener(System.Net.IPAddress.Loopback, port);
            this.routes = routes;
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
                await networkStream.ReadAsync(buffer, 0, buffer.Length);

                data.AddRange(buffer);
            }
            while (networkStream.DataAvailable);

            HttpResponse response;

            try
            {
                string stringRequest = Encoding.UTF8.GetString(data.ToArray());
                HttpRequest request = HttpRequest.Parse(stringRequest);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => Console.WriteLine($"{DateTime.UtcNow}: {request.Method.ToString().ToUpper()} -> {request.Path}"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                HttpRoute route = routes.First(r => r.Method == request.Method && string.Compare(r.Path, request.Path, true) == 0);

                response = route.Action(request);
            }
            catch (Exception ex)
            {
                response = new HttpResponse(httpVersion, HttpResponseCode.NotFound)
                {
                    Body = Encoding.UTF8.GetBytes(ex.ToString())
                };

                response.AddHeader(new HttpHeader("Content-Type", "text/txt"));
                response.AddHeader(new HttpHeader("Content-Length", response.Body.Length.ToString()));
            }

            byte[] actionResult = response.GetBytes(Encoding.UTF8);
            await networkStream.WriteAsync(actionResult, 0, actionResult.Length);
        }
    }
}