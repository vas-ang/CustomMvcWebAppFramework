using System;

namespace CustomFramework.Http.Exceptions
{
    public class BadRequestException : Exception // TODO: Throwing inner exceptions to all custom exceptions could make debugging easier and it won't affect the user experience.
    {
        public BadRequestException(string message)
            : base(message)
        { }
    }
}
