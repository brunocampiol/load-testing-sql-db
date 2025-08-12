using ConsoleApp.Helpers;

namespace ConsoleApp;

public class ProductNameProvider
{
    private readonly Queue<string> _queue;
    private ulong Count;

    public ProductNameProvider()
    {
        var products = ProductNameGenerator.GenerateAllProducts();
        _queue = new Queue<string>(products);
    }

    public string GetNextName()
    {
        if (_queue.Count == 0)
        {
            Count++;
            return $"Fictional Product Name {Count}";
        }

        return _queue.Dequeue();
    }
}