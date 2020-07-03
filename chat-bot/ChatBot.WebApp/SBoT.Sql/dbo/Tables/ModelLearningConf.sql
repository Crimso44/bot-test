CREATE TABLE [dbo].[ModelLearningConf]
(
	[Id] INT NOT NULL Identity(1, 1) PRIMARY KEY, 
    [ModelLearningId] UNIQUEIDENTIFIER NOT NULL,
    [CategoryId] UNIQUEIDENTIFIER NULL, 
    [Confusion] FLOAT NULL, 
    [ToCategoryId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [FK_ModelLearningConf_ModelLearning] FOREIGN KEY ([ModelLearningId]) REFERENCES [ModelLearning]([Id])
)
