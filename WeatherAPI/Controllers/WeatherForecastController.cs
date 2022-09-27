using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WeatherAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

		private static string _message = string.Empty;

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
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
				_message = Encoding.ASCII.GetString(body.ToArray());
				_message = $" [x] Received from Rabbit: {_message}";
				Console.WriteLine(_message);
			};

			channel.BasicConsume(queue: "hello",
				autoAck: false,
				consumer: consumer);
		}

		//[HttpGet]
		//public IEnumerable<WeatherForecast> Get()
		//{
		//	return Enumerable.Range(1, 5).Select(index => new WeatherForecast
		//	{
		//		Date = DateTime.Now.AddDays(index),
		//		TemperatureC = Random.Shared.Next(-20, 55),
		//		Summary = Summaries[Random.Shared.Next(Summaries.Length)]
		//	})
		//	.ToArray();
		//}

		[HttpGet]
		public string Get()
		{
			return _message;
		}
	}
}