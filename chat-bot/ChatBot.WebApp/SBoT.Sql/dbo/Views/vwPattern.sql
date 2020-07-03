CREATE VIEW [dbo].[vwPattern]
	AS SELECT 
		pt.* ,
		c.Name + ' - ' + p.Name + ' (' + pp.Name + ')' as CategoryName,
		c.PartitionId as CategoryPartitionId,
		p.ParentId as CategoryUpperPartitionId,
		c.OriginId as CategoryOriginId,
		c.IsTest
	FROM Pattern pt
	left join Category c on pt.CategoryId = c.Id
    left join Partition p on p.Id = c.PartitionId
    left join Partition pp on pp.Id = p.ParentId