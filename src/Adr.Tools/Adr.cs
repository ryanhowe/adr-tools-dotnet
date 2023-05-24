using System.CommandLine;

namespace Adr.Tools;

public class Adr
{
    private const string DefaultAdrPath = "docs/adr";
    private string _baseDirectory = string.Empty;
    private const string AdrConfig = ".adr-dir";

    private Adr()
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
            Console.WriteLine($"{path}");
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


    private void Init(string path)
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


    private void NewEntry(string? arg, int superseded)
    {
        DiscoverAdrPath();
        if (string.IsNullOrEmpty(_baseDirectory))
            return;

        DiscoverAdrEntries();
        Console.WriteLine($"{_baseDirectory}");
        Console.WriteLine($"{arg}");
        Console.WriteLine($"{superseded}");
    }

    private void DiscoverAdrEntries()
    {
        var files = Directory.GetFiles(_baseDirectory);
        Entries = Entry.From(files);
    }


    public static Command GetNewEntryCommand()
    {
        var newEntry = new Command("new", "Generate new ADR");
        newEntry.AddAlias("-n");

        var title = new Argument<string>("title", "Ex: \"Record architecture decisions\"");
        newEntry.AddArgument(title);

        var supersede = new Option<Int32>("supersedes", "The ADR number to be superseded");
        supersede.AddAlias("-s");

        newEntry.AddOption(supersede);

        newEntry.SetHandler(NewEntryHandler, title, supersede);
        return newEntry;

        void NewEntryHandler(string? arg, int superseded)
        {
            var adr = new Adr();
            adr.NewEntry(arg, superseded);
        }
    }

    public static Command GetInitCommand()
    {
        var path = new Option<string>("path", "Path to initialize ADR in (default docs/adr)");
        path.AddAlias("-p");
        path.SetDefaultValue(DefaultAdrPath);

        var cmd = new Command("init", "InitHandler ADR") { path };
        cmd.SetHandler(InitHandler, path);
        return cmd;

        void InitHandler(string? path)
        {
            var adr = new Adr();
            adr.Init(path ?? DefaultAdrPath);
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
}
