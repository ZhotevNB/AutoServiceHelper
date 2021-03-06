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
              
        public async Task<string> AddOffer(AddOfferViewModel model)
        {
            string result = null;
            var offer = new Offer()
            {
                ShopId = model.ShopId,
                IssueId = model.IssueId,
                AdditionalInfo = model.AdditionalInfo
            };

            try
            {
                repo.Add(offer);
                repo.SaveChanges();
            }
            catch (Exception)
            {
                result = "Invalid Operation";
                throw new InvalidOperationException("Offer was not addet");
            }

            return result;
        }

        public async Task<string> AddServiceToOffer(AddShopServiceViewModel model, Guid shopId)
        {
            string msg = null;

            var shopPricePerHour = await repo.All<AutoShop>()
                .Where(x => x.Id == shopId)
                .Select(x => x.PricePerHour)
                .FirstOrDefaultAsync();

            var service = new ShopService()
            {
                Name = model.Name,
                NeededHourOfWork = model.NeededHourOfWork,
                OfferId = Guid.Parse(model.offerId),
                Type = model.Type,
                PricePerHouer = shopPricePerHour,
                Price = shopPricePerHour * (decimal)model.NeededHourOfWork
            };

            try
            {
                repo.Add(service);
                repo.SaveChanges();
            }
            catch (Exception)
            {
                msg = "Error";
                throw new InvalidOperationException("Could not update DB");
            }
            return msg;
        }

        public async Task<string> AddPartToService(AddPartViewModel model)
        {
            string result = null;

            var part = new Part()
            {
                Name = model.Name,
                Number = model.Number,
                Price = model.Price,
                QuantitiNeeded = model.QuantitiNeeded,
                ShopServiceId = model.ServiceId
            };

            repo.Add(part);

            try
            {
                repo.SaveChanges();
            }
            catch (Exception)
            {
                result = "Invalid Operation";
                throw new InvalidOperationException("Could not update DB");
            }


            return result;
        }

        public async Task<IEnumerable<ViewIssueModel>> GetIssues(string userId)
        {
            var types = await GetShopTypes(userId);

            var result = await repo.All<Issue>()                
                .Where(x=>x.OfferID==null)
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
                     Type = x.Type,
                     Car = new CarViewModel
                     {
                         Id = x.Car.Id,
                         Color = x.Car.Color,
                         Vin = x.Car.Vin,
                         Manifacture = x.Car.Manifacture,
                         Model = x.Car.Model,
                         Year = x.Car.Year

                     }
                 }).ToListAsync();

            result = result.Where(x => types.Any(t => t == x.Type)).ToList();
                     
            return result;
        }

        public async Task<IEnumerable<OfferViewModel>> GetOffers(string shopId)
        {
            var result = await repo.All<Offer>()
                 .Where(x => x.ShopId == shopId)
                 .Select(x => new OfferViewModel
                 {
                     Id = x.Id,
                     AdditionalInfo = x.AdditionalInfo,
                     IssueId = x.IssueId,
                     SubmitionDate = x.SubmitionDate,
                     TotalPrice = x.Services.Sum(s => s.Price),
                     ShopId = x.ShopId

                 }).ToListAsync();

            foreach (var item in result)
            {
                var partsPriceList = await repo.All<ShopService>()
                      .Where(x => x.OfferId == item.Id)
                      .Select(x => x.Parts.Sum(p => p.Price)).ToListAsync();

                item.TotalPrice += partsPriceList.Sum();
            }

            return result;
        }

        public async Task<Guid> GetShopID(string userId)
        {
            var re = await repo.All<AutoShop>()
                .ToListAsync();

                var shopId=re.Where(x => x.ManegerId == userId)
                .Select(x => x.Id)
                .FirstOrDefault();

            return shopId;
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

        public async Task<string> GetOfferIdByServiceId(string serviceId)
        {
            var result = await repo.All<ShopService>()
                  .Where(x => x.Id.ToString() == serviceId)
                  .Select(x => x.OfferId)
                  .FirstOrDefaultAsync();

            return result.ToString();
        }

        public async Task<string> RemoveServiceFromOffer(string serviceId)
        {
            string result = null;

            var service = await repo.All<ShopService>()
                 .Where(x => x.Id.ToString() == serviceId)
                 .FirstOrDefaultAsync();

            var serviceParts = await repo.All<Part>()
                .Where(x => x.ShopServiceId.ToString() == serviceId)
                .ToListAsync();
            try
            {
                if (serviceParts != null)
                {
                    repo.DeleteRange(serviceParts);
                }
                repo.Delete(service);
                repo.SaveChanges();
            }
            catch (Exception)
            {
                result = "Invalid operation";
                throw;
            }
            return result;
        }
               
        public async Task<string> RemovePartFromService(string partId)
        {
            string result = null;
            var part = await repo.All<Part>()
                .Where(x => x.Id.ToString() == partId)
                .FirstOrDefaultAsync();

            try
            {
                repo.Delete(part);
                repo.SaveChanges();
            }
            catch (Exception)
            {
                result = "Part can not be removed";
                throw;
            }
            return result;
        }
    }
}
