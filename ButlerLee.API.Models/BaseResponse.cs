using ButlerLee.API.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models
{
    public class BaseResponse
    {
        public ResponseStatus Status { get; set; }

        public string Message { get; set; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public int Count { get; set; }

        public int Page { get; set; }

        public int TotalPages { get; set; }
    }
}
