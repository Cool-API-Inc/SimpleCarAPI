using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarAPI.ResultTypes
{
    public class UpdateResult
    {
        /* შედეგის კოდი */
        public enum ResultCode
        {
            // წარმატებული განახლება
            SUCCESS,
            // ID არაა მითითებული
            ID_REQUIRED,
            // მითითებული ID ვერ მოიძებნა
            ID_NOT_FOUND,
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
        public int FieldsAffected { get; set; }
        public ResultCode ReturnCode { get; set; }
        public string ReturnMessage { get; set; }

        public UpdateResult(bool success, ResultCode returnCode, int fieldsAffected = 0)
        {
            Success = success;
            ReturnCode = returnCode;
            FieldsAffected = fieldsAffected;
            ReturnMessage = ReturnCode.ToString();
        }
    }
}
