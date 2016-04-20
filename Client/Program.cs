using MassTransit;
using System;
using MassTransit.AzureServiceBusTransport;
using Microsoft.Azure;
using Sample.MessageTypes;

namespace Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var busControl = ConfigureBus();
			busControl.Start();

			Console.WriteLine("请输入需要测试的数据总数：");
			Console.Write("> ");

			string value = Console.ReadLine();
			var total = int.Parse(value);

			for (int i = 0; i <= total; i++)
			{
				IRequestClient<PingMessage, PongMessage> client = new PublishRequestClient<PingMessage, PongMessage>(busControl, TimeSpan.FromSeconds(10));
				var response = client.Request(new PingMessage
				{
					Time = DateTime.Now,
					Text = i.ToString()
				}).Result;

				IRequestClient<PingMessageTwo, PongMessageTwo> clientTwo = new PublishRequestClient<PingMessageTwo, PongMessageTwo>(busControl, TimeSpan.FromSeconds(10));
				var responseTwo = clientTwo.Request(new PingMessageTwo
				{
					Time = DateTime.Now,
					Text = i.ToString()
				}).Result;

				Console.WriteLine($"The First Request Text:{response.Text} and use time is :{(DateTime.Now - response.Time).TotalSeconds}");
				Console.WriteLine($"The First Request Text:{responseTwo.Text} and use time is :{(DateTime.Now - responseTwo.Time).TotalSeconds}");
			}

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
				cfg.UseServiceBusMessageScheduler();
			});
		}
	}
}
