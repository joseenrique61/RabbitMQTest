using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQTest.Domain;
using RabbitMQTest.Domain.Shop;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces;

namespace RabbitMQTest.Presentation.ConsoleApp;

public class ConsoleManager(ILogger<ConsoleManager> logger, IHostApplicationLifetime host, IShop shop, IProducer producer) : IHostedService
{
    private static async Task SelectOption(ILogger<ConsoleManager> logger, List<Option> options, bool addBackOption = true)
    {
        while (true)
        {
            Console.WriteLine("Enter an option:");
            for (var i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i].Text}");
            }
            if (addBackOption) Console.WriteLine($"{options.Count + 1}. Back");

            try
            {
                var option = int.Parse(Console.ReadLine()!);
                if (option <= 0 || option > options.Count + (addBackOption ? 1 : 0))
                {
                    throw new Exception();
                }

                if (option == options.Count + 1 && addBackOption)
                {
                    return;
                }

                if (option == options.Count && !addBackOption)
                {
                    await options[option - 1].Action.Invoke();
                    return;
                }

                await options[option - 1].Action.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid option");
                logger.LogError(e.Message);
            }
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        List<Option> options = [
            new("Buy product", BuyProduct),
            new("Exit", async () =>
            {
                host.StopApplication();
            })
        ];
        await SelectOption(logger, options, addBackOption: false);
    }

    private async Task BuyProduct()
    {
        List<Option> options = [];
        foreach (var product in shop.Products)
        {
            options.Add(new Option(product.Name, async () =>
            {
                Console.WriteLine($"Thanks for buying {product.Name}!");
                await producer.SendProductAlert(new ProductMessage(product.Id), "purchases", "");
            }));
        }
        await SelectOption(logger, options);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}