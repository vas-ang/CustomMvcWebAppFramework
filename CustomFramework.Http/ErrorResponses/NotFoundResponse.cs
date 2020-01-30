namespace CustomFramework.Http.ErrorResponses
{
    public class NotFoundResponse : HttpResponse
    {
        private new static readonly HttpResponseCode ResponseCode = new HttpResponseCode(404, "Not Found");

        public NotFoundResponse(Version httpVersion)
            : base(httpVersion, ResponseCode)
        { }
    }
}
