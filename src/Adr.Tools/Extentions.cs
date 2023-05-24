namespace Adr.Tools;

public static class Extentions
{
    public static (int number, string title) ParseEntryNumberAndTitle(this string file)
    {
        // 0001-record-adr
        var parts = file.Split('-');

        int.TryParse(parts[0], out var number);

        var title = String.Join(' ', parts.Take(1..parts.Length)).TrimEnd(".md".ToCharArray());
        title = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        return (number, title);
    }

    public static string ToFileName(this Entry entry)
    {
        return $"{entry.Number}-{entry.Title.ToFileName()}.md";
    }

    public static string ToFileName(this string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new ArgumentOutOfRangeException(title, $"{nameof(title)} can not be null or empty");
        
        return $"{title.ToLower().Replace(' ', '-')}.md";
    }
}
