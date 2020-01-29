using System;

namespace CustomFramework.Http
{
    public static class HttpVersion
    {
        private const string Version10 = "HTTP/1.0";
        private const string Version11 = "HTTP/1.1";
        private const string Version20 = "HTTP/2.0";

        private static readonly Version http10 = new Version(Version10);
        private static readonly Version http11 = new Version(Version11);
        private static readonly Version http20 = new Version(Version20);

        public static Version Http10 => http10;

        public static Version Http11 => http11;

        public static Version Http20 => http20;

        public static Version Parse(string versionString)
        {
            var version = versionString switch
            {
                Version10 => http10,
                Version11 => http11,
                Version20 => http20,
                _ => throw new ArgumentException($"HTTP version {versionString} is invalid."),
            };

            return version;
        }
    }
}