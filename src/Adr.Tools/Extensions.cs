namespace Adr.Tools;

public static class Extensions
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

    public static string ReplaceTemplateDate(this string entryText, DateTime date)
    {
        return entryText.Replace("DATE", $"{date:yyyy-M-d}");
    }

    public static string ReplaceTemplateTitle(this string entryText, string title)
    {
        return entryText.Replace("TITLE", title);
    }

    public static string ReplaceTemplateNumber(this string entryText, int number)
    {
        return entryText.Replace("NUMBER", number.ToString());
    }

    public static string AddLinkToEntry(this string entryText, Entry linkedEntry, string linkText)
    {
        var link = $"{linkText} [{linkedEntry.Number}. {linkedEntry.Title}]({linkedEntry.FileName})";
        return entryText.Replace("## Context", $"{link}\n\n## Context");
    }

    public static string AddSupersededByLink(this string entryText, Entry supersedingEntry)
    {
        return entryText.AddLinkToEntry(supersedingEntry, "Superseded by");
    }

    public static string AddSupersedesLinks(this string entryText, int[] superseded, IEnumerable<Entry> entries)
    {
        foreach (var s in superseded)
        {
            var entry = entries.FirstOrDefault(e => e.Number == s);
            if (entry is null || string.IsNullOrEmpty(entry.FileName))
                continue;

            entryText = entryText.AddLinkToEntry(entry, "Supersedes");
        }

        return entryText;
    }

    public static string AddReverseEntryLink(this string entryText, Entry linkedEntry, LinkParameter link)
    {
        return entryText.AddLinkToEntry(linkedEntry, link.TargetText);
    }

    public static string AddEntryLinks(this string entryText, IEnumerable<LinkParameter> links,
        IEnumerable<Entry> entries)
    {
        foreach (var link in links)
        {
            var entry = entries.FirstOrDefault(e => e.Number == link.Number);
            if (entry is null || string.IsNullOrEmpty(entry.FileName))
                continue;

            entryText = entryText.AddLinkToEntry(entry, link.SourceText);
        }

        return entryText;
    }

    public static IEnumerable<LinkParameter> ParseLinkParameters(this string[] links)
    {
        // ex: "2:Clarifies:Clarified by"

        var linkParams = new List<LinkParameter>();
        foreach (var link in links)
        {
            var parts = links[0].Split(":");
            if (parts.Length != 3)
            {
                Console.WriteLine($"Invalid link parameter: {link}");
                continue;
            }

            int.TryParse(parts[0], out var number);
            if (number == 0)
            {
                Console.WriteLine($"Invalid entry Number {parts[0]}");
                continue;
            }

            linkParams.Add(new LinkParameter(number, parts[1], parts[2]));
        }

        return linkParams;
    }
}

public record LinkParameter(int Number, string SourceText, string TargetText);
