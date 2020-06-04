using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQManagement;
using AdvertisementManagement.Services;
using AdvertisementManagement.Models;
using Newtonsoft.Json;

namespace AdvertisementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        RabbitMQMessagePublisher _messagePublisher;
        private readonly AdvertisementService _advertisementService;


        public AdvertisementController(RabbitMQMessagePublisher messagePublisher, AdvertisementService advertisementService)
        {
            _messagePublisher = messagePublisher;
            _advertisementService = advertisementService;
        }

        [HttpGet]
        public ActionResult<List<AdvertisementModel>> Get()
        {
            return _advertisementService.Get();
        }

        [HttpPost]
        public ActionResult<AdvertisementModel> Post([FromBody] AdvertisementModel ad)
        {
            var newAd = _advertisementService.Create(ad);
            return Ok(JsonConvert.SerializeObject(newAd));
        }
    }
}
