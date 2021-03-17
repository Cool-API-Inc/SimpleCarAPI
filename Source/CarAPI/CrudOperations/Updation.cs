using CarAPI.ResultTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/* კლასი ემსახურება ბაზაში რეგისტრირებული მანქანის მონაცემების განახლებას */

namespace CarAPI.CrudOperations
{
    public class Updation
    {
        public static UpdationResult UpdateCar(HttpRequest Request, string id, string brand, int? year, string desc, IFormFile img, int? features,
            double? cost, string curr)
        {

            if (id == null)
            {
                return new UpdationResult(false, UpdationResult.ResultCode.ID_REQUIRED);
            }
            else
            {
                // შეცვლილი ველების რაოდენობა
                int affected = 0;

                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;

                /* რადგან წინასწარ არ ვიცით რომელი ველების შეცვლა მოგვიწევს რედაქტირების თითოეულ ბრძანებას 
                 * კოლექციაში ვინახავთ */
                List<string> queryAsm = new List<string>();

                /* სათითაოდ ვამოწმებთ პარამეტრებს, ვახდენთ არანულოვანი პარამეტრების ვალიდაციას და ვამატებთ მათ
                 * მნიშვნელობას Query-ში, შესაბამის ბრძანებასთან ერთად */

                if (brand != null) // ვამოწმებთ მოწოდებულ ბრენდს
                {
                    if (DataValidator.ValidateBrand(brand))
                    {
                        queryAsm.Add("Brand = @brand");
                        command.Parameters.AddWithValue(@"brand", brand);
                        affected++;
                    }
                    else
                    {
                        return new UpdationResult(false, UpdationResult.ResultCode.INVALID_BRAND);
                    }
                }
                if (year != null) // ვამოწმებთ მოწოდებულ გამოშვების წელს
                {
                    if (DataValidator.ValidateYear(year))
                    {
                        queryAsm.Add("PYear = @year");
                        command.Parameters.AddWithValue("@year", year);
                        affected++;
                    }
                    else
                    {
                        return new UpdationResult(false, UpdationResult.ResultCode.INVALID_YEAR);
                    }
                }
                if (desc != null) // ვამოწმებთ მოწოდებულ აღწერას
                {
                    if (DataValidator.ValidateDescription(desc))
                    {
                        queryAsm.Add("CarDesc = @desc");
                        command.Parameters.AddWithValue("@desc", desc);
                        affected++;
                    }
                    else
                    {
                        return new UpdationResult(false, UpdationResult.ResultCode.INVALID_DESCRIPTION);
                    }
                }
                if (img != null) // ვამოწმებთ მოწოდებულ ფოტოს
                {
                    if (DataValidator.ValidateImage(img))
                    {
                        queryAsm.Add("ImgUrl = @url");
                        command.Parameters.AddWithValue("@url",
                            DataProcessor.UploadImage(img, Request.Host.Value));
                        affected++;
                    }
                    else
                    {
                        return new UpdationResult(false, UpdationResult.ResultCode.INVALID_IMAGE);
                    }
                }
                if (features != null) // ვამოწმებთ მოწოდებულ მახასიათებლების ნამრავლს
                {
                    if (DataValidator.ValidateFeatures(features))
                    {
                        queryAsm.Add("Features = @feat");
                        command.Parameters.AddWithValue("@feat", features);
                        affected++;
                    }
                    else
                    {
                        return new UpdationResult(false, UpdationResult.ResultCode.INVALID_FEATURES);
                    }
                }

                // ვამოწმებთ მოწოდებულ ღირებულებას და ვალუტას (ცალ-ცალკე მათი ვალიდაცია უაზრო იქნებოდა)
                if (cost != null && curr != null)
                {
                    if (!DataValidator.ValidateCost(cost))
                        return new UpdationResult(false, UpdationResult.ResultCode.INVALID_COST);
                    if (!DataValidator.ValidateCurrency(curr))
                        return new UpdationResult(false, UpdationResult.ResultCode.INVALID_CURRENCY);

                    queryAsm.Add("Cost = @cost");
                    queryAsm.Add("Currency = @curr");
                    queryAsm.Add("CostGel = @costGel");

                    command.Parameters.AddWithValue("@cost", cost);
                    command.Parameters.AddWithValue("@curr", curr);
                    command.Parameters.AddWithValue("@costGel", DataProcessor.GetCostGel(cost ?? default(double), curr));

                    affected += 2;
                }

                // საბოლოოდ ვადგენთ ველების რედაქტირებისთვის საჭირო SQL ბრძანებას
                command.CommandText = "UPDATE CAR_INFO SET " +
                    String.Join(',', queryAsm) +
                    " WHERE CarID = @id";
                command.Parameters.AddWithValue(@"id", id);

                // ვუშვებთ ბრძანებას
                if (DatabaseConnector.ExecuteCommand(command))
                {
                    return new UpdationResult(true, UpdationResult.ResultCode.SUCCESS, affected);
                }
                else
                {
                    return new UpdationResult(false, UpdationResult.ResultCode.ID_NOT_FOUND);
                }
            }

        }
    }
}
