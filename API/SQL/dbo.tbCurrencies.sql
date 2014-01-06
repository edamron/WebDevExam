USE [Currency]
GO

/****** Object: Table [dbo].[tbCurrencies] Script Date: 1/6/2014 11:32:07 AM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbCurrencies] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Code]        NVARCHAR (3)  NOT NULL,
    [Description] NVARCHAR (50) NOT NULL
);


