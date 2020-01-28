using System;

using CustomFramework.Http.Enumerators;

namespace CustomFramework.Http
{
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
