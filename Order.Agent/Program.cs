using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Common.Models;
using OrderCommon;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Agent
{
    class Program
    {
        #region Nested classes to support running as service
        public const string ServiceName = "OrderAgentService";

        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = Program.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }
        #endregion


        static QueueService queue;

        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        static void Main(string[] args)
        {
             

        var  serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<QueueService, QueueService>()
                .Configure<AppSettings>(Configuration)
                .BuildServiceProvider();

                queue = serviceProvider.GetService<QueueService>();


            if (!Environment.UserInteractive)
                using (var service = new Service())
                    ServiceBase.Run(service);
            else
            {
                Task.Run(() => Start(args));
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
                Stop();
            }
        }

        private async  static void Start(string[] args)
        {
            var rnd = new Random();
            var myRnd = rnd.Next();
            Console.WriteLine("I’m agent {0}, my magic number is {1}",  Guid.NewGuid(), myRnd);
            using (var agent = new Agent(queue))
            {
                while(true)
                {
                    var orderItem=  await agent.GetMessage();
                    if (orderItem!=null &&  orderItem?.Random== myRnd) 
                    {
                        break;
                    }
                    Thread.Sleep(2000);
                }
            }
        }

        private static void Stop()
        {
        }
    }
}
