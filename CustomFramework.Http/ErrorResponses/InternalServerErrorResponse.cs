﻿using CustomFramework.Http.Enumerators;

namespace CustomFramework.Http.ErrorResponses
{
    public class InternalServerErrorResponse : HttpResponse
    {
        private new const HttpResponseCode ResponseCode = HttpResponseCode.InternalServerError;

        public InternalServerErrorResponse(Version version)
            : base(version, ResponseCode)
        { }
    }
}
