using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Services;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using AutoServiceHelper.Test.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoServiceHelper.Test.Services
{
    public class InformationServicesTest
    {
        private Guid Id = Guid.NewGuid();
        private Guid orderId = Guid.NewGuid();
        private Guid shopId = Guid.NewGuid();
        private Guid issueId = Guid.NewGuid();
        private Guid shopServiceId = Guid.NewGuid();
        private Guid offerId = Guid.NewGuid();
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
                .AddSingleton<IInformationServices, InformationServices>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IRepository>();
            await SeedDbAsync(repo);
        }
        [Test]
        public async Task GetPartsForServiceCountSuccessful()
        {
            //Arrange

            //Act
            var service = serviceProvider.GetService<IInformationServices>();
            var result = await service.GetPartsForService(shopServiceId.ToString());
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public async Task GetIssueIdByOfferIdSuccessful()
        {
            //Arrange

            //Act
            var service = serviceProvider.GetService<IInformationServices>();
            var result = await service.GetIssueIdByOfferId(offerId.ToString());
            Assert.AreEqual(issueId.ToString(), result);
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
            repo.Add(user);

            var activity1 = new Activity()
            {
                Id = 1,
                ActivityName = TypeActivity.SuspensionElectrical
            };
            repo.Add(activity1);

            var activity2 = new Activity()
            {
                Id = 2,
                ActivityName = TypeActivity.EnginMechanical
            };
            repo.Add(activity2);

            var activity3 = new Activity()
            {
                Id = 3,
                ActivityName = TypeActivity.EnginElectrical
            };
            repo.Add(activity3);


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
                        Activity=activity1
                        },
                    new AutoShopActivity()
                    {
                        AutoShopId=Id,
                        Activity= activity2
                    } ,
                    new AutoShopActivity()
                     {
                         AutoShopId = Id,
                        Activity = activity3
                    }
                }
            };
            repo.Add(shop);

            var mechanicActivity1 = new MechanicActivity()
            {
                MechanicId = userId,
                Activity = activity1

            };
            repo.Add(mechanicActivity1);

            var mechanicActivity2 = new MechanicActivity()
            {
                MechanicId = userId,
                Activity = activity2
            };
            repo.Add(mechanicActivity2);

            var mechanicActivity3 = new MechanicActivity()
            {
                MechanicId = userId,
                Activity = activity3
            };
            repo.Add(mechanicActivity3);

            var car = new Car()
            {
                Color = "Blue",
                Manifacture = "BMW",
                Model = "3-series",
                Vin = "qwe123rt456ew987459",
                Year = 2000,
                UserId = userId,

            };
            repo.Add(car);

            var issue = new Issue()
            {
                Id=issueId,
                Car = car,
                CarId = car.Id,
                CarOdometer = 11222,
                Description = "Klisimor",
                SubmitetByUserId = userId,
                Type = TypeActivity.EnginElectrical
            };
            repo.Add(issue);

            var offer = new Offer()
            {
                Id = offerId,
                AdditionalInfo = "Klasaciq",
                IssueId = issue.Id,
                ShopId = shop.Id.ToString(),

            };
            repo.Add(offer);

            var shopService = new ShopService()
            {
                Id = shopServiceId,
                Name = "Kalapalapa",
                NeededHourOfWork = 1,
                PricePerHouer = 27,
                OfferId = offer.Id,
                Offer = offer,
                Type = TypeActivity.BreakeElectrical

            };
            repo.Add(shopService);

            var part1 = new Part()
            {
                ShopServiceId = shopServiceId,
                Name = "part1",
                QuantitiNeeded = 1,
                Number = "A1222",
                Price = 9.67m
            };
            repo.Add(part1);

            var part2 = new Part()
            {
                ShopServiceId = shopServiceId,
                Name = "part2",
                QuantitiNeeded = 2,
                Number = "A1232",
                Price = 5.67m
            };
            repo.Add(part2);

            var order1 = new Order()
            {
                Id = orderId,
                Issue = issue,
                IssueId = issue.Id,
                OfferId = offer.Id,
                Offer = offer,
                MechanicId = userId,

            };
            repo.Add(order1);

            var order2 = new Order()
            {
                Issue = issue,
                IssueId = issue.Id,
                OfferId = offer.Id,
                Offer = offer,

            };
            repo.Add(order2);

            repo.SaveChanges();
        }
    }
}

