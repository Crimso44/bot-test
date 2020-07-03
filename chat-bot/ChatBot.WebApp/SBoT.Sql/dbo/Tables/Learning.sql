CREATE TABLE [dbo].[Learning]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Question] NVARCHAR(450) NOT NULL, 
    [CategoryId] UNIQUEIDENTIFIER NULL, 
    [PartitionId] UNIQUEIDENTIFIER NULL, 
    [Tokens] NVARCHAR(450) NULL
)

GO

CREATE INDEX [IX_Learning_Question] ON [dbo].[Learning] ([Question])
