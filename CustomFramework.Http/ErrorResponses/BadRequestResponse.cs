using CustomFramework.Http.Enumerators;

namespace CustomFramework.Http.ErrorResponses
{
    public class BadRequestResponse : HttpResponse
    {
        private new const HttpResponseCodeEnum ResponseCode = HttpResponseCodeEnum.BadRequest;

        public BadRequestResponse(Version httpVersion)
            : base(httpVersion, ResponseCode)
        { }
    }
}
