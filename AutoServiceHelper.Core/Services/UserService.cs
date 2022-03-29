using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Users;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace AutoServiceHelper.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repository;


        public UserService(IRepository _repository)
        {
            repository = _repository;
        }

        public Task ChangeRolle(string userId)
        {
            throw new NotImplementedException();
        }

        public Task ChangeUserInfo(string userId, UsersSetingsFormModel model)
        {
            var info = repository.All<UserInfo>().Where(u => u.UserId == userId).FirstOrDefault();
            if (info == null)
            {
                info = new UserInfo
                {
                    UserId = userId,
                    NickName = model.NickName,
                    FirstName = model.FirstName,
                    LastName = model.LastName

                };
                repository.Add(info);
            }

            info.NickName = model.NickName;
            info.FirstName = model.FirstName;
            info.LastName = model.LastName;
            info.AskToChangeRollManager = model.Manager;
            info.AskToChangeRollMechanic = model.Mechanic;

          repository.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<UserChangeRollViewModel>> GetUserForMechanicRolleChange()
        {
            var users = repository.All<UserInfo>().Where(u => u.AskToChangeRollMechanic == true).Select(u => new UserChangeRollViewModel
            {
                Id = u.UserId
            });

            return GetUsersName(users);
        }
        public Task<IEnumerable<UserChangeRollViewModel>> GetUserForManagerRolleChange()
        {
            var users = repository.All<UserInfo>().Where(u => u.AskToChangeRollManager == true).Select(u => new UserChangeRollViewModel
            {
                Id = u.UserId
            });

            return GetUsersName(users);

        }
        private async Task<IEnumerable<UserChangeRollViewModel>> GetUsersName(IEnumerable<UserChangeRollViewModel> users)
        {
            var usersName = repository.All<IdentityUser>().Select(x => new
            {
                Id = x.Id,
                Name = x.UserName
            }).ToList();

            foreach (var user in users)
            {
                user.Name = usersName.First(u => u.Id == user.Id).Name;
            }

            return users;
        }

        public UsersSetingsFormModel GetUserInfo(string userId)
        {
         var respons=repository.All<UserInfo>()
                .Where(x => x.UserId == userId)
                .Select(x=>new UsersSetingsFormModel
           {
                    NickName=x.NickName,
                    FirstName=x.FirstName,
                    LastName=x.LastName,
                    UserId=userId,
                    Manager=x.AskToChangeRollManager,
                    Mechanic=x.AskToChangeRollMechanic

           })
                .FirstOrDefault();

            return respons;
        }
    }
}
