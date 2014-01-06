USE [Currency]
GO

/****** Object: Table [dbo].[tbExchangeRates] Script Date: 1/6/2014 11:32:22 AM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbExchangeRates] (
    [CurrencyID]   INT        NOT NULL,
    [LoadDate]     DATE       NOT NULL,
    [ExchangeRate] FLOAT (53) NOT NULL
);


