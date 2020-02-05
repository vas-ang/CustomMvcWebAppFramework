namespace CustomFramework.Mvc.Attributes
{
    using System;

    using CustomFramework.Http.Elements;

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPutAttribute : HttpMethodAttribute
    {
        private static readonly HttpMethod HttpMethodConst = HttpMethod.Put;

        public HttpPutAttribute(string path)
            : base(path, HttpMethodConst)
        { }

        public HttpPutAttribute()
            : base(HttpMethodConst)
        { }
    }
}
