namespace CustomFramework.Http
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    using Elements;
    using Exceptions;

    using static Common.HttpConstants;
    using static Common.ExceptionStrings;

    public class HttpRequest
    {
        private readonly ICollection<HttpHeader> headers;
        private readonly ICollection<HttpCookie> cookies;

        public HttpRequest(HttpMethod method, string path, HttpVersion httpVersion)
        {
            this.Method = method;
            this.Path = path;
            this.HttpVersion = httpVersion;

            this.headers = new List<HttpHeader>();
            this.cookies = new List<HttpCookie>();
        }

        public HttpRequest(HttpMethod method, string path, HttpVersion httpVersion, ICollection<HttpHeader> headers, ICollection<HttpCookie> cookies)
            : this(method, path, httpVersion)
        {
            this.headers = headers;
            this.cookies = cookies;
        }

        public HttpMethod Method { get; set; }

        public string Path { get; set; }

        public HttpVersion HttpVersion { get; set; }

        public IReadOnlyCollection<HttpHeader> Headers => this.headers.ToList().AsReadOnly();

        public IReadOnlyCollection<HttpCookie> Cookies => this.cookies.ToList().AsReadOnly();

        public string Body { get; set; }

        public HttpFormData FormData { get; set; }

        public IDictionary<string, string> SessionData { get; set; }

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

        public void AddCookie(HttpCookie cookie)
        {
            this.cookies.Add(cookie);
        }

        public string GetCookieValue(string name)
        {
            HttpCookie cookie = this.cookies.FirstOrDefault(c => c.Name == name);

            if (cookie == null)
            {
                throw new InvalidOperationException(string.Format(CookieWithNameDoesNotExist, name));
            }

            return cookie.Value;
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

        public static HttpRequest Parse(string request)
        {
            string[] lines = request.Split(new string[] { NewLine }, StringSplitOptions.None);
            string[] requestLineTokens = lines[0].Split(' ');

            if (requestLineTokens.Length != 3)
            {
                throw new BadRequestException(InvalidRequestLine);
            }

            string[] linkTokens = requestLineTokens[1].Split(new char[] { '?' }, 2);

            HttpMethod method = HttpMethod.Parse(requestLineTokens[0]);
            string path = linkTokens[0];
            HttpVersion version = HttpVersion.Parse(requestLineTokens[2]);

            HttpRequest httpRequest = new HttpRequest(method, path, version);


            if (linkTokens.Length == 2)
            {
                httpRequest.FormData = HttpFormData.Parse(linkTokens[1]);
            }

            int i = 0;
            try
            {
                while (!string.IsNullOrEmpty(lines[++i]))
                {
                    HttpHeader header = HttpHeader.Parse(lines[i]);

                    if (header.Name.ToLower() == "cookie")
                    {
                        string[] cookies = header.Value.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var cookie in cookies)
                        {
                            string[] cookieKvp = cookie.Split('=');

                            if (cookieKvp.Length != 2)
                            {
                                throw new BadRequestException(InvalidCookieHeader);
                            }

                            httpRequest.AddCookie(new HttpCookie(cookieKvp[0], cookieKvp[1]));
                        }
                    }
                    else
                    {
                        httpRequest.AddHeader(header);
                    }
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new BadRequestException(NoEndOfHeaders, ex);
            }

            if (i < lines.Length - 1)
            {
                StringBuilder body = new StringBuilder();

                while (++i != lines.Length)
                {
                    string line = lines[i];

                    body.Append(line + NewLine);
                }

                httpRequest.Body = body.ToString()
                    .TrimEnd(new char[] { '\r', '\n' });

                if (!string.IsNullOrEmpty(httpRequest.Body))
                {
                    httpRequest.FormData = HttpFormData.Parse(httpRequest.Body);
                }
            }

            return httpRequest;
        }

        public override string ToString()
        {
            StringBuilder request = new StringBuilder();

            request.Append($"{this.Method} {this.Path} {this.HttpVersion}{NewLine}");

            foreach (var header in headers)
            {
                request.Append($"{header}{NewLine}");
            }

            string allCookies = string.Join("; ", this.cookies.Select(c => c.Name + "=" + c.Value));

            request.Append(new HttpHeader("Cookie", allCookies));

            request.Append(NewLine);

            if (!string.IsNullOrEmpty(this.Body))
            {
                request.Append(this.Body);
            }

            return request.ToString();
        }
    }
}