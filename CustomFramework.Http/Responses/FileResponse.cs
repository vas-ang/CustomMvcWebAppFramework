namespace CustomFramework.Http.Responses
{
    using Common;
    using Elements;

    public class FileResponse : HttpResponse
    {
        public FileResponse(HttpResponseCode responseCode, byte[] body, string mimeType)
            : base(ServerConfiguration.HttpVersion, responseCode)
        {
            this.AddHeader(new HttpHeader("Content-Type", mimeType));
            this.AddHeader(new HttpHeader("Content-Length", body.Length.ToString()));

            this.Body = body;
        }
    }
}
