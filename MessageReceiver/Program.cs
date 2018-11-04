using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageReceiver
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("This is receiver application!");

			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "msgKey",
					durable: false,
					exclusive: false,
					autoDelete: false,
					arguments: null);
				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var body = ea.Body;
					var message = Encoding.UTF8.GetString(body);
					Console.WriteLine($" -- Received {message}");
				};
				channel.BasicConsume(queue: "msgKey",
					autoAck: true,
					consumer: consumer);
				Console.WriteLine("Press enter to exit");
				Console.ReadLine();
			}


		}
	}
}
