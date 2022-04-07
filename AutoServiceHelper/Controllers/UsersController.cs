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


            userService.ChangeUserInfo(model.UserId, model);

            return Redirect("/");
        }

        [Authorize(Roles = UsersConstants.Rolles.Administrator)]
        public async Task<ActionResult> RoleManage()
        {
            var users = await userService.GetUsers();
                      
            return View(users);

        }

        [Authorize(Roles =UsersConstants.Rolles.Administrator)]
        public async Task<ActionResult> RoleChange(string userId)
        {

            var modelList = await userService.GetUsers();
            var model = modelList.First(m => m.Id == userId);

            var isInMechanicRolle = await userManager.IsInRoleAsync(await userService.GetUserById(model.Id), "Mechanic");
            var isInManagerRolle = await userManager.IsInRoleAsync(await userService.GetUserById(model.Id), "Manager");


            {
                ViewBag.Model = model;
                ViewBag.MechanicRole = isInMechanicRolle;
                ViewBag.ManagerRole = isInManagerRolle;

            }
            return View();

        }

        [Authorize(Roles = UsersConstants.Rolles.Administrator)]
        [HttpPost]
        public async Task<ActionResult> RoleChange(UserChangeRollViewModel model)
        {
           var user= await userManager.Users.FirstOrDefaultAsync(m => m.Id == model.Id);

            var isInMechanicRolle = await userManager.IsInRoleAsync(await userService.GetUserById(model.Id), "Mechanic");
            var isInManagerRolle = await userManager.IsInRoleAsync(await userService.GetUserById(model.Id), "Manager");

            if (model.WantToBeManager!= isInManagerRolle)
            {
                if (model.WantToBeManager)
                {
                   await userManager.AddToRoleAsync(user, UsersConstants.Rolles.Manager);
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(user, UsersConstants.Rolles.Manager);

                }
            }
            if (model.WantToBeMechanic != isInMechanicRolle)
            {
                if (model.WantToBeMechanic)
                {
                    await userManager.AddToRoleAsync(user, UsersConstants.Rolles.Mechanic);
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(user, UsersConstants.Rolles.Mechanic);

                }
            }


            return RedirectToAction("RoleManage");
           
        }

        [Authorize(Roles = UsersConstants.Rolles.Administrator)]
        public async Task<IActionResult> CreateRole()
        {
            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Mechanic"
            //});
            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Manager"
            //});

            return Ok();
        }

        public async Task<IActionResult> ContactInfo()
        {
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