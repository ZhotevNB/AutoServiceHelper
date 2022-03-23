using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;


namespace AutoServiceHelper.Core.Services
{
    public class CarServices : ICarService
    {
        private readonly IRepository repository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;


        public CarServices(IRepository _repository,
            SignInManager<IdentityUser> _signInManager,
            UserManager<IdentityUser> _userManager)
        {

            repository = _repository;
            signInManager = _signInManager;
            userManager = _userManager;
        }


        public async Task AddCar(AddCarFormModel car, string userId)
        {

            var newCar = new Car
            {
                Manifacture = car.Manifacture,
                Model = car.Model,
                Vin = car.Vin,
                Color = car.Color,
                Year = car.Year,
                UserId = userId

            };
                     
                repository.Add(newCar);
                repository.SaveChanges();

        }
     
        public string AddIssue(AddIssueFormModel model,string carId,string userId)
        {
           var milage =repository.All<Issue>()
                .Where(x => x.CarId == carId)
                  .OrderByDescending(x => x.SubmitionDate.Date)
                .OrderByDescending(x => x.SubmitionDate.Hour)
                .OrderByDescending(x => x.SubmitionDate.Minute)
                .Select(x=>x.CarOdometer)
                .FirstOrDefault();

            if (milage>model.CarOdometer)
            {
                
                return "Invalid Operation";
            }
            repository.Add<Issue>(new Issue
            {
                Type=model.Type,
                CarOdometer = model.CarOdometer,
                Description = model.Description,
                CarId=carId,
                SubmitetByUserId=userId
            });
          repository.SaveChanges();
            return carId;
        }

        public IEnumerable<CarViewModel> AllCars(string userId)
        {
            var allCars = repository
                .All<Car>()
                .Where(c => c.UserId == userId)
                .Select(c => new CarViewModel
                {
                    Manifacture = c.Manifacture,
                    Model = c.Model,
                    Color = c.Color,
                    Year = c.Year,
                    Vin = c.Vin,
                    Id = c.Id,
                    UserId = c.UserId

                })
                .ToList();
            return allCars;
        }

        public IEnumerable<ViewIssueModel> ViewIssues(string id)
        {
            var car = repository.All<Car>().Where(c => c.Id == id).Select(c => new CarViewModel
            {
                Manifacture = c.Manifacture,
                Model = c.Model,
                Color = c.Color,
                Year = c.Year,
                Vin = c.Vin,
                Id = c.Id,
                UserId = c.UserId
            }).First();

            var issues = repository.All<Issue>()
                .Where(i => i.CarId == id)
                .Select(i => new ViewIssueModel
                {
                    Id = i.Id,
                    CarId = id,
                    Car = car,
                    IsFixed=i.isFixed,
                    CarOdometer = i.CarOdometer,
                    SubmitionDate = i.SubmitionDate,
                    Description = i.Description,
                    Status = i.Status,
                    SubmitetByUserId = i.SubmitetByUserId,
                    Type = i.Type

                })
                .OrderByDescending(x=>x.SubmitionDate.Date)
                .OrderByDescending(x=>x.SubmitionDate.Hour)
                .OrderByDescending(x=>x.SubmitionDate.Minute)
                .ToList();

            return issues;
        }
        public IEnumerable<string>GetIssueTypes()
        {
            return Enum.GetNames(typeof(TypeActivity)).ToList();
        }

        public void FixIssue(string issueId)
        {
            var issue=repository.All<Issue>()
                .Where(x => x.Id.ToString() == issueId)
                .First();

            issue.isFixed = true;

            repository.SaveChanges();

           
        }
    }
    
}
