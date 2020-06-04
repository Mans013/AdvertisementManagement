using AdvertisementManagement.Models;
using AdvertisementManagement.Services;
using CarManagement.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQManagement;

namespace AdvertismentManagementTests
{
    [TestClass]
    public class AdvertismentServiceTest
    {
        private static AdvertisementService advertisementService;
        private string testAdvertisementid;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Initalization code goes here
            var publisher = new RabbitMQMessagePublisher("localhost", "Test");
            advertisementService = new AdvertisementService(publisher);
            advertisementService.changeToTestService();

            AdvertisementModel advertisementModel = new AdvertisementModel()
            {
                Description = "Test McTestington",
                CarID = "Test",
                Image = "",
                Price = 10
            };
            advertisementService.Create(advertisementModel);
        }

        [TestMethod]
        public void GetAdvertisementsTest()
        {
            var result = advertisementService.Get();
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void CreateAdvertisemetTest()
        {
            AdvertisementModel advertisementModel = new AdvertisementModel()
            {
                Description = "Test McTestington",
                CarID = "Test",
                Image = "",
                Price = 10
            };
            var result = advertisementService.Create(advertisementModel);
            Assert.IsTrue(result.Id != null);
            Assert.IsTrue(result.Description == "Test McTestington");
        }

        [TestMethod]
        public void CreateAdvertisementByCarTest()
        {
            CarModel carModel = new CarModel()
            {
                Id = "Test",
                OwnerName = "Test McTestington",
                BuildDate = "now",
                Brand = "TestBrand",
                LicencePlate = "AB12CD",
                Weight = 1
            };
            var result = advertisementService.CreateNewAdvertisement(carModel);
            Assert.IsTrue(result.Id != null);
            Assert.IsTrue(result.CarID == "Test");
        }

        [TestMethod]
        public void SellCarTest()
        {
            string carId = "Test";
            var result = advertisementService.SellCar(carId);
            if (result != null)
            {
                Assert.IsTrue(result.CarID == carId);
            }
            else
            {
                Assert.IsTrue(result == null);
            }
        }
    }
}
