using System;

namespace CustomFramework.Http
{
    public class HttpResponseCode
    {
        private short responseCode;

        public HttpResponseCode(short responseCode, string message = null)
        {
            this.ResponseCode = responseCode;
            this.Message = message;
        }

        public short ResponseCode
        {
            get => this.responseCode;
            set
            {
                if (100 > value || value > 999)
                {
                    throw new ArgumentException($"The response code {value} is not valid. It should be between 100 and 999");
                }

                this.responseCode = value;
            }
        }

        public string Message { get; set; }

        public override string ToString()
        {
            return $"{this.ResponseCode} {this.Message}".TrimEnd();
        }

        #region Static Properties
        public static HttpResponseCode Continue { get; } = new HttpResponseCode(100, "Continue");
        public static HttpResponseCode SwitchingProtocols { get; } = new HttpResponseCode(101, "Switching Protocols");
        public static HttpResponseCode Ok { get; } = new HttpResponseCode(200, "OK");
        public static HttpResponseCode Created { get; } = new HttpResponseCode(201, "Created");
        public static HttpResponseCode Accepted { get; } = new HttpResponseCode(202, "Accepted");
        public static HttpResponseCode MovedPermanently { get; } = new HttpResponseCode(301, "Moved Permanently");
        public static HttpResponseCode Found { get; } = new HttpResponseCode(302, "Found");
        public static HttpResponseCode NotModified { get; } = new HttpResponseCode(304, "Not Modified");
        public static HttpResponseCode TemporaryRedirect { get; } = new HttpResponseCode(307, "Temporary Redirect");
        public static HttpResponseCode PermanentRedirect { get; } = new HttpResponseCode(308, "Permanent Redirect");
        public static HttpResponseCode BadRequest { get; } = new HttpResponseCode(400, "Bad Request");
        public static HttpResponseCode Unauthorised { get; } = new HttpResponseCode(401, "Unauthorised");
        public static HttpResponseCode PaymentRequired { get; } = new HttpResponseCode(402, "Payment Required");
        public static HttpResponseCode Forbidden { get; } = new HttpResponseCode(403, "Forbidden");
        public static HttpResponseCode NotFound { get; } = new HttpResponseCode(404, "Not Found");
        public static HttpResponseCode RequestTimeout { get; } = new HttpResponseCode(408, "Request Timeout");
        public static HttpResponseCode InternalServerError { get; } = new HttpResponseCode(500, "Internal Server Error");
        public static HttpResponseCode NotImplemented { get; } = new HttpResponseCode(501, "Not Implemented");
        public static HttpResponseCode BadGateway { get; } = new HttpResponseCode(502, "Bad Gateway");
        public static HttpResponseCode ServiceUnavailable { get; } = new HttpResponseCode(503, "Service Unavaliable");
        #endregion
    }
}
