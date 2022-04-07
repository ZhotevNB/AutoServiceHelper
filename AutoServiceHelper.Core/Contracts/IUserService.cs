using AutoServiceHelper.Core.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IUserService
    {
           

        public Task <string>ChangeUserInfo(string userId,UsersSetingsFormModel model);//used

        public Task<string> ChangeUserContactInfo(string userId, UserContactInfoModel model);

        public Task<UsersSetingsFormModel> GetUserInfo(string userId);//used

        public Task<IEnumerable<UserChangeRollViewModel>> GetUsers();//used

        public Task<UserChangeRollViewModel> GetUserInfoById();

        public Task<UserContactInfoModel> GetUserContactInfo(string userId);//used

        public  Task<IdentityUser> GetUserById(string userId);


    }
}
