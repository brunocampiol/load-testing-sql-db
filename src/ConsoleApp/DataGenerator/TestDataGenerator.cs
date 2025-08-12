using ConsoleApp.Constants;
using ConsoleApp.Extensions;
using ConsoleApp.Models;
using System.Diagnostics;
using System.Text;

namespace ConsoleApp.DataGenerator;

public static class TestDataGenerator
{
    static readonly Random Rnd = new();

    public static async Task GenerateDataAsync(Options options)
    {
        Console.WriteLine($"Generating {options.NumberOfRecords:N0} records...");
        var startTime = Stopwatch.GetTimestamp();

        var customerIds = await GenerateCustomerDataAsync(options);
        var productIds = await GenerateProductDataAsync(options);
        var orderIds = await GenerateOrderDataAsync(options, customerIds.ToList());
        var orderItemsIds = await GenerateOrderItemsDataAsync(options, productIds.ToList(), orderIds.ToList());

        var totalItems = customerIds.Count() + productIds.Count() + orderIds.Count() + orderItemsIds.Count();
        var totalTime = Stopwatch.GetElapsedTime(startTime);

        Console.WriteLine("\n\nCompleted generating '{0:N0}' records in {1}", totalItems, totalTime.ToShortDisplayString());
        Console.WriteLine("Average speed: '{0:N0}' records/second", totalItems / totalTime.TotalSeconds);
    }

    private static async Task<IEnumerable<Guid>> GenerateCustomerDataAsync(Options options)
    {
        //var fileName = $"customers-{GetNumberName(options.NumberOfRecords)}.csv";
        var fileName = $"_customers.csv";
        var fullFilePath = $@"{AppContext.BaseDirectory}\{fileName}";
        using var writer = new StreamWriter(fullFilePath, false, Encoding.UTF8);
        await writer.WriteLineAsync("ID,Name,DateOfBirth,CreatedAt");

        var today = DateTime.Now;
        var startDate = today.AddYears(-80);
        var endDate = today.AddYears(-18);
        var range = (endDate - startDate).Days;

        // Use buffer to reduce file writes
        var sb = new StringBuilder();
        int batchSize = 10000;

        // Progress tracking variables
        var startTime = Stopwatch.GetTimestamp();
        int totalGenerated = 0;
        int count = options.NumberOfRecords;

        var customerIds = new List<Guid>(count);

        for (int i = 1; i <= count; i++)
        {
            var id = Guid.NewGuid();
            var firstName = NameConstants.FirstNames[Rnd.Next(NameConstants.FirstNames.Length)];
            var middleName = NameConstants.LastNames[Rnd.Next(NameConstants.LastNames.Length)];
            var lastName = NameConstants.LastNames[Rnd.Next(NameConstants.LastNames.Length)];
            var name = $"{firstName} {middleName} {lastName}";

            var dobDaysOffset = Rnd.Next(range);
            var dob = startDate.AddDays(dobDaysOffset).ToString("yyyy-MM-dd");

            var createdDaysAgo = Rnd.Next(365);
            var createdDate = today.AddDays(-createdDaysAgo).ToString("yyyy-MM-dd HH:mm:ss");

            customerIds.Add(id);

            // Format all fields with quotes to ensure proper SQL interpretation
            sb.AppendLine($"\"{id}\",\"{name}\",\"{dob}\",\"{createdDate}\"");

            if (i % batchSize == 0 || i == count)
            {
                await writer.WriteAsync(sb.ToString());
                sb.Clear();

                totalGenerated = i;
                double percentComplete = (double)totalGenerated / count * 100;
                var elapsedTime = Stopwatch.GetElapsedTime(startTime);
                var estimatedTotalTime = TimeSpan.FromTicks((long)(elapsedTime.Ticks / (percentComplete / 100)));
                var remainingTime = estimatedTotalTime - elapsedTime;

                // Clear the current line
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

                // Print progress bar
                Console.Write($"Progress: [{percentComplete:F1}%] {totalGenerated:N0} of {count:N0} records | ");
                Console.Write($"Elapsed: {elapsedTime.ToShortDisplayString()} | Remaining: {remainingTime.ToShortDisplayString()}");
            }
        }

        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
        Console.WriteLine($"Finished inserting '{totalGenerated}' customers in {Stopwatch.GetElapsedTime(startTime)}");

        return customerIds;
    }

    private static async Task<IEnumerable<Guid>> GenerateProductDataAsync(Options options)
    {
        //var fileName = $"products-{GetNumberName(options.NumberOfRecords)}.csv";
        var fileName = $"_products.csv";
        var fullFilePath = $@"{AppContext.BaseDirectory}\{fileName}";
        using var writer = new StreamWriter(fullFilePath, false, Encoding.UTF8);
        await writer.WriteLineAsync("ID,ProductName,Price,CreatedAt");

        // Use buffer to reduce file writes
        var sb = new StringBuilder();
        int batchSize = 10000;

        // Progress tracking variables
        var startTime = Stopwatch.GetTimestamp();
        int totalGenerated = 0;
        int count = options.NumberOfRecords;

        var today = DateTime.Now;
        var nameQueue = new ProductNameProvider();
        var productIds = new List<Guid>(count);

        for (int i = 1; i <= count; i++)
        {
            var id = Guid.NewGuid();
            var productName = RemoveCsvUnwantedCharacters(nameQueue.GetNextName());
            var price = Math.Round(0.01 + (Rnd.NextDouble() * 49999.99), 2);
            var createdDaysAgo = Rnd.Next(365);
            var createdDate = today.AddDays(-createdDaysAgo).ToString("yyyy-MM-dd HH:mm:ss");

            productIds.Add(id);

            // Format all fields with quotes to ensure proper SQL interpretation
            sb.AppendLine($"\"{id}\",\"{productName}\",\"{price}\",\"{createdDate}\"");

            if (i % batchSize == 0 || i == count)
            {
                await writer.WriteAsync(sb.ToString());
                sb.Clear();

                totalGenerated = i;
                double percentComplete = (double)totalGenerated / count * 100;
                var elapsedTime = Stopwatch.GetElapsedTime(startTime);
                var estimatedTotalTime = TimeSpan.FromTicks((long)(elapsedTime.Ticks / (percentComplete / 100)));
                var remainingTime = estimatedTotalTime - elapsedTime;

                // Clear the current line
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

                // Print progress bar
                Console.Write($"Progress: [{percentComplete:F1}%] {totalGenerated:N0} of {count:N0} records | ");
                Console.Write($"Elapsed: {elapsedTime.ToShortDisplayString()} | Remaining: {remainingTime.ToShortDisplayString()}");
            }
        }

        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
        Console.WriteLine($"Finished inserting '{totalGenerated}' products in {Stopwatch.GetElapsedTime(startTime)}");

        return productIds;
    }

    private static async Task<IEnumerable<Guid>> GenerateOrderDataAsync(Options options, IList<Guid> customerIds)
    {
        //var fileName = $"orders-{GetNumberName(options.NumberOfRecords)}.csv";
        var fileName = $"_orders.csv";
        var fullFilePath = $@"{AppContext.BaseDirectory}\{fileName}";
        using var writer = new StreamWriter(fullFilePath, false, Encoding.UTF8);
        await writer.WriteLineAsync("ID,CustomerId,CreatedAt");

        // Use buffer to reduce file writes
        var sb = new StringBuilder();
        int batchSize = 10000;

        // Progress tracking variables
        var startTime = Stopwatch.GetTimestamp();
        int totalGenerated = 0;
        int count = options.NumberOfRecords;

        var today = DateTime.Now;
        var orderIds = new List<Guid>(count);

        for (int i = 1; i <= count; i++)
        {
            var id = Guid.NewGuid();
            var customerId = customerIds[Rnd.Next(customerIds.Count)];
            var createdDaysAgo = Rnd.Next(365);
            var createdDate = today.AddDays(-createdDaysAgo).ToString("yyyy-MM-dd HH:mm:ss");

            orderIds.Add(id);

            // Format all fields with quotes to ensure proper SQL interpretation
            sb.AppendLine($"\"{id}\",\"{customerId}\",\"{createdDate}\"");

            if (i % batchSize == 0 || i == count)
            {
                await writer.WriteAsync(sb.ToString());
                sb.Clear();

                totalGenerated = i;
                double percentComplete = (double)totalGenerated / count * 100;
                var elapsedTime = Stopwatch.GetElapsedTime(startTime);
                var estimatedTotalTime = TimeSpan.FromTicks((long)(elapsedTime.Ticks / (percentComplete / 100)));
                var remainingTime = estimatedTotalTime - elapsedTime;

                // Clear the current line
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

                // Print progress bar
                Console.Write($"Progress: [{percentComplete:F1}%] {totalGenerated:N0} of {count:N0} records | ");
                Console.Write($"Elapsed: {elapsedTime.ToShortDisplayString()}  | Remaining:  {remainingTime.ToShortDisplayString()}");
            }
        }

        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
        Console.WriteLine($"Finished inserting '{totalGenerated}' orders in {Stopwatch.GetElapsedTime(startTime)}");

        return orderIds;
    }

