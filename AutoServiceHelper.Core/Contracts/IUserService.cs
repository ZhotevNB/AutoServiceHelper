using AutoServiceHelper.Core.Models.Users;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IUserService
    {
        
        public Task ChangeRolle(string userId);

        public Task<IEnumerable<UserChangeRollViewModel>> GetUserForMechanicRolleChange();
        public UsersSetingsFormModel GetUserInfo(string userId);

        public Task<IEnumerable<UserChangeRollViewModel>> GetUserForManagerRolleChange();

        public Task ChangeUserInfo(string userId,UsersSetingsFormModel model);
    }
}
