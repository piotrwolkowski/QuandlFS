module QuandlRequest

// TODO:
// - Add documentation - check if there is any plugin that will support standrd .Net documentation
// - Check if works for forex data
// - Move it to a separate project in VS and in GitHub. Try to moving it to QuantAnalyzerFSharp.

open System
open FSharp.Core
open FSharp.Data
open QuandlTypes

let private QUANDL_API_DATASET_URL = @"https://www.quandl.com/api/v3/datasets"

let createDataRequest dr =
    BaseUrl.Url (String.concat "/" [|QUANDL_API_DATASET_URL;dr.DataBase;dr.DataSet|])

let createMetadataRequest dr =
    BaseUrl.Url (String.concat "/" [|QUANDL_API_DATASET_URL;dr.DataBase;dr.DataSet;"metadata"|])

let addFormatting (df:DataFormatting) (baseUrl:BaseUrl) =
    let format = 
        match df.DataFormat with
        | DataFormatEnum.CSV -> "csv"
        | DataFormatEnum.XML -> "xml"
        | DataFormatEnum.JSON -> "json"
    let (BaseUrl.Url baseUrl') = baseUrl
    let formatted = sprintf "%s.%s" baseUrl' format
    let dataUrl =
        match df.SortOrder with
        | Some x when x = SortOrderEnum.Ascending -> sprintf "%s?order=%s" formatted "asc"
        | Some x when x = SortOrderEnum.Descending -> sprintf "%s?order=%s" formatted "desc"
        | None -> formatted
    DataUrl.Url dataUrl

let addKey key dataUrl =
    let (DataUrl.Url dataUrl') = dataUrl
    let connector = if dataUrl'.Contains("?") then "&" else "?"
    RegisteredUrl.Url(sprintf "%s%sapi_key=%s" dataUrl' connector key)
    
let addConstrains (cnstr:Constraints) (noCnstrUrl:INoConstraintsRequest) =
    let exCl = if cnstr.ExcludeColumnNames.IsSome && cnstr.ExcludeColumnNames.Value then "exclude_column_names=true" else ""
    let trnc = if cnstr.Truncate.IsSome then sprintf "rows=%i" cnstr.Truncate.Value else ""
    let strt = if cnstr.StartDate.IsSome then sprintf "start_date=%s" (cnstr.StartDate.Value.ToString("yyyy-mm-dd")) else ""
    let stp = if cnstr.EndDate.IsSome then sprintf "end_date=%s" (cnstr.EndDate.Value.ToString("yyyy-mm-dd")) else ""
    let clIx = if cnstr.ColumnIndex.IsSome then sprintf "column_index=%i" cnstr.ColumnIndex.Value else ""
    let frq = 
        if cnstr.Frequency.IsSome then
            match cnstr.Frequency.Value with
            | FrequencyEnum.None -> "collapse=none"
            | FrequencyEnum.Daily -> "collapse=daily"
            | FrequencyEnum.Weekly -> "collapse=weekly"
            | FrequencyEnum.Monthly -> "collapse=monthly"
            | FrequencyEnum.Quarterly -> "collapse=quarterly"
            | FrequencyEnum.Annual -> "collapse=annual"
        else ""
    let clc = 
        if cnstr.Calculations.IsSome then
            match cnstr.Calculations.Value with
            | CalculationsEnum.Diff -> "transform=diff"
            | CalculationsEnum.RDiff -> "transform=rdiff"
            | CalculationsEnum.Cumul -> "transform=cumul"
            | CalculationsEnum.Normalize -> "transform=normalize"
        else ""
    
    let cnstrs = String.concat "&" [|exCl;trnc;strt;stp;clIx;frq;clc|]
    let noCnstrUrl' =
        match noCnstrUrl with
        | :? DataUrl as dataUrl -> 
            let (DataUrl.Url dataUrl') = dataUrl
            dataUrl'
        | :? RegisteredUrl as regUrl -> 
            let (RegisteredUrl.Url regUrl') = regUrl
            regUrl'
        | _ -> failwith "Unsupported URL type [%A]" <| noCnstrUrl.GetType()
    let connector = if noCnstrUrl'.Contains("?") then "&" else "?"
    ConstrainedUrl.Url(sprintf "%s%s%s" noCnstrUrl' connector cnstrs)

let sendDataRequest (req:IRequest) =
    let req' = 
        match req with
        | :? DataUrl as dataUrl ->
            let (DataUrl.Url dataUrl') = dataUrl
            dataUrl'
        | :? RegisteredUrl as regUrl ->
            let (RegisteredUrl.Url regUrl') = regUrl
            regUrl'
        | :? ConstrainedUrl as cnrUrl ->
            let (ConstrainedUrl.Url cnrUrl') = cnrUrl
            cnrUrl'
        | :? MetadataUrl as metaUrl ->
            let (MetadataUrl.Url metaUrl') = metaUrl
            metaUrl'
        | _ -> failwith "Unsupported URL type [%A]" <| req.GetType()
    Http.RequestString (sprintf "%s" req')