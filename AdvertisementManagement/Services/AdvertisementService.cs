using AdvertisementManagement.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using RabbitMQManagement;
using System;
using CarManagement.Models;
using MongoDB.Bson;

namespace AdvertisementManagement.Services
{
    public class AdvertisementService
    {
        private IMongoCollection<AdvertisementModel> ads;
        RabbitMQMessagePublisher _messagePublisher;
        public AdvertisementService(RabbitMQMessagePublisher messagePublisher)
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("CarChampDb");
            ads = database.GetCollection<AdvertisementModel>("advertisements");
            this._messagePublisher = messagePublisher;
        }

        public void changeToTestService()
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("CarChampDb");
            ads = database.GetCollection<AdvertisementModel>("Test");
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

        public AdvertisementModel CreateNewAdvertisement(CarModel car)
        {
            AdvertisementModel advertisement = new AdvertisementModel();
            advertisement.CarID = car.Id;
            advertisement.Image = "";
            advertisement.Price = 1000;
            advertisement.Description = "Lorum Ipsum";
            return Create(advertisement);
        }

        public AdvertisementModel SellCar(string carId)
        {
            var filter = Builders<AdvertisementModel>.Filter.Eq("CarID", carId);
            var result = ads.Find(filter).FirstOrDefault();
            _messagePublisher.PublishMessageAsync("CarSold", result, "advertisement.log");
            ads.DeleteOne(filter);
            return result;
        }

    }
}
