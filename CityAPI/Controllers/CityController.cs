using Microsoft.AspNetCore.Mvc;
using IMessageService = CityAPI.Services.IMessageService;
using System.Text.Json;
using System.Text;

namespace CityAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController : ControllerBase
{
	private static readonly string[] Summaries = new[]
	{
		"Tokyo", "Delhi", "Shanghai", "Sao Paulo", "Mexico City"
	};

	private static readonly int[] PeopleCount = new[]
	{
		13515271, 16753235, 24870895, 12252023, 9209944
	};

	private readonly ILogger<CityController> _logger;

	private readonly IMessageService _messageService = new Services.MessageService();

	public CityController(ILogger<CityController> logger)
	{
		_logger = logger;

	}

	[HttpGet]
	public CityForecast Get()
	{
		var i = Random.Shared.Next(Summaries.Length);
		var cityForecast = new CityForecast
		{
			Date = DateTime.Now,
			PeopleCount = PeopleCount[i],
			CityName = Summaries[i]
		};
		var json = JsonSerializer.Serialize(cityForecast);
		_messageService.Enqueue(json);
		return cityForecast;
	}

	[HttpPost]
	public void Post([FromBody] string payload)
	{
		Console.WriteLine("received a Post: " + payload);
		//_messageService.Enqueue(payload);
	}
}