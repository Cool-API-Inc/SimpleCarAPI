using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/* ფაილში მოცემულია მანქანის მოდელი და მასთან სამუშაოდ საჭირო ზოგიერთი ფუნქცია */

namespace CarAPI.Models
{
    public class Car
    {
        public string ID { get; set; }
        public string Brand { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public int Features { get; set; }
        public double Cost { get; set; }
        public string Currency { get; set; }
        public double CostInGel { get; set; }

        public Car() { }
        public Car(string ID, string Brand, int Year, string Description, string ImgUrl, int Features,
            double Cost, string Currency, double CostInGel)
        {
            this.ID = ID;
            this.Brand = Brand;
            this.Year = Year;
            this.Description = Description;
            this.ImgUrl = ImgUrl;
            this.Features = Features;
            this.Cost = Cost;
            this.Currency = Currency;
            this.CostInGel = CostInGel;
        }


        // მანქანის მახასიათებლები
        public enum Feature
        {
            /* მახასიათებლებთან მუშაობის გასამარტივებლად თითოეულს მარტივი რიცხვი რიცხვი შევუსაბამეთ ,
             * (შედეგად მახასიათებლები შეგვიძლია ერთი რიცხვის, მათი ნამრავლის სახით დავიმახსოვროთ)
             * გაშიფრვისას კი ზუსტად დავადგენთ რომელი მახასიათებლების "ნამრავლი" შევინახეთ */
            ABS = 2,
            ELECTRIC_WINDOWS = 3,
            SUNROOF = 5,
            BLUETOOTH = 7,
            BURGLAR_ALARM = 11,
            PARKING_CONTROL = 13,
            NAVIGATION = 17,
            ON_BOARD_COMPUTER = 19,
            MULTI_STEERING_WHEEL = 23
        }


        // ფუნქცია მახასიათებლების ნამრავლს გარდაქმნის კოლექციად
        public static List<Feature> FeaturesToList(int features)
        {
            List<Feature> result = new List<Feature>();

            /* ვამოწმებთ ნამრავლს ყველა მარტივ რიცხვზე, თუ გაყოფადი აღმოჩნდება სიაში ვამატებთ შესაბამის მახასიათებელს, 
             * რიცხვს კი ვყოფთ, რომ თავიდან ავირიდოთ დუპლიკატები */
            if (features % 2 == 0)
            {
                result.Add(Feature.ABS);
                features /= 2;
            }
            if (features % 3 == 0)
            {
                result.Add(Feature.ELECTRIC_WINDOWS);
                features /= 3;
            }
            if (features % 5 == 0)
            {
                result.Add(Feature.SUNROOF);
                features /= 5;
            }
            if (features % 7 == 0)
            {
                result.Add(Feature.BLUETOOTH);
                features /= 7;
            }
            if (features % 11 == 0)
            {
                result.Add(Feature.BURGLAR_ALARM);
                features /= 11;
            }
            if (features % 13 == 0)
            {
                result.Add(Feature.PARKING_CONTROL);
                features /= 13;
            }
            if (features % 17 == 0)
            {
                result.Add(Feature.NAVIGATION);
                features /= 17;
            }
            if (features % 19 == 0)
            {
                result.Add(Feature.ON_BOARD_COMPUTER);
                features /= 19;
            }
            if (features % 23 == 0)
            {
                result.Add(Feature.MULTI_STEERING_WHEEL);
                features /= 23;
            }

            // თუ ყველა შესაძლო გაყოფის შემდეგ 1-ს არ ვღებულობთ ვასკვნით რომ რიცხვი არასწორადაა შედგენილი
            if (features == 1)
                return result;
            else
                return null;

        }


        // ფუნქცია მახასიათებლების კოლექციას გარდაქმნის ნამრავლად
        public static int ListToFeatures(List<Feature> lst)
        {
            int result = 1;
            foreach (Feature item in (new HashSet<Feature>(lst))) // დავრწმუნდეთ რომ ელემენტები უნიკალურია
            {
                result *= (int)item;
            }
            return result;
        }

    }
}
