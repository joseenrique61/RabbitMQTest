namespace RabbitMQTest.Domain;

public record class ProductMessage(string name, float price, string routingKey);


