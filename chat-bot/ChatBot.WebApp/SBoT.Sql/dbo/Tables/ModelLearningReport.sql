CREATE TABLE [dbo].[ModelLearningReport]
(
	[Id] INT NOT NULL Identity(1, 1) PRIMARY KEY, 
    [ModelLearningId] UNIQUEIDENTIFIER NOT NULL, 
    [CategoryId] UNIQUEIDENTIFIER NULL, 
    [Markup] INT NULL, 
    [Accuracy] FLOAT NULL, 
    [Precision] FLOAT NULL, 
    [Recall] FLOAT NULL, 
    [F1] FLOAT NULL, 
    CONSTRAINT [FK_ModelLearningReport_ModelLearning] FOREIGN KEY ([ModelLearningId]) REFERENCES [ModelLearning]([Id])
)
