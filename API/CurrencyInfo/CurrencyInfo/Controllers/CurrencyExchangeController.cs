using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CurrencyInfo.Models;

namespace CurrencyInfo.Controllers
{
    /// <summary>
    /// The one and only controller for the Currency Exchange API.  It includes
    /// definitions of all action methods, along with a constructor that instantiates
    /// the underlying model.
    /// 
    /// As it stands as of this date, the API can be referenced at earl.azurewebsites.net
    /// (with the more specific URI's provided in the documentation for each action method).
    /// </summary>
    public class CurrencyExchangeController : ApiController
    {
        // This code is being kept so it can be easily copied into my test harness,
        // once I get a change to work on.
        //
        // load all the currencies, as they relate to the USD,
        // from the database (which is loaded nightly from openexchangerates.org)
        //
        //Currency[] exchangeRates = new Currency[] {
        //    new Currency {Code = "USD", CountryAndCurrency = "United States Dollar", ExchangeRate = 1},
        //    new Currency {Code = "AED", CountryAndCurrency = "United Arab Emirates Dirham", ExchangeRate = 3.672887},
        //    new Currency {Code = "AFN", CountryAndCurrency = "Afghan Afghani", ExchangeRate = 55.9825},
        //    new Currency {Code = "ALL", CountryAndCurrency = "Albanian Lek", ExchangeRate = 102.8739},
        //    new Currency {Code = "AMD", CountryAndCurrency = "Armenian Dram", ExchangeRate = 404.402503},
        //    new Currency {Code = "ANG", CountryAndCurrency = "Netherlands Antillean Guilder", ExchangeRate = 1.78896},
        //    new Currency {Code = "AOA", CountryAndCurrency = "Angolan Kwanza", ExchangeRate = 97.616776},
        //    new Currency {Code = "ARS", CountryAndCurrency = "Argentine Peso", ExchangeRate = 6.553796},
        //    new Currency {Code = "AUD", CountryAndCurrency = "Austrailian Dollar", ExchangeRate = 1.117862}
        //};

        Currency[] exchangeRates;

        /// <summary>
        /// The base class constructor, which connects to the database in MS Azure
        /// and reads the latest exchange rates from the Currency database.
        /// </summary>
        public CurrencyExchangeController() {
            using (var db = new CurrencyEntitiesContext()) {
                var query = from er in db.vwExchangeRates
                            orderby er.Description
                            select er;

                exchangeRates = new Currency[query.Count()];
                int exchangeIndex = 0;

                foreach (var exchangeRate in query) {
                    exchangeRates[exchangeIndex] = new Currency { Code = exchangeRate.Code, CountryAndCurrency = exchangeRate.Description, ExchangeRate = exchangeRate.ExchangeRate };
                    exchangeIndex++;
                }
            }
        }

        /// <summary>
        /// Returns the entire list of available currencies, including all 
        /// countries & exchange rates, with all rates as compared to the US dollar.
        /// </summary>
        /// <returns>An inumerable list of all Currency objects (Code, Description, ExchangeRate)</returns>
        /// <example>http://earl.azurewebsites.net/api/currencyexchange</example>
        public IEnumerable<Currency> GetExchangeInfo() {
            return exchangeRates;
        }

        /// <summary>
        /// Returns the currency information for a single currency, including the 
        /// country code, country description & currency, and the exchange rate, 
        /// with the rate as compared to the US dollar.
        /// </summary>
        /// <returns>A single Currency object (Code, Description, ExchangeRate)</returns>
        /// <param name="Source">The 3-character code of the desired currency</param>
        /// <example>http://earl.azurewebsites.net/api/currencyexchange?source=AUD</example>
        public IHttpActionResult GetExchangeInfo(string Source) {
            var currency = exchangeRates.FirstOrDefault((c) => c.Code == Source);

            if (currency == null) {
                return NotFound();
            }

            return Ok(currency);
        }

        /// <summary>
        /// Performs a currency conversion, and returns currency information for both the source
        /// and target currencies, along with the actual converted amount.
        /// </summary>
        /// <returns>Source & Target Currency objects (Code, Description, ExchangeRate), and the conversion amount</returns>
        /// <param name="Source">The 3-character code of the source currency</param>
        /// <param name="Target">The 3-character code of the target currency</param>
        /// <param name="Amount">The amount of source currecy to convert to the target</param>
        /// <example>http://earl.azurewebsites.net/api/currencyexchange?source=AUD&target=USD&amount=100</example>
        public IHttpActionResult GetExchangeInfo(string Source, string Target, double Amount) {
            Conversion conversion = new Conversion();

            conversion.Source = exchangeRates.FirstOrDefault((c) => c.Code == Source);

            if (conversion.Source == null) {
                return NotFound();
            }

            conversion.Target = exchangeRates.FirstOrDefault((c) => c.Code == Target);

            if (conversion.Target == null) {
                return NotFound();
            }

            // calculate the actual exchange amount here (basing all my conversions
            // on the fact that https://openexchangerates.org returns all rates
            // relative to the US Dollar)
            double rateToUse = 0;

            if (conversion.Source.Code == "USD") {
                // if converting from USD, simply use the target rate
                rateToUse = conversion.Target.ExchangeRate;
	        }
            else if (conversion.Target.Code == "USD") {
                // if converting to USD, use the inverse of the source rate
                rateToUse = 1 / conversion.Source.ExchangeRate;
            }
            else {
                // if the US Dollar isn't involved at all, convert the source rate
                // to the corresponding USD rate, and determine the ultimate rate 
                // from that
                rateToUse = conversion.Target.ExchangeRate * (1 / conversion.Source.ExchangeRate);
            }

            conversion.ConversionAmount = Amount * rateToUse;

            return Ok(conversion);
        }
    }
}
