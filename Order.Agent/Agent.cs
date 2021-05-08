using Newtonsoft.Json;
using Order.Common.Models;
using OrderCommon;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Order.Agent
{
    public class Agent:IDisposable
    {

        private QueueService _queue;
        
        public Agent()
        { }

        public Agent(QueueService queue)
        {
            _queue = queue;
        }

        public void Dispose()
        {
            
        }

        public  async Task<OrderItem> GetMessage()
        {

            var orderItem= await _queue.DequeueMessage("orders");
            return orderItem;

            // returning the status to the Supervisor by calling POST API is suggested
            
            //if( msg.Equals("Request",StringComparison.OrdinalIgnoreCase))
            //{
            //    using (var client = new HttpClient())
            //    {
            //        client.BaseAddress = new Uri("https://localhost:44361/order/");
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //        Console.WriteLine("POST");

            //        var payload = new OrderRequest()
            //        {
            //            Command = "Accepted"
            //        };

            //        var stringPayload = JsonConvert.SerializeObject(payload);
            //        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            //        HttpResponseMessage response = await client.PostAsync("https://localhost:44361/order/", httpContent);
            //        if (response.IsSuccessStatusCode)
            //        {
            //             Console.WriteLine("Request was accepted");
            //        }

            //    }
            //}
        }

    }
}
