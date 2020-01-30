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
    }
}
