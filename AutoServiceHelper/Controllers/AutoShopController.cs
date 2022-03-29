using AutoServiceHelper.Core.Contracts;
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

        private async Task<string> GetUserId()
        {
            var user = await userManager.GetUserAsync(User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            return currentUserId;
        }
       
    }
}
