namespace CustomFramework.Http.ErrorResponses
{
    public class BadRequestResponse : HttpResponse
    {
        private new static readonly HttpResponseCode ResponseCode = new HttpResponseCode(400, "Bad Request");

        public BadRequestResponse(Version httpVersion)
            : base(httpVersion, ResponseCode)
        { }
    }
}
