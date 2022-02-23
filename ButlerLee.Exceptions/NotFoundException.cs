using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string errorCode,string errorMessage)
            : base(System.Net.HttpStatusCode.NotFound, errorCode, errorMessage)
        {
        }
    }
}
