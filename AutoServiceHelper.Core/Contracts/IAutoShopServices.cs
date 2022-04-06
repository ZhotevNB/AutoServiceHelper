using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Constants;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IAutoShopServices
    {
        public Task<IEnumerable<ViewIssueModel>> GetIssues(string userId);

        public Task<IEnumerable<OfferViewModel>> GetOffers(string shopId);

        public Task<IEnumerable<ViewIssueModel>> GetShopOrders(string shopId);


        public Task <string>AddServiceToOffer(Guid id);
        public Task AddOffer(AddOfferViewModel model);

        public Task <string>AddPartToService(Guid id);

        public Task<string> AddMechanicToOrder(Guid id);
        public Task<string> GetShopID(string id);



    }
}
