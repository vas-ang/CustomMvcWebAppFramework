using System;

namespace CustomFramework.Http
{
    public class HttpVersion
    {
        private const string Version10 = "HTTP/1.0";
        private const string Version11 = "HTTP/1.1";

        private readonly string version;

        private HttpVersion()
        {

        }

        private HttpVersion(string version)
        {
            if (version != Version10 && version != Version11)
            {
                throw new ArgumentException($"HTTP version {version} is invalid or not supported.");
            }

            this.version = version;
        }

        public static HttpVersion Parse(string versionString)
        {
            var version = versionString switch
            {
                Version10 => Http10,
                Version11 => Http11,
                _ => throw new ArgumentException($"HTTP version {versionString} is invalid or not supported.")
            };

            return version;
        }

        public override string ToString()
        {
            return this.version;
        }

        #region Static Properties
        public static HttpVersion Http10 { get; } = new HttpVersion(Version10);
        public static HttpVersion Http11 { get; } = new HttpVersion(Version11);
        #endregion
    }
}