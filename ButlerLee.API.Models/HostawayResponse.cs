using ButlerLee.API.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models
{
    public class HostawayResponse<TResult> : BaseResponse
    {
        public HostawayResponse() { }

        public HostawayResponse(ResponseStatus responseStatus, TResult result)
        {
            base.Status = responseStatus;
            this.Result = result;
        }

        public TResult Result { get; set; }
    }
}
