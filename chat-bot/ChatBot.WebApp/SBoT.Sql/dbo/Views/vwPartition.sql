CREATE VIEW [dbo].[vwPartition]
	AS SELECT p.Id, p.ParentId, p.Name as Title, 
		p.Name + case when pp.Id is null then '' else ' - ' + pp.Name end as FullTitle ,
        pp.Name as ParentTitle,
		(select count(*) from Category c join Partition px on c.PartitionId = px.Id where (c.PartitionId = p.Id or px.ParentId = p.Id) and c.IsTest = 1) as CategoryCount,
		(select count(*) from Category c join Partition px on c.PartitionId = px.Id where (c.PartitionId = p.Id or px.ParentId = p.Id) and c.IsTest = 0) as CategoryPublishedCount,
		(select count(*) from Partition ppx where p.Id = ppx.ParentId) as SubpartitionCount,
		(select count(*) from Learning l join Partition px on px.Id = l.PartitionId where px.Id = p.Id or px.ParentId = p.Id) as LearningCount 
	FROM Partition p
	Left Join Partition pp on p.ParentId = pp.Id
