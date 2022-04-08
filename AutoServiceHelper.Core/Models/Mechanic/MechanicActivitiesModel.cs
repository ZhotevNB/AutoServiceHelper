using AutoServiceHelper.Infrastructure.Data.Models;

namespace AutoServiceHelper.Core.Models.Mechanic
{
    public class MechanicActivitiesModel
    {
        public string UserId { get; set; }
        public List<int> ActivityIds { get; set; }

       
    }
}
