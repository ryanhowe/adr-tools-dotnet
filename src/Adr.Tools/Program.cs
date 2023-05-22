// See https://aka.ms/new-console-template for more information

using System.CommandLine;

using Adr.Tools;


var rootCommand = new RootCommand("ADR-Tools")
{
    Entry.GetCommand(),
    Initializer.GetCommand()
};

rootCommand.Description = "ADR-Tools \nTools for managing architectural decision logs";

rootCommand.SetHandler(() =>
{
    Console.WriteLine("Hello");
});

await rootCommand.InvokeAsync(args);
