using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YahooFinanceApi;

namespace Algorithm
{
    class Program
    {
        // ---------------------------------------------------------------------------------------------------------------
        // MAIN

        static async Task Main()
        {

            List<CLASS_Ticker_DataBase> LIST_Ticker_Database = await LIST_Get_Database_Of_Ticker("TRCH", 120);








            string[] ARRAY_My_Tickers = new string[] { "TRCH", "AMTX", "KOPN", "ANVS", "ORGO", "PYR.TO", "RIOT", "DAC", "MARA", "DXYN", "SI", "DQ", "AQMS", "ATOM", "PRTS", "AWH", "REGI", "FD.V" };
            List<CLASS_Ticker_Info> LIST_Ticker_Objects = await LIST_Get_All_Ticker_Current_Info_From_Array(ARRAY_My_Tickers);
            Show_Opens_In_Console(LIST_Ticker_Objects);
        }

        // ---------------------------------------------------------------------------------------------------------------
        // FUNCTIONS

        static async Task<List<CLASS_Ticker_DataBase>> LIST_Get_Database_Of_Ticker(string Ticker, double DaysToGoBack = 60)
        {
            List<CLASS_Ticker_DataBase> LIST_DATABase = new List<CLASS_Ticker_DataBase>();

            Yahoo.IgnoreEmptyRows = true;

            if (DaysToGoBack < 0) { DaysToGoBack = -1 * DaysToGoBack; }

            IReadOnlyList<Candle> Data = await Yahoo.GetHistoricalAsync(Ticker, DateTime.Today.AddDays(-1 * DaysToGoBack), DateTime.Today, Period.Daily);

            CLASS_Ticker_DataBase OBJECT_DataBase_Day = new CLASS_Ticker_DataBase();
            foreach (Candle Date in Data)
            {
                OBJECT_DataBase_Day = new CLASS_Ticker_DataBase();
                OBJECT_DataBase_Day.Date = Date.DateTime;
                OBJECT_DataBase_Day.Open = Decimal.ToDouble(Date.Open);
                OBJECT_DataBase_Day.Close = Decimal.ToDouble(Date.Close);
                OBJECT_DataBase_Day.High = Decimal.ToDouble(Date.High);
                OBJECT_DataBase_Day.Low = Decimal.ToDouble(Date.Low);
                LIST_DATABase.Add(OBJECT_DataBase_Day);
            }

            return LIST_DATABase;
        }

        static void Show_Opens_In_Console(List<CLASS_Ticker_Info> LIST_Ticker_Object)
        {
            Console.Clear();
            foreach (CLASS_Ticker_Info Ticker in LIST_Ticker_Object)
            {
                Console.WriteLine(Ticker.Ticker + " " + Ticker.Open);
            }
            Console.ReadLine();
        }

        static async Task<List<CLASS_Ticker_Info>> LIST_Get_All_Ticker_Current_Info_From_Array(string[] ARRAY_Input_Tickers)
        {
            List<CLASS_Ticker_Info> LIST_Tickers = new List<CLASS_Ticker_Info>();
            CLASS_Ticker_Info OBJECT_Actual_Ticker = new CLASS_Ticker_Info();

            Yahoo.IgnoreEmptyRows = true;

            IReadOnlyDictionary<string, Security> Securities = await Yahoo.Symbols(ARRAY_Input_Tickers).Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketDayLow, Field.RegularMarketOpen, Field.RegularMarketDayHigh).QueryAsync();

            for (int i = 0; i <= ARRAY_Input_Tickers.Length - 1; i++)
            {
                try
                {
                    OBJECT_Actual_Ticker = new CLASS_Ticker_Info();
                    OBJECT_Actual_Ticker.Ticker = ARRAY_Input_Tickers[i];
                    try { OBJECT_Actual_Ticker.Open = Securities[ARRAY_Input_Tickers[i]][Field.RegularMarketOpen]; } catch { }
                    try { OBJECT_Actual_Ticker.Current = Securities[ARRAY_Input_Tickers[i]][Field.RegularMarketPrice]; } catch { }
                    try { OBJECT_Actual_Ticker.High = Securities[ARRAY_Input_Tickers[i]][Field.RegularMarketDayHigh]; } catch { }
                    try { OBJECT_Actual_Ticker.Low = Securities[ARRAY_Input_Tickers[i]][Field.RegularMarketDayLow]; } catch { }
                    LIST_Tickers.Add(OBJECT_Actual_Ticker);
                }
                catch { }
            }

            return LIST_Tickers;
        }

        static async Task<CLASS_Ticker_Info> OBJECT_Get_Ticker_Current_Info(string STRING_Ticker)
        {
            CLASS_Ticker_Info OBJECT_Ticker = new CLASS_Ticker_Info();

            Yahoo.IgnoreEmptyRows = true;

            OBJECT_Ticker.Ticker = STRING_Ticker;

            IReadOnlyDictionary<string, Security> Securities = await Yahoo.Symbols(STRING_Ticker).Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketDayLow, Field.RegularMarketOpen, Field.RegularMarketPreviousClose).QueryAsync();

            try
            {
                OBJECT_Ticker.Current = Securities[STRING_Ticker][Field.RegularMarketPrice];
                OBJECT_Ticker.Open = Securities[STRING_Ticker][Field.RegularMarketOpen];
            }
            catch
            {
                OBJECT_Ticker.Current = 0;
                OBJECT_Ticker.Open = 0;
            }

            return OBJECT_Ticker;
        }

        static async Task Archivess()
        {
            // Sometimes, yahoo returns broken rows for historical calls, you could decide if these invalid rows is ignored or not by the following statement
            Yahoo.IgnoreEmptyRows = true;



            object securities;
            // You could query multiple symbols with multiple fields through the following steps:
            await Task.Run(() => securities = Yahoo.Symbols("TRCH", "GOOG").Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketDayLow, Field.RegularMarketOpen, Field.RegularMarketPreviousClose).QueryAsync());

            //var TRCH = securities["TRCH"];
            //var price = TRCH[Field.RegularMarketPrice]; // or, you could use aapl.RegularMarketPrice directly for typed-value





            var testo = await Yahoo.Symbols("TRCH").Fields(Field.RegularMarketPrice, Field.RegularMarketDayLow, Field.RegularMarketOpen, Field.RegularMarketDayHigh).QueryAsync();



            // You should be able to query data from various markets including US, HK, TW
            // The startTime & endTime here defaults to EST timezone
            var history = await Yahoo.GetHistoricalAsync("SI", DateTime.Today.AddDays(-60), DateTime.Today, Period.Daily);

            foreach (var candle in history)
            {
                Console.WriteLine($"DateTime: {candle.DateTime}, Open: {candle.Open}, High: {candle.High}, Low: {candle.Low}, Close: {candle.Close}, Volume: {candle.Volume}, AdjustedClose: {candle.AdjustedClose}");
            }

        }
    }

    // ---------------------------------------------------------------------------------------------------------------
    // CLASSES

    class CLASS_Ticker_DataBase
    {
        public System.DateTime Date { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
    }

    class CLASS_Ticker_Info
    {
        public string Ticker { get; set; }
        public double Open { get; set; }
        public double Current { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
    }
}
