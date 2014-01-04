using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyConverter.Models {
    public class CountryExchangeInfo {
        public string Code { get; set; }
        public string CountryAndCurrency { get; set; }
        public double ExchangeRate { get; set; }
    }

    public class CurrencyConversion {
        public CountryExchangeInfo SourceCountry { get; set; }
        public CountryExchangeInfo TargetCountry { get; set; }
    }
}