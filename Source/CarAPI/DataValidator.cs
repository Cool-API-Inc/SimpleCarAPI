using CarAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

/* კლასში მოცემულია სტატიკური ფუნქციები მიღებული პარამეტრების ვალიდურობის შესამოწმებლად */

namespace CarAPI
{
    public static class DataValidator
    {
        // დასაშვები ბრენდები
        private static string[] ValidBrands = new string[] { "Audi", "BMW", "Mercedes", "Toyota", "Mitsubishi" };

        // ფუნქცია ამოწმებს ვალიდურია თუ არა მოწოდეული ბრენდი
        public static bool ValidateBrand(string brand)
        {
            return ValidBrands.Contains(brand);
        }

        // ფუნქცია ამოწმებს ვალიდურია თუ არა მოწოდებული გამოშვების წელი
        public static bool ValidateYear(int? year)
        {
            return (year >= 1900 && year <= DateTime.Now.Year);
        }

        //  ფუნქცია ამოწმებს ვალიდურია თუ არა მოწოდებული მანქანის აღწერა
        public static bool ValidateDescription(string desc)
        {
            // დარეზერვებულია სამომავლო ვცლილებისთვის..
            return true;
        }

        // ფუნქცია ამოწმებს ვალიდურია თუ არა მოწოდებული ფოტო (ასევე ვბლოკავთ მცირე რეზოლუციის ფოტოებს)
        public static bool ValidateImage(IFormFile img)
        {
            try
            {
                Stream stream = img.OpenReadStream();
                var temp = System.Drawing.Image.FromStream(stream);
                stream.Close();
                if (temp.Width > 100 && temp.Height > 100)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ფუნქცია ამოწმებს ვალიდურია თუ არა მოწოდებული მახასიათებლების ნამრავლი
        public static bool ValidateFeatures(int? features)
        {
            return (Car.FeaturesToList(features.Value) != null);
        }

        // ფუნქცია ამოწმებს ვალიდურია თუ არა მოწოდებული ფასი (ზედა ზღვარი პირობითადაა დაწესებული)
        public static bool ValidateCost(double? cost)
        {
            double icost = cost.Value;
            return (icost > 0 && icost < 1000000000);
        }

        // დასაშვები ვალუტები
        private static string[] ValidCurrencies = new string[] { "GEL", "EUR", "USD" };

        // ფუნქცია ამოწმებს ვალიდურია თუ არა მოწოდებული ვალუტა
        public static bool ValidateCurrency(string currency)
        {
            return ValidCurrencies.Contains(currency);
        }

    }
}
