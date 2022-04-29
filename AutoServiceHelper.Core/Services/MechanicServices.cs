using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Core.Models.Mechanic;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Core.Models.Orders;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
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
            string result = null;

            var remov = repo.All<MechanicActivity>()
                .Where(x => x.MechanicId == model.UserId)
                .ToList();
                foreach (var item in remov)
                {
                    repo.Delete(item);
                   
                }
            try
            {

                foreach (var item in model.ActivityIds)
                {
                    repo.Add(new MechanicActivity()
                    {
                        ActivityId = item,
                        MechanicId = model.UserId

                    });
                }

                repo.SaveChanges();
            }
            catch (Exception)
            {
                result = "unsuccessful record";
                throw new InvalidOperationException("Could not update DB");
            }
            return result;
        }

        public async Task<IEnumerable<Activity>> GetAllActivities()
        {
            return await repo.All<Activity>()
                .ToListAsync();

        }

        public async Task<IEnumerable<OrderViewModel>> GetAllOrders(string userId)
        {
            var shopId = await repo.All<AutoShop>()
                .Where(x => x.Mechanics.Any(m => m.UserId == userId))
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var activites = await GetMchanicActivities(userId);

            var types = await repo.All<Activity>()
                .Where(x => activites.ActivityIds.Any(a => x.Id == a))
                .Select(x => x.ActivityName)
                .ToListAsync();


            var orders = await repo.All<Order>()
                .Where(x => x.Offer.ShopId == shopId.ToString()
            && (x.Status == OrderStatus.WaitingForMechanic)
            && x.MechanicId == null
                )
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
            orders = orders
                 .Where(x => types.Any(t => t == x.Issue.Type))
                 .ToList();

            return orders;
        }

        public async Task<IEnumerable<OrderViewModel>> GetMyOrders(string userId)
        {


            var orders = await repo.All<Order>()
                .Where(x => x.MechanicId == userId)
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

        public async Task<string> PreservingOrders(string userId, string orderId)
        {
            string result = null;

            var order = await repo.All<Order>()
                .Where(o => o.Id.ToString() == orderId)
                .FirstOrDefaultAsync();

            order.MechanicId = userId;
            order.Status = OrderStatus.InProgress;


            try
            {
                repo.SaveChanges();
                result = "Successfully added order";
            }
            catch (Exception)
            {
                result = "Error";
                throw;
            }
            return result;
        }

        public async Task<string> CompleteOrder(string orderId)
        {
            string result = null;
            var orderIdGuid = Guid.Parse(orderId);

            try
            {
            var order = await repo.All<Order>()
                .Where(x => x.Id == orderIdGuid)
                .FirstOrDefaultAsync();

            order.Status = OrderStatus.Compled;

            var issue = await repo.All<Issue>()
                .Where(x => x.Id == order.IssueId)
                .FirstOrDefaultAsync();

            issue.Status = IssueStatus.Completed;
            issue.isFixed = true;

                repo.SaveChanges();
                result = "Successfull complete Order";
            }
            catch (Exception)
            {
                result = "Error";
                throw new InvalidOperationException("Error");
            }
            return result;
        }
    }
}
