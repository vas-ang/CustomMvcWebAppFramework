namespace CustomFramework.Http.Responses
{
    using Common;
    using Elements;

    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string redirectPath)
            : base(ServerConfiguration.HttpVersion, HttpResponseCode.Found)
        {
            this.AddHeader(new HttpHeader("Location", redirectPath));
        }
    }
}
