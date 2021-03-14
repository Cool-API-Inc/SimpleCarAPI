using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarAPI.ResultTypes
{
    public class RegistrationResult
    {
        /* შედეგის კოდი */ 
        public enum ResultCode
        {
            // წარმატებული რეგისტრაცია
            SUCCESS,
            // აკლია ზოგიერთი სავალდებულო პარამეტრი
            REQUIRED_FIELDS_MISSING,
            // მოწოდებული პარამეტრი არავალიდურია
            INVALID_BRAND,
            INVALID_YEAR,
            INVALID_DESCRIPTION,
            INVALID_IMAGE,
            INVALID_FEATURES,
            INVALID_COST,
            INVALID_CURRENCY,
            // სხვა ნებისმიერი შეცდომა პროგრამაში
            UNKNOWN_ERROR
        };

        public bool Success { get; set; }
        public string RegisteredID { get; set; }
        public ResultCode ReturnCode { get; set; }
        public string ReturnMessage { get; set; }

        public RegistrationResult(bool success, ResultCode returnCode, string registeredID = null)
        {
            Success = success;
            RegisteredID = registeredID;
            ReturnCode = returnCode;
            ReturnMessage = ReturnCode.ToString();
        }
    }
}
