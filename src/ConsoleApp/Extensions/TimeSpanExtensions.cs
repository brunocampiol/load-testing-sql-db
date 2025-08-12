namespace ConsoleApp.Extensions;

public static class TimeSpanExtensions
{
    public static string ToShortDisplayString(this TimeSpan ts)
    {
        if (ts.TotalHours >= 1) return $"{ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
        else if (ts.TotalMinutes >= 1) return $"{ts.Minutes}m {ts.Seconds}s";
        else return $"{ts.Seconds}s";
    }
}