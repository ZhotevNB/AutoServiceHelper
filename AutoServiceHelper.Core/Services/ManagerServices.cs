using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.AutoShop;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceHelper.Core.Services
{
    public class ManagerServices:IManagerServices
    {
        private readonly IRepository repo;
        private readonly UserManager<IdentityUser> userManager;

        public ManagerServices(IRepository _repo,
           UserManager<IdentityUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;

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
                throw new InvalidDataException("Could not update DB");
            }

            return result;
        }
       
        public async Task<IEnumerable<ShopMechanicsViewModel>> GetPosibleMechanicsList(string shopId)
        {
            var mechanicsList = await userManager.Users
                .ToListAsync();

            var allBusyUser = await repo.All<Mechanic>()
                .Where(x => x.AutoShopId.ToString() != shopId)
                .Select(x => x.UserId)
                .ToListAsync();

            var allShopMechanics = await repo.All<Mechanic>()
               .Where(x => x.AutoShopId.ToString() == shopId)
               .Select(x => x.UserId)
               .ToListAsync();

            var mechanicsId = mechanicsList
               .Where(x => userManager.IsInRoleAsync(x, "Mechanic").Result)
                .Where(x => allBusyUser.All(a => a != x.Id))
                .Select(x => x.Id)
                .ToList();

            var mechanicInfo = await repo.All<UserInfo>()
                .Where(x => mechanicsId.Any(a => x.UserId == a))
                .Select(x => new ShopMechanicsViewModel()
                {
                    Id = x.UserId,
                    Name = $"{x.FirstName} {x.LastName}",
                    WorkForShop = allShopMechanics.Any(a => x.UserId == a) ? true : false,
                  City=x.ContactInfo.City,
                  Email=x.ContactInfo.Email,
                  PhoneNumber=x.ContactInfo.PhoneNumber,

                })
                .ToListAsync();
            var index = 1;
            foreach (var mechanic in mechanicInfo)
            {
                mechanic.Row = $"Id{index++}";

               
                mechanic.Activitys = await repo.All<MechanicActivity>()
                    .Where(x => x.MechanicId == mechanic.Id)
                    .Select(x => x.Activity.ActivityName.ToString())
                    .ToListAsync();
                
            }

            return mechanicInfo;
        }
        
        public async Task<string> HireMechanic(string userId, Guid shopId)
        {
            string result = null;

            var mechanic = new Mechanic()
            {
                AutoShopId = shopId,
                UserId = userId
            };

            try
            {
                repo.Add(mechanic);
                repo.SaveChanges();
            }
            catch (Exception)
            {
                result = "could not update DB";
                throw;
            }

            AddMechanicActivitysToShop(shopId);
            return result;
        }

        public async Task<string> FireMechanic(string userId, Guid shopId)
        {
            string result = null;
            var mechanic = new Mechanic()
            {
                AutoShopId = shopId,
                UserId = userId
            };

            try
            {
                repo.Delete(mechanic);
                repo.SaveChanges();
            }
            catch (Exception)
            {
                result = "could not update DB";
                throw;
            }
            AddMechanicActivitysToShop(shopId);
            return result;
        }

        private async Task AddMechanicActivitysToShop(Guid shopId)
        {
            //find way to improve this method
            try
            {
                var result = repo.All<Mechanic>()
                    .Where(x => x.AutoShopId == shopId)
                    .Select(x => x.UserId);

                if (result.FirstOrDefault() == null)
                {
                    var entetyToRemove = repo.All<AutoShopActivity>()
                         .Where(x => x.AutoShopId == shopId);
                    repo.DeleteRange(entetyToRemove);
                }
                else
                {
                    var shopActivitys = repo.All<MechanicActivity>()
                    .Where(x => result.Any(r => x.MechanicId == r))
                    .Select(x => x.ActivityId);

                    var curentShopActivitys = repo.All<AutoShopActivity>()
                         .Where(x => x.AutoShopId == shopId)
                         .Select(x => x.ActivityId);

                    foreach (var activity in shopActivitys)
                    {
                        if (!curentShopActivitys.Contains(activity))
                        {
                            var newShopActivity = new AutoShopActivity()
                            {
                                ActivityId = activity,
                                AutoShopId = shopId
                            };
                            repo.Add(newShopActivity);
                        }
                    }
                    foreach (var activity in curentShopActivitys)
                    {
                        if (!shopActivitys.Contains(activity))
                        {
                            var newShopActivity = new AutoShopActivity()
                            {
                                ActivityId = activity,
                                AutoShopId = shopId
                            };
                            repo.Delete(newShopActivity);
                        }
                    }
                }

                repo.SaveChanges();
            }


            catch (Exception)
            {

                throw;
            }
            return;
        }
    }
}
