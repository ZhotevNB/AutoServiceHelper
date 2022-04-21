using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Models.Offers;

namespace AutoServiceHelper.Core.Contracts
{
    public interface ICarService
    {

        public Task AddCar(AddCarFormModel car,string userId);

        public  Task <IEnumerable<CarViewModel>> AllCars (string userId);

        public string AddIssue(AddIssueFormModel model,string carId,string userId);

        public IEnumerable<ViewIssueModel> ViewIssues( string carId);

        public Task<IEnumerable<OfferViewModel>> ViewOffers( string carId);      

        public IEnumerable<string> GetIssueTypes();

        public void FixIssue(string issueId);

    }
}
