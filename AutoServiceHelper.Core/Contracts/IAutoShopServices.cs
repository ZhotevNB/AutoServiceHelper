using AutoServiceHelper.Core.Models.AutoShop;
using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Constants;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IAutoShopServices
    {
      
        public Task AddOffer(AddOfferViewModel model);

        public Task <string>AddServiceToOffer(Guid id);

        public Task <string>AddPartToService(Guid id);

        public Task<string> AddMechanicToOrder(Guid id);

        public Task<string> AddContactInfo(AutoShopInfoModel model,string managerId);

        public Task<string> GetShopID(string id);

        public Task<IEnumerable<TypeActivity>> GetShopTypes(string userId);

        public Task<IEnumerable<ViewIssueModel>> GetShopOrders(string shopId);

        public Task<AutoShopInfoModel> GetShopInfo(string managerId);

        public Task<IEnumerable<TypeActivity>> GetTypesActivity();

        public Task<IEnumerable<ViewIssueModel>> GetIssues(string userId);

        public Task<IEnumerable<OfferViewModel>> GetOffers(string shopId);

        public Task<List<(string id,string name)>> GetShopMechanics(string shopId);

    }
}
