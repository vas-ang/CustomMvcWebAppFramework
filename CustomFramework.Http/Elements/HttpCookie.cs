namespace CustomFramework.Http.Elements
{
    using System;
    using System.Text;

    using Enumerators;

    public class HttpCookie
    {
        public HttpCookie(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Domain { get; set; }

        public string Path { get; set; }

        public DateTime? Expires { get; set; }

        public long? MaxAge { get; set; }

        public bool HttpOnly { get; set; }

        public bool Secure { get; set; }

        public SameSite? SameSite { get; set; }

        public override string ToString()
        {
            StringBuilder cookie = new StringBuilder();

            cookie.Append($"{Name}={Value}");

            if (!string.IsNullOrEmpty(Domain))
            {
                cookie.Append($"; Domain={Domain}");
            }

            if (!string.IsNullOrEmpty(Path))
            {
                cookie.Append($"; Path={Path}");
            }

            if (Expires.HasValue)
            {
                cookie.Append($"; Expires={Expires.Value.ToString("r")}");
            }
            else if (MaxAge.HasValue)
            {
                cookie.Append($"; Max-Age={MaxAge.Value}");
            }

            if (HttpOnly)
            {
                cookie.Append($"; HttpOnly");
            }

            if (Secure)
            {
                cookie.Append($"; Secure");
            }

            if (SameSite.HasValue)
            {
                cookie.Append($"; SameSite={SameSite.Value}");
            }

            return cookie.ToString();
        }
    }
}
