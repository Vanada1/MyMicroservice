namespace MessageService;

public interface IMessageService
{
	bool Enqueue(string message);
}