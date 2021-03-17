using CarAPI.Models;
using CarAPI.ResultTypes;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

/* კლასი ემსახურება მანქანის მახასიათებლების ჩამონათვალში ცვლილებების განხორციელებას */

namespace CarAPI.CrudOperations
{
    public class PropertyUpdation
    {
        public static PropertyUpdationResult SetProperties(string id, bool? abs, bool? ewindow, bool? sunroof, bool? bluetooth,
            bool? alarm, bool? parkingCtrl, bool? navigation, bool? boardComputer, bool? mltwheel)
        {
            // სიმარტივისთვის ვიყენებთ წამკითხველ ფუნქციას რომ მოვძებნოთ შესაბამისი ID-ს მქონე ჩანაწერი
            PropertySelectionResult result = PropertySelection.GetProperties(id);
            int affected = 0;

            if (!result.Success)
                return new PropertyUpdationResult(false);

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
                return new PropertyUpdationResult(true, affected);
            }
            else
            {
                return new PropertyUpdationResult(false);
            }

        }
    }
}
