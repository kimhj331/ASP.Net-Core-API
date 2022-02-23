using System;
using System.Linq.Expressions;
using System.Net;

namespace ButlerLee.Exceptions
{
    public class CustomException : Exception
    {
        public HttpStatusCode HttpStatus { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public CustomException(HttpStatusCode httpStatus, string errorCode, string errorMessage)
        {
            HttpStatus = httpStatus;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public CustomException(HttpStatusCode httpStatus, string errorCode)
        {
            HttpStatus = httpStatus;
            //ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
