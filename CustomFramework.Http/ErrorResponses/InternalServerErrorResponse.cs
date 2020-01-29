using CustomFramework.Http.Enumerators;

namespace CustomFramework.Http.ErrorResponses
{
    public class InternalServerErrorResponse : HttpResponse
    {
        private new const HttpResponseCodeEnum ResponseCode = HttpResponseCodeEnum.InternalServerError;

        public InternalServerErrorResponse(Version version)
            : base(version, ResponseCode)
        { }
    }
}
