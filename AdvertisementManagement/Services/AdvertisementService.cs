using AdvertisementManagement.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using RabbitMQManagement;
using System;

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
            _messagePublisher.PublishMessageAsync("TestCreated", ad, "advertisement.log");
            return ad;
        }
    }
}
