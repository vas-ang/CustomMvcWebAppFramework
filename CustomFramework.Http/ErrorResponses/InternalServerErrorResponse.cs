namespace CustomFramework.Http.ErrorResponses
{
    public class InternalServerErrorResponse : HttpResponse
    {
        private new static readonly HttpResponseCode ResponseCode = new HttpResponseCode(500, "Internal Server Error");

        public InternalServerErrorResponse(Version version)
            : base(version, ResponseCode)
        { }
    }
}
