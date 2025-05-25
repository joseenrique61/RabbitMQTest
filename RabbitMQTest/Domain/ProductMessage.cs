namespace RabbitMQTest.Domain;

public record class ProductMessage(int id, string name, float price, string routingKey);


