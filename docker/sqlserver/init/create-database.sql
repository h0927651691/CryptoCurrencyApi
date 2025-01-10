IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CryptoCurrency')
BEGIN
    CREATE DATABASE CryptoCurrency;
END
GO

USE CryptoCurrency;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Currencies]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Currencies](
        [Code] [nvarchar](3) NOT NULL,
        [ChineseName] [nvarchar](50) NOT NULL,
        [CreatedAt] [datetime2](7) NOT NULL,
        [UpdatedAt] [datetime2](7) NULL,
        CONSTRAINT [PK_Currencies] PRIMARY KEY CLUSTERED ([Code] ASC)
    )
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Currencies_Code' AND object_id = OBJECT_ID('Currencies'))
BEGIN
    CREATE INDEX [IX_Currencies_Code] ON [dbo].[Currencies]([Code])
END
GO