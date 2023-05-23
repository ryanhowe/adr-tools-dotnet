using System.CommandLine;

namespace Adr.Tools;

public class Adr
{
    public string DiscoverAdrPath()
    {
        return string.Empty;
    }

    public static void Initialize(string? path)
    {
        path = path ?? "docs/adr";
        if (PathExists(path))
        {
            Console.WriteLine("Path already exists");
            return;
        }

        CreatePath(path);
        File.WriteAllText(path + "/0001-record-adr.md", EntryTemplate.InitTemplate());
    }

    private static void CreatePath(string path)
    {
        Directory.CreateDirectory(path);
    }

    private static bool PathExists(string path)
    {
        return Directory.Exists(path);
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

        newEntry.SetHandler(NewEntry, title, supersede);
        return newEntry;
    }

    private static void NewEntry(string? arg, int superseded)
    {
        Console.WriteLine($"{arg}");
        Console.WriteLine($"{superseded}");
    }

    public static Command GetInitCommand()
    {
        var path = new Option<string>("path", "Path to initialize ADR in (default docs/adr)");
        path.AddAlias("-p");

        var cmd = new Command("init", "Initialize ADR") { path };
        cmd.SetHandler(Initialize, path);

        return cmd;
    }
}
