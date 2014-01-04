using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CurrencyInfo.Models;

namespace CurrencyInfo.Controllers
{
    public class CurrencyExchangeController : ApiController
    {
        // load all the currencies, as they relate to the USD
        Currency[] exchangeRates = new Currency[] {
            new Currency {Code = "USD", CountryAndCurrency = "United States Dollar", ExchangeRate = 1},
            new Currency {Code = "AED", CountryAndCurrency = "United Arab Emirates Dirham", ExchangeRate = 3.672887},
            new Currency {Code = "AFN", CountryAndCurrency = "Afghan Afghani", ExchangeRate = 55.9825},
            new Currency {Code = "ALL", CountryAndCurrency = "Albanian Lek", ExchangeRate = 102.8739},
            new Currency {Code = "AMD", CountryAndCurrency = "Armenian Dram", ExchangeRate = 404.402503},
            new Currency {Code = "ANG", CountryAndCurrency = "Netherlands Antillean Guilder", ExchangeRate = 1.78896},
            new Currency {Code = "AOA", CountryAndCurrency = "Angolan Kwanza", ExchangeRate = 97.616776},
            new Currency {Code = "ARS", CountryAndCurrency = "Argentine Peso", ExchangeRate = 6.553796},
            new Currency {Code = "AUD", CountryAndCurrency = "Austrailian Dollar", ExchangeRate = 1.117862}
        };

        public IEnumerable<Currency> GetAllExchangeRates() {
            return exchangeRates;
        }

        public IHttpActionResult GetExchangeInfo(string CountryCode) {
            var country = exchangeRates.FirstOrDefault((c) => c.Code == CountryCode);

            if (country == null) {
                return NotFound();
            }

            return Ok(country);
        }

        public IHttpActionResult GetExchangeInfo(string Source, string Destination, double Amount) {
            Exchange exchange = new Exchange();

            exchange.Source = exchangeRates.FirstOrDefault((c) => c.Code == Source);

            if (exchange.Source == null) {
                return NotFound();
            }

            exchange.Target = exchangeRates.FirstOrDefault((c) => c.Code == Destination);

            if (exchange.Target == null) {
                return NotFound();
            }

            // calculate the actual exchange amount here (based on the algorithm I found earlier)
            // let usd_gbp = 0.6438, usd_hkd = 7.7668
            // if converting from USD, simply multiply the amount by the exchange rate
            if (exchange.Source.Code == "USD") {
                exchange.ConversionAmount = Amount * exchange.Target.ExchangeRate;
	        }
            else if (exchange.Target.Code == "USD") {
                exchange.ConversionAmount = Amount * (1 / exchange.Source.ExchangeRate);
            }
            else {
                //gbp_hkd = usd_hkd * (1 / usd_gbp)
                double adjustedRate = exchange.Target.ExchangeRate * (1 / exchange.Source.ExchangeRate);
                exchange.ConversionAmount = Amount * adjustedRate;
            }

            return Ok(exchange);
        }
    }
}
