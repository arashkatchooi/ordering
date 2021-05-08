using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Order.Common.Models;
using OrderCommon;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Supervisor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly QueueService _queueService;
        private Random _Rnd;

        public OrderController(ILogger<OrderController> logger, QueueService queue)
        {
            _logger = logger;
            _queueService = queue;
             _Rnd = new Random();
            
        }

        [HttpGet("")]
        public async Task<ActionResult<OrderItem>> Get(Guid id)
        {
            var _Id = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Id")))
            {
                _Id = int.Parse(HttpContext.Session.GetString("_Id"));
            }
            return  new OrderItem
            {
                Id = _Id,
                Date = DateTime.Now,
                Random = _Rnd.Next(),
                OrderText = _Rnd.Next().ToString()
            };
        }

        [HttpPost("Request")]
        public async Task<ActionResult<OrderItem>> Post([FromBody] OrderRequest body)
        {
            var result = new OrderItem();
            try
            {
                var _Id = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Id")))
                {
                     _Id = int.Parse(HttpContext.Session.GetString("_Id"));
                }
                var orderItem= new OrderItem
                {
                    Id = _Id+1,
                    Date = DateTime.Now,
                    Random = _Rnd.Next(),
                    OrderText = _Rnd.Next().ToString()
                };
                await _queueService.QueueMessage("orders", orderItem);
                Console.WriteLine($"Send order {0} with random number {1}", orderItem.Id, orderItem.Random);
                HttpContext.Session.SetString("_Id", orderItem.Id.ToString());
                return orderItem;

            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = ex.Message
                };
            }
        }


    }
}
