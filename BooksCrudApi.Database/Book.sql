CREATE TABLE [dbo].[Book]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(500) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [Author] NVARCHAR(200) NOT NULL,
    [ISBN] NVARCHAR(20) NULL,
    [Publisher] NVARCHAR(200) NULL,
    [PublicationYear] INT NULL,
    [PageCount] INT NULL,
    [Genre] NVARCHAR(100) NULL,
    [Language] NVARCHAR(50) NULL,
    [Price] DECIMAL(18,2) NULL,
    [IsAvailable] BIT NULL DEFAULT 0,
    [CreatedOn] DATETIME2 NULL DEFAULT GETUTCDATE(),
    [UpdatedOn] DATETIME2 NULL,
    [CoverImageUrl] NVARCHAR(500) NULL
)
