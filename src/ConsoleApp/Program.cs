using ConsoleApp.DataGenerator;
using ConsoleApp.Models;
using System.Data;
using System.Diagnostics;
using System.Text;

public static class Program
{
    
    static readonly int _defaultBatchSize = 100_000;
    static readonly string ConnectionString = "Server=.;Database=LoadTestingDb;Trusted_Connection=True;TrustServerCertificate=True;";

    static async Task Main(string[] args)
    {
        var options = ParseAndValidateArguments(args);
        var startTime = Stopwatch.GetTimestamp();

        switch (options.ExecutionMode)
        {
            case ExecutionMode.Create:
                await TestDataGenerator.GenerateDataAsync(options);
                break;
            case ExecutionMode.Insert:
                await TestDataInserter.InsertGeneratedDataDirectlyAsync(ConnectionString, _defaultBatchSize);
                break;
            default:
                throw new InvalidOperationException();
        }

        Console.WriteLine($"Finished in '{Stopwatch.GetElapsedTime(startTime)}'");
    }

    private static Options ParseAndValidateArguments(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            throw new ArgumentException("No arguments provided");
        }

        string? executionModeArg = null;
        string? numberOfRecordsArg = null;

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--mode":
                    executionModeArg = GetArgumentValue(args, i);
                    break;
                case "--records":
                    numberOfRecordsArg = GetArgumentValue(args, i);
                    break;
            }
        }

        // Collect all missing arguments
        var missingArguments = new List<string>();

        // Required arguments
        if (executionModeArg == null) missingArguments.Add("Execution mode (--mode)");

        if (missingArguments.Count > 0)
        {
            throw new ArgumentException($"Missing required arguments:\n- {string.Join("\n- ", missingArguments)}");
        }

        // Validate argument values
        var validationErrors = new List<string>();

        if (!Enum.TryParse(executionModeArg, true, out ExecutionMode executionMode) || executionMode == ExecutionMode.Invalid)
        {
            validationErrors.Add("Execution mode must be either 'Create' or 'Insert'");
        }

        int numberOfRecords = 0;
        if (executionMode == ExecutionMode.Create)
        {
            if (!int.TryParse(numberOfRecordsArg, out numberOfRecords))
            {
                validationErrors.Add("Number of records is not a positive integer bigger than zero");
            }
            if (numberOfRecords <= 0)
            {
                validationErrors.Add("Number of records must be a positive integer bigger than zero");
            }
        }

        if (validationErrors.Count > 0)
        {
            throw new ArgumentException($"Validation errors:\n- {string.Join("\n- ", validationErrors)}");
        }

        return new Options
        {
            ExecutionMode = executionMode,
            NumberOfRecords = numberOfRecords
        };
    }

    private static string GetArgumentValue(string[] args, int index)
    {
        if (index + 1 < args.Length && !args[index + 1].StartsWith("--"))
        {
            return args[index + 1];
        }

        throw new ArgumentException($"Value for argument {args[index]} is missing");
    }

}