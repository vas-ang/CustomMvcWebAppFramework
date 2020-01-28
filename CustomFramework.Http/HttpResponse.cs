using System.Linq;
using System.Text;
using System.Collections.Generic;

using CustomFramework.Http.Enumerators;

namespace CustomFramework.Http
{
    public class HttpResponse
    {
        private readonly List<HttpHeader> headers;
        private readonly List<HttpCookie> cookies;

        private HttpResponse()
        {
            headers = new List<HttpHeader>();
            cookies = new List<HttpCookie>();
        }

        public HttpResponse(HttpVersion version, HttpResponseCode responseCode)
             : this()
        {
            Version = version;
            ResponseCode = responseCode;
        }

        public HttpVersion Version { get; set; }

        public HttpResponseCode ResponseCode { get; set; }

        public IReadOnlyCollection<HttpHeader> Headers => headers.AsReadOnly();

        public IReadOnlyCollection<HttpCookie> Cookies => cookies.AsReadOnly();

        public byte[] Body { get; set; }

        public void AddHeader(HttpHeader header)
        {
            headers.Add(header);
        }

        public bool RemoveHeader(string name)
        {
            HttpHeader header = headers.FirstOrDefault(h => h.Name == name);

            if (header == null)
            {
                return false;
            }

            return headers.Remove(header);
        }

        public void AppendCookie(HttpCookie cookie)
        {
            cookies.Add(cookie);
        }

        public bool RemoveCookie(string name)
        {
            HttpCookie cookie = cookies.FirstOrDefault(h => h.Name == name);

            if (cookie == null)
            {
                return false;
            }

            return cookies.Remove(cookie);
        }

        public byte[] GetBytes(Encoding stringEncoding)
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(stringEncoding.GetBytes(ToString()));

            if (Body != null)
            {
                bytes.AddRange(Body);
            }

            return bytes.ToArray();
        }

        public override string ToString()
        {
            StringBuilder response = new StringBuilder();
            string version = Version switch
            {
                HttpVersion.Http10 => "HTTP/1.0",
                HttpVersion.Http11 => "HTTP/1.1",
                HttpVersion.Http20 => "HTTP/2.0"
            };

            response.Append($"{version} {(int)ResponseCode} {ResponseCode}" + NewLine);

            foreach (var header in headers)
            {
                response.Append(header.ToString() + NewLine);
            }

            foreach (var cookie in cookies)
            {
                response.Append(new HttpHeader("Set-Cookie", cookie.ToString()));
            }

            response.Append(NewLine);

            return response.ToString();
        }
    }
}
