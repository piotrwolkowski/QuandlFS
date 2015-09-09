
module QuandlTypes

    open System
    open FSharp.Core
    open FSharp.Data
    
    /// The interface indicating the request without constraints.
    type INoConstraintsRequest = interface end

    /// The interface indicating an request.
    type IRequest= interface end

    /// When the frequency of a dataset is changed, Quandl returns 
    /// the last observation for the given period. So, if you collapse 
    /// a daily dataset to monthly, you will get a sample of the original 
    /// dataset where the observation for each month is the last data point 
    /// available for that month. Thus this transformation does not work 
    /// well for datasets that measure percentage changes, period averages 
    /// or period extremes (highs and lows).
    type FrequencyEnum =
        /// No frequency
        | None = 1
        
        /// Daily values
        | Daily = 2
        
        /// Weekly values
        | Weekly = 4

        /// Monthly values
        | Monthly = 8

        /// Quarterly values
        | Quarterly = 16

        /// Annual values
        | Annual = 32

    /// The format code to request the Quandl data in specified format.
    type DataFormatEnum =
        /// The data in CSV format
        | CSV = 1

        /// The data in JSON format
        | JSON = 2

        /// The data in XML format
        | XML = 4

    /// The sort order of the data. Ascending by default.
    type SortOrderEnum =
        /// The data in ascending order
        | Ascending = 1

        /// The data in descending order
        | Descending = 2

    /// Quandl allows to perform certain elementary calculations on the data prior to downloading. 
    /// The transformations currently available are row-on-row change, percentage change, 
    /// cumulative sum, and normalize (set starting value at 100).
    type CalculationsEnum =
        /// Row-on-row change. y'[t] = y[t] - y[t-1] where y[t] denotes a datapoint for time
        /// and y'[t] denotes transformed data
        | Diff = 1

        /// Row-on-row percent change. y'[t] = (y[t] - y[t-1])/y[t-1] where y[t] denotes 
        /// a datapoint for time and y'[t] denotes transformed data
        | RDiff = 2

        /// Cumulative sum. y'[t] = y[t] +y[t-1] + ... + y[0] where y[t] denotes 
        /// a datapoint for time and y'[t] denotes transformed data
        | Cumul = 4

        /// Normalize (set starting value at 100). y'[t] = (y[t]/y[0]) * 100 where y[t] denotes 
        /// a datapoint for time and y'[t] denotes transformed data
        | Normalize = 8

    /// The record determining the data base and the data set.
    type DataRequest =
        {
            // The data base name as available from Quandl.
            DataBase:string

            // The data set name as available from Quandl.
            DataSet:string
        }

    /// The record determining the data format and data sort order.
    type DataFormatting = 
        {
            /// The data format
            DataFormat:DataFormatEnum

            /// The data sort order
            SortOrder:SortOrderEnum option
        }

    /// The record determining additional data constraints.
    type Constraints =
        {
            /// Excludes the headers
            ExcludeColumnNames:bool option

            /// Get n first rows
            Truncate:int option
        
            /// Set a start date
            StartDate:DateTime option
        
            /// Set an end date
            EndDate:DateTime option
        
            /// Get specified column
            ColumnIndex:int option
        
            /// Set data frequency
            Frequency:FrequencyEnum option
        
            /// Set expected calculations
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

