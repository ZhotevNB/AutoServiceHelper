using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceHelper.Controllers
{
   // [Authorize(Roles = UsersConstants.Rolles.Manager)]
    public class AutoShopController : BaseController
    {
     
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAutoShopServices shopServices;

        public AutoShopController(UserManager<IdentityUser> _userManager,
            IAutoShopServices _shopService)
        {
            userManager = _userManager;
            shopServices = _shopService;
          
        }

        public async Task<IActionResult> IssuesList()
        {
            var id = GetUserId().Result;

            var model=shopServices.GetIssues(id).Result;

            return View(model);
        }
        public async Task<IActionResult> AddOffer(Guid issueId)
        {
            AddOfferViewModel offer = new AddOfferViewModel();
            offer.IssueId = issueId;

             return View(offer);
        }

        [HttpPost]
        public async Task<IActionResult> AddOffer(AddOfferViewModel model)
        {
            var id = GetUserId().Result;

            if (id == null)
            {
                ViewData["ErrorMessage"] = "You are not authorize";
                return View();
            }
            model.ShopId = shopServices.GetShopID(id).Result;
            shopServices.AddOffer(model);

            return RedirectToAction("/IssuesList");
        }

        private async Task<string> GetUserId()
        {
            var user = await userManager.GetUserAsync(User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            return currentUserId;
        }
       
    }
}
