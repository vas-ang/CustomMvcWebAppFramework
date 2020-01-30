using System.Text;

namespace CustomFramework.Http.Responses
{
    public class HtmlResponse : FileResponse
    {
        private const string FileType = "text/html";

        public HtmlResponse(string html)
            : base(Encoding.UTF8.GetBytes(html), FileType)
        { }
    }
}