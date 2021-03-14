using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarAPI.ResultTypes
{
    public class SetPropertiesResult
    {
        public bool Success { get; set; }
        public int PropertiesAffected { get; set; }
        public SetPropertiesResult(bool success, int propertiesAffected = 0)
        {
            Success = success;
            PropertiesAffected = propertiesAffected;
        }
    }
}
