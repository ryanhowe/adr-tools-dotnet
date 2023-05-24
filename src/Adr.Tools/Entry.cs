using Adr.Tools;

using static System.Int32;

public record Entry(int Number, string Title, string FileName)
{
    public static IEnumerable<Entry> From(string[] files)
    {
        var entries = new List<Entry>();
        foreach (var file in files)
        {
            entries.Add(From(file));
        }

        return entries;
    }

    private static Entry From(string file)
    {
        (int number, string title) = file.ParseEntryNumberAndTitle();
        return new Entry(number, title, file);
    }
}
