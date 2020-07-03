CREATE TABLE [dbo].[PatternWordRel]
(
	[PatternId] INT NOT NULL , 
    [WordId] INT NOT NULL, 
    PRIMARY KEY ([PatternId], [WordId]), 
    CONSTRAINT [FK_PatternWordRel_Pattern] FOREIGN KEY ([PatternId]) REFERENCES [Pattern]([Id]),
    CONSTRAINT [FK_PatternWordRel_Word] FOREIGN KEY ([WordId]) REFERENCES [Word]([Id])
)
