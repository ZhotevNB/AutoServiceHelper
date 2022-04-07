using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.AutoShop;
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

        public async Task<string> AddContactInfo(AutoShopInfoModel model, string managerId)
        {
            string result = null;

            var contactInfo = await repo.All<ContactInfo>()
                .Where(x => x.Id == model.ShopContactInfo.Id)
                .FirstOrDefaultAsync();

            if (contactInfo == null)
            {
                contactInfo = new ContactInfo();
                repo.Add(contactInfo);
            }

            contactInfo.Country = model.ShopContactInfo.Country;
            contactInfo.City = model.ShopContactInfo.City;
            contactInfo.Address = model.ShopContactInfo.Address;
            contactInfo.PhoneNumber = model.ShopContactInfo.PhoneNumber;
            contactInfo.Email = model.ShopContactInfo.Email;
            contactInfo.AdditionalInfo = model.ShopContactInfo.AdditionalInfo;


         

            var shopInfo = await repo.All<AutoShop>()
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (shopInfo == null)
            {
                shopInfo = new AutoShop();
                repo.Add(shopInfo);
            }
            shopInfo.Name = model.Name;
            shopInfo.PricePerHour = model.PricePerHour;
            shopInfo.ContactInfo = contactInfo;
            shopInfo.ContactInfoId = contactInfo.Id;
            shopInfo.ManegerId = managerId;


            try
            {
                
                repo.SaveChanges();
            }
            catch (Exception)
            {
                result = "Възникна грешка при Записа";
                throw;
            }

            return result;
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
            var re = await repo.All<AutoShop>()
                .Where(x => x.ManegerId == id)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return re.ToString();
        }

        public Task<IEnumerable<ViewIssueModel>> GetShopOrders(string shopId)
        {
            throw new NotImplementedException();
        }
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
        public async Task<IEnumerable<TypeActivity>> GetTypesActivity()
        {

            var types = await repo.All<Activity>()
                .Select(x => x.ActivityName)
                .ToListAsync();

            return types;

        }
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

        public Task<List<(string, string)>> GetShopMechanics(string shopId)
        {
            //To Do .... Need to make MechanicSelectModel

            //var result = repo.All<Mechanic>()
            //    .Where(x=>x.AutoShopId.ToString() == shopId)
            //    .SelectMany(x => 
            //    {
            //        res.id = x.UserId,
            //        res.name = x.User.UserName

            //    }



            throw new NotImplementedException();

        }


    }
}
