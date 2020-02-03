namespace CustomFramework.Http.Responses
{
    using Elements;

    public class FileResponse : HttpResponse
    {
        private static readonly HttpVersion httpVersion = HttpVersion.Http10;
        private static readonly HttpResponseCode httpResponseCode = HttpResponseCode.Ok;

        public FileResponse(byte[] body, string fileType)
            : base(httpVersion, httpResponseCode)
        {
            this.AddHeader(new HttpHeader("Content-Type", fileType));
            this.AddHeader(new HttpHeader("Content-Length", body.Length.ToString()));
            this.Body = body;
        }
    }
}
