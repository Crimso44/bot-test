CREATE VIEW [dbo].[vwLearning]
	AS SELECT l.*, 
		c.Name + ' - ' + p.Name + ' (' + pp.Name + ')' as CategoryName,
		c.PartitionId as CategoryPartitionId,
		p.ParentId as CategoryUpperPartitionId
	FROM [Learning] l
	left join Category c on c.IsTest != 1 and c.OriginId = l.CategoryId
    left join Partition p on p.Id = c.PartitionId
    left join Partition pp on pp.Id = p.ParentId

