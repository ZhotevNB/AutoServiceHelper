using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Infrastructure.Data.Constants;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IAutoShopServices
    {
        public Task<IEnumerable<ViewIssueModel>> GetIssues(string userId);

        public Task<IEnumerable<ViewIssueModel>> GetOffers(string shopId);

        public Task<IEnumerable<ViewIssueModel>> GetShopOrders(string shopId);


        public Task <string>AddServiceToOffer(Guid id);

        public Task <string>AddPartToService(Guid id);

        public Task<string> AddMechanicToOrder(Guid id);



    }
}
