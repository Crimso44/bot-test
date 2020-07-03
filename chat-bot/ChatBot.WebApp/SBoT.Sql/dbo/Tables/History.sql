CREATE TABLE [dbo].[History]
(
	[Id] INT NOT NULL Identity (1,1) PRIMARY KEY, 
    [QuestionDate] DATETIME NOT NULL, 
    [SigmaLogin] NVARCHAR(255) NULL, 
    [Question] NVARCHAR(MAX) NULL, 
    [Answer] NVARCHAR(255) NULL, 
    [Rate] DECIMAL(18, 2) NULL, 
    [Context] NVARCHAR(255) NULL, 
    [SetContext] NVARCHAR(255) NULL, 
    [IsButton] BIT NULL, 
    [Like] SMALLINT NULL, 
    [AnswerText] NVARCHAR(MAX) NULL, 
    [OriginalQuestion] NVARCHAR(MAX) NULL, 
    [AnswerType] NVARCHAR(50) NULL, 
    [CategoryOriginId] UNIQUEIDENTIFIER NULL, 
    [IsMto] BIT NULL, 
    [MtoThresholds] NVARCHAR(255) NULL, 
    [UserName] NVARCHAR(255) NULL, 
    [Source] CHAR(3) NULL
)
