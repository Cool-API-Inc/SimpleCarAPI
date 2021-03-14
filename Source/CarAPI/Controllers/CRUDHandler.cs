using CarAPI.Models;
using CarAPI.ResultTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

/* კონტროლერში მოცემულია ყველა CRUD ფუნქცია */

namespace CarAPI.Controllers
{
    [Route("API/")]
    [ApiController]
    public class CRUDHandler : ControllerBase
    {
        [HttpPost("RegisterCar")]
        public RegistrationResult RegisterCar(string brand, int? year, string desc, IFormFile img, int? features,
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

                // ვამოწმებთ ბრენდს
                if (brand != null)
                {
                    if (DataValidator.ValidateBrand(brand)) command.Parameters.AddWithValue("@brand", brand);
                    else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_BRAND);
                }
                else
                {
                    command.Parameters.AddWithValue("@brand", DBNull.Value);
                }

                // ვამოწმებთ გამოშვების წელს
                if (year != null)
                {
                    if (DataValidator.ValidateYear(year)) command.Parameters.AddWithValue("@year", year);
                    else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_YEAR);
                }
                else
                {
                    command.Parameters.AddWithValue("@year", DBNull.Value);
                }

                // ვამოწმებთ ტექსტურ აღწერას
                if (desc != null)
                {
                    if (DataValidator.ValidateDescription(desc)) command.Parameters.AddWithValue("@desc", desc);
                    else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_DESCRIPTION);
                }
                else
                {
                    command.Parameters.AddWithValue("@desc", DBNull.Value);
                }

                // ვამოწმებთ სურათს
                if (img != null)
                {
                    if (DataValidator.ValidateImage(img))
                        command.Parameters.AddWithValue("@url", DataProcessor.UploadImage(img,
                            HttpContext.Request.Host.Value));
                    else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_IMAGE);
                }
                else
                {
                    command.Parameters.AddWithValue("@url", DBNull.Value);
                }

                // ვამოწმებთ მახასიათებლებს
                if (features != null)
                {
                    if (DataValidator.ValidateFeatures(features)) command.Parameters.AddWithValue("@feat", features);
                    else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_FEATURES);
                }
                else
                {
                    command.Parameters.AddWithValue("@feat", DBNull.Value);
                }

                // ვამოწმებთ მოწოდებულ ღირებულებას
                if (cost != null)
                {
                    if (DataValidator.ValidateCost(cost)) command.Parameters.AddWithValue("@cost", cost);
                    else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_COST);
                }
                else
                {
                    command.Parameters.AddWithValue("@cost", DBNull.Value);
                }

                // ვამოწმებთ მოწოდებულ ვალუტას
                if (curr != null)
                {
                    if (DataValidator.ValidateCurrency(curr)) command.Parameters.AddWithValue("@curr", curr);
                    else return new RegistrationResult(false, RegistrationResult.ResultCode.INVALID_CURRENCY);
                }
                else
                {
                    command.Parameters.AddWithValue("@curr", DBNull.Value);
                }

                // ვითვლით ფასს ლარში
                if (curr != null && cost != null)
                {
                    command.Parameters.AddWithValue("@costGel", DataProcessor.GetCostGel(cost ?? default(double), curr));
                }
                else
                {
                    command.Parameters.AddWithValue("@costGel", DBNull.Value);
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

        [HttpPost("UpdateCar")]
        public UpdateResult UpdateCar(string id, string brand, int? year, string desc, IFormFile img, int? features,
            double? cost, string curr)
        {

            if (id == null)
            {
                return new UpdateResult(false, UpdateResult.ResultCode.ID_REQUIRED);
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
                        return new UpdateResult(false, UpdateResult.ResultCode.INVALID_BRAND);
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
                        return new UpdateResult(false, UpdateResult.ResultCode.INVALID_YEAR);
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
                        return new UpdateResult(false, UpdateResult.ResultCode.INVALID_DESCRIPTION);
                    }
                }
                if (img != null) // ვამოწმებთ მოწოდებულ ფოტოს
                {
                    if (DataValidator.ValidateImage(img))
                    {
                        queryAsm.Add("ImgUrl = @url");
                        command.Parameters.AddWithValue("@url",
                            DataProcessor.UploadImage(img, HttpContext.Request.Host.Value));
                        affected++;
                    }
                    else
                    {
                        return new UpdateResult(false, UpdateResult.ResultCode.INVALID_IMAGE);
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
                        return new UpdateResult(false, UpdateResult.ResultCode.INVALID_FEATURES);
                    }
                }

                // ვამოწმებთ მოწოდებულ ღირებულებას და ვალუტას (ცალ-ცალკე მათი ვალიდაცია უაზრო იქნებოდა)
                if (cost != null && curr != null) 
                {
                    if (!DataValidator.ValidateCost(cost))
                        return new UpdateResult(false, UpdateResult.ResultCode.INVALID_COST);
                    if (!DataValidator.ValidateCurrency(curr))
                        return new UpdateResult(false, UpdateResult.ResultCode.INVALID_CURRENCY);

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
                    return new UpdateResult(true, UpdateResult.ResultCode.SUCCESS, affected);
                }
                else
                {
                    return new UpdateResult(false, UpdateResult.ResultCode.ID_NOT_FOUND);
                }
            }

        }

        [HttpPost("DeleteCar")]
        public DeletionResult DeleteCar(string id)
        {
            if (id == null)
                return new DeletionResult(false);

            SqlCommand command = new SqlCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "DELETE FROM CAR_INFO WHERE CarID = @id";
            command.Parameters.AddWithValue("@id", id);

            if (DatabaseConnector.ExecuteCommand(command))
                return new DeletionResult(true);
            else
                return new DeletionResult(false);

        }

        [HttpPost("GetCar")]
        public GetResult GetCar(string id)
        {
            if (id != null)  // თუ id მითითებულია ვაბრუნებთ მის შესაბამის ჩანაწერს
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    
                    command.CommandText = "SELECT CarID, Brand, PYear, CarDesc, ImgUrl, Features, Cost, Currency, CostGel " +
                        "FROM CAR_INFO WHERE CarID = @id";
                    command.Parameters.AddWithValue("@id", id);

                    List<Car> cars = DatabaseConnector.ExecuteWithReader(command);

                    if (cars.Count > 0)
                        return new GetResult(true, cars);
                    else
                        return new GetResult(false);

                }
                catch (Exception)
                {
                    return new GetResult(false);
                }
            }
            else // წინააღმდეგ შემთხვევაში ვაბრუნებთ ყველა მნიშვნელობას
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT CarID, Brand, PYear, CarDesc, ImgUrl, Features, Cost, Currency, CostGel " +
                        "FROM CAR_INFO";
                    return new GetResult(true, DatabaseConnector.ExecuteWithReader(command));
                }
                catch (Exception)
                {
                    return new GetResult(false);
                }
            }
        }

        [HttpPost("GetProperties")]
        public GetPropertiesResult GetProperties(string id)
        {
            if (id == null)
                return new GetPropertiesResult(false);

            try
            {
                // ვაგენერირებთ ბრძანებას ბაზიდან ID-ს შესაბამისი ჩანაწერის მოსაძებნად
                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT CarID, Brand, PYear, CarDesc, ImgUrl, Features, Cost, Currency, CostGel " +
                    "FROM CAR_INFO WHERE CarID = @id";
                command.Parameters.AddWithValue("@id", id);
                List<Car> cars = DatabaseConnector.ExecuteWithReader(command);

                /* თუ მოიძებნება შესაბამისი ID-ს მქონე ჩანაწერი ვაბრუნებთ მის მახასიათებლების ნამრავლს კოლექციად 
                 * გარდაქმნილ მნიშვნელობასთან ერთად */
                if (cars.Count > 0) 
                {
                    int rFeat = cars[0].Features;
                    List<string> rList = Car.FeaturesToList(rFeat).Select(x => x.ToString()).ToList();
                    return new GetPropertiesResult(true, rFeat, rList);
                }
                else
                {
                    return new GetPropertiesResult(false);
                }
            }
            catch (Exception)
            {
                return new GetPropertiesResult(false);
            }

        }

        [HttpPost("SetProperties")]
        public SetPropertiesResult SetProperties(string id, bool? abs, bool? ewindow, bool? sunroof, bool? bluetooth,
            bool? alarm, bool? parkingCtrl, bool? navigation, bool? boardComputer, bool? mltwheel)
        {
            // სიმარტივისთვის ვიყენებთ წამკითხველ ფუნქციას რომ მოვძებნოთ შესაბამისი ID-ს მქონე ჩანაწერი
            GetPropertiesResult result = GetProperties(id);
            int affected = 0;

            if (!result.Success)
                return new SetPropertiesResult(false);

            // ამგვარად თავს ვიზღვევთ დუპლიკატი ჩანაწერებისგან
            var featSet = new HashSet<Car.Feature>(Car.FeaturesToList(result.Features.Value));

            /* სათითაოდ ვამოწმებთ მახასიათებლებს და არანულოვან შემთხვევაში მნიშვნელობიდან გამომდინარე ვშლით ან
             * ვამატებთ მას სიაში */

            if (abs != null) // ვამოწმებთ ABS-ის შესაბამის პარამეტრს
            {
                if (abs.Value)
                    featSet.Add(Car.Feature.ABS);
                else
                    featSet.Remove(Car.Feature.ABS);
                affected++;
            }
            if (ewindow != null) // ვამოწმებთ ელექტრონული შუშების ამწევის შესაბამის პარამეტრს
            {
                if (ewindow.Value)
                    featSet.Add(Car.Feature.ELECTRIC_WINDOWS);
                else
                    featSet.Remove(Car.Feature.ELECTRIC_WINDOWS);
                affected++;
            }
            if (sunroof != null) // ვამოწმებთ ლუქის შესაბამის პარამეტრს
            {
                if (sunroof.Value)
                    featSet.Add(Car.Feature.SUNROOF);
                else
                    featSet.Remove(Car.Feature.SUNROOF);
                affected++;
            }
            if (bluetooth != null) // ვამოწმებთ Bluetooth-ის შესაბამის პარამეტრს
            {
                if (bluetooth.Value)
                    featSet.Add(Car.Feature.BLUETOOTH);
                else
                    featSet.Remove(Car.Feature.BLUETOOTH);
                affected++;
            }
            if (alarm != null) // ვამოწმებთ სიგნალიზაციის შესაბამის პარამეტრს
            {
                if (alarm.Value)
                    featSet.Add(Car.Feature.BURGLAR_ALARM);
                else
                    featSet.Remove(Car.Feature.BURGLAR_ALARM);
                affected++;
            }
            if (parkingCtrl != null)  // ვამოწმებთ პარგინგკონტროლის შესაბამის პარამეტრს
            {
                if (parkingCtrl.Value)
                    featSet.Add(Car.Feature.PARKING_CONTROL);
                else
                    featSet.Remove(Car.Feature.PARKING_CONTROL);
                affected++;
            }
            if (navigation != null)  // ვამოწმებთ ნავიგაციის შესაბამის პარამეტრს
            {
                if (navigation.Value)
                    featSet.Add(Car.Feature.NAVIGATION);
                else
                    featSet.Remove(Car.Feature.NAVIGATION);
                affected++;
            }
            if (boardComputer != null) // ვამოწმებთ ბორდკომპიუტერის შესაბამის პარამეტრს
            {
                if (boardComputer.Value)
                    featSet.Add(Car.Feature.ON_BOARD_COMPUTER);
                else
                    featSet.Remove(Car.Feature.ON_BOARD_COMPUTER);
                affected++;
            }
            if (mltwheel != null)  // ვამოწმებთ მულტი საჭის შესაბამის პარამეტრს
            {
                if (mltwheel.Value)
                    featSet.Add(Car.Feature.MULTI_STEERING_WHEEL);
                else
                    featSet.Remove(Car.Feature.MULTI_STEERING_WHEEL);
                affected++;
            }

            // ვაგენერირებთ ბრძანებას ცვლილების ბაზაში შესატანად
            SqlCommand command = new SqlCommand();

            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "UPDATE CAR_INFO SET Features = @feat WHERE CarID = @id";

            command.Parameters.AddWithValue("@id", id);

            // ბაზაში შესანახად მნიშვნელობა კოლექციიდან ისევ ნამრავლში გადაგვყავს
            command.Parameters.AddWithValue("@feat", Car.ListToFeatures(featSet.ToList()));

            if (DatabaseConnector.ExecuteCommand(command))
            {
                return new SetPropertiesResult(true, affected);
            }
            else
            {
                return new SetPropertiesResult(false);
            }

        }

    }
}
