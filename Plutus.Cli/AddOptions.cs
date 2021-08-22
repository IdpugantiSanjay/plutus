// See https://aka.ms/new-console-template for more information

using CommandLine;


[Verb("add", HelpText = "Add transactions")]
class AddOptions
{
    [Option('a', "amount", Required = true, HelpText = "Amount")]
    public decimal Amount { get; init; }

    [Option('d', "date", Required = true, HelpText = "Date")]
    public DateTime DateTime { get; init; }

    [Option('c', "category", Required = true, HelpText = "Category")]
    public string Category { get; init; }

    [Option("description", HelpText = "Description")]
    public string? Description { get; init; }

    [Option("credit", HelpText = "Is the transaction credit")]
    public bool Credit { get; init; }
}