using ConsoleApp.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace ConsoleApp.DataGenerator;

public static class TestDataInserter
{
    public static async Task InsertGeneratedDataDirectlyAsync(string connectionString, int batchSize)
    {
        // TODO: Compare timings with the native SQL
        // This is a way to insert from console app to the SQL database
        // There is a alternative way which is to use the sql script in sql management studio
        Console.WriteLine("Start inserting records");
        var startTime = Stopwatch.GetTimestamp();

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        Console.WriteLine("Connected to SQL Server successfully");

        // Insert Customers
        var customerCount = await InsertCustomersInDbFromFileAsync(connection, batchSize);

        // Insert Products
        var productCount = await InsertProductsInDbFromFileAsync(connection, batchSize);

        // Insert Orders
        var orderCount = await InsertOrdersInDbFromFileAsync(connection, batchSize);

        // Insert OrderItems
        var orderItemCount = await InsertOrderItemsInDbFromFileAsync(connection, batchSize);

        var totalItems = customerCount + productCount + orderCount + orderItemCount;
        var totalTime = Stopwatch.GetElapsedTime(startTime);
        Console.WriteLine("\n\nCompleted generating '{0:N0}' records in {1}", totalItems, totalTime.ToShortDisplayString());
        Console.WriteLine("Average speed: '{0:N0}' records/second", totalItems / totalTime.TotalSeconds);
    }

    private static async Task<int> InsertOrderItemsInDbFromFileAsync(SqlConnection sqlConnection, int batchSize)
    {
        var fileName = $"_orderItems.csv";
        var fullFilePath = $@"{AppContext.BaseDirectory}\{fileName}";
        if (!File.Exists(fullFilePath))
        {
            Console.WriteLine($"File '{fullFilePath}' does not exist. Please generate data first.");
            return 0;
        }

        Console.WriteLine($"Reading order items from file '{fileName}'.");

        var fileLines = await ReadAllCsvLinesAsync(fullFilePath);
        var numberOfItems = fileLines.Length - 1;

        // Matching table structure
        DataTable table = new DataTable();
        table.Columns.Add("ID", typeof(Guid));
        table.Columns.Add("OrderId", typeof(Guid));
        table.Columns.Add("ProductId", typeof(Guid));
        table.Columns.Add("Quantity", typeof(int));

        int totalInserted = 0;
        int linesRead = 0;
        var startTime = Stopwatch.GetTimestamp();

        Console.WriteLine($"Inserting '{numberOfItems:N0}' order item records in batches of '{batchSize:N0}'.");

        using (var bulkCopy = new SqlBulkCopy(sqlConnection)
        {
            DestinationTableName = "OrderItems",
            BatchSize = batchSize,
            BulkCopyTimeout = 0 // No timeout
        })
        {
            // Add column mappings
            bulkCopy.ColumnMappings.Add("ID", "ID");
            bulkCopy.ColumnMappings.Add("OrderId", "OrderId");
            bulkCopy.ColumnMappings.Add("ProductId", "ProductId");
            bulkCopy.ColumnMappings.Add("Quantity", "Quantity");

            // Set up notification for each batch completion
            bulkCopy.SqlRowsCopied += (sender, e) =>
            {
                Console.WriteLine($"Batch copied: {e.RowsCopied} rows");
            };

            for (int i = 0; i < numberOfItems; i += batchSize)
            {
                table.Clear();

                // Generate one batch of records
                int currentBatchSize = Math.Min(batchSize, numberOfItems - i);
                for (int j = 0; j < currentBatchSize; j++)
                {
                    var currentLine = fileLines[linesRead + 1];
                    var lineContents = currentLine.Split(',');

                    if (lineContents.Length != 4) throw new InvalidOperationException($"Invalid line data: '{currentLine}'");

                    var id = lineContents[0].RemoveAllCharacterOccurrences('"');
                    var orderId = lineContents[1].RemoveAllCharacterOccurrences('"');
                    var productId = lineContents[2].RemoveAllCharacterOccurrences('"');
                    var quantity = lineContents[3].RemoveAllCharacterOccurrences('"');

                    table.Rows.Add(id, orderId, productId, quantity);
                    linesRead++;
                }

                // Write the batch to SQL Server
                await bulkCopy.WriteToServerAsync(table);

                totalInserted += currentBatchSize;
                double percentComplete = (double)totalInserted / numberOfItems * 100;
                var elapsedTime = Stopwatch.GetElapsedTime(startTime);
                var estimatedTotalTime = TimeSpan.FromTicks((long)(elapsedTime.Ticks / (percentComplete / 100)));
                var remainingTime = estimatedTotalTime - elapsedTime;

                // Clear the current line
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

                // Print progress bar
                Console.Write($"Progress: [{percentComplete:F1}%] {totalInserted:N0} of {numberOfItems:N0} records | ");
                Console.Write($"Elapsed: {elapsedTime.ToShortDisplayString()}  | Remaining:  {remainingTime.ToShortDisplayString()}");
            }
        }

        var totalTime = Stopwatch.GetElapsedTime(startTime);
        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
        Console.WriteLine("Completed inserting {0:N0} records in {1}", totalInserted, totalTime.ToShortDisplayString());
        Console.WriteLine("Average speed: {0:N0} records/second", totalInserted / totalTime.TotalSeconds);
        return totalInserted;
    }

