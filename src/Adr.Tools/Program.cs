// See https://aka.ms/new-console-template for more information

using System.CommandLine;

using Adr.Tools;


var rootCommand = new RootCommand("ADR-Tools")
{
    Adr.Tools.Adr.GetInitCommand(),
    Adr.Tools.Adr.GetNewEntryCommand(),
};

rootCommand.Description = "ADR-Tools \nTools for managing architectural decision logs";

// rootCommand.SetHandler(() =>
// {
//     
// });

await rootCommand.InvokeAsync(args);
