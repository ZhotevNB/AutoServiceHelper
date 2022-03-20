using AutoServiceHelper.Core.Models.Cars;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceHelper.Controllers
{
    public class CarsController : BaseController
    {
        public IActionResult AddCar()=> View();
        

        [HttpPost]
        public IActionResult AddCar(AddCarFormModel car)
        {
            
            return Redirect("/Cars/MyCars");
        }
        public IActionResult MyCars() => View();


     
        public IActionResult AddCarIssue(Guid id) => View();


        [HttpPost]
        public IActionResult AddIssue(string carIssue)
        {
            
            return View();
        }
    }
}