    private static async Task<int> InsertOrdersInDbFromFileAsync(SqlConnection sqlConnection, int batchSize)
    {
        var fileName = $"_orders.csv";
        var fullFilePath = $@"{AppContext.BaseDirectory}\{fileName}";
        if (!File.Exists(fullFilePath))
        {
            Console.WriteLine($"File '{fullFilePath}' does not exist. Please generate data first.");
            return 0;
        }

        Console.WriteLine($"Reading order from file '{fileName}'.");

        var fileLines = await ReadAllCsvLinesAsync(fullFilePath);
        var numberOfItems = fileLines.Length - 1;

        // Matching table structure
        DataTable table = new DataTable();
        table.Columns.Add("ID", typeof(Guid));
        table.Columns.Add("CustomerId", typeof(Guid));
        table.Columns.Add("CreatedAt", typeof(DateTime));

        int totalInserted = 0;
        int linesRead = 0;
        var startTime = Stopwatch.GetTimestamp();

        Console.WriteLine($"Inserting '{numberOfItems:N0}' orders records in batches of '{batchSize:N0}'.");

        using (var bulkCopy = new SqlBulkCopy(sqlConnection)
        {
            DestinationTableName = "Orders",
            BatchSize = batchSize,
            BulkCopyTimeout = 0 // No timeout
        })
        {
            // Add column mappings
            bulkCopy.ColumnMappings.Add("ID", "ID");
            bulkCopy.ColumnMappings.Add("CustomerId", "CustomerId");
            bulkCopy.ColumnMappings.Add("CreatedAt", "CreatedAt");

            // Set up notification for each batch completion
            bulkCopy.SqlRowsCopied += (sender, e) =>
            {
                Console.WriteLine($"Batch copied: {e.RowsCopied} rows");
            };

            for (int i = 0; i < numberOfItems; i += batchSize)
            {
                table.Clear();

                // Generate one batch of records
                int currentBatchSize = Math.Min(batchSize, numberOfItems - i);
                for (int j = 0; j < currentBatchSize; j++)
                {
                    var currentLine = fileLines[linesRead + 1];
                    var lineContents = currentLine.Split(',');

                    if (lineContents.Length != 3) throw new InvalidOperationException($"Invalid line data: '{currentLine}'");

                    var id = lineContents[0].RemoveAllCharacterOccurrences('"');
                    var customerId = lineContents[1].RemoveAllCharacterOccurrences('"');
                    var createdDate = lineContents[2].RemoveAllCharacterOccurrences('"');

                    table.Rows.Add(id, customerId, createdDate);
                    linesRead++;
                }

                // Write the batch to SQL Server
                await bulkCopy.WriteToServerAsync(table);

                totalInserted += currentBatchSize;
                double percentComplete = (double)totalInserted / numberOfItems * 100;
                var elapsedTime = Stopwatch.GetElapsedTime(startTime);
                var estimatedTotalTime = TimeSpan.FromTicks((long)(elapsedTime.Ticks / (percentComplete / 100)));
                var remainingTime = estimatedTotalTime - elapsedTime;

                // Clear the current line
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

                // Print progress bar
                Console.Write($"Progress: [{percentComplete:F1}%] {totalInserted:N0} of {numberOfItems:N0} records | ");
                Console.Write($"Elapsed: {elapsedTime.ToShortDisplayString()}  | Remaining:  {remainingTime.ToShortDisplayString()}");
            }
        }

        var totalTime = Stopwatch.GetElapsedTime(startTime);
        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
        Console.WriteLine("Completed inserting {0:N0} records in {1}", totalInserted, totalTime.ToShortDisplayString());
        Console.WriteLine("Average speed: {0:N0} records/second", totalInserted / totalTime.TotalSeconds);
        return totalInserted;
    }

