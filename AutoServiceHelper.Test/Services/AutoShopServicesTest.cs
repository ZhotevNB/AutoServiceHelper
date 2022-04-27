using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Core.Services;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using AutoServiceHelper.Test.Database;
using AutoServiceHelper.Test.Mock;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoServiceHelper.Test.Services
{
    public class AutoShopServicesTest
    {
        private Guid Id = Guid.NewGuid();
        private string userId = "TestManagerId";
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;


        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            var serviceColection = new ServiceCollection();

            serviceProvider = serviceColection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddSingleton<IAutoShopServices, AutoshopServices>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IRepository>();
            await SeedDbAsync(repo);

        }

        [Test]
        public async Task IsShopIdCorect()
        {
            //Arrange
            var service = serviceProvider.GetService<IAutoShopServices>();
            var userId = "TestManagerId";
            //Act
            var resulr = await service.GetShopID(userId);
            //Assert
            Assert.AreEqual(Id, resulr);
        }

        [Test]
        public async Task AddOfferUnSuccsesfull()
        {
            //Arrange
            var offer = new AddOfferViewModel();
            //Act
            var service = serviceProvider.GetService<IAutoShopServices>();
            Assert.CatchAsync<InvalidOperationException>(async () => await service.AddOffer(offer), "Offer was not addet");
        }

        [Test]
        public async Task AddOfferSuccsesfull()
        {
            var repo = serviceProvider.GetService<IRepository>();
            var isseId = await repo.All<Issue>().Select(x => x.Id).FirstAsync();
            //Arrange
            var offer = new AddOfferViewModel()
            {
                ShopId = Id.ToString(),
                AdditionalInfo = "Klaostrofobia",
                IssueId = isseId

            };
            //Act
            var service = serviceProvider.GetService<IAutoShopServices>();
            Assert.DoesNotThrowAsync(async () => await service.AddOffer(offer));
        }

        [Test]
        public async Task AddServiceToOfferUnSuccsesfull()
        {

            var repo = serviceProvider.GetService<IRepository>();
            var offerId = await repo.All<Offer>().Select(x => x.Id).FirstAsync();
            //Arrange
            var shopservice = new AddShopServiceViewModel()
            {
                offerId = offerId.ToString()
            };
            //Act
            var service = serviceProvider.GetService<IAutoShopServices>();
            Assert.CatchAsync<InvalidOperationException>(async () => await service.AddServiceToOffer(shopservice, Id), "Could not update DB");
        }

        [Test]
        public async Task AddServiceToOfferSuccsesfull()
        {
            var repo = serviceProvider.GetService<IRepository>();
            var offerId = await repo.All<Offer>().Select(x => x.Id).FirstAsync();
            //Arrange
            var shopService = new AddShopServiceViewModel()
            {
                Name = "Kalapalapa",
                NeededHourOfWork = 1,
                PricePerHouer = 27,
                offerId = offerId.ToString(),
                Type = TypeActivity.BreakeElectrical

            };
            //Act
            var service = serviceProvider.GetService<IAutoShopServices>();
            Assert.DoesNotThrowAsync(async () => await service.AddServiceToOffer(shopService, Id));
        }

        [Test]
        public async Task AddPartToServiceUnSuccsesfull()
        {

            var repo = serviceProvider.GetService<IRepository>();
            var serviceId = await repo.All<ShopService>().Select(x => x.Id).FirstAsync();
            //Arrange
            var part = new AddPartViewModel()
            {
                ServiceId = serviceId
            };
            //Act
            var service = serviceProvider.GetService<IAutoShopServices>();
            Assert.CatchAsync<InvalidOperationException>(async () => await service.AddPartToService(part), "Could not update DB");
        }

        [Test]
        public async Task AddPartToServiceSuccsesfull()
        {
            var repo = serviceProvider.GetService<IRepository>();
            var serviceId = await repo.All<ShopService>().Select(x => x.Id).FirstAsync();

            //Arrange
            var shopService = new AddPartViewModel()
            {
                Name = "Kalapalapa",
                Number = "1df35d6",
                Price = 27,
                QuantitiNeeded = 1,
                ServiceId = serviceId

            };
            //Act
            var service = serviceProvider.GetService<IAutoShopServices>();
            Assert.DoesNotThrowAsync(async () => await service.AddPartToService(shopService));
        }

        [Test]
        public async Task GetCorectCarIssueCount()
        {

            var service = serviceProvider.GetService<IAutoShopServices>();
            var repo = serviceProvider.GetService<IRepository>();

            var shop = await repo.All<AutoShop>().Where(x => x.ManegerId == userId).FirstAsync();

            var issues = await service.GetIssues(userId);
            var count = issues.Count();

            Assert.AreEqual(1, count);
        }
        [Test]
        public async Task GetCorectShopTypesCount()
        {

            var service = serviceProvider.GetService<IAutoShopServices>();
            var repo = serviceProvider.GetService<IRepository>();
          

            var types = await service.GetShopTypes(userId);
            var count = types.Count();

            Assert.AreEqual(3, count);
        }
        [TearDown]
        public void Teardown()
        {
            dbContext.Dispose();
        }
        private async Task SeedDbAsync(IRepository repo)
        {


            var user = new IdentityUser()
            {
                UserName = "TestUser",
                Id = userId,
                Email = "ZHA@asd",

            };
            var shop = new AutoShop()
            {
                Id = Id,
                ManegerId = userId,
                Name = "SuperShop",
                ContactInfoId = 5,
                ContactInfo = new ContactInfo()
                {
                    Id = 5,
                    AdditionalInfo = "Posible",
                    Address = "Posible",
                    City = "Sofia",
                    Country = "Bulgaria",
                    Email = "ZHA@asd",
                    PhoneNumber = "088987216"
                },
                PricePerHour = 23,
                Activities = new List<AutoShopActivity>()
                {
                    new AutoShopActivity()
                    {
                        AutoShopId=Id,
                        Activity= new Activity()
                        {
                            Id=1,
                            ActivityName=TypeActivity.SuspensionElectrical
                        }
                    } ,
                    new AutoShopActivity()
                    {
                        AutoShopId=Id,
                        Activity= new Activity()
                        {
                            Id=2,
                            ActivityName=TypeActivity.EnginMechanical
                        }
                    } ,
                    new AutoShopActivity()
                     {
                         AutoShopId = Id,
                        Activity = new Activity()
                        {
                         Id = 3,
                         ActivityName = TypeActivity.EnginElectrical
                        }
                    }
                }
            };



            var car = new Car()
            {
                Color = "Blue",
                Manifacture = "BMW",
                Model = "3-series",
                Vin = "qwe123rt456ew987459",
                Year = 2000,
                UserId = userId,

            };
            var issue = new Issue()
            {
                Car = car,
                CarId = car.Id,
                CarOdometer = 11222,
                Description = "Klisimor",
                SubmitetByUserId = userId,
                Type = TypeActivity.EnginElectrical
            };
            var offer = new Offer()
            {
                AdditionalInfo = "Klasaciq",
                Id = Guid.NewGuid(),
                IssueId = issue.Id,
                ShopId = shop.Id.ToString(),

            };
            var shopService = new ShopService()
            {
                Name = "Kalapalapa",
                NeededHourOfWork = 1,
                PricePerHouer = 27,
                OfferId = offer.Id,
                Offer = offer,
                Type = TypeActivity.BreakeElectrical

            };
            repo.Add(user);
            repo.Add(shop);
            repo.Add(car);
            repo.Add(issue);
            repo.Add(offer);
            repo.Add(shopService);
            repo.SaveChanges();
        }
    }
}
