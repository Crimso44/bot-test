CREATE TABLE [dbo].[ModelLearning]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [CreateDate] DATETIME NOT NULL, 
    [Command] NVARCHAR(1000) NOT NULL,
    [Timestamp] TIMESTAMP NOT NULL, 
    [AnswerDate] DATETIME NULL, 
    [Markup] INT NULL, 
    [Accuracy] FLOAT NULL, 
    [Precision] FLOAT NULL, 
    [Recall] FLOAT NULL, 
    [F1] FLOAT NULL, 
    [ModelDocumentId] UNIQUEIDENTIFIER NULL, 
    [IsActive] BIT NOT NULL DEFAULT (0), 
    [FullAnswer] TEXT NULL
)
