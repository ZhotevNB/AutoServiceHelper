using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.AutoShop;
using AutoServiceHelper.Infrastructure.Data.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceHelper.Controllers
{
    [Authorize(Roles = UsersConstants.Rolles.Manager)]
    public class ShopManagerController : BaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAutoShopServices shopServices;

        public ShopManagerController(UserManager<IdentityUser> _userManager,
            IAutoShopServices _shopService)
        {
            userManager = _userManager;
            shopServices = _shopService;

        }

        public async Task<IActionResult> ShopMechanics()
        {
            var shopId = await shopServices.GetShopID(await GetUserId());
            var result = await shopServices.GetPosibleMechanicsList(shopId);

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

            var result = await shopServices.AddContactInfo(model, userId);

            if (result != null)
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
