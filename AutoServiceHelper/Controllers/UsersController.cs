using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Users;
using AutoServiceHelper.Infrastructure.Data.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceHelper.Controllers
{
    public class UsersController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserService userService;


        public UsersController(RoleManager<IdentityRole> _roleManager,
            UserManager<IdentityUser> _userManager,
            IUserService _userService)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            userService = _userService;
        }

        public async Task<IActionResult> Settings()
        {

            var userId = await GetUserId();

            var model = await userService.GetUserInfo(userId);

            if (model == null)
            {
                return View();
            }


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(UsersSetingsFormModel model)
        {

            model.UserId = await GetUserId();


            var result= await userService.ChangeUserInfo(model.UserId, model);

            if (result!=null)
            {
                ViewData["ErrorMessage"] = result;
                return View();
            }

            return Redirect("/");
        }

        public async Task<IActionResult> ContactInfo()
        {
            var userId=await GetUserId();
            var model= await userService.GetUserContactInfo(userId);

            if (model==null)
            {
              return View();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ContactInfo(UserContactInfoModel model)
        {
            var userId=await GetUserId();
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid update info";
                return View(model);
            }

            var result = await userService.ChangeUserContactInfo(userId, model);

            if (result!=null)
            {
                ViewData["ErrorMessage"] = result;
            }

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