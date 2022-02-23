using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.Exceptions
{
    public class UnAuthorizedException : CustomException
    {
        public UnAuthorizedException(string errorCode)
            : base(System.Net.HttpStatusCode.Unauthorized, errorCode)
        {
        }
    }
}
