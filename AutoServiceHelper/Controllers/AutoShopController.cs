using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Constants;
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
        private readonly IInformationServices infoServices;

        public AutoShopController(UserManager<IdentityUser> _userManager,
            IAutoShopServices _shopService,
            IInformationServices _infoServices)
        {
            userManager = _userManager;
            shopServices = _shopService;
            infoServices = _infoServices;

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

        public async Task<IActionResult> AddPart(Guid serviceId)
        {
            ViewBag.serviceId = serviceId;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddPart(AddPartViewModel model)
        {
            ViewBag.serviceId = model.ServiceId;
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Indvalid input";
            }
            else
            {
                var result = await shopServices.AddPartToService(model);

                if (result!=null)
                {
                    ViewData["ErrorMessage"] = result;
                }
                else
                {
                    ViewData["SuccessMessage"] = "Part added";
                }
                model = null;
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
            var temp = await shopServices.GetShopID(id);

            model.ShopId = temp.ToString();
            await shopServices.AddOffer(model);

            return RedirectToAction("IssuesList");
        }

        public async Task<IActionResult> AddService(string offerId)
        {
            ViewBag.OfferId = offerId;
            ViewBag.Types = await shopServices.GetShopTypes(await GetUserId());
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddService(AddShopServiceViewModel model)
        {
                ViewBag.OfferId = model.offerId;
                ViewBag.Types = await shopServices.GetShopTypes(await GetUserId());
            if (!ModelState.IsValid)
            {
                return View();
               
            }

            var userId = await GetUserId();
            var shopId = await shopServices.GetShopID(userId);
            var msg = await shopServices.AddServiceToOffer(model, shopId);

            if (msg != null)
            {
                ViewData["ErrorMessage"] = msg;                
                return View();

            }

            ViewData["SuccessMessage"] = "Service Added";           
            return View();
        }

        public async Task<IActionResult> RemoveService(string serviceId)
        {
            var result = await shopServices.RemoveServiceFromOffer(serviceId);


            if (result != null)
            {
                ViewData["ErrorMessage"] = result;
            }

            return RedirectToAction("ShopOffers");
        }
        public async Task<IActionResult> RemovePart(string partId)
        {
              var result = await shopServices.RemovePartFromService(partId);


            if (result != null)
            {
                ViewData["ErrorMessage"] = result;
            }

            return RedirectToAction("ShopOffers");
        }

        public async Task<IActionResult> ShopOffers()
        {
            var userId = await GetUserId();
            var shopId = await shopServices.GetShopID(userId);

            var result = await shopServices.GetOffers(shopId.ToString());

            return View(result);
        }

        public async Task<IActionResult> ServiceParts(string serviceId)
        {
            var result= await infoServices.GetPartsForService(serviceId);
            ViewBag.offerId = await shopServices.GetOfferIdByServiceId(serviceId);
            ViewBag.ServiceId = Guid.Parse(serviceId);

            return View(result);
        }
        private async Task<string> GetUserId()
        {
            var user = await userManager.GetUserAsync(User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            return currentUserId;
        }

        public async Task<IActionResult> Services(string offerId)
        {
            var model = await infoServices.GetServicesForOffer(offerId);
            ViewBag.OfferId = offerId;
            if (model == null)
            {

                return View();
            }


            return View(model);
        }

    }
}