    private static async Task<IEnumerable<Guid>> GenerateOrderItemsDataAsync(Options options, IList<Guid> productIds, IList<Guid> orderIds)
    {
        //var fileName = $"orderItems-{GetNumberName(options.NumberOfRecords)}.csv";
        var fileName = $"_orderItems.csv";
        var fullFilePath = $@"{AppContext.BaseDirectory}\{fileName}";
        using var writer = new StreamWriter(fullFilePath, false, Encoding.UTF8);
        await writer.WriteLineAsync("ID,OrderId,ProductId,Quantity");

        // Use buffer to reduce file writes
        var sb = new StringBuilder();
        int batchSize = 10000;

        // Progress tracking variables
        var startTime = Stopwatch.GetTimestamp();
        int totalGenerated = 0;
        int count = options.NumberOfRecords;

        var orderItemsIds = new List<Guid>(count);

        for (int i = 1; i <= count; i++)
        {
            var numberOfProducts = Rnd.Next(1, 15);

            for (int j = 0; j < numberOfProducts; j++)
            {
                var id = Guid.NewGuid();
                var orderId = orderIds[Rnd.Next(productIds.Count)];
                var productId = productIds[Rnd.Next(productIds.Count)];
                var quantity = Rnd.Next(1, 20);

                orderItemsIds.Add(id);

                // Format all fields with quotes to ensure proper SQL interpretation
                sb.AppendLine($"\"{id}\",\"{orderId}\",\"{productId}\",\"{quantity}\"");
            }

            if (i % batchSize == 0 || i == count)
            {
                await writer.WriteAsync(sb.ToString());
                sb.Clear();

                totalGenerated = i;
                double percentComplete = (double)totalGenerated / count * 100;
                var elapsedTime = Stopwatch.GetElapsedTime(startTime);
                var estimatedTotalTime = TimeSpan.FromTicks((long)(elapsedTime.Ticks / (percentComplete / 100)));
                var remainingTime = estimatedTotalTime - elapsedTime;

                // Clear the current line
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

                // Print progress bar
                Console.Write($"Progress: [{percentComplete:F1}%] {totalGenerated:N0} of {count:N0} records | ");
                Console.Write($"Elapsed: {elapsedTime.ToShortDisplayString()}   | Remaining:   {remainingTime.ToShortDisplayString()}");
            }
        }

        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
        Console.WriteLine($"Finished inserting '{totalGenerated}' order items in {Stopwatch.GetElapsedTime(startTime)}");

        return orderItemsIds;
    }

    private static string RemoveCsvUnwantedCharacters(string field)
    {
        ArgumentNullException.ThrowIfNull(field);

        var unwanted = new[] { ',', '"', '\n', '\r' };
        var sb = new StringBuilder(field.Length);
        foreach (var c in field)
        {
            if (Array.IndexOf(unwanted, c) == -1)
                sb.Append(c);
        }
        return sb.ToString();
    }
}