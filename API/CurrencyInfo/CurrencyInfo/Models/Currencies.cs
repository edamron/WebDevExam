using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyInfo.Models {
    public class Currency {
        public string Code { get; set; }
        public string CountryAndCurrency { get; set; }
        public double ExchangeRate { get; set; }
    }

    public class Exchange {
        public Currency Source { get; set; }
        public Currency Target { get; set; }
        public double ConversionAmount { get; set; }
    }
}