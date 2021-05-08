using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Order.Common.Models;
using OrderCommon;
using System.Threading.Tasks;

namespace Order.Test
{
    public class AzureStorageTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task QueueAndDequeueMessageTest()
        {
            var _appSettings = new Mock<IOptions<AppSettings>>();
            var queue = new QueueService(_appSettings.Object);
            var orderItem= new OrderItem()
            {
                Date = System.DateTime.Now,
                Id = 1
            };
           var msgSent= await queue.QueueMessage("orders", orderItem);
           var returnedorderItem= queue.DequeueMessage("orders");

            Assert.IsNotNull(returnedorderItem);
        }
    }
}