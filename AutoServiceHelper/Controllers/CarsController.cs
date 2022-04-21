﻿using AutoServiceHelper.Core.Contracts;
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
            var carId = await infoServices.GetCarIdByOfferId(offerId);
            var model = await infoServices.GetServicesForOffer(offerId);
            ViewBag.CarId = carId;
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

        public async Task<IActionResult>CarOffer(string carId)
        {
            var model = await carServices.ViewOffers(carId);
            ViewBag.CarId = carId;

            return View(model);
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

        public IActionResult Orders(string id)
        {
            
                
            
            return View();
        }
    }
}
