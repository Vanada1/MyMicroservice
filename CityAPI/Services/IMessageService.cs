namespace CityAPI.Services;

public interface IMessageService
{
	bool Enqueue(string message);
}