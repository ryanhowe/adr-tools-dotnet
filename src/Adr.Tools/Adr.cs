namespace Adr.Tools;

public class Adr
{
    public const string DefaultAdrPath = "docs/adr";
    private string _baseDirectory = string.Empty;
    private const string AdrConfig = ".adr-dir";

    internal Adr()
    {
    }

    public IEnumerable<Entry> Entries { get; private set; } = new List<Entry>();

    private void DiscoverAdrPath(string? path = null)
    {
        if (!string.IsNullOrEmpty(_baseDirectory))
            return;

        path ??= Directory.GetCurrentDirectory();
        var file = $"{path}/{AdrConfig}";
        if (File.Exists(file))
        {
            _baseDirectory = File.ReadAllText(file);
            if (PathExists(_baseDirectory))
                return;
        }

        if (IsRoot(path))
        {
            Console.WriteLine("ADR is not initialized");
            return;
        }

        var parent = Directory.GetParent(path)?.FullName;
        if (!string.IsNullOrEmpty(parent))
            DiscoverAdrPath(parent);
    }

    private bool IsRoot(string path)
    {
        return Directory.Exists(path + "/.git");
    }


    internal void Init(string path)
    {
        path = string.IsNullOrEmpty(path)
            ? DefaultAdrPath
            : path;

        if (PathExists(path) && PathContainsRecords(path))
        {
            Console.WriteLine($"ADR already exists in {path}");
            return;
        }

        CreatePath(path);
        File.WriteAllText(".adr-dir", _baseDirectory);
        File.WriteAllText(_baseDirectory + "/0001-record-adr.md", EntryTemplates.InitTemplate());
        Console.WriteLine($"ADR Log created at {_baseDirectory}");
    }

    private void CreatePath(string path)
    {
        var dir = Directory.CreateDirectory(path);
        _baseDirectory = dir.FullName;
    }


    internal void NewEntry(string title, int[] superseded, string[] links)
    {
        if (!DiscoverAdrEntries())
            return;

        var entry = Entry.Create(NextEntryNumber(), title);

        var entryLinks = links.ParseLinkParameters();

        var entryText = GetTemplateText()
            .ReplaceTemplateDate(DateTime.Today)
            .ReplaceTemplateNumber(entry.Number)
            .ReplaceTemplateTitle(entry.Title)
            .AddSupersedesLinks(superseded, Entries)
            .AddEntryLinks(entryLinks, Entries);

        WriteEntry(entry, entryText);

        UpdateSupersededEntries(superseded, entry);
        UpdateLinkedEntries(entryLinks, entry);

        Console.WriteLine($"{entry.Number} {entry.Title} {entry.FileName}");
    }

    private void UpdateLinkedEntries(IEnumerable<LinkParameter> entryLinks, Entry entry)
    {
        foreach (var linkParameter in entryLinks)
        {
            var linkedEntry = GetEntryByNumber(linkParameter.Number);
            if (linkedEntry is null || string.IsNullOrEmpty(linkedEntry.FileName))
            {
                Console.WriteLine($"Invalid entry number to be linked {linkParameter.Number}");
                continue;
            }

            var entryText = GetEntryText(linkedEntry);
            entryText = entryText.AddReverseEntryLink(entry, linkParameter);
            WriteEntry(linkedEntry, entryText);
        }
    }

    private void UpdateSupersededEntries(int[] supersededEntryNumbers, Entry entry)
    {
        foreach (var s in supersededEntryNumbers)
        {
            var supersededEntry = GetEntryByNumber(s);
            if (supersededEntry is null || string.IsNullOrEmpty(supersededEntry.FileName))
            {
                Console.WriteLine($"Invalid entry number to be superseded {s}");
                continue;
            }

            var entryText = GetEntryText(supersededEntry);
            entryText = entryText.AddSupersededByLink(entry);
            WriteEntry(supersededEntry, entryText);
        }
    }

    private Entry? GetEntryByNumber(int entryNumber) => Entries.FirstOrDefault(e => e.Number == entryNumber);


    private string GetEntryText(Entry entry)
    {
        DiscoverAdrPath();
        return File.Exists(FullEntryPath(entry))
            ? File.ReadAllText(FullEntryPath(entry))
            : string.Empty;
    }

    private string FullEntryPath(Entry entry) => $"{_baseDirectory}/{entry.FileName}";


    private string GetTemplateText()
    {
        DiscoverAdrPath();
        return File.Exists(TemplatePath)
            ? File.ReadAllText(TemplatePath)
            : EntryTemplates.TemplateText;
    }

    private string TemplatePath => $"{_baseDirectory}/template.md";

    private void WriteEntry(Entry entry, string entryText)
    {
        File.WriteAllText(_baseDirectory + $"/{entry.FileName}", entryText);
    }

    private int NextEntryNumber()
    {
        var lastEntry = Entries.Last();
        return lastEntry.Number + 1;
    }

    private bool DiscoverAdrEntries()
    {
        DiscoverAdrPath();
        if (string.IsNullOrEmpty(_baseDirectory))
            return false;

        Entries = Entry.From(Directory
            .EnumerateFiles(_baseDirectory, "*.md")
            .Select(Path.GetFileName)
            .ToArray());

        return true;
    }

    internal void ListEntries()
    {
        if (!DiscoverAdrEntries())
            return;

        foreach (var entry in Entries)
        {
            Console.WriteLine(entry.ToString());
        }
    }


    private static bool PathContainsRecords(string path)
    {
        return Directory.GetFiles(path, "*.md").Any();
    }


    private static bool PathExists(string path)
    {
        return Directory.Exists(path);
    }

    public void LinkEntries(int source, string sourceLink, int target, string targetLink)
    {
        if (!DiscoverAdrEntries())
            return;

        var sourceEntry = GetEntryByNumber(source);
        var targetEntry = GetEntryByNumber(target);

        if (sourceEntry is null || string.IsNullOrEmpty(sourceEntry.FileName))
        {
            Console.Write($"Invalid source entry {source}");
            return;
        }

        if (targetEntry is null || string.IsNullOrEmpty(targetEntry.FileName))
        {
            Console.Write($"Invalid target entry {target}");
            return;
        }

        var sourceText = GetEntryText(sourceEntry)
            .AddLinkToEntry(targetEntry, sourceLink);

        var targetText = GetEntryText(targetEntry)
            .AddLinkToEntry(sourceEntry, targetLink);
        
        WriteEntry(sourceEntry, sourceText);
        WriteEntry(targetEntry, targetText);
    }
}
