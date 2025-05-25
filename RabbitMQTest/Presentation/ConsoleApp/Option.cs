namespace RabbitMQTest.Presentation.ConsoleApp;

public record Option(string Text, Func<Task> Action);