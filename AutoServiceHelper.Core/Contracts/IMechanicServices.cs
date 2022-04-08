using AutoServiceHelper.Core.Models.Mechanic;
using AutoServiceHelper.Infrastructure.Data.Models;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IMechanicServices
    {
        public Task<string> AddMechanicActivities(MechanicActivitiesModel model);
        public Task<IEnumerable<Activity>> GetAllActivities();

        public Task<MechanicActivitiesModel> GetMchanicActivities(string userId);
    }
}
