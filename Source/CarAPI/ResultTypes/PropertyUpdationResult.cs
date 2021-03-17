using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarAPI.ResultTypes
{
    public class PropertyUpdationResult
    {
        public bool Success { get; set; }
        public int PropertiesAffected { get; set; }
        public PropertyUpdationResult(bool success, int propertiesAffected = 0)
        {
            Success = success;
            PropertiesAffected = propertiesAffected;
        }
    }
}
