using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarAPI.ResultTypes
{
    public class BaseResult
    {
        public bool Success { get; set; }
        public BaseResult(bool success)
        {
            Success = success;
        }
    }
}
