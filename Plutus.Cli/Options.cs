using System;
using CommandLine;

namespace Plutus.Cli
{
    internal class Options
    {
        [Option('a', "amount", HelpText = "Transaction Amount")]
        public decimal Amount { get; init; }

        [Option("description", HelpText = "Transaction Description")]
        public string Description { get; init; } = "";

        [Option('c', "category", HelpText = "Transaction Category")]
        public string Category { get; init; }

        public string Username => "sanjay";

        [Option("credit", HelpText = "Is Transaction Credit Type")]
        public bool IsCredit { get; init; }

        [Option('d', "date", HelpText = "Transaction Date Time")]
        public DateTime DateTime { get; init; }

        [Option("token", HelpText = "Authentication Token")]
        public string Token { get; init; }
    }
}