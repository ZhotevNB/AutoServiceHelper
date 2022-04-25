using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Core.Models.Issues;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceHelper.Controllers
{
    public class CarsController : BaseController
    {
        private readonly ICarService carServices;
        public readonly IInformationServices infoServices;
        private readonly UserManager<IdentityUser> userManager;

        public CarsController(ICarService _carServices, 
            UserManager<IdentityUser> _userManager,
            IInformationServices _infoServices)
        {
            infoServices = _infoServices;
            carServices = _carServices;
            userManager = _userManager;
        }

        public IActionResult AddCar() => View();

        [HttpPost]
        public async Task<IActionResult> AddCar(AddCarFormModel car)
        {
            var user = await userManager.GetUserAsync(User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            if (!ModelState.IsValid)
            {
                return View();
            }
            carServices.AddCar(car, currentUserId);

            return RedirectToAction("MyCars");



        }

        public async Task<IActionResult> MyCars()
        {
            var user = await userManager.GetUserAsync(User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            var model = await carServices.AllCars(currentUserId);

            return View(model);
        }

        public async Task<IActionResult>Services(string offerId)
        {
            var issueId = await infoServices.GetIssueIdByOfferId(offerId);
            var model = await infoServices.GetServicesForOffer(offerId);
            ViewBag.issueId = issueId;
            return View(model);
        }

        public async Task<IActionResult>CarParts(string serviceId, string offerId)
        {
            var result = await infoServices.GetPartsForService(serviceId);
            ViewBag.offerId = offerId;          

            return View(result);
        }

        public IActionResult Issues(string carId)
        {
            var model = carServices.ViewIssues(carId);

            return View(model);
        }

        public async Task<IActionResult>CarOffer(string issueId)
        {
            var model = await carServices.ViewOffers(issueId);
                var carId = await carServices.GetCarIdByIssueId(issueId);
            ViewBag.CarId = carId.ToString();

            return View(model);
        }

        public async Task<IActionResult> AcceptOffer(string offerId,string issueId)
        {
            var result = await carServices.OrderOffer(offerId, issueId);

            if (result != null)
            {
                ViewData["ErrorMessage"] = result;
            }

            ViewData["SuccessMessage"] = "Order Successfull";
            return RedirectToAction("MyCars",ViewData);
        }
        public IActionResult AddIssue(string id)
        {
            var model = carServices.GetIssueTypes();
            var Issue = new AddIssueFormModel()
            {
                ListTypes = model
            };
            return View(Issue);
        }

        [HttpPost]
        public async Task<IActionResult> AddIssue(AddIssueFormModel carIssue,string carId)
        {
            var user = await userManager.GetUserAsync(User);
            var userId = await userManager.GetUserIdAsync(user);

            var result=carServices.AddIssue(carIssue,carId, userId);
            if (result== "Invalid Operation")
            {
                //must hapand somting to inform user the operation was not succsesful
                var model = carServices.GetIssueTypes();
                var Issue = new AddIssueFormModel()
                {
                    ListTypes = model
                };
                return View(Issue);
            }
            return RedirectToAction("MyCars");
        }

        public IActionResult FixIssue(string issueId)
        {
            carServices.FixIssue(issueId);
            return RedirectToAction("MyCars");
        }

        public async Task<IActionResult> Orders(string carId)
        {
            
                var model = await carServices.GetMyOrders(carId);
            
            return View(model);
        }
    }
}
