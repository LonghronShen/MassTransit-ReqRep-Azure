using MassTransit;
using Sample.MessageTypes;
using System;
using System.Threading.Tasks;

namespace Server
{
	public class MessageConsumer : IConsumer<PingMessage>
	{
		public async Task Consume(ConsumeContext<PingMessage> context)
		{
			Console.WriteLine($"The Request Text is:{context.Message.Text}");
			await context.RespondAsync(new PongMessage
			{
				Text = context.Message.Text,
				Time = context.Message.Time
			});
		}
	}
}
