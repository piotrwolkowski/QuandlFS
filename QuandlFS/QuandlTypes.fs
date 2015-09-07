
module QuandlTypes

    open System
    open FSharp.Core
    open FSharp.Data
    
    type INoConstraintsRequest = interface end
    type IRequest= interface end

    type FrequencyEnum =
        | None = 1
        | Daily = 2
        | Weekly = 4
        | Monthly = 8
        | Quarterly = 16
        | Annual = 32

    type DataFormatEnum =
        | CSV = 1
        | JSON = 2
        | XML = 4

    type SortOrderEnum =
        | Ascending = 1
        | Descending = 2

    type CalculationsEnum =
        | Diff = 1
        | RDiff = 2
        | Cumul = 4
        | Normalize = 8

    type DataRequest =
        {
            DataBase:string
            DataSet:string
        }

    type DataFormatting = 
        {
            DataFormat:DataFormatEnum
            SortOrder:SortOrderEnum option
        }

    // Set any expected constraint. If it's not set it will be ignored.
    type Constraints =
        {
            // do not include data column names
            ExcludeColumnNames:bool option

            // get n first rows
            Truncate:int option
        
            // set a start date
            StartDate:DateTime option
        
            // set an end date
            EndDate:DateTime option
        
            // get specific column
            ColumnIndex:int option
        
            // set data frequency
            Frequency:FrequencyEnum option
        
            // set expected calculations
            Calculations:CalculationsEnum option
        }

    type BaseUrl =
        internal | Url of string

    type DataUrl = 
        internal | Url of string
        interface INoConstraintsRequest
        interface IRequest

    type RegisteredUrl =
        internal | Url of string
        interface INoConstraintsRequest
        interface IRequest

    type ConstrainedUrl  =
        internal | Url of string
        interface IRequest

    type MetadataUrl =
        internal | Url of string
        interface IRequest

