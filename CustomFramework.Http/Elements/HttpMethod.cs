namespace CustomFramework.Http.Elements
{
    using System;

    using static Common.ExceptionStrings;

    public class HttpMethod
    {
        private const string GetString = "GET";
        private const string PostString = "POST";
        private const string PutString = "PUT";
        private const string DeleteString = "DELETE";

        private static HttpMethod get;
        private static HttpMethod post;
        private static HttpMethod put;
        private static HttpMethod delete;

        private readonly string method;

        protected HttpMethod(string method)
        {
            this.method = method;
        }

        public static HttpMethod Parse(string methodString)
        {
            var method = methodString switch
            {
                GetString => Get,
                PostString => Post,
                PutString => Put,
                DeleteString => Delete,
                _ => throw new ArgumentException(string.Format(ElementInvalidOrNotSupported, "method", methodString)),
            };

            return method;
        }

        public override string ToString()
        {
            return this.method;
        }

        #region Static Properties
        public static HttpMethod Get
        {
            get
            {
                if (get == null)
                {
                    get = new HttpMethod(GetString);
                }

                return get;
            }
        }

        public static HttpMethod Post
        {
            get
            {
                if (post == null)
                {
                    post = new HttpMethod(PostString);
                }

                return post;
            }
        }

        public static HttpMethod Put
        {
            get
            {
                if (put == null)
                {
                    put = new HttpMethod(PutString);
                }

                return put;
            }
        }

        public static HttpMethod Delete
        {
            get
            {
                if (delete == null)
                {
                    delete = new HttpMethod(DeleteString);
                }

                return delete;
            }
        }
        #endregion
    }
}
