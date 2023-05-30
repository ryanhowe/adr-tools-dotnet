using Adr.Tools;

public record Entry(int Number, string Title, string FileName)
{
    public static IEnumerable<Entry> From(string?[] files)
    {
        var entries = new List<Entry>();
        foreach (var file in files)
        {
            if (string.IsNullOrEmpty(file))
                continue;

            if (string.Equals(file, "template.md", StringComparison.OrdinalIgnoreCase))
                continue;

            entries.Add(From(file));
        }

        return entries;
    }

    public override string ToString() => $"{Number}. {Title}";

    private static Entry From(string file)
    {
        (int number, string title) = file.ParseEntryNumberAndTitle();
        return new Entry(number, title, file);
    }

    public static Entry Create(int entryNumber, string title)
    {
        var fileName = $"{entryNumber:0000}-{title.ToFileName()}";
        return new Entry(entryNumber, title, fileName);
    }
}