    private static async Task<int> InsertProductsInDbFromFileAsync(SqlConnection sqlConnection, int batchSize)
    {
        var fileName = $"_products.csv";
        var fullFilePath = $@"{AppContext.BaseDirectory}\{fileName}";
        if (!File.Exists(fullFilePath))
        {
            Console.WriteLine($"File '{fullFilePath}' does not exist. Please generate data first.");
            return 0;
        }

        Console.WriteLine($"Reading products from file '{fileName}'.");

        var fileLines = await ReadAllCsvLinesAsync(fullFilePath);
        var numberOfItems = fileLines.Length - 1;

        // Matching table structure
        DataTable table = new DataTable();
        table.Columns.Add("ID", typeof(Guid));
        table.Columns.Add("ProductName", typeof(string));
        table.Columns.Add("Price", typeof(Decimal));
        table.Columns.Add("CreatedAt", typeof(DateTime));

        int totalInserted = 0;
        int linesRead = 0;
        var startTime = Stopwatch.GetTimestamp();

        Console.WriteLine($"Inserting '{numberOfItems:N0}' products in batches of '{batchSize:N0}'.");

        using (var bulkCopy = new SqlBulkCopy(sqlConnection)
        {
            DestinationTableName = "Products",
            BatchSize = batchSize,
            BulkCopyTimeout = 0 // No timeout
        })
        {
            // Add column mappings
            bulkCopy.ColumnMappings.Add("ID", "ID");
            bulkCopy.ColumnMappings.Add("ProductName", "ProductName");
            bulkCopy.ColumnMappings.Add("Price", "Price");
            bulkCopy.ColumnMappings.Add("CreatedAt", "CreatedAt");

            // Set up notification for each batch completion
            bulkCopy.SqlRowsCopied += (sender, e) =>
            {
                Console.WriteLine($"Batch copied: {e.RowsCopied} rows");
            };

            for (int i = 0; i < numberOfItems; i += batchSize)
            {
                table.Clear();

                // Generate one batch of records
                int currentBatchSize = Math.Min(batchSize, numberOfItems - i);
                for (int j = 0; j < currentBatchSize; j++)
                {
                    var currentLine = fileLines[linesRead + 1];
                    var lineContents = currentLine.Split(',');

                    if (lineContents.Length != 4) throw new InvalidOperationException($"Invalid line data: '{currentLine}'");

                    var id = lineContents[0].RemoveAllCharacterOccurrences('"');
                    var productName = lineContents[1].RemoveAllCharacterOccurrences('"');
                    var price = lineContents[2].RemoveAllCharacterOccurrences('"');
                    var createdDate = lineContents[3].RemoveAllCharacterOccurrences('"');

                    table.Rows.Add(id, productName, price, createdDate);
                    linesRead++;
                }

                // Write the batch to SQL Server
                await bulkCopy.WriteToServerAsync(table);

                totalInserted += currentBatchSize;
                double percentComplete = (double)totalInserted / numberOfItems * 100;
                var elapsedTime = Stopwatch.GetElapsedTime(startTime);
                var estimatedTotalTime = TimeSpan.FromTicks((long)(elapsedTime.Ticks / (percentComplete / 100)));
                var remainingTime = estimatedTotalTime - elapsedTime;

                // Clear the current line
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

                // Print progress bar
                Console.Write($"Progress: [{percentComplete:F1}%] {totalInserted:N0} of {numberOfItems:N0} records | ");
                Console.Write($"Elapsed: {elapsedTime.ToShortDisplayString()}  | Remaining:  {remainingTime.ToShortDisplayString()}");
            }
        }

        var totalTime = Stopwatch.GetElapsedTime(startTime);
        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
        Console.WriteLine("Completed inserting {0:N0} records in {1}", totalInserted, totalTime.ToShortDisplayString());
        Console.WriteLine("Average speed: {0:N0} records/second", totalInserted / totalTime.TotalSeconds);
        return totalInserted;
    }

