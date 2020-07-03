CREATE TABLE [dbo].[ModelLearningMarkup]
(
	[Id] INT NOT NULL Identity(1, 1) PRIMARY KEY, 
    [ModelLearningId] UNIQUEIDENTIFIER NOT NULL,
	[CategoryFrom] UNIQUEIDENTIFIER NULL,
	[CategoryTo] UNIQUEIDENTIFIER NULL,
    [Question] NVARCHAR(450) NOT NULL, 
    CONSTRAINT [FK_ModelLearningMarkup_ModelLearning] FOREIGN KEY ([ModelLearningId]) REFERENCES [ModelLearning]([Id])
)
