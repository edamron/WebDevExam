USE [Currency]
GO

/****** Object: View [dbo].[vwExchangeRates] Script Date: 1/6/2014 11:32:37 AM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE VIEW [dbo].[vwExchangeRates]
AS
SELECT Code, 
       Description, 
       ExchangeRate
  FROM tbCurrencies c INNER JOIN tbExchangeRates er ON c.ID = er.CurrencyID
 WHERE er.LoadDate = (SELECT MAX(LoadDate) FROM tbExchangeRates)
