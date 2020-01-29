namespace CustomFramework.Http.Enumerators
{
    public enum HttpResponseCodeEnum // TODO: Structure that is not enum could be better for the string representation of this data.
    {
        Continue = 100,
        Ok = 200,
        MovedPermanently = 201,
        NotModified = 304,
        BadRequest = 400,
        Unauthorised = 401,
        Forbidden = 403,
        NotFound = 404,
        ImATeapot = 418,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502
    }
}
