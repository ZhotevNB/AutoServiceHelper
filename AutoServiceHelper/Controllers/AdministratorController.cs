using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Users;
using AutoServiceHelper.Infrastructure.Data.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceHelper.Controllers
{
    [Authorize(Roles = UsersConstants.Rolles.Administrator)]
    public class AdministratorController : BaseController
    {       
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserService userService;


        public AdministratorController(UserManager<IdentityUser> _userManager,
            IUserService _userService)
        {
          
            userManager = _userManager;
            userService = _userService;
        }

      
        public async Task<ActionResult> RoleManage()
        {
            var users = await userService.GetUsers();

            return View(users);

        }
      
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

        [HttpPost]
        public async Task<ActionResult> RoleChange(UserChangeRollViewModel model)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(m => m.Id == model.Id);

            var isInMechanicRolle = await userManager.IsInRoleAsync(await userService.GetUserById(model.Id), "Mechanic");
            var isInManagerRolle = await userManager.IsInRoleAsync(await userService.GetUserById(model.Id), "Manager");

            if (model.WantToBeManager != isInManagerRolle)
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

    }
}
