using System;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CurrencyConverter.Models {
    public class CountryExchangeInfo {
        public string Code { get; set; }
        public string CountryAndCurrency { get; set; }
        public double ExchangeRate { get; set; }
    }

    public class WorldCurrencies {
        public IEnumerable<CountryExchangeInfo> Currencies { get; set; }
        public Dictionary<string, double> TargetRates { get; set; }

        public WorldCurrencies() {
            string url = "http://earl.azurewebsites.net/api/currencyexchange";
            WebRequest request = WebRequest.Create(url);
            WebResponse ws = request.GetResponse();

            System.IO.StreamReader sr = new System.IO.StreamReader(ws.GetResponseStream());
            string json = sr.ReadToEnd();

            Currencies = JsonConvert.DeserializeObject<IEnumerable<CountryExchangeInfo>>(json);

            // this still needs to be dynamic
            // (by using the rates as is for the US Dollar,
            //  or changing them all if not the US Dollar)
            TargetRates = new Dictionary<string, double>();

            foreach (var countryExchangeInfo in Currencies) {
                TargetRates.Add(countryExchangeInfo.Code, countryExchangeInfo.ExchangeRate);
            }
        }

        public WorldCurrencies(string SourceCurrency) : this() {
            // adjust the exchange rates to be based on the selected source currency
            double newSourceRate = 1;
            TargetRates.TryGetValue(SourceCurrency, out newSourceRate);

            TargetRates.Clear();

            foreach (var countryExchangeInfo in Currencies) {
                TargetRates.Add(countryExchangeInfo.Code, countryExchangeInfo.ExchangeRate / newSourceRate);
            }
        }
    }

    public class ConversionRates {
        public Dictionary<string, double> DestinationRates { get; set; }
    }
}