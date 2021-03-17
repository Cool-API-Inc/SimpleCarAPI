using CarAPI.CrudOperations;
using CarAPI.ResultTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/* კლასი წარმოადგენს ძირითად Handler-ს რათა გამოიძახოს შესაბამისი CRUD ფუნქციები */

namespace CarAPI.Controllers
{
    [Route("API/")]
    [ApiController]
    public class MainHandler : ControllerBase
    {
        // ფუნქცია ამატებს მანქანას მონაცემთა ბაზაში
        [HttpPost("RegisterCar")]
        public RegistrationResult RegisterCar(string brand, int? year, string desc, IFormFile img, int? features,
            double? cost, string curr) => 
            Registration.RegisterCar(Request, brand, year, desc, img, features, cost, curr);

        // ფუნქცია მანქანის მითითებულ ველებს ცვლის ახალი მნიშვნელობებით
        [HttpPost("UpdateCar")]
        public UpdationResult UpdateCar(string id, string brand, int? year, string desc, IFormFile img, int? features,
            double? cost, string curr) => 
            Updation.UpdateCar(Request, id, brand, year, desc, img, features, cost, curr);

        // ფუნქცია მონაცემთა ბაზიდან იღებს მითითებულ მანქანას
        [HttpPost("DeleteCar")]
        public DeletionResult DeleteCar(string id) => Deletion.DeleteCar(id);

        // ფუნქცია აბრუნებს შესაბამისი ID-ს მქონე მანქანის, ან თუ მითითებული არაა ყველა მანქანის მონაცემებს
        [AcceptVerbs("GET", "POST", Route = "GetCar")]  // დასაშვებია წვდომა როგორც GET ისე POST მეთოდით
        public SelectionResult GetCar(string id) => Selection.GetCar(id);

        // ფუნქცია აბრუნებს შესაბამისი მანქანის მახასიათებლების ჩამონათვალს
        [HttpPost("GetProperties")]
        public PropertySelectionResult GetProperties(string id) => PropertySelection.GetProperties(id);

        // ფუნქცია ცვლის შესაბამისი მანქანის მახასიათებლების ჩამონათვალს (შესაძლებელია როგორც ამოღება ისე დამატება)
        [HttpPost("SetProperties")]
        public PropertyUpdationResult SetProperties(string id, bool? abs, bool? ewindow, bool? sunroof, bool? bluetooth,
            bool? alarm, bool? parkingCtrl, bool? navigation, bool? boardComputer, bool? mltwheel) =>
            PropertyUpdation.SetProperties(id, abs, ewindow, sunroof, bluetooth, alarm, parkingCtrl, 
                navigation, boardComputer, mltwheel);


    }
}
