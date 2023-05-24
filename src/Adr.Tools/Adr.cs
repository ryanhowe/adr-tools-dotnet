using System.CommandLine;

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
        File.WriteAllText(_baseDirectory + "/0001-record-adr.md", EntryTemplate.InitTemplate());
        Console.WriteLine($"ADR Log created at {_baseDirectory}");
    }

    private void CreatePath(string path)
    {
        var dir = Directory.CreateDirectory(path);
        _baseDirectory = dir.FullName;
    }


    internal void NewEntry(string? arg, int superseded)
    {
        DiscoverAdrPath();
        if (string.IsNullOrEmpty(_baseDirectory))
            return;

        DiscoverAdrEntries();

        var entry = new Entry(NextEntryNumber(), arg, arg.ToFileName());
        Console.WriteLine($"{entry.Number} {entry.Title} {entry.FileName}");
    }

    private int NextEntryNumber()
    {
        var lastEntry = Entries.Last();
        return lastEntry.Number + 1;
    }

    private void DiscoverAdrEntries()
    {
        if (string.IsNullOrEmpty(_baseDirectory))
            return;
        
        var files = Directory.EnumerateFiles(_baseDirectory, "*.md").Select(Path.GetFileName).ToArray();
        Entries = Entry.From(files);
    }




    internal void ListEntries()
    {
        DiscoverAdrPath();
        DiscoverAdrEntries();
         
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
        DiscoverAdrPath();
        DiscoverAdrEntries();
    }
}
