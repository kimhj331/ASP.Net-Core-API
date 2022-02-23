using System;
using System.Net;

namespace ButlerLee.API.Models.Exceptions
{
    public class CustomException : Exception
    {
        public HttpStatusCode HttpStatus { get; set; }
        public string ErrorCode { get; set; }

        public CustomException(HttpStatusCode httpStatus, string errorCode)
        {
            HttpStatus = httpStatus;
            ErrorCode = errorCode;
        }
    }
}
