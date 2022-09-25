using Microsoft.AspNetCore.Mvc;

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

	public CityController(ILogger<CityController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public CityForecast Get()
	{
		var i = Random.Shared.Next(Summaries.Length);
		return new CityForecast
		{
			Date = DateTime.Now,
			PeopleCount = PeopleCount[i],
			CityName = Summaries[i]
		};
	}
}