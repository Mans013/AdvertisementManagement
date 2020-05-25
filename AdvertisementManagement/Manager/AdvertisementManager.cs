using Microsoft.Extensions.Hosting;
using RabbitMQManagement;
using System;
using System.Threading;
using System.Threading.Tasks;
using AdvertisementManagement.Services;
using AdvertisementManagement.Models;

namespace AdvertisementManagement.Manager
{
    public class AdvertisementManager : IHostedService, IMessageHandlerCallback
    {
        private RabbitMQMessageHandler _messageHandler;
        private AdvertisementService _advertisementService;

        public AdvertisementManager(RabbitMQMessageHandler messageHandler, AdvertisementService advertisementService)
        {
            _messageHandler = messageHandler;
            _advertisementService = advertisementService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {
            try
            {
                switch (messageType)
                {
                    case "SellCar":
                        _advertisementService.Create(MessageSerializer.Deserialize<AdvertisementModel>(message));
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return true;
        }

    }
}
