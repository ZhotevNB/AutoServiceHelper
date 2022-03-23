using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using Microsoft.AspNetCore.Identity;

namespace AutoServiceHelper.Core.Contracts
{
    public interface ICarService
    {

        public Task AddCar(AddCarFormModel car,string userId);

        public IEnumerable<CarViewModel> AllCars (string userId);

        public string AddIssue(AddIssueFormModel model,string carId,string userId);

        public IEnumerable<ViewIssueModel> ViewIssues( string carId);

        public IEnumerable<string> GetIssueTypes();

        public void FixIssue(string issueId);

    }
}
