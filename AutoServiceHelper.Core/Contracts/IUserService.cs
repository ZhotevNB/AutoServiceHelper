using AutoServiceHelper.Core.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IUserService
    {
           

        public Task <string>ChangeUserInfo(string userId,UsersSetingsFormModel model);

        public Task<string> ChangeUserContactInfo(string userId, UserContactInfoModel model);

        public Task<UsersSetingsFormModel> GetUserInfo(string userId);

        public Task<IEnumerable<UserChangeRollViewModel>> GetUsers();       

        public Task<UserContactInfoModel> GetUserContactInfo(string userId);

        public  Task<IdentityUser> GetUserById(string userId);


    }
}
