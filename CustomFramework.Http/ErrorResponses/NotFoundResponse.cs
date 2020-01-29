using CustomFramework.Http.Enumerators;

namespace CustomFramework.Http.ErrorResponses
{
    public class NotFoundResponse : HttpResponse
    {
        private new const HttpResponseCode ResponseCode = HttpResponseCode.InternalServerError;

        public NotFoundResponse(Version httpVersion)
            : base(httpVersion, ResponseCode)
        { }
    }
}
