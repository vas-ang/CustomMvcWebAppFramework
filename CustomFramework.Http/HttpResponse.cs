namespace CustomFramework.Http
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using Common;
    using Elements;

    using static Common.HttpConstants;

    public class HttpResponse
    {
        private readonly ICollection<HttpHeader> headers;
        private readonly ICollection<HttpCookie> cookies;

        public HttpResponse(HttpVersion httpVersion, HttpResponseCode responseCode)
        {
            this.HttpVersion = httpVersion;
            this.ResponseCode = responseCode;

            this.headers = new List<HttpHeader>();
            this.cookies = new List<HttpCookie>();
        }

        public HttpResponse(HttpVersion httpVersion, HttpResponseCode responseCode, ICollection<HttpHeader> headers, ICollection<HttpCookie> cookies)
            : this(httpVersion, responseCode)
        {
            this.headers = headers;
            this.cookies = cookies;
        }

        public HttpVersion HttpVersion { get; set; }

        public HttpResponseCode ResponseCode { get; set; }

        public IReadOnlyCollection<HttpHeader> Headers => this.headers.ToList().AsReadOnly();

        public IReadOnlyCollection<HttpCookie> Cookies => this.cookies.ToList().AsReadOnly();

        public byte[] Body { get; set; }

        public void AddHeader(HttpHeader header)
        {
            this.headers.Add(header);
        }

        public bool RemoveHeader(string name)
        {
            HttpHeader header = this.headers.FirstOrDefault(h => h.Name == name);

            if (header == null)
            {
                return false;
            }

            return this.headers.Remove(header);
        }

        public void AppendCookie(HttpCookie cookie)
        {
            this.cookies.Add(cookie);
        }

        public bool RemoveCookie(string name)
        {
            HttpCookie cookie = this.cookies.FirstOrDefault(h => h.Name == name);

            if (cookie == null)
            {
                return false;
            }

            return this.cookies.Remove(cookie);
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(ServerConfiguration.Encoding.GetBytes(ToString()));

            if (this.Body != null)
            {
                bytes.AddRange(this.Body);
            }

            return bytes.ToArray();
        }

        public override string ToString()
        {
            StringBuilder response = new StringBuilder();

            response.Append($"{this.HttpVersion} {this.ResponseCode}{NewLine}");

            foreach (var header in this.headers)
            {
                response.Append($"{header}{NewLine}");
            }

            foreach (var cookie in this.cookies)
            {
                response.Append(new HttpHeader("Set-Cookie", cookie.ToString()) + NewLine);
            }

            response.Append(NewLine);

            return response.ToString();
        }
    }
}
