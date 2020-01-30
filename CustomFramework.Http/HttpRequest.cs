using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using CustomFramework.Http.Exceptions;
using CustomFramework.Http.Enumerators;

using static CustomFramework.Http.HttpConstants;

namespace CustomFramework.Http
{
    public class HttpRequest
    {
        private readonly IList<HttpHeader> headers;
        private readonly IList<HttpCookie> cookies;

        private HttpRequest()
        {
            this.headers = new List<HttpHeader>();
            this.cookies = new List<HttpCookie>();
        }

        public HttpRequest(HttpMethod method, string path, HttpVersion httpVersion)
            : this()
        {
            this.Method = method;
            this.Path = path;
            this.HttpVersion = httpVersion;
        }

        public HttpMethod Method { get; set; }

        public string Path { get; set; }

        public HttpVersion HttpVersion { get; set; }

        public IReadOnlyCollection<HttpHeader> Headers => ((List<HttpHeader>)this.headers).AsReadOnly();

        public IReadOnlyCollection<HttpCookie> Cookies => ((List<HttpCookie>)this.cookies).AsReadOnly();

        public string Body { get; set; }

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

        public void AddCookie(HttpCookie cookie)
        {
            cookies.Add(cookie);
        }

        public string GetCookieValue(string name)
        {
            HttpCookie cookie = this.cookies.FirstOrDefault(c => c.Name == name);

            if (cookie == null)
            {
                throw new InvalidOperationException($"Cookie with name {name} does not exist.");
            }

            return cookie.Value;
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

        public static HttpRequest Parse(string request)
        {
            string[] lines = request.Split(new string[] { NewLine }, StringSplitOptions.None);
            string[] requestLineTokens = lines[0].Split(' ');

            if (requestLineTokens.Length != 3)
            {
                throw new BadRequestException("The request line is invalid.");
            }

            HttpRequest httpRequest = new HttpRequest
            {
                Method = requestLineTokens[0] switch
                {
                    "GET" => HttpMethod.Get,
                    "POST" => HttpMethod.Post,
                    "PUT" => HttpMethod.Put,
                    "DELETE" => HttpMethod.Delete,
                    _ => throw new BadRequestException("The request method is invalid or not supported.")
                },
                Path = requestLineTokens[1],
                HttpVersion = Http.HttpVersion.Parse(requestLineTokens[2])
            };

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
                                throw new BadRequestException("Cookie header has invalid value.");
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
                throw new BadRequestException("There is no end of headers.", ex);
            }

            if (i != lines.Length - 1)
            {
                while (++i != lines.Length)
                {
                    httpRequest.Body += lines[i] + NewLine;
                }
            }

            return httpRequest;
        }

        public override string ToString()
        {
            StringBuilder request = new StringBuilder();

            request.Append($"{Method.ToString().ToUpper()} {Path} {this.HttpVersion.ToString()}" + NewLine);

            foreach (var header in headers)
            {
                request.Append(header.ToString() + NewLine);
            }

            string allCookies = string.Join("; ", this.cookies.Select(c => c.Name + "=" + c.Value));

            request.Append(new HttpHeader("Cookie", allCookies));

            request.Append(NewLine);

            if (!string.IsNullOrEmpty(Body))
            {
                request.Append(Body);
            }

            return request.ToString();
        }
    }
}