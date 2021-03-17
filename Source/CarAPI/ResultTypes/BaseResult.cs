
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
