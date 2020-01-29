using CustomFramework.Http.Enumerators;

namespace CustomFramework.Http.ErrorResponses
{
    public class BadRequestResponse : HttpResponse
    {
        private new const HttpResponseCode ResponseCode = HttpResponseCode.BadRequest;

        public BadRequestResponse(Version httpVersion)
            : base(httpVersion, ResponseCode)
        { }
    }
}
