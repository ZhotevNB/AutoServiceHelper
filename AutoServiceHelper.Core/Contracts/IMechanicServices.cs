using AutoServiceHelper.Core.Models.Mechanic;
using AutoServiceHelper.Core.Models.Orders;
using AutoServiceHelper.Infrastructure.Data.Models;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IMechanicServices
    {
        public Task<string> AddMechanicActivities(MechanicActivitiesModel model);
        public Task<IEnumerable<Activity>> GetAllActivities();

        public Task<MechanicActivitiesModel> GetMchanicActivities(string userId);

        public Task<IEnumerable<OrderViewModel>> GetAllOrders(string userId);

        public Task<IEnumerable<OrderViewModel>> GetMyOrders(string userId);

        public Task<string> PreservingOrders(string userId,string orderId);
    }
}
