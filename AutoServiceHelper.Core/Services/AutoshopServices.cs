using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.AutoShop;
using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceHelper.Core.Services
{
    public class AutoshopServices : IAutoShopServices
    {
        private readonly IRepository repo;
        private readonly UserManager<IdentityUser> userManager;


        public AutoshopServices(IRepository _repo,
            UserManager<IdentityUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;

        }

        public Task<string> AddMechanicToOrder(Guid id)
        {
            throw new NotImplementedException();
        }

        //Inplemented
        public async Task AddOffer(AddOfferViewModel model)
        {
            var offer = new Offer()
            {
                ShopId = model.ShopId,
                IssueId = model.IssueId,
                AdditionalInfo = model.AdditionalInfo
            };

            repo.Add(offer);
            repo.SaveChanges();

        }

        public Task<string> AddPartToService(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<string> AddServiceToOffer(Guid id)
        {
            throw new NotImplementedException();
        }
              
        //Implemented
        public async Task<IEnumerable<ViewIssueModel>> GetIssues(string userId)
        {
            var types = await GetShopTypes(userId);
            var issues = new List<ViewIssueModel>();

            foreach (var type in types)
            {
                var result = repo
                    .All<Issue>()
                    .Where(x => x.Type == type && x.isFixed == false)
                    .Select(x => new ViewIssueModel
                    {
                        Id = x.Id,
                        CarId = x.CarId,
                        CarOdometer = x.CarOdometer,
                        Description = x.Description,
                        SubmitionDate = x.SubmitionDate,
                        Status = x.Status,
                        SubmitetByUserId = x.SubmitetByUserId,
                        IsFixed = x.isFixed,
                        Type = x.Type

                    }).ToList();
                issues.AddRange(result);
            }

            foreach (var issue in issues)
            {
                issue.Car = repo
                    .All<Car>()
                    .Where(x => x.Id == issue.CarId)
                    .Select(c => new CarViewModel
                    {
                        Id = c.Id,
                        Color = c.Color,
                        Vin = c.Vin,
                        Manifacture = c.Manifacture,
                        Model = c.Model,
                        Year = c.Year
                    }).First();
            }
            return issues;
        }

        //Implemented
        public async Task<IEnumerable<OfferViewModel>> GetOffers(string shopId)
        {
            return repo.All<Offer>()
                .Where(x => x.ShopId == shopId)
                .Select(x => new OfferViewModel
                {
                    AdditionalInfo = x.AdditionalInfo,
                    IssueId = x.IssueId,
                    SubmitionDate = x.SubmitionDate,
                    TotalPrice = x.TotalPrice,
                    ShopId = x.ShopId
                }).ToList();

        }

        //implemented
        public async Task<Guid> GetShopID(string userId)
        {
            var re = await repo.All<AutoShop>()
                .Where(x => x.ManegerId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return re;
        }

        public Task<IEnumerable<ViewIssueModel>> GetShopOrders(string shopId)
        {
            throw new NotImplementedException();
        }

        //Implemented
        public async Task<IEnumerable<TypeActivity>> GetShopTypes(string userId)
        {

            var types = new List<TypeActivity>();

            var result = await repo.All<AutoShop>()
               .Where(x => x.ManegerId == userId)
               .Select(x => new
               {
                   activities = x.Activities.ToList().Select(c => c.Activity.ActivityName)
               }).Select(x => x.activities).ToListAsync();

            types.AddRange(result[0]);

            return types;


        }

        //Implemented
        public async Task<IEnumerable<TypeActivity>> GetTypesActivity()
        {

            var types = await repo.All<Activity>()
                .Select(x => x.ActivityName)
                .ToListAsync();

            return types;

        }

        //Implemented to be sent in manager services
        public async Task<AutoShopInfoModel> GetShopInfo(string managerId)
        {
            var result = await repo.All<AutoShop>()
                .Where(x => x.ManegerId == managerId)
                .Select(x => new AutoShopInfoModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PricePerHour = x.PricePerHour,
                    ShopContactInfo = new ShopContactInfoModel
                    {
                        Id = x.ContactInfo.Id,
                        PhoneNumber = x.ContactInfo.PhoneNumber,
                        City = x.ContactInfo.City,
                        Country = x.ContactInfo.Country,
                        Address = x.ContactInfo.Address,
                        Email = x.ContactInfo.Email,
                        AdditionalInfo = x.ContactInfo.AdditionalInfo
                    }

                })
                .FirstOrDefaultAsync();

            return result;
        }

      
        
    }
}
