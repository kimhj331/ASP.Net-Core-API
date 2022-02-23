using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string errorCode,string errorMessage) 
            : base(System.Net.HttpStatusCode.BadRequest, errorCode, errorMessage)
        { 
        }
    }
}
