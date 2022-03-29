using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Core.Models.Issues;

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
