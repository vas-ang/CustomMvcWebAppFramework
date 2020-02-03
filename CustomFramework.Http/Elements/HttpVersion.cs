namespace CustomFramework.Http.Elements
{
    using System;

    using static Common.ExceptionStrings;

    public class HttpVersion
    {
        private const string Version10 = "HTTP/1.0";
        private const string Version11 = "HTTP/1.1";

        private static HttpVersion version10;
        private static HttpVersion version11;

        private readonly string version;

        protected HttpVersion(string version)
        {
            this.version = version;
        }

        public static HttpVersion Parse(string versionString)
        {
            HttpVersion httpVersion = versionString switch
            {
                Version10 => Http10,
                Version11 => Http11,
                _ => throw new ArgumentException(string.Format(ElementInvalidOrNotSupported, "version", versionString)),
            };

            return httpVersion;
        }

        public override string ToString()
        {
            return this.version;
        }

        #region Static Properties
        public static HttpVersion Http10
        {
            get
            {
                if (version10 == null)
                {
                    version10 = new HttpVersion(Version10);
                }

                return version10;
            }
        }

        public static HttpVersion Http11
        {
            get
            {
                if (version11 == null)
                {
                    version11 = new HttpVersion(Version11);
                }

                return version11;
            }
        }
        #endregion
    }
}