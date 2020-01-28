using CustomFramework.Http.Enumerators;

namespace CustomFramework.Http.ErrorResponses
{
    public class BadRequestResponse : HttpResponse
    {
        private new const HttpResponseCode ResponseCode = HttpResponseCode.BadRequest;

        public BadRequestResponse(HttpVersion version)
            : base(version, ResponseCode)
        { }
    }
}
