CREATE VIEW [dbo].[vwModelLearningReport]
	AS SELECT r.*, c.Name CategoryName, c.PartitionId, p.ParentId UpperPartitionId
	FROM [ModelLearningReport] r
	left join [Category] c on r.CategoryId = c.OriginId and c.IsTest = 0
	left join [Partition] p on p.Id = c.PartitionId

