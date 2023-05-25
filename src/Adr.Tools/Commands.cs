using System.CommandLine;

namespace Adr.Tools;

public static class Commands
{
    public static Command GetNewEntryCommand()
    {
        var newEntry = new Command("new", Help.NewDescription);
        newEntry.AddAlias("-n");

        var supersede = SupersedeOption();
        newEntry.AddOption(supersede);

        var link = LinkOption();
        newEntry.AddOption(link);

        var title = new Argument<string>("title", Help.TitleOptionDescription);
        newEntry.AddArgument(title);

        newEntry.SetHandler(NewEntryHandler, title, supersede, link);
        return newEntry;

        void NewEntryHandler(string title, int[] superseded, string[] links)
        {
            var adr = new Adr();
            adr.NewEntry(title, superseded, links);
        }
    }


    private static Option<string[]> LinkOption()
    {
        var link = new Option<string[]>("link",Help.LinkOptionDescription);
        link.AddAlias("-l");
        link.Arity = ArgumentArity.ZeroOrMore;
        return link;
    }

    private static Option<int[]> SupersedeOption()
    {
        var supersede = new Option<int[]>("supersedes", Help.SupersedeOptionDescription);
        supersede.AddAlias("-s");
        supersede.Arity = ArgumentArity.ZeroOrMore;
        return supersede;
    }

    public static Command GetLinkEntriesCommand()
    {
        var link = new Command("link", Help.LinkCommandDescription);

        var source = new Argument<Int32>("source", Help.SourceArgumentDescription);
        link.AddArgument(source);

        var sourceLink = new Argument<string>("source-text", Help.SourceTextArgumentDescription);
        link.AddArgument(sourceLink);
        sourceLink.SetDefaultValue("Amends");

        var target = new Argument<Int32>("target", Help.TargetArgumentDescription);
        link.AddArgument(target);

        var targetLink = new Argument<string>("target-text", Help.TargetTextArgumentDescription);
        targetLink.SetDefaultValue("Amended by");
        link.AddArgument(targetLink);

        link.SetHandler(LinkHandler, source, sourceLink, target, targetLink);
        return link;

        void LinkHandler(int source, string sourceLink, int target, string targetLink)
        {
            var adr = new Adr();
            adr.LinkEntries(source, sourceLink, target, targetLink);
        }
    }

    public static Command GetInitCommand()
    {
        var cmd = new Command("init", Help.InitCommandDescription);

        var path = new Option<string>("path", Help.PathOptionDescription);
        path.AddAlias("-p");
        path.SetDefaultValue(Adr.DefaultAdrPath);
        cmd.AddOption(path);

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
        var listEntries = new Command("list", Help.ListCommandDescription);
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
