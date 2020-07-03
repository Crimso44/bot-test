CREATE TABLE [dbo].[Word]
(
	[Id] INT NOT NULL Identity(1,1) PRIMARY KEY, 
    [WordName] NVARCHAR(255) NOT NULL, 
    [WordTypeId] INT NULL 
)
