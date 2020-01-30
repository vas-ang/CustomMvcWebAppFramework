using System;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

using CustomFramework.Http.Contracts;

namespace CustomFramework.Http
{
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

                Task.Run(() => ProcessClientAsync(tcpClient));
            }
        }

        public void Stop()
        {
            tcpListener.Stop();
        }

        private async Task ProcessClientAsync(TcpClient tcpClient) // TODO: Create cookie management.
        {
            using NetworkStream networkStream = tcpClient.GetStream();
            byte[] receivedBytes = new byte[1_000_000]; // TODO: Use buffer when receiving info!
            await networkStream.ReadAsync(receivedBytes, 0, receivedBytes.Length);

            HttpResponse response;

            try
            {
                string stringRequest = Encoding.UTF8.GetString(receivedBytes);
                HttpRequest request = HttpRequest.Parse(stringRequest);
                Task.Run(() => Console.WriteLine($"{DateTime.UtcNow}: {request.Method.ToString().ToUpper()} -> {request.Path}"));

                HttpRoute route = routes.First(r => r.Path == request.Path);

                response = route.Action(request);
            }
            catch (Exception ex)
            {
                response = new HttpResponse(httpVersion, new HttpResponseCode(200, "OK"));
                response.Body = Encoding.UTF8.GetBytes(ex.ToString());
                response.AddHeader(new HttpHeader("Content-Type", "text/txt"));
                response.AddHeader(new HttpHeader("Content-Length", response.Body.Length.ToString()));
            }

            byte[] actionResult = response.GetBytes(Encoding.UTF8);
            await networkStream.WriteAsync(actionResult, 0, actionResult.Length); // TODO: Use buffer when responding!
        }
    }
}