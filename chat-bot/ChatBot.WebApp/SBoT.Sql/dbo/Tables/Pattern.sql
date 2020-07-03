CREATE TABLE [dbo].[Pattern]
(
	[Id] INT NOT NULL Identity(1,1) PRIMARY KEY, 
    [CategoryId] INT NOT NULL, 
    [Phrase] NVARCHAR(255) NOT NULL, 
    [WordCount] INT NULL, 
    [Context] NVARCHAR(255) NULL, 
    [OnlyContext] BIT NULL, 
    CONSTRAINT [FK_Pattern_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id])
)
