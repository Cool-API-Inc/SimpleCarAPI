using System.Collections.Generic;

namespace CarAPI.ResultTypes
{
    public class PropertySelectionResult : BaseResult
    {
        public int? Features { get; set; }
        public List<string> FeatureList { get; set; }

        public PropertySelectionResult(bool success, int? features = null, List<string> featureList = null) : base(success)
        {
            Features = features;
            FeatureList = featureList;
        }

    }
}
