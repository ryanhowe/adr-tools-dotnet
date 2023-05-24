using System.CommandLine;

namespace Adr.Tools;

public static class Commands
{
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

    public static Command GetLinkEntriesCommand()
    {
        var link = new Command("link", 
        @"Creates a link between two ADRs, from SOURCE to TARGET new.
SOURCE and TARGET are both a reference (number or partial filename) to an ADR
LINK is the description of the link created in the SOURCE.
    REVERSE-LINK is the description of the link created in the TARGET

E.g. to create link ADR 12 to ADR 10

adr link 12 Amends 10 ""Amended by""");
        
        var source = new Argument<Int32>("source", "Source entry number");
        var target = new Argument<Int32>("target", "Target entry number");
        var sourceLink = new Argument<string>("source-text", "Source link text");
        sourceLink.SetDefaultValue("Amends");
        var targetLink = new Argument<string>("target-text", "target link text");
        targetLink.SetDefaultValue("Amended by");
        link.AddArgument(source);
        link.AddArgument(sourceLink);
        link.AddArgument(target);
        link.AddArgument(targetLink);
        
        link.SetHandler(LinkHandler,source, sourceLink, target, targetLink);
        return link;

        void LinkHandler(int source, string sourceLink, int target, string targetLink)
        {
            var adr = new Adr();
            adr.LinkEntries(source, sourceLink, target, targetLink);
        }
    }

    public static Command GetInitCommand()
    {
        var path = new Option<string>("path", "Path to initialize ADR in (default docs/adr)");
        path.AddAlias("-p");
        path.SetDefaultValue(Adr.DefaultAdrPath);

        var cmd = new Command("init", "InitHandler ADR") { path };
        cmd.SetHandler(InitHandler, path);
        return cmd;

        void InitHandler(string? path)
        {
            var adr = new Adr();
            adr.Init(path ?? Adr.DefaultAdrPath);
        }
    }


    public static Command GetListEntriesCommand()
    {
        var listEntries = new Command("list", "List all entries");
        listEntries.AddAlias("-l");
        listEntries.SetHandler(ListEntries);

        return listEntries;

        void ListEntries()
        {
            var adr = new Adr();
            adr.ListEntries();
        }
    }
}
