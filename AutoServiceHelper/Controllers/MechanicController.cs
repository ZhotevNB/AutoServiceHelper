using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Mechanic;
using AutoServiceHelper.Infrastructure.Data.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoServiceHelper.Controllers
{
    [Authorize(Roles = UsersConstants.Rolles.Mechanic)]
    public class MechanicController : BaseController
    {
        private readonly IMechanicServices mechanicServices;
        private readonly UserManager<IdentityUser> userManage;

        public MechanicController(IMechanicServices _mechanicServices,
            UserManager<IdentityUser> _userManage)
        {
            mechanicServices = _mechanicServices;
            userManage = _userManage;
        }

        public async Task<IActionResult> MechanicActivities()
        {
            var userId = await GetUserId();
            var model = await mechanicServices.GetAllActivities();
            var mechanicActivitys = await mechanicServices.GetMchanicActivities(userId);


            var res = model.Select(a => new SelectListItem()
            {
                Text = a.ActivityName.ToString(),
                Value = a.Id.ToString(),
                Selected = mechanicActivitys.ActivityIds.Any(x => x == a.Id)
            });
            ViewBag.MechanicActivitys = res;

            return View(mechanicActivitys);
        }
        [HttpPost]
        public async Task<IActionResult> MechanicActivities(MechanicActivitiesModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid Data";
            }
            var msg = mechanicServices.AddMechanicActivities(model);

            if (msg != null)
            {
                ViewData["ErrorMessage"] = msg;
                return RedirectToPage("/");
            }

            return View();
        }


        public async Task<IActionResult> MechanicOrder()
        {
            var userId = await GetUserId();
            var result = await mechanicServices.GetMyOrders(userId);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> MechanicOrder(string orderId)
        {
            var userId = await GetUserId();
            var msg = await mechanicServices.CompleteOrder(orderId);
            if (msg == "Error")
            {
                ViewData["ErrorMessage"] = msg;
            }
            else
            {
                ViewData["SuccessMessage"] = msg;
            }
            var result = await mechanicServices.GetMyOrders(userId);
            return View(result);
        }

        public async Task<IActionResult> FreeOrders()
        {
            var userId = await GetUserId();
            var result = await mechanicServices.GetAllOrders(userId);            

            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> FreeOrders(string orderId)
        {
            var userId = await GetUserId();
            var msg = await mechanicServices.PreservingOrders(userId,orderId);

            if (msg=="Error")
            {
                ViewData["ErrorMessage"] = msg;
            }
            else
            {
                ViewData["SuccessMessage"] = msg;
            }

            var result = await mechanicServices.GetAllOrders(userId);

            return View(result);
        }
        private async Task<string> GetUserId()
        {
            var user = await userManage.GetUserAsync(User);
            var currentUserId = await userManage.GetUserIdAsync(user);

            return currentUserId;
        }
    }
}
