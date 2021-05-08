using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;
using Order.Common.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderCommon
{
    public  class QueueService: IQueueService
    {
        public QueueService()
        { }

        private AppSettings _appSettings;

        public QueueService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async  Task<OrderItem> DequeueMessage(string queueName)
        {

            string connectionString = _appSettings.ConnectionString;
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            OrderItem result = null;
            if (queueClient.Exists())
            {
                    QueueMessage retrievedMessage =  await queueClient.ReceiveMessageAsync();

                    if(retrievedMessage!=null)
                    { 
                        Console.WriteLine($"Dequeued message: '{retrievedMessage.MessageText}'");
                        result = JsonSerializer.Deserialize<OrderItem>( retrievedMessage.MessageText);
                                        
                        queueClient.DeleteMessage(retrievedMessage.MessageId, retrievedMessage.PopReceipt);
                    }
            }
            return result;
        }

        public async Task QueueMessage(string queueName, OrderItem orderItem)
        {
            string connectionString = _appSettings.ConnectionString;
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();

            if (await queueClient.ExistsAsync())
            {
                Console.WriteLine($"Queue '{queueClient.Name}' created");
            }
            else
            {
                Console.WriteLine($"Queue '{queueClient.Name}' exists");
            }

            await queueClient.SendMessageAsync(JsonSerializer.Serialize(orderItem));
            Console.WriteLine($"Message added");

        }

    }

    public interface IQueueService
    {
        public Task QueueMessage(string queueName, OrderItem orderItem);

        public Task<OrderItem> DequeueMessage(string queueName);
    }
}
