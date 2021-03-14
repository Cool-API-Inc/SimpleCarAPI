using CarAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

/* კლასში მოცემულია ზოგიერთი მოწოდებული პარამეტრის დასამუშავებლად საჭირო სტატიკური ფუნქციები */

namespace CarAPI
{
    public static class DataProcessor
    {
        // ტექსტი წყალნიშნისთვის
        static string watermarkText = "Cool API Inc.";

        // ფუნქციით ატვირთულ ფოტოს ვამუშავებთ და ვაკოპირებთ ლოკალურ ფოლდერში (სადაც წვდომადი იქნება ImageHandler-ის საშუალებით) 
        public static string UploadImage(IFormFile img, string url)
        {
            // ვაგენერირებთ უნიკალურ ფაილის სახელს სურათისთვის, შესაბამის მისამართს და ლინკს გარე წვდომისთვის
            string uname = Guid.NewGuid().ToString() + ".jpg";
            string outPath = ImageHandler.UploadFolderPath + uname;
            string uploadUrl = "https://" + url + "/" + ImageHandler.UploadFolderUrl + uname;

            // ვკითხულობთ ფოტოს BMP ფორმატით (ასე უფრო მარტივი დასამუშავებელია)
            Stream tmpStream = img.OpenReadStream();
            System.Drawing.Image bmp = Bitmap.FromStream(tmpStream);
            tmpStream.Close();

            // ვამატებთ კომპანიის წყალნიშანს ფოტოზე
            Font font = new Font("Arial", 25, FontStyle.Italic, GraphicsUnit.Pixel);
            Color color = Color.FromArgb(128, 0, 255, 0);
            Point atpoint = new Point(bmp.Width / 2, bmp.Height / 2);
            SolidBrush brush = new SolidBrush(color);
            Graphics graphics = Graphics.FromImage(bmp);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            graphics.DrawString(watermarkText, font, brush, atpoint, sf);
            graphics.Dispose();

            // დამუშავებული ფოტო გადაგვყავს JPEG ფორმატში და ვინახავთ შესაბამის ფოლდერში
            MemoryStream m = new MemoryStream();
            bmp.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] convertedToBytes = m.ToArray();
            File.WriteAllBytes(outPath,convertedToBytes);

            // ვაბრუნებთ უკვე ატვირთული ფოტოს ლინკს
            return uploadUrl;
        }



        // API ლინკები ევროსა და დოლარის კურსის წასაკითხად
        static string Usd2GelUrl = "https://free.currconv.com/api/v7/convert?q=USD_GEL&compact=ultra&apiKey=600597cea079782d1b16";
        static string Eur2GelUrl = "https://free.currconv.com/api/v7/convert?q=EUR_GEL&compact=ultra&apiKey=600597cea079782d1b16";


        // ფუნქცია დანართი API-ს დახმარებით ადგენს მოწოდებული ვალუტის გაცვლით კურსს და გადაგვყავს ღირებლება ლარში 
        public static double GetCostGel(double cost, string curr)
        {
            if (curr == "USD")
            {
                var result = JsonConvert.DeserializeObject<Dictionary<string, double>>((new WebClient()).DownloadString(Usd2GelUrl));
                return cost * result["USD_GEL"];
            }
            else if (curr == "EUR")
            {
                var result = JsonConvert.DeserializeObject<Dictionary<string, double>>((new WebClient()).DownloadString(Eur2GelUrl));
                return cost * result["EUR_GEL"];
            }
            else  // სხვა შემთხვევაში ვგულისხმობთ რომ ვალუტა ლარია
            {
                return cost;
            }
        }
    }
}
