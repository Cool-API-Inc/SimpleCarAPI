using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarAPI.ResultTypes
{
    public class PropertySelectionResult
    {
        public bool Success { get; set; }
        public int? Features { get; set; }
        public List<string> FeatureList { get; set; }

        public PropertySelectionResult(bool success, int? features = null, List<string> featureList = null)
        {
            Success = success;
            Features = features;
            FeatureList = featureList;
        }

    }
}