    private static async Task<int> InsertCustomersInDbFromFileAsync(SqlConnection sqlConnection, int batchSize)
    {
        var fileName = $"_customers.csv";
        var fullFilePath = $@"{AppContext.BaseDirectory}\{fileName}";
        if (!File.Exists(fullFilePath))
        {
            Console.WriteLine($"File '{fullFilePath}' does not exist. Please generate data first.");
            return 0;
        }

        Console.WriteLine($"Reading customers from file '{fileName}'.");

        var fileLines = await ReadAllCsvLinesAsync(fullFilePath);
        var numberOfItems = fileLines.Length - 1;

        // Matching table structure
        DataTable table = new DataTable();
        table.Columns.Add("ID", typeof(Guid));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("DateOfBirth", typeof(DateTime));
        table.Columns.Add("CreatedAt", typeof(DateTime));

        int totalInserted = 0;
        int linesRead = 0;
        var startTime = Stopwatch.GetTimestamp();

        Console.WriteLine($"Inserting '{numberOfItems:N0}' products in batches of '{batchSize:N0}'.");

        using (var bulkCopy = new SqlBulkCopy(sqlConnection)
        {
            DestinationTableName = "Customers",
            BatchSize = batchSize,
            BulkCopyTimeout = 0 // No timeout
        })
        {
            // Add column mappings
            bulkCopy.ColumnMappings.Add("ID", "ID");
            bulkCopy.ColumnMappings.Add("Name", "Name");
            bulkCopy.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
            bulkCopy.ColumnMappings.Add("CreatedAt", "CreatedAt");

            // Set up notification for each batch completion
            bulkCopy.SqlRowsCopied += (sender, e) =>
            {
                Console.WriteLine($"Batch copied: {e.RowsCopied} rows");
            };

            for (int i = 0; i < numberOfItems; i += batchSize)
            {
                table.Clear();

                // Generate one batch of records
                int currentBatchSize = Math.Min(batchSize, numberOfItems - i);
                for (int j = 0; j < currentBatchSize; j++)
                {
                    var currentLine = fileLines[linesRead + 1];
                    var lineContents = currentLine.Split(',');

                    if (lineContents.Length != 4) throw new InvalidOperationException($"Invalid line data: '{currentLine}'");

                    var id = lineContents[0].RemoveAllCharacterOccurrences('"');
                    var name = lineContents[1].RemoveAllCharacterOccurrences('"');
                    var dob = lineContents[2].RemoveAllCharacterOccurrences('"');
                    var createdDate = lineContents[3].RemoveAllCharacterOccurrences('"');

                    table.Rows.Add(id, name, dob, createdDate);
                    linesRead++;
                }

                // Write the batch to SQL Server
                await bulkCopy.WriteToServerAsync(table);

                totalInserted += currentBatchSize;
                double percentComplete = (double)totalInserted / numberOfItems * 100;
                var elapsedTime = Stopwatch.GetElapsedTime(startTime);
                var estimatedTotalTime = TimeSpan.FromTicks((long)(elapsedTime.Ticks / (percentComplete / 100)));
                var remainingTime = estimatedTotalTime - elapsedTime;

                // Clear the current line
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

                // Print progress bar
                Console.Write($"Progress: [{percentComplete:F1}%] {totalInserted:N0} of {numberOfItems:N0} records | ");
                Console.Write($"Elapsed: {elapsedTime.ToShortDisplayString()}   | Remaining:   {remainingTime.ToShortDisplayString()}");
            }
        }

        var totalTime = Stopwatch.GetElapsedTime(startTime);
        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
        Console.WriteLine("Completed inserting {0:N0} records in {1}", totalInserted, totalTime.ToShortDisplayString());
        Console.WriteLine("Average speed: {0:N0} records/second", totalInserted / totalTime.TotalSeconds);
        return totalInserted;
    }

    private static async Task<string[]> ReadAllCsvLinesAsync(string fullFilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullFilePath);

        using (var reader = new StreamReader(fullFilePath))
        {
            var content = await reader.ReadToEndAsync();
            var contentLines = content.Split(Environment.NewLine);
            // TODO: remove empty line in generation process
            // If removing, remember to update the all insert logic
            return [.. contentLines.Where(x => !string.IsNullOrWhiteSpace(x))];
        }
    }
}