namespace CustomFramework.Http.Responses
{
    using System.Text;

    public class HtmlResponse : FileResponse
    {
        private const string FileType = "text/html";

        private static Encoding defaultEncoding = Encoding.UTF8;

        public HtmlResponse(string html)
         : base(defaultEncoding.GetBytes(html), FileType)
        { }

        public HtmlResponse(string html, Encoding encoding)
         : base(encoding.GetBytes(html), FileType)
        { }
    }
}