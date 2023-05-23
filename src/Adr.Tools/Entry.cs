using System.CommandLine;

public class Entry
{
    public static Command GetCommand()
    {
        var newEntry = new Command("new");
        newEntry.AddAlias("-n");
        var title = new Argument<string>("title");
        newEntry.AddArgument(title);
        var supersede = new Option<Int32>("supersede");
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

    // internal static string Template() =>
    //     @$"# 1. Record Architecture Decisions
    //
    // {DateTime.Now:yyyy-M-d}
    //
    // ## Status
    //
    // Accepted
    //
    // ## Context
    //
    // We need to record the architectural decisions made on this project.
    //
    // ## Decision
    //
    // We will use Architecture Decision Records, as described by Michael Nygard in this article: <http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions>.
    //
    // ## Consequences
    //
    // See Michael Nygard's article, linked above.";
}
