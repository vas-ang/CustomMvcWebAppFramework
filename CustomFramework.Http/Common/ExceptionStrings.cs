namespace CustomFramework.Http.Common
{
    public static class ExceptionStrings
    {
        public const string ElementInvalidOrNotSupported = "HTTP {0} {1} is invalid or not supported.";

        public const string InvalidHeader = "Invalid header.";

        public const string CookieWithNameDoesNotExist = "Cookie with name {0} does not exist.";

        public const string InvalidRequestLine = "The request line is invalid.";

        public const string InvalidCookieHeader = "Cookie header has invalid value.";

        public const string NoEndOfHeaders = "There is no end of headers.";
    }
}
