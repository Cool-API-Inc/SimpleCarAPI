
namespace CarAPI.ResultTypes
{
    public class PropertyUpdationResult : BaseResult
    {
        public int PropertiesAffected { get; set; }
        public PropertyUpdationResult(bool success, int propertiesAffected = 0) : base(success)
        {
            PropertiesAffected = propertiesAffected;
        }
    }
}
