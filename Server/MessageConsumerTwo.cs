using MassTransit;
using Sample.MessageTypes;
using System;
using System.Threading.Tasks;

namespace Server
{
	public class MessageConsumerTwo : IConsumer<PingMessageTwo>
	{
		public async Task Consume(ConsumeContext<PingMessageTwo> context)
		{
			Console.WriteLine($"The Request Two Text is:{context.Message.Text}");
			await context.RespondAsync(new PongMessageTwo
			{
				Text = context.Message.Text,
				Time = context.Message.Time
			});
		}
	}
}
