namespace CustomFramework.Mvc.Attributes
{
    using System;

    using CustomFramework.Http.Elements;

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpGetAttribute : HttpMethodAttribute
    {
        private static readonly HttpMethod HttpMethodConst = HttpMethod.Get;

        public HttpGetAttribute(string path)
            : base(path, HttpMethodConst)
        { }

        public HttpGetAttribute()
            : base(HttpMethodConst)
        { }
    }
}
