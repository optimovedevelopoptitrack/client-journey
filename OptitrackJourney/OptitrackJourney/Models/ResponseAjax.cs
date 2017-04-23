using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptitrackJourney.Models
{
    public class ResponseAjax
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public string ErrorMsg;

        /// <summary>
        /// Default status is Success
        /// </summary>
        public ResponseAjax()
        {
            IsSuccess = true;
        }
        /// <summary>
        /// Default status is Success  
        /// </summary>
        /// <param name="data">object to include in the response</param>
        public ResponseAjax(object data)
        {
            IsSuccess = true;
            Data = data;
        }

        public ResponseAjax(bool isSuccess, object data, string errorMsg)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMsg = errorMsg;
        }

    }
}