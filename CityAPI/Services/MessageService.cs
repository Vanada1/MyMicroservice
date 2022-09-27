using System.Text;
using RabbitMQ.Client;

namespace CityAPI.Services;

public class MessageService : IMessageService
{
	readonly ConnectionFactory _factory;
	readonly IConnection _conn;
	readonly IModel _channel;
	public MessageService()
	{
		Console.WriteLine("about to connect to rabbit");

		_factory = new ConnectionFactory
		{
			HostName = "rabbitmq", Port = 5672,
			UserName = "guest",
			Password = "guest"
		};
		_conn = _factory.CreateConnection();
		_channel = _conn.CreateModel();
		_channel.QueueDeclare(queue: "hello",
			durable: false,
			exclusive: false,
			autoDelete: false,
			arguments: null);
	}

	public bool Enqueue(string messageString)
	{
		var body = Encoding.UTF8.GetBytes("server processed " + messageString);
		_channel.BasicPublish(exchange: "",
			routingKey: "hello",
			basicProperties: null,
			body: body);
		Console.WriteLine($" [x] Published {messageString} to RabbitMQ");
		return true;
	}
}