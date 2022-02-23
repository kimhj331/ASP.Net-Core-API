using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.Exceptions
{
    public class ForbiddenException : CustomException
    {
        public ForbiddenException(string errorCode) 
            : base(System.Net.HttpStatusCode.Forbidden, errorCode) 
        {
        }
    }
}
