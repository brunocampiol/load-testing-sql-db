using System.Text;

namespace ConsoleApp.Extensions;

public static class StringExtensions
{
    public static string RemoveAllCharacterOccurrences(this string text, char character)
    {
        ArgumentNullException.ThrowIfNull(text);

        // Use StringBuilder for efficient string manipulation.
        var result = new StringBuilder();

        foreach (var ch in text)
        {
            if (ch != character)
            {
                result.Append(ch);
            }
        }

        return result.ToString();
    }
}