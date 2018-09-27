using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IexTradingExample
{
    class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        //IexTrading https://iextrading.com
        //Doc https://iextrading.com/developer/docs/

        static void Main()
        {
            var stock = GetQuote("aapl");
            //var stock = GetQuoteAsync("aapl").Result
            Console.WriteLine($"Name:{stock.CompanyName}\nLatest Price:{stock.LatestPrice}\nUpdate Time(UTC):{stock.GetLatestUpdateTime()}");
            Console.ReadLine();
        }

        /// <summary>
        /// Get stock quote from iexTrading
        /// It just example so exception will handle on implementation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static IexTradingStock GetQuote(string name)
        {
            string responseString = string.Empty;
            try
            {
                responseString = Client.GetStringAsync($"https://api.iextrading.com/1.0/stock/{name}/quote").Result;
            }
            catch (HttpRequestException hre)
            {
                Console.WriteLine(hre.Message);
                //TODO do something
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //TODO do something
            }
            //quit if get content fail
            if (responseString == string.Empty) return null;

            try
            {
                var stock = JsonConvert.DeserializeObject<IexTradingStock>(responseString);
                return stock;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //TODO do something
                return null;
            }
        }

        /// <summary>
        /// just a async version
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static async Task<IexTradingStock> GetQuoteAsync(string name)
        {
            string responseString = string.Empty;
            try
            {
                responseString = await Client.GetStringAsync($"https://api.iextrading.com/1.0/stock/{name}/quote");
            }
            catch (HttpRequestException hre)
            {
                Console.WriteLine(hre.Message);
                //TODO do something
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //TODO do something
            }
            //quit if get content fail
            if (responseString == string.Empty) return null;

            try
            {
                var stock = JsonConvert.DeserializeObject<IexTradingStock>(responseString);
                return stock;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //TODO do something
                return null;
            }
        }
    }

    public class IexTradingStock
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string PrimaryExchange { get; set; }
        public string Sector { get; set; }
        public string CalculationPrice { get; set; }
        public decimal Open { get; set; }
        public long OpenTime { get; set; }
        public decimal Close { get; set; }
        public long CloseTime { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal LatestPrice { get; set; }
        public string LatestSource { get; set; }
        public string LatestTime { get; set; }
        public long LatestUpdate { get; set; }
        public long LatestVolume { get; set; }
        public string IexRealtimePrice { get; set; }
        public string IexRealtimeSize { get; set; }
        public string IexLastUpdated { get; set; }
        public decimal DelayedPrice { get; set; }
        public long DelayedPriceTime { get; set; }
        public decimal ExtendedPrice { get; set; }
        public decimal ExtendedChange { get; set; }
        public decimal ExtendedChangePercent { get; set; }
        public long ExtendedPriceTime { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public string IexMarketPercent { get; set; }
        public string IexVolume { get; set; }
        public long AvgTotalVolume { get; set; }
        public string IexBidPrice { get; set; }
        public string IexBidSize { get; set; }
        public string IexAskPrice { get; set; }
        public string IexAskSize { get; set; }
        public long MarketCap { get; set; }
        public decimal PeRatio { get; set; }
        public decimal Week52High { get; set; }
        public decimal Week52Low { get; set; }
        public decimal YtdChange { get; set; }

        public DateTime GetOpenTime(TimeZoneInfo zone=null)
        {
            return TimestampToDateTime(OpenTime, zone);
        }

        public DateTime GetCloseTime(TimeZoneInfo zone = null)
        {
            return TimestampToDateTime(CloseTime, zone);
        }

        public DateTime GetLatestUpdateTime(TimeZoneInfo zone = null)
        {
            return TimestampToDateTime(LatestUpdate, zone);
        }

        public DateTime GetDelayedPriceTime(TimeZoneInfo zone = null)
        {
            return TimestampToDateTime(DelayedPriceTime, zone);
        }

        public DateTime GetExtendedPriceTime(TimeZoneInfo zone = null)
        {
            return TimestampToDateTime(ExtendedPriceTime, zone);
        }

        private static DateTime TimestampToDateTime(long stamp, TimeZoneInfo zone=null)
        {
            var time =  DateTimeOffset.FromUnixTimeMilliseconds(stamp).UtcDateTime;
            return zone == null ? time : TimeZoneInfo.ConvertTimeFromUtc(time, zone);
        }
    }
}
