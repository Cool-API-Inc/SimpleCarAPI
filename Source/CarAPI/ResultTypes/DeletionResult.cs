using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarAPI.ResultTypes
{
    public class DeletionResult 
    {
        public bool Success { get; set; }
        public DeletionResult(bool success)
        {
            Success = success;
        }
    }
}
