using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ButlerLee.API.Models.Exceptions
{
    public class PaymentException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ErrorCode { get; set; } = "Payment";

        public PaymentException(HttpStatusCode statusCode, string content) : base(content)
        {
            StatusCode = statusCode;
        }
    }
}
