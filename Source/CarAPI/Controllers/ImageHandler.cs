using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

/* მოცემული კონტროლერის საშუალებით შეგვიძლია მივწვდეთ ატვირთულ ფოტოებს */

namespace CarAPI.Controllers
{
    [Route("IMG/")]
    [ApiController]
    public class ImageHandler : ControllerBase
    {
        public static string UploadFolderPath = @".\Upload\";
        public static string UploadFolderUrl = @"IMG/";

        [HttpGet("{imgFile}")]
        public ActionResult Get(string imgFile)
        {
            // თავს ვიზღვევთ რომ სახელი არ შეიცავდეს Directory Separator - ს 
            imgFile = Path.GetFileName(imgFile);

            // ვკითხულობთ ფაილის მთლიან კონტენტს და გარდავქმნით ბაიტების მასივად
            FileStream tmpStream = new FileStream(UploadFolderPath + imgFile, FileMode.Open);
            MemoryStream memStream = new MemoryStream();
            tmpStream.CopyTo(memStream);
            tmpStream.Close();
            byte[] cont = memStream.ToArray();
            memStream.Close();

            // რადგან წყალნიშნის დადებისას ყველა ფოტოს JPEG ფორმატით ვინახავთ ტიპის განსაზღვრა აღარ გვჭირდება
            return base.File(cont, "image/jpeg");
        }
    }
}
