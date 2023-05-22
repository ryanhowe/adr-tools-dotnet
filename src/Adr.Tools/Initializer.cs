using System.CommandLine;

namespace Adr.Tools;

public class Initializer
{
    public static void Initialize(string? path)
    {
        path = path ?? "docs/adr";
        if (PathExists(path))
        {
            Console.WriteLine("Path already exists");
            return;
        }

        CreatePath(path);
        File.WriteAllText(path + "/0001-record-adr.md", Entry.Template);
    }

    private static void CreatePath(string path)
    {
        Directory.CreateDirectory(path);
    }

    private static bool PathExists(string path)
    {
        return Directory.Exists(path);
    }

    public static Command GetCommand()
    {
        var path = new Option<string>("path", "Path to initialize ADR in (default docs/adr)");
        path.AddAlias("-p");
        var cmd = new Command("init", "Initialize ADR") { path };
        cmd.SetHandler(Initialize, path);
        return cmd;
    }
}
