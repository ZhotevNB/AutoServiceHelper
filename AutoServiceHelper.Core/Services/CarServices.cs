using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Core.Models.Orders;
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

            try
            {
                repository.Add(newCar);
                repository.SaveChanges();
            }
            catch (Exception )
            {
                
                throw new InvalidOperationException("Car was not addet");
            }

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

        public async Task<string> AddIssue(AddIssueFormModel model, string carId, string userId)
        {
            var milage =  await repository.All<Issue>()
                 .Where(x => x.CarId == carId)
                   .OrderByDescending(x => x.SubmitionDate.Date)
                 .OrderByDescending(x => x.SubmitionDate.Hour)
                 .OrderByDescending(x => x.SubmitionDate.Minute)
                 .Select(x => x.CarOdometer)
                 .FirstOrDefaultAsync();

            if (milage > model.CarOdometer)
            {

                return "Milage are less than the previos issue";
            }
            repository.Add<Issue>(new Issue
            {
                Type = model.Type,
                CarOdometer = model.CarOdometer,
                Description = model.Description,
                CarId = carId,
                SubmitetByUserId = userId
            });
            try
            {
                repository.SaveChanges();
            }
            catch (Exception)
            {

                throw new InvalidOperationException("Could not update DB");
            }
            return carId;
        }

        public async Task<IEnumerable<ViewIssueModel>> ViewIssues(string id)
        {
           
            var issues =  await repository.All<Issue>()
                .Where(i => i.CarId == id)
                .Select(i => new ViewIssueModel
                {
                    Id = i.Id,
                    CarId = id,
                    Car = new CarViewModel()
                    {
                        Manifacture = i.Car.Manifacture,
                        Model = i.Car.Model,
                        Color = i.Car.Color,
                        Year = i.Car.Year,
                        Vin = i.Car.Vin,
                        Id = i.Car.Id,
                        UserId = i.Car.UserId
                    },
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
                .ToListAsync();

            return issues;
        }

        public async Task<IEnumerable<OfferViewModel>> ViewOffers(string issueId)
        {
            var offerId = await repository.All<Order>()
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

            if (offerId!=Guid.Empty)
            {
                result = result.Where(x => x.Id == offerId).ToList();
                result.ForEach(x=>x.IsSelected=true);
            }

            foreach (var item in result)
            {
                item.TotalPrice = item.ServicePrice + item.PartPrice.Sum();
                item.ShopName = await GetShopName(item.ShopId);
            }
            return result;
        }
        public IEnumerable<string> GetIssueTypes()
        {
            return Enum.GetNames(typeof(TypeActivity)).ToList();
        }

        public async Task FixIssue(string issueId)
        {
            var issue = await repository.All<Issue>()
                .Where(x => x.Id.ToString() == issueId)
                .FirstAsync();

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

        public async Task<IEnumerable<OrderViewModel>> GetMyOrders(string carId)
        {


            var orders = await repository.All<Order>()
                .Where(x => x.Issue.CarId==carId)
                .Select(x => new OrderViewModel()
                {
                    Id = x.Id,
                    IssueId = x.IssueId,
                    Offer = new OfferViewModel()
                    {
                        AdditionalInfo = x.Offer.AdditionalInfo,
                        SubmitionDate = x.Offer.SubmitionDate,
                        IssueId = x.IssueId,
                        Services = x.Offer.Services.Select(o => new ServiceViewModel()
                        {
                            Name = o.Name,
                            Type = o.Type,
                            NeededHourOfWork = o.NeededHourOfWork,
                            PricePerHouer = o.PricePerHouer,
                            ServiceId = o.Id,
                            offerId = o.OfferId,
                            Parts = x.Offer.Services.SelectMany(s => s.Parts.Select(p => new PartsViewModel()
                            {
                                Name = p.Name,
                                QuantitiNeeded = p.QuantitiNeeded,
                                Number = p.Number,
                                Price = p.Price,
                                Id = p.Id.ToString()
                            })
                              .ToList())
                        }).ToList(),
                        Id = x.OfferId,
                        ShopId = x.OfferId.ToString()
                    },
                    Issue = new IssueOrderViewModel()
                    {
                        Description = x.Issue.Description,
                        Car = new CarViewModel()
                        {
                            Color = x.Issue.Car.Color,
                            Manifacture = x.Issue.Car.Manifacture,
                            Model = x.Issue.Car.Model,
                            Year = x.Issue.Car.Year,
                            Vin = x.Issue.Car.Vin,
                            Id = x.Issue.Car.Id,
                            UserId = x.Issue.Car.UserId
                        },
                        SubmitionDate = x.Issue.SubmitionDate,
                        Status = x.Issue.Status,
                        CarId = x.Issue.CarId,
                        IsFixed = x.Issue.isFixed,
                        Type = x.Issue.Type,
                        SubmitetByUserId = x.Issue.SubmitetByUserId

                    }


                })
                .ToListAsync();

            return orders;
        }

        private async Task<string> GetShopName(string id)
        {
            var result = await repository.All<AutoShop>()
                 .Where(x => x.Id.ToString() == id)
                 .Select(x => x.Name)
                 .FirstOrDefaultAsync();

            return result;
        }
    }

}
