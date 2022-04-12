using AutoServiceHelper.Core.Models.AutoShop;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IManagerServices
    {
        public Task<string> AddContactInfo(AutoShopInfoModel model, string managerId);

        public Task<IEnumerable<ShopMechanicsViewModel>> GetPosibleMechanicsList(string shopId);

        public Task<string> HireMechanic(string userId, Guid shopId);

        public Task<string> FireMechanic(string userId, Guid shopId);
    }
}
