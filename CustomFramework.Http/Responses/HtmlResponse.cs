namespace CustomFramework.Http.Responses
{
    using Common;
    using Elements;

    public class HtmlResponse : FileResponse
    {
        private const string MimeType = "text/html";

        public HtmlResponse(string html)
         : base(HttpResponseCode.Ok, ServerConfiguration.Encoding.GetBytes(html), MimeType)
        { }
    }
}