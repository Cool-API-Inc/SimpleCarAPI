using CarAPI.Models;
using CarAPI.ResultTypes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

/* კლასი ემსახურება მანქანის მახასიათებლების წაკითხვას */

namespace CarAPI.CrudOperations
{
    public static class PropertySelection
    {
        public static PropertySelectionResult GetProperties(string id)
        {
            if (id == null)
                return new PropertySelectionResult(false);

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
                    return new PropertySelectionResult(true, rFeat, rList);
                }
                else
                {
                    return new PropertySelectionResult(false);
                }
            }
            catch (Exception)
            {
                return new PropertySelectionResult(false);
            }

        }
    }
}
