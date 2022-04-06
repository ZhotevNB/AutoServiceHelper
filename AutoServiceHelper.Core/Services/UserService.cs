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

        public Task ChangeRolle(string userId)
        {
            throw new NotImplementedException();
        }

        public Task ChangeUserInfo(string userId, UsersSetingsFormModel model)
        {
            var info = repository.All<UserInfo>()
                .Where(u => u.UserId == userId)
                .FirstOrDefault();

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

           
            repository.SaveChanges();

            return Task.CompletedTask;
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
    }
}
