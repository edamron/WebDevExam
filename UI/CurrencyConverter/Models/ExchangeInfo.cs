using System;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CurrencyConverter.Models {
    /// <summary>
    /// The basic unit of information from which all conversions are done.
    /// </summary>
    public class CountryExchangeInfo {
        /// <summary>
        /// The 3-character code that uniquely identifies a current (e.g., USD, GBP, etc.)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// The description for the currency that includes both country & currency names
        /// </summary>
        public string CountryAndCurrency { get; set; }
        /// <summary>
        /// The exchange rate for the given currency, in relation to the US Dollar
        /// </summary>
        public double ExchangeRate { get; set; }
    }

    /// <summary>
    /// An enumerable list of currencies info objects, along with a 
    /// dictionary of rates that have been adjusted to work with the currency 
    /// being converted from.
    /// </summary>
    public class WorldCurrencies {
        /// <summary>
        /// An enumerable list of all the currencies available to the application
        /// (which ultimately come from a SQL Azure database table in earl.azurewebsites.net
        /// called Currency.dbo.tbExchangeRates)
        /// </summary>
        public IEnumerable<CountryExchangeInfo> Currencies { get; set; }
        /// <summary>
        /// A list of exchange rates that have been converted from their base amount
        /// (vs. the US Dollar) to the given base (if necessary).
        /// </summary>
        public Dictionary<string, double> TargetRates { get; set; }

        /// <summary>
        /// Gets all the exchange rates available for the most recent date they
        /// were downloaded from the CurrencyExchange API.  A list of adjusted
        /// rates is also created, but in the case of using the US Dollar as the
        /// base, but will match the rate from the API (since this constructor
        /// is not given an alternate base to work from).
        /// </summary>
        public WorldCurrencies() {
            string url = "http://earl.azurewebsites.net/api/currencyexchange";
            WebRequest request = WebRequest.Create(url);
            WebResponse ws = request.GetResponse();

            System.IO.StreamReader sr = new System.IO.StreamReader(ws.GetResponseStream());
            string json = sr.ReadToEnd();

            Currencies = JsonConvert.DeserializeObject<IEnumerable<CountryExchangeInfo>>(json);

            TargetRates = new Dictionary<string, double>();

            foreach (var countryExchangeInfo in Currencies) {
                TargetRates.Add(countryExchangeInfo.Code, countryExchangeInfo.ExchangeRate);
            }
        }

        /// <summary>
        /// Gets all the exchange rates available for the most recent date they
        /// were downloaded from the CurrencyExchange API.  A list of adjusted
        /// rates is also created vs. the US Dollar (which is the only data available 
        /// via openexchangerates.org) for convertions against a difference base.
        /// </summary>
        public WorldCurrencies(string SourceCurrency)
            : this() {
            // adjust the exchange rates to be based on the selected source currency
            double newSourceRate = 1;
            TargetRates.TryGetValue(SourceCurrency, out newSourceRate);

            TargetRates.Clear();

            foreach (var countryExchangeInfo in Currencies) {
                //TargetRates.Add(countryExchangeInfo.Code, countryExchangeInfo.ExchangeRate / newSourceRate);
                countryExchangeInfo.ExchangeRate = countryExchangeInfo.ExchangeRate / newSourceRate;
            }
        }
    }
}