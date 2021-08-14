// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.Text.Json
open CommandLine
open System.IO

type options = {
  [<Option('a', "amount", Required = true, HelpText = "Amount")>] amount : decimal;
  [<Option('d', "date", Required = true, HelpText = "Date")>] date : DateTime;
  [<Option('c', "category", Required = true, HelpText = "Category")>] category : string;
  [<Option("description", HelpText = "Description")>] description : string;
  [<Option("credit", HelpText = "Credit")>] credit : bool;
}

let run (o: options) = 
    printfn "%s" (o.ToString())

    use transactionStream = File.OpenRead("Transactions.json")
    let mutable transactions = JsonSerializer.Deserialize<options list>(transactionStream)
    transactionStream.Close()

    transactions <- transactions @ [o]

    printfn "%s" (transactions.ToString())

    use appendStream = File.Open("Transactions.json", FileMode.Create)
    JsonSerializer.Serialize(appendStream, transactions) |> ignore
    ()


let fail o = ()


[<EntryPoint>]
let main argv =
  let result = CommandLine.Parser.Default.ParseArguments<options>(argv)
  match result with
  | :? Parsed<options> as parsed -> run parsed.Value
  | :? NotParsed<options> as notParsed -> fail notParsed.Errors
  0