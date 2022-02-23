using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.Exceptions
{
    public class InternalServerErrorException : CustomException
    {
        public InternalServerErrorException(string errorCode)
            : base(System.Net.HttpStatusCode.InternalServerError, errorCode)
        {
        }
    }
}
