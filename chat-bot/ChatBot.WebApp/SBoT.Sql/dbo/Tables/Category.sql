CREATE TABLE [dbo].[Category]
(
	[Id] INT NOT NULL Identity(1,1) PRIMARY KEY, 
    [Name] NVARCHAR(255) NOT NULL, 
    [Response] NVARCHAR(MAX) NOT NULL, 
    [SetContext] NVARCHAR(255) NULL, 
    [IsDefault] BIT NULL, 
    [IsTest] BIT NULL, 
    [IsChanged] BIT NULL, 
    [ParentId] INT NULL, 
    [IsAdded] BIT NULL, 
    [PartitionId] UNIQUEIDENTIFIER NULL, 
    [ChangedOn] DATETIME NULL, 
    [ChangedBy] NVARCHAR(255) NULL, 
    [PublishedOn] DATETIME NULL, 
    [OriginId] UNIQUEIDENTIFIER NULL, 
    [IsIneligible] BIT NULL, 
    [RequiredRoster] NCHAR(4) NULL, 
    [IsDisabled] BIT NULL, 
    [SetMode] INT NULL, 
    CONSTRAINT [FK_Category_Partition] FOREIGN KEY ([PartitionId]) REFERENCES [Partition]([Id])
)
