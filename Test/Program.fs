// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open QuandlRequest
open QuandlTypes

[<EntryPoint>]
let main argv = 
    let dr = {DataBase = "WIKI"; DataSet = "ARPI"}
    let df = {DataFormat = DataFormatEnum.CSV; SortOrder = None}
    let data = 
        dr 
        |> QuandlRequest.createDataRequest 
        |> QuandlRequest.addFormatting df
        |> QuandlRequest.sendDataRequest
    printfn "%A" data

    0 // return an integer exit code

    // add tests to assert a structure of a URL