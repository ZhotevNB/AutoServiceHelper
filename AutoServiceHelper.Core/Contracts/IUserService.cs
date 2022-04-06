using AutoServiceHelper.Core.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IUserService
    {
        
        public Task ChangeRolle(string userId);

        public Task<UsersSetingsFormModel> GetUserInfo(string userId);

        public Task<IEnumerable<UserChangeRollViewModel>> GetUsers();

        public Task<UserChangeRollViewModel> GetUserInfoById();

        public Task ChangeUserInfo(string userId,UsersSetingsFormModel model);

        public  Task<IdentityUser> GetUserById(string userId);
    }
}
