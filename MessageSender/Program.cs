using System;
using System.Text;
using RabbitMQ.Client;

namespace MessageSender
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("This is sender application");
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "msgKey",
					durable: false,
					exclusive: false,
					autoDelete: false,
					arguments: null);
				Console.WriteLine("Enter message to send");
				var msg = "";
				do
				{
					msg = Console.ReadLine();
					if (!string.IsNullOrEmpty(msg))
					{
						SendMessage(channel, msg);
					}
					else
					{
						Console.WriteLine("Empty input, please try again");
					}
				} while (string.IsNullOrEmpty(msg));
			}

			Console.WriteLine("Press enter to exit");
			Console.ReadLine();
		}

		public static void SendMessage(IModel channel, string message)
		{
			var body = Encoding.UTF8.GetBytes(message);
			channel.BasicPublish(exchange: "",
				routingKey: "msgKey",
				basicProperties: null,
				body: body);
			Console.WriteLine($" -- Sent {message}");
		}
	}
}
