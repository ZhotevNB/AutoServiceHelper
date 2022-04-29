using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Users;
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
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceHelper.Test.Services
{
    public class UserServiceTest
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
                .AddSingleton<IUserService, UserService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IRepository>();
            await SeedDbAsync(repo);
        }
        [Test]
        public async Task ChangeUserInfoUnSuccessful()
        {
            //Arrange
            var model = new UsersSetingsFormModel()
            {

            };
            //Act
            var service = serviceProvider.GetService<IUserService>();
          
            Assert.CatchAsync<InvalidOperationException>(async () => await service.ChangeUserInfo(userId, model), "Неуспешен запис");
        }
        [Test]
        public async Task ChangeUserInfoSuccessful()
        {
            //Arrange
            var model = new UsersSetingsFormModel()
            {
                NickName="Test",
                FirstName="Test",
                LastName="Test",
                UserId=userId,
                Manager=true
                
            };
            //Act
            var service = serviceProvider.GetService<IUserService>();
            var result = await service.ChangeUserInfo(userId, model);
            Assert.AreEqual(null,result);
        }
        [Test]
        public async Task ChangeUserContactInfoSuccessful()
        {
            //Arrange
            var model = new UserContactInfoModel()
            {
              City="Sofia",
              Country="Bulgaria",
              Address="SFjndcfkjn",
              Email="akala@avc.vf",
              PhoneNumber="09888752",
              AdditionalInfo="Kccachbkjjs"

            };
            //Act
            var service = serviceProvider.GetService<IUserService>();
            var result = await service.ChangeUserContactInfo(userId, model);
            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task ChangeUserContactInfoUnSuccessful()
        {
            //Arrange
            var model = new UserContactInfoModel();
            

           
            //Act
            var service = serviceProvider.GetService<IUserService>();
          
            Assert.CatchAsync<InvalidOperationException>(async () => await service.ChangeUserContactInfo(userId, model), "Възникна грешка при Записа");
        }
        [Test]
        public async Task GetUsersCountSuccessful()
        {
            //Arrange


            //Act
            var service = serviceProvider.GetService<IUserService>();
            var result= await service.GetUsers();
            Assert.AreEqual(1,result.Count());
           
        }
        [Test]
        public async Task GetUsersInfoSuccessful()
        {
            //Arrange

            var model = new UsersSetingsFormModel()
            {
                NickName = "Test",
                FirstName = "Test",
                LastName = "Test",
                UserId = userId,
                Manager = true

            };
            //Act
            var service = serviceProvider.GetService<IUserService>();
            var r = await service.ChangeUserInfo(userId, model);
            //Act
            
            var result = await service.GetUserInfo(userId);
            Assert.AreEqual(model.FirstName, result.FirstName);
            Assert.AreEqual(model.LastName, result.LastName);
            Assert.AreEqual(model.NickName, result.NickName);
            Assert.AreEqual(model.Mechanic, result.Mechanic);

        }
        [Test]
        public async Task GetUsersContactInfoSuccessful()
        {
            var model = new UserContactInfoModel()
            {
                City = "Sofia",
                Country = "Bulgaria",
                Address = "SFjndcfkjn",
                Email = "akala@avc.vf",
                PhoneNumber = "09888752",
                AdditionalInfo = "Kccachbkjjs"

            };
            //Act
            var service = serviceProvider.GetService<IUserService>();
            var r = await service.ChangeUserContactInfo(userId, model);

            var result = await service.GetUserContactInfo(userId);
            Assert.AreEqual(model.City, result.City);
            Assert.AreEqual(model.Country, result.Country);
            Assert.AreEqual(model.Email, result.Email);
            Assert.AreEqual(model.PhoneNumber, result.PhoneNumber);

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

            var userInfo = new UserInfo()
            {
                UserId = userId,
                NickName = "Test",
                FirstName = "Test",
                LastName = "Test",
                AskToChangeRollManager = true,
                User = user
            };
            repo.Add(userInfo);

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
                Id = issueId,
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
