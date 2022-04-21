using AutoServiceHelper.Core.Models.AutoShop;
using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Constants;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IAutoShopServices
    {
      
        public Task<string> AddOffer(AddOfferViewModel model);

        public Task<string> AddServiceToOffer(AddShopServiceViewModel model, Guid shopId);
        public Task<string> RemoveServiceFromOffer(string serviceId);

        public Task <string>AddPartToService(AddPartViewModel model);

        public Task<string> RemovePartFromService(string partId);

        public Task<string> AddMechanicToOrder(Guid id);

        public Task<Guid> GetShopID(string userId);

        public Task<IEnumerable<TypeActivity>> GetShopTypes(string userId);

        public Task<IEnumerable<ViewIssueModel>> GetShopOrders(string shopId);

        public Task<AutoShopInfoModel> GetShopInfo(string managerId);

        public Task<IEnumerable<TypeActivity>> GetTypesActivity();

        public Task<IEnumerable<ViewIssueModel>> GetIssues(string userId);

        public Task<IEnumerable<OfferViewModel>> GetOffers(string shopId);

        public Task<IEnumerable<PartsViewModel>> GetPartsForService(string serviceId);

        public Task<IEnumerable<ServiceViewModel>> GetServicesForOffer(string offerId);
        public Task<string> GetOfferIdByServiceId(string serviceId);





    }
}
