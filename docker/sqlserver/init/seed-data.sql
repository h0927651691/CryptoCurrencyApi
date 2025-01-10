USE CryptoCurrency;
GO

IF NOT EXISTS (SELECT * FROM Currencies WHERE Code = 'USD')
BEGIN
    INSERT INTO Currencies (Code, ChineseName, CreatedAt) VALUES
    ('USD', N'美元', GETUTCDATE()),
    ('EUR', N'歐元', GETUTCDATE()),
    ('JPY', N'日圓', GETUTCDATE()),
    ('GBP', N'英鎊', GETUTCDATE())
END
GO