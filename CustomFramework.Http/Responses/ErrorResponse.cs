namespace CustomFramework.Http.Responses
{
    using Common;
    using Elements;

    public class ErrorResponse : HttpResponse
    {
        private const string MimeType = "text/txt";


        public ErrorResponse(HttpResponseCode responseCode, string errorText)
           : base(ServerConfiguration.HttpVersion, responseCode)
        {
            this.Body = ServerConfiguration.Encoding.GetBytes(errorText);

            this.AddHeader(new HttpHeader("Content-Type", MimeType));
            this.AddHeader(new HttpHeader("Content-Length", this.Body.Length.ToString()));
        }

        public ErrorResponse(HttpResponseCode responseCode)
            : base(ServerConfiguration.HttpVersion, responseCode)
        {

        }
    }
}
