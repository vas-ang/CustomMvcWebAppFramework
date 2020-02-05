namespace CustomFramework.Mvc.Attributes
{
    using System;

    using CustomFramework.Http.Elements;

    public abstract class HttpMethodAttribute : Attribute
    {
        public HttpMethodAttribute(HttpMethod httpMethod)
        {
            this.HttpMethod = httpMethod;
        }

        public HttpMethodAttribute(string path, HttpMethod httpMethod)
            : this(httpMethod)
        {
            this.Path = path;
        }

        public string Path { get; }

        public HttpMethod HttpMethod { get; }
    }
}
