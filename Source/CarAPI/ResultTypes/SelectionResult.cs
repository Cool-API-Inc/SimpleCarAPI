using CarAPI.Models;
using System.Collections.Generic;

namespace CarAPI.ResultTypes
{
    public class SelectionResult : BaseResult
    {
        public List<Car> Cars { get; set; }

        public SelectionResult(bool success, List<Car> cars = null) : base(success)
        {
            Cars = cars;
        }
    }
}
