﻿using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.AutoShop;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceHelper.Controllers
{
    [Authorize(Roles = UsersConstants.Rolles.Manager)]
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
            var id = await GetUserId();

            var model = await shopServices.GetIssues(id);

            if (!ModelState.IsValid || model == null)
            {
                ViewData["ErrorMessage"] = "You are not authorize";
                return View();
            }

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
            var id = await GetUserId();

            if (id == null)
            {
                ViewData["ErrorMessage"] = "You are not authorize";
                return View();
            }
            model.ShopId = await shopServices.GetShopID(id);
            await shopServices.AddOffer(model);

            return RedirectToAction("IssuesList");
        }
        public async Task<IActionResult> ShopOffers()
        {
            var userId = await GetUserId();
            var shopId = await shopServices.GetShopID(userId);

            var result = await shopServices.GetOffers(shopId);

            return View(result);
        }
        public async Task<IActionResult> AutoShopInfo()
        {
            var userId = await GetUserId();
            var info = await shopServices.GetShopInfo(userId);
                      
          
            if (info != null)
            {
               return View(info);
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AutoShopInfo(AutoShopInfoModel model)
        {
            var userId = await GetUserId();


            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Unable to update info";
                return View();
            }

            var result=await shopServices.AddContactInfo(model,userId);

            if (result!=null)
            {
                ViewData["ErrorMessage"] = result;
            }

            return View();
        }


        private async Task<string> GetUserId()
        {
            var user = await userManager.GetUserAsync(User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            return currentUserId;
        }


    }
}
