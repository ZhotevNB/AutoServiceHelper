using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceHelper.Core.Services
{
    public class AutoshopServices : IAutoShopServices
    {
        private readonly IRepository repo;

        public AutoshopServices(IRepository _repo)
        {
            repo = _repo;
        }

        public Task<string> AddMechanicToOrder(Guid id)
        {
            throw new NotImplementedException();
        }

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

        public async Task<string> GetShopID(string id)
        {
            var re = repo.All<AutoShop>()
                .Where(x => x.ManegerId == id)
                .Select(x => x.Id)
                .FirstOrDefault().ToString();

            return re;
        }

        public Task<IEnumerable<ViewIssueModel>> GetShopOrders(string shopId)
        {
            throw new NotImplementedException();
        }
        private async Task<IEnumerable<TypeActivity>> GetShopTypes(string userId)
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
    }
}
