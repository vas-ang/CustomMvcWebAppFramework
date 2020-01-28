using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using CustomFramework.Http.Exceptions;
using CustomFramework.Http.Enumerators;

namespace CustomFramework.Http
{
    public class HttpRequest
    {
        private readonly List<HttpHeader> headers;
        private readonly List<HttpCookie> cookies;

        private HttpRequest()
        {
            headers = new List<HttpHeader>();
            cookies = new List<HttpCookie>();
        }

        public HttpRequest(HttpMethod method, string path, HttpVersion version)
            : this()
        {
            Method = method;
            Path = path;
            Version = version;
        }

        public HttpMethod Method { get; set; }

        public string Path { get; set; }

        public HttpVersion Version { get; set; }

        public IReadOnlyCollection<HttpHeader> Headers => headers.AsReadOnly();

        public IReadOnlyCollection<HttpCookie> Cookies => cookies.AsReadOnly();

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
                    _ => throw new BadRequestException("The request method is invalid.")
                },
                Path = requestLineTokens[1],
                Version = requestLineTokens[2] switch
                {
                    "HTTP/1.0" => HttpVersion.Http10,
                    "HTTP/1.1" => HttpVersion.Http11,
                    "HTTP/2.0" => HttpVersion.Http20,
                    _ => throw new BadRequestException("The http version is invalid.")
                }
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
            catch (IndexOutOfRangeException)
            {
                throw new BadRequestException("There is no end of headers.");
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
            string version = Version switch
            {
                HttpVersion.Http10 => "HTTP/1.0",
                HttpVersion.Http11 => "HTTP/1.1",
                HttpVersion.Http20 => "HTTP/2.0"
            };

            request.Append($"{Method.ToString().ToUpper()} {Path} {version}" + NewLine);

            foreach (var header in headers)
            {
                request.Append(header.ToString() + NewLine);
            }

            request.Append(new HttpHeader("Cookie", string.Join("; ", cookies.Select(c => c.Name + "=" + c.Value))));

            request.Append(NewLine);

            if (!string.IsNullOrEmpty(Body))
            {
                request.Append(Body);
            }

            return request.ToString();
        }
    }
}