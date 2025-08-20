namespace SLN.Utility.Extensions;

public static class StringExtension
{
    public static List<string> ToLines(this string str)
    {
        if (string.IsNullOrEmpty(str)) return new List<string>();

        return str.Replace("\r\n", "\n")
            .Split("\n")
            .ToList();
    }
}