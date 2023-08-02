namespace PersonApi.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string? text)
    {
        return !string.IsNullOrEmpty(text);
    }

    public static IEnumerable<string> SplitToList(this string text)
    {
        return text.Replace(" ", "").Split(',').ToList();
    }
}
