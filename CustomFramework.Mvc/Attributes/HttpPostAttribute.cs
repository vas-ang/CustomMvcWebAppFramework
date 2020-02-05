namespace CustomFramework.Mvc.Attributes
{
    using System;

    using CustomFramework.Http.Elements;

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : HttpMethodAttribute
    {
        private static readonly HttpMethod HttpMethodConst = HttpMethod.Post;

        public HttpPostAttribute(string path)
            : base(path, HttpMethodConst)
        { }

        public HttpPostAttribute()
            : base(HttpMethodConst)
        { }
    }
}
