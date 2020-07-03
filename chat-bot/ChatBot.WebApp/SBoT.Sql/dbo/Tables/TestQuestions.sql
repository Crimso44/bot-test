CREATE TABLE [dbo].[TestQuestions]
(
	[Id] INT NOT NULL Identity(1,1) PRIMARY KEY, 
    [Question] NVARCHAR(1024) NOT NULL, 
    [SetName] NVARCHAR(255) NOT NULL
)
