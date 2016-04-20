using MassTransit;
using System;
using MassTransit.AzureServiceBusTransport;
using Microsoft.Azure;

namespace Server
{
	class Program
	{
		static void Main(string[] args)
		{
			var busControl = ConfigureBus();
			busControl.Start();
			
			Console.ReadKey();
			busControl.Stop();
		}

		static IBusControl ConfigureBus()
		{
			return Bus.Factory.CreateUsingAzureServiceBus(cfg =>
			{
				var host = cfg.Host(CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString"), h =>
				{
					h.OperationTimeout = TimeSpan.FromSeconds(30);
					h.TransportType = Microsoft.ServiceBus.Messaging.TransportType.Amqp;
				});
				cfg.ReceiveEndpoint(host, "Test", x =>
				{
					x.Consumer<MessageConsumer>();
					x.Consumer<MessageConsumerTwo>();
				});
				cfg.UseServiceBusMessageScheduler();
			});
		}
	}
}
