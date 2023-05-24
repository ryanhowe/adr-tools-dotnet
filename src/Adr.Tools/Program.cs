// See https://aka.ms/new-console-template for more information

using System.CommandLine;

using Adr.Tools;


var rootCommand = new RootCommand("ADR-Tools")
{
    Commands.GetInitCommand(),
    Commands.GetNewEntryCommand(),
    Commands.GetListEntriesCommand(),
    Commands.GetLinkEntriesCommand()
};

rootCommand.Description = "ADR-Tools \nTools for managing architectural decision logs";

await rootCommand.InvokeAsync(args);
