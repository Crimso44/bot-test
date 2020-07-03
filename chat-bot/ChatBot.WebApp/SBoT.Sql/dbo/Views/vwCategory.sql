CREATE VIEW [dbo].[vwCategory]
	AS SELECT *,
		(select count(*) from Learning l where c.OriginId = l.CategoryId) as LearningCount 
	FROM Category c
