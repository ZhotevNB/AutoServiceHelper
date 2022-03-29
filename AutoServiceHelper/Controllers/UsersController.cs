using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Users;
using AutoServiceHelper.Infrastructure.Data.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            var user = await userManager.GetUserAsync(User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            var model=userService.GetUserInfo(currentUserId);

            if (model==null)
            {
                return View();
            }


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(UsersSetingsFormModel model)
        {
            var user = await userManager.GetUserAsync(User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            model.UserId = currentUserId;

            //if (!ModelState.IsValid)
            //{
            //    ViewData["ErrorMessage"] = "Възникна грешка";
            //    return View();
            //}

            userService.ChangeUserInfo(model.UserId, model);

            return Redirect("/");
        }


        [Authorize(Roles = UsersConstants.Rolles.Administrator)]

        public async Task<ActionResult> ManageUsers()
        {
            return View();

        }

        public async Task<IActionResult> CreateRolle()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Administrator"
            });
            return Ok();
        }
    }
}