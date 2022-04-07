using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Users;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceHelper.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repository;


        public UserService(IRepository _repository)
        {
            repository = _repository;
        }
               
        public async Task<string> ChangeUserInfo(string userId, UsersSetingsFormModel model)
        {
            string result = null;
            var info = await repository.All<UserInfo>()
                .Where(u => u.UserId == userId)
                .FirstOrDefaultAsync();

            if (info == null)
            { 
                info = new UserInfo();
                info.UserId = userId;
                repository.Add(info);
            }
            info.NickName = model.NickName;
            info.FirstName = model.FirstName;
            info.LastName = model.LastName;
            info.AskToChangeRollManager = model.Manager;
            info.AskToChangeRollMechanic = model.Mechanic;

            try
            {
                repository.SaveChanges();
            }
            catch (Exception)
            {
                result = "Неуспешен запис ";
                throw;
            }

            return result;
        }

        public async Task<string> ChangeUserContactInfo(string userId, UserContactInfoModel model)
        {
            string result = null;

            var contactInfo = await repository.All<ContactInfo>()
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (contactInfo == null)
            {
                contactInfo = new ContactInfo();
                repository.Add(contactInfo);
            }

            contactInfo.Country = model.Country;
            contactInfo.City = model.City;
            contactInfo.Address = model.Address;
            contactInfo.PhoneNumber = model.PhoneNumber;
            contactInfo.Email = model.Email;
            contactInfo.AdditionalInfo = model.AdditionalInfo;




            var userInfo = await repository.All<UserInfo>()
                .Where(x=>x.UserId==userId)
                .FirstOrDefaultAsync();

            userInfo.ContactInfo = contactInfo;
           
            try
            {

                repository.SaveChanges();
            }
            catch (Exception)
            {
                result = "Възникна грешка при Записа";
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<UserChangeRollViewModel>> GetUsers()
        {
            var users = await repository
                .All<UserInfo>()
                .Select(u => new UserChangeRollViewModel
                {
                    Id = u.UserId,
                    Name = $"{u.FirstName} {u.LastName}",
                    WantToBeManager = u.AskToChangeRollManager,
                    WantToBeMechanic = u.AskToChangeRollMechanic
                }).ToListAsync();

            return users;

        }

        public async Task<UserChangeRollViewModel> GetUserInfoById()
        {
            var users = await repository
                .All<UserInfo>()
                .Select(u => new UserChangeRollViewModel
                {
                    Id = u.UserId,
                    Name = $"{u.FirstName} {u.LastName}",
                    WantToBeManager = u.AskToChangeRollManager,
                    WantToBeMechanic = u.AskToChangeRollMechanic
                }).FirstAsync();

            return users;

        }

        public async Task<UsersSetingsFormModel> GetUserInfo(string userId)
        {
            var respons = await repository.All<UserInfo>()
                   .Where(x => x.UserId == userId)
                   .Select(x => new UsersSetingsFormModel
                   {
                       NickName = x.NickName,
                       FirstName = x.FirstName,
                       LastName = x.LastName,
                       UserId = userId,
                       Manager = x.AskToChangeRollManager,
                       Mechanic = x.AskToChangeRollMechanic

                   })
                   .FirstOrDefaultAsync();

            return respons;
        }

        public async Task<IdentityUser> GetUserById(string userId)
        {

            var respons = await repository.All<IdentityUser>()
                   .Where(x => x.Id == userId)
                   .FirstOrDefaultAsync();

            return respons;
        }

        public async Task<UserContactInfoModel> GetUserContactInfo(string userId)
        {
           var userInfo= await repository.All<UserInfo>()
                .Where(x=>x.UserId == userId)                
                .FirstOrDefaultAsync();

            if (userInfo.ContactInfoId!=null)
            {
                var result = await repository.All<ContactInfo>()
                .Where(x => x.Id == userInfo.ContactInfoId)
                .Select(x => new UserContactInfoModel
                {
                    Id = x.Id,
                    City = x.City,
                    Country = x.Country,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address,
                    AdditionalInfo = x.AdditionalInfo
                }).FirstOrDefaultAsync();
                
                return result;
            }

            return null;
         
        }

       
    }
}
