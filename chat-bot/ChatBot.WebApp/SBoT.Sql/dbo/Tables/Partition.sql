CREATE TABLE [dbo].[Partition]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(255) NOT NULL, 
    [ParentId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [FK_Partition_Partition] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Partition]([Id])
)
