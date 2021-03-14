using CarAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarAPI.ResultTypes
{
    public class GetResult
    {
        public bool Success { get; set; }
        public List<Car> Cars { get; set; }

        public GetResult(bool success, List<Car> cars = null)
        {
            Success = success;
            Cars = cars;
        }
    }
}
