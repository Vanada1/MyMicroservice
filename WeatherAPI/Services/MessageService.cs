using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WeatherAPI.Services;

public class MessageService
{
    public string Message {get; private set;} = string.Empty;

    public MessageService()
    {
        var factory = new ConnectionFactory
			{
				HostName = "rabbitmq", Port = 5672,
				UserName = "guest",
				Password = "guest"
			};
			var conn = factory.CreateConnection();
			var channel = conn.CreateModel();
			channel.QueueDeclare(queue: "hello",
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null);

			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body;
				Message = Encoding.ASCII.GetString(body.ToArray());
				Message = $" [x] Received from Rabbit: {Message}";
				Console.WriteLine(Message);
			};

			channel.BasicConsume(queue: "hello",
				autoAck: false,
				consumer: consumer);
    }
}