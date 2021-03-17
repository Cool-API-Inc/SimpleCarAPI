using CarAPI.ResultTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.Data.SqlClient;


/* კლასი ემსახურება მოწოდებული პარამეტრებით მანქანის ბაზაში რეგისტრაციას */

namespace CarAPI.CrudOperations
{
    public static class Registration
    {
        public static RegistrationResult RegisterCar(HttpRequest Request, string brand, int? year, string desc, IFormFile img, int? features,
            double? cost, string curr)
        {
            // ვამოწმებთ სავალდებულო პარამეტრებს
            if (brand == null || year == null || features == null || cost == null || curr == null)
            {
                return new RegistrationResult(false, RegistrationResult.ResultCode.REQUIRED_FIELDS_MISSING);
            }
            else
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;

                // უნიკალური ID
                string uid = Guid.NewGuid().ToString();

                command.CommandText = "INSERT INTO CAR_INFO "
                    + "(CarID, Brand, PYear, CarDesc, ImgUrl, Features, Cost, Currency, CostGel) " +
                    "VALUES (@id, @brand, @year, @desc, @url, @feat, @cost, @curr, @costGel)";

                command.Parameters.AddWithValue("@id", uid);

                /* ვამოწმებთ მოწოდებულია თუ არა შემდეგი პარამეტრები (ყველა პარამეტრი სავალდებულო არაა), თუ მოწოდებულია
                 * მაშინ ვამოწმეთ მის ვალიდურობას, საჭიროების შემთხვევაში ვამუშავებთ და ვამატებთ მნიშვნელობას ბაზაში შესანახად 
                 */
                {
                    // ვამოწმებთ ბრენდს
                    if (brand != null)
                    {
                        if (DataValidator.ValidateBrand(brand)) command.Parameters.AddWithValue("@brand", brand);
                        else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_BRAND);
                    }
                    else { command.Parameters.AddWithValue("@brand", DBNull.Value); }

                    // ვამოწმებთ გამოშვების წელს
                    if (year != null)
                    {
                        if (DataValidator.ValidateYear(year)) command.Parameters.AddWithValue("@year", year);
                        else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_YEAR);
                    }
                    else { command.Parameters.AddWithValue("@year", DBNull.Value); }

                    // ვამოწმებთ ტექსტურ აღწერას
                    if (desc != null)
                    {
                        if (DataValidator.ValidateDescription(desc)) command.Parameters.AddWithValue("@desc", desc);
                        else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_DESCRIPTION);
                    }
                    else { command.Parameters.AddWithValue("@desc", DBNull.Value); }

                    // ვამოწმებთ სურათს
                    if (img != null)
                    {
                        if (DataValidator.ValidateImage(img))
                            command.Parameters.AddWithValue("@url", DataProcessor.UploadImage(img,
                                Request.Host.Value));
                        else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_IMAGE);
                    }
                    else { command.Parameters.AddWithValue("@url", DBNull.Value); }

                    // ვამოწმებთ მახასიათებლებს
                    if (features != null)
                    {
                        if (DataValidator.ValidateFeatures(features)) command.Parameters.AddWithValue("@feat", features);
                        else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_FEATURES);
                    }
                    else { command.Parameters.AddWithValue("@feat", DBNull.Value); }

                    // ვამოწმებთ მოწოდებულ ღირებულებას
                    if (cost != null)
                    {
                        if (DataValidator.ValidateCost(cost)) command.Parameters.AddWithValue("@cost", cost);
                        else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_COST);
                    }
                    else { command.Parameters.AddWithValue("@cost", DBNull.Value); }

                    // ვამოწმებთ მოწოდებულ ვალუტას
                    if (curr != null)
                    {
                        if (DataValidator.ValidateCurrency(curr)) command.Parameters.AddWithValue("@curr", curr);
                        else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_CURRENCY);
                    }
                    else { command.Parameters.AddWithValue("@curr", DBNull.Value); }

                    // ვითვლით ფასს ლარში
                    if (curr != null && cost != null)
                    {
                        command.Parameters.AddWithValue("@costGel", DataProcessor.GetCostGel(cost ?? default(double), curr));
                    }
                    else { command.Parameters.AddWithValue("@costGel", DBNull.Value); }

                }

                // ვუშვებთ ბრძანებას (წარუმატებლობის შემთხვევაში ვაბრუნებთ შეცდომას)
                if (DatabaseConnector.ExecuteCommand(command))
                {
                    return new RegistrationResult(true, RegistrationResult.ResultCode.SUCCESS, uid);
                }
                else
                {
                    return new RegistrationResult(false, RegistrationResult.ResultCode.UNKNOWN_ERROR);
                }

            }
        }
    }
}
