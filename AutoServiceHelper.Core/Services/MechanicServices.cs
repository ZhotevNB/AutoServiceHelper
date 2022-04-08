using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Mechanic;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceHelper.Core.Services
{
    public class MechanicServices : IMechanicServices
    {
        private readonly IRepository repo;

        public MechanicServices(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<string> AddMechanicActivities(MechanicActivitiesModel model)
        {
            string result= null;

            var remov = repo.All<MechanicActivity>()
                .Where(x => x.MechanicId == model.UserId)
                .ToList();

            foreach (var item in remov)
            {
                repo.Delete(item);
            }

            foreach (var item in model.ActivityIds)
            {
                repo.Add(new MechanicActivity()
                {
                    ActivityId = item,
                    MechanicId = model.UserId

                });      
            }
            try
            {
                repo.SaveChanges();
            }
            catch (Exception)
            {
                result= "unsuccessful record";
                throw;
            }
            return result;
        }

        public async Task<IEnumerable<Activity>> GetAllActivities()
        {
            return await repo.All<Activity>()
                .ToListAsync();

        }
        public async Task<MechanicActivitiesModel> GetMchanicActivities(string userId)
        {
            var activitiyIds = await repo.All<MechanicActivity>()
               .Where(x => x.MechanicId == userId)
                .Select(x => x.ActivityId)
                .ToArrayAsync();

            var activitiesIds = await repo.All<Activity>()
               .Where(x => activitiyIds.Any(a => x.Id == a))
                .Select(x => x.Id)
                 .ToListAsync();

            var result = new MechanicActivitiesModel()
            {
                UserId = userId,
                ActivityIds = activitiesIds,
            };

            return result;
        }
    }
}
