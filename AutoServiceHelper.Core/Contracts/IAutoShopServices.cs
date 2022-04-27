using AutoServiceHelper.Core.Models.AutoShop;
using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Constants;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IAutoShopServices
    {
      
        public Task<string> AddOffer(AddOfferViewModel model);

        public Task <string>AddPartToService(AddPartViewModel model);

        public Task<string> AddServiceToOffer(AddShopServiceViewModel model, Guid shopId);

        public Task<IEnumerable<ViewIssueModel>> GetIssues(string userId);
               

        public Task<Guid> GetShopID(string userId);

        public Task<IEnumerable<TypeActivity>> GetShopTypes(string userId);
                
        public Task<AutoShopInfoModel> GetShopInfo(string managerId);

        public Task<IEnumerable<TypeActivity>> GetTypesActivity();


        public Task<IEnumerable<OfferViewModel>> GetOffers(string shopId);

        public Task<string> GetOfferIdByServiceId(string serviceId);

        public Task<string> RemoveServiceFromOffer(string serviceId);

        public Task<string> RemovePartFromService(string partId);




    }
}
