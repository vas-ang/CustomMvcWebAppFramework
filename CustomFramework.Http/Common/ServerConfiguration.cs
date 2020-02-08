namespace CustomFramework.Http.Common
{
    using System.Text;

    using Elements;

    public static class ServerConfiguration
    {
        public static HttpVersion HttpVersion => HttpVersion.Http10;

        public static Encoding Encoding => Encoding.UTF8;
    }
}
