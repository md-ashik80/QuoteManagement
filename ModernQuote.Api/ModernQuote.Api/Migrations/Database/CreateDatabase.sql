USE master
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PensionQuotesDB')
BEGIN
  CREATE DATABASE PensionQuotesDB;
END;
GO

USE PensionQuotesDB
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [QuoteRequests] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(30) NOT NULL,
    [Sex] nvarchar(max) NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [MobileNumber] nvarchar(10) NOT NULL,
    [PensionPlan] nvarchar(25) NOT NULL,
    [InvestmentAmount] decimal(18,2) NOT NULL,
    [RetirementAge] int NOT NULL,
    [MaturityAmount] decimal(18,2) NOT NULL,
    [QuoteDate] datetime2 NOT NULL,
    CONSTRAINT [PK_QuoteRequests] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200820030657_CreateQuoteRequestTable', N'3.1.7');

GO

