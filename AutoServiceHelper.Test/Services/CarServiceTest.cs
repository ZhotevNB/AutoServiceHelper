using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Core.Models.Issues;
using AutoServiceHelper.Core.Services;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using AutoServiceHelper.Test.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceHelper.Test.Services
{
    public class CarServiceTest
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
                .AddSingleton<ICarService, CarServices>()

                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IRepository>();
            await SeedDbAsync(repo);

        }
        [Test]
        public async Task AddCarUnSuccsesfull()
        {
            //Arrange
            var car = new AddCarFormModel();
            //Act
            var service = serviceProvider.GetService<ICarService>();
            Assert.CatchAsync<InvalidOperationException>(async () => await service.AddCar(car, userId), "Car was not addet");
        }
        [Test]
        public async Task AddCarSuccsesfull()
        {
            //Arrange
            var car = new AddCarFormModel()
            {
                Color = "Blue",
                Manifacture = "BMW",
                Model = "3-series",
                Vin = "qwe123rt456ew987459",
                Year = 2000,

            };
            //Act
            var service = serviceProvider.GetService<ICarService>();
            Assert.DoesNotThrowAsync(async () => await service.AddCar(car, userId));
        }
        [Test]
        public async Task GetCorectAllCarsCount()
        {
           
            var service = serviceProvider.GetService<ICarService>();
            var cars = await service.AllCars(userId);
            var count = cars.Count();
            Assert.AreEqual(2,count);
        }
        [Test]
        public async Task GetUncorectAllCarsCount()
        {

            var service = serviceProvider.GetService<ICarService>();
            var cars = await service.AllCars(userId);
            var count = cars.Count();
            Assert.AreNotEqual(1, count);
            Assert.AreNotEqual(3, count);
        }
        [Test]
        public async Task AddIssueSuccsesfull()
        {
            //Arrange
            var issue = new AddIssueFormModel()
            {
                Type=TypeActivity.BodyElectrical,
                Description="Kolkoto Poveche Tolkova Poveche",
               CarOdometer=1120
            };
            //Act
            var service = serviceProvider.GetService<ICarService>();
            var repo = serviceProvider.GetService<IRepository>();
            var carId = await repo.All<Car>().Select(x => x.Id).FirstAsync();
            Assert.DoesNotThrowAsync(async () => await service.AddIssue(issue,carId,userId
                ));
        }
        [Test]
        public async Task AddIssueUnSuccsesfull()
        {
            //Arrange
            var issue = new AddIssueFormModel();
            //Act
            var service = serviceProvider.GetService<ICarService>();
            var repo = serviceProvider.GetService<IRepository>();
            var carId = await repo.All<Car>().Select(x => x.Id).FirstAsync();
            Assert.CatchAsync<InvalidOperationException>(async () => await service.AddIssue(issue, carId, userId
                ), "Could not update DB");
        }
        [Test]
        public async Task AddIssueUnSuccsesfullRongOddometer()
        {
            //Arrange
            var issue1 = new AddIssueFormModel()
            {
                Type = TypeActivity.BodyElectrical,
                Description = "Kolkoto Poveche Tolkova Poveche",
                CarOdometer = 1120
            };
            var issue2 = new AddIssueFormModel()
            {
                Type = TypeActivity.BodyElectrical,
                Description = "Kolkoto Poveche Tolkova Poveche",
                CarOdometer = 1000
            };
            //Act
            var service = serviceProvider.GetService<ICarService>();
            var repo = serviceProvider.GetService<IRepository>();
            var carId = await repo.All<Car>().Select(x => x.Id).FirstAsync();
            service.AddIssue(issue1,carId,userId);
            var result=await service.AddIssue(issue2,carId,userId);
            Assert.AreEqual("Milage are less than the previos issue",result);
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
                Id = userId,
                UserName = "Nikolay",
                Email = "ZHA@ds.s",


            };

            var car1 = new Car()
            {
                Color = "Blue",
                Manifacture = "BMW",
                Model = "3-series",
                Vin = "qwe123rt456ew987459",
                Year = 2000,
                UserId = userId,

            };
            var car2 = new Car()
            {
                Color = "Red",
                Manifacture = "Mercedes",
                Model = "E-Class",
                Vin = "qwe123rt456ew477459",
                Year = 2003,
                UserId = userId,
            
            };
            repo.Add(user);
            repo.Add(car1);
            repo.Add(car2);
            repo.SaveChanges();
        }
    }
}
