namespace CustomFramework.Http
{
    using System;

    using Elements;

    public class HttpRoute
    {
        public HttpRoute(HttpMethod method, string path, Func<HttpRequest, HttpResponse> action)
        {
            Method = method;
            Path = path;
            Action = action;
        }

        public HttpMethod Method { get; set; }

        public string Path { get; set; }

        public Func<HttpRequest, HttpResponse> Action { get; set; }
    }
}
