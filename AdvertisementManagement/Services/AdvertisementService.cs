using AdvertisementManagement.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using RabbitMQManagement;
using System;
using CarManagement.Models;

namespace AdvertisementManagement.Services
{
    public class AdvertisementService
    {
        private readonly IMongoCollection<AdvertisementModel> ads;
        RabbitMQMessagePublisher _messagePublisher;
        public AdvertisementService(IConfiguration config, RabbitMQMessagePublisher messagePublisher)
        {
            var client = new MongoClient(config.GetConnectionString("CarChampDb"));
            var database = client.GetDatabase("CarChampDb");
            ads = database.GetCollection<AdvertisementModel>("advertisements");
            this._messagePublisher = messagePublisher;
        }

        public List<AdvertisementModel> Get()
        {
            return ads.Find(ad => true).ToList();
        }

        public AdvertisementModel Create(AdvertisementModel ad)
        {
            ads.InsertOne(ad);
            _messagePublisher.PublishMessageAsync("AdvertisementAdded", ad, "advertisement.log");
            return ad;
        }

        public void CreateNewAdvertisement(CarModel car)
        {
            AdvertisementModel advertisement = new AdvertisementModel();
            advertisement.CarID = car.Id;
            advertisement.Image = "";
            advertisement.Price = 1000;
            advertisement.Description = "Lorum Ipsum";
            Create(advertisement);
        }

        public void SellCar()
        {
            var result = ads.Find(ad => true).FirstOrDefault();
            _messagePublisher.PublishMessageAsync("CarSold", result, "advertisement.log"); ;
        }
    }
}
