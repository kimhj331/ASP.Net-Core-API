using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException(string errorCode)
            : base(System.Net.HttpStatusCode.Conflict, errorCode)
        {
        }
    }
}
