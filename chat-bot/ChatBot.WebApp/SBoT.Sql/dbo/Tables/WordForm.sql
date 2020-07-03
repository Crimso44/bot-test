CREATE TABLE [dbo].[WordForm]
(
	[Id] INT NOT NULL Identity(1,1) PRIMARY KEY, 
    [WordId] INT NOT NULL, 
    [Form] NVARCHAR(255) NOT NULL, 
    CONSTRAINT [FK_WordForm_Word] FOREIGN KEY ([WordId]) REFERENCES [Word]([Id])
)

GO

CREATE NONCLUSTERED INDEX [IX_WordForm] ON [dbo].[WordForm] ([Form] ASC)
