using AutoServiceHelper.Controllers;
using NUnit.Framework;

namespace AutoServiceHelper.Test.Controller
{
    public class CarsControllerTest
    {
        [Test]
        public void Test()
        {
            //Arange
            var carControler = new CarsController(null,null,null);
            //Act
            var result = carControler.AddCar();
            //Assert
        }
    }
}
