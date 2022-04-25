using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceHelper.Core.Services
{
    public class CarServices : ICarService
    {
        private readonly IRepository repository;



        public CarServices(IRepository _repository)
        {
            repository = _repository;

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

        public async Task<IEnumerable<CarViewModel>> AllCars(string userId)
        {
            var allCars = await repository
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
                .ToListAsync();
            return allCars;
        }

        public string AddIssue(AddIssueFormModel model, string carId, string userId)
        {
            var milage = repository.All<Issue>()
                 .Where(x => x.CarId == carId)
                   .OrderByDescending(x => x.SubmitionDate.Date)
                 .OrderByDescending(x => x.SubmitionDate.Hour)
                 .OrderByDescending(x => x.SubmitionDate.Minute)
                 .Select(x => x.CarOdometer)
                 .FirstOrDefault();

            if (milage > model.CarOdometer)
            {

                return "Invalid Operation";
            }
            repository.Add<Issue>(new Issue
            {
                Type = model.Type,
                CarOdometer = model.CarOdometer,
                Description = model.Description,
                CarId = carId,
                SubmitetByUserId = userId
            });
            repository.SaveChanges();
            return carId;
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
                    IsFixed = i.isFixed,
                    CarOdometer = i.CarOdometer,
                    SubmitionDate = i.SubmitionDate,
                    Description = i.Description,
                    Status = i.Status,
                    SubmitetByUserId = i.SubmitetByUserId,
                    Type = i.Type

                })
                .OrderByDescending(x => x.SubmitionDate.Date)
                .OrderByDescending(x => x.SubmitionDate.Hour)
                .OrderByDescending(x => x.SubmitionDate.Minute)
                .ToList();

            return issues;
        }

        public async Task<IEnumerable<OfferViewModel>> ViewOffers(string issueId)
        {
            var issue = await repository.All<Order>()
         .Where(x => x.IssueId.ToString() == issueId)
         .Select(x => x.OfferId)
         .FirstOrDefaultAsync();


            var result = await repository.All<Offer>()
                .Where(x => x.IssueId.ToString() == issueId)
                .Select(x => new OfferViewModel()
                {
                    Id = x.Id,
                    PartPrice = x.Services.Select(s => s.Parts.Sum(p => p.QuantitiNeeded * p.Price)),
                    ServicePrice = x.Services.Sum(s => (decimal)s.NeededHourOfWork * s.PricePerHouer),
                    SubmitionDate = x.SubmitionDate,
                    AdditionalInfo = x.AdditionalInfo,
                    IssueId = x.IssueId,
                    ShopId = x.ShopId,

                })
                .ToListAsync();

            if (issue!=Guid.Empty)
            {
                result = result.Where(x => x.Id == issue).ToList();
                result.ForEach(x=>x.IsSelected=true);
            }

            foreach (var item in result)
            {
                item.TotalPrice = item.ServicePrice + item.PartPrice.Sum();
            }
            return result;
        }
        public IEnumerable<string> GetIssueTypes()
        {
            return Enum.GetNames(typeof(TypeActivity)).ToList();
        }

        public void FixIssue(string issueId)
        {
            var issue = repository.All<Issue>()
                .Where(x => x.Id.ToString() == issueId)
                .First();

            issue.isFixed = true;

            repository.SaveChanges();


        }

        public async Task<string> OrderOffer(string offerId, string issueId)
        {
            string result = null;
            var order = new Order()
            {
                IssueId = Guid.Parse(issueId),
                OfferId = Guid.Parse(offerId),

            };

            var issue = await repository.All<Issue>()
                .Where(x => x.Id.ToString() == issueId)
                .FirstOrDefaultAsync();

            issue.OfferID = offerId;
            issue.Status = IssueStatus.InProgress;

            try
            {
                repository.Add(order);
                repository.SaveChanges();
            }
            catch (Exception)
            {
                result = "Can not add Order";
                throw;
            }
            return result;
        }

        public async Task<string> GetCarIdByIssueId(string issueId)
        {
            var resul = await repository.All<Issue>()
                .Where(x => x.Id.ToString() == issueId)
                .Select(x => x.CarId)
                .FirstOrDefaultAsync();
            return resul;
        }
    }

}
