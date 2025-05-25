using RabbitMQTest.Domain.Shop;

namespace RabbitMQTest.Presentation.ConsoleApp;

public class ConsoleManager(IShop shop)
{
    private IShop _shop = shop;

    private static async Task SelectOption(List<Option> options)
    {
        while (true)
        {
            Console.WriteLine("Enter an option:");
            for (var i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i].Text}");
            }
            Console.WriteLine($"{options.Count + 1}. Back");

            try
            {
                var option = int.Parse(Console.ReadLine()!);
                if (option is <= 0 or > 3)
                {
                    throw new Exception();
                }

                if (option == options.Count + 1)
                {
                    return;
                }

                await options[option - 1].Action.Invoke();
            }
            catch
            {
                Console.WriteLine("Invalid option");
            }
        }
    }

    public async Task RunAsync()
    {
        List<Option> options = [new("Buy product", BuyProduct)];
        await SelectOption(options);
    }

    private async Task BuyProduct()
    {
        List<Option> options = [];
        foreach (var product in _shop.Products)
        {
            options.Add(new(product.Name, ));
        }
        await SelectOption(options);
    }
}