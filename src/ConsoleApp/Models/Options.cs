namespace ConsoleApp.Models;

public record Options
{
    public required ExecutionMode ExecutionMode { get; init; }
    public required int NumberOfRecords { get; init; }
}