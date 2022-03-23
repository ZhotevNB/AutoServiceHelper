using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Identity;

namespace AutoServiceHelper.Core.Contracts
{
    public interface ICarService
    {

        public Task AddCar(AddCarFormModel car,string userId);

        public IEnumerable<CarViewModel> AllCars (string userId);

        public string AddIssue(string carId,string userId);

        public IEnumerable<ViewIssueModel> ViewIssues( string carId);

        public IEnumerable<string> GetIssueTypes();

    }
}
