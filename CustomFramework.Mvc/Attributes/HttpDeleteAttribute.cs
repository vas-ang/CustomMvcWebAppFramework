namespace CustomFramework.Mvc.Attributes
{
    using System;

    using CustomFramework.Http.Elements;

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        private static readonly HttpMethod HttpMethodConst = HttpMethod.Delete;

        public HttpDeleteAttribute(string path)
            : base(path, HttpMethodConst)
        { }

        public HttpDeleteAttribute()
            : base(HttpMethodConst)
        { }
    }
}
