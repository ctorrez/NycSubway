using System;
using System.Collections.Generic;
using System.Text;

namespace NycSubway.Core.Models
{
    public class ResultResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ResultResponse SuccessResponse()
        {
            return new ResultResponse()
            {
                Success = true,
                Message = "Ok"
            };
        }
    }
}
