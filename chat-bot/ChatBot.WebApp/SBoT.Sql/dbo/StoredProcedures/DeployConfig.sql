CREATE PROCEDURE [dbo].[DeployConfig] (@SubPartition uniqueidentifier, @Partition uniqueidentifier)
AS

begin transaction

delete from PatternWordRel 
from PatternWordRel pwr
join Pattern p on p.Id = pwr.PatternId
join Category c on c.Id = p.CategoryId
left join Partition pt on pt.Id = c.PartitionId
where IsNull(c.IsTest,0) = 0
	and (@SubPartition is null or @SubPartition = c.PartitionId)
	and (@Partition is null or @Partition = pt.ParentId)

delete from Pattern
from Pattern p 
join Category c on c.Id = p.CategoryId
left join Partition pt on pt.Id = c.PartitionId
where IsNull(c.IsTest,0) = 0
	and (@SubPartition is null or @SubPartition = c.PartitionId)
	and (@Partition is null or @Partition = pt.ParentId)

delete from Category 
from Category c
left join Partition pt on pt.Id = c.PartitionId
where IsNull(IsTest,0) = 0
	and (@SubPartition is null or @SubPartition = c.PartitionId)
	and (@Partition is null or @Partition = pt.ParentId)

update Category set PublishedOn = GetDate()
from Category c
left join Partition pt on pt.Id = c.PartitionId
where IsChanged = 1 or IsAdded = 1
	and (@SubPartition is null or @SubPartition = c.PartitionId)
	and (@Partition is null or @Partition = pt.ParentId)

insert into Category (Name, Response, SetContext, IsDefault, IsTest, IsDisabled, IsChanged, ParentId, IsAdded, PartitionId, ChangedOn, ChangedBy, PublishedOn, OriginId, IsIneligible, RequiredRoster)
select c.Name, c.Response, c.SetContext, c.IsDefault, 0, c.IsDisabled, c.IsChanged, c.Id, c.IsAdded, c.PartitionId, c.ChangedOn, c.ChangedBy, c.PublishedOn, IsNull(c.OriginId, NewId()), c.IsIneligible, c.RequiredRoster
From Category c
left join Partition pt on pt.Id = c.PartitionId
where 
	    (@SubPartition is null or @SubPartition = c.PartitionId)
	and (@Partition is null or @Partition = pt.ParentId)

Insert into Pattern (CategoryId, Phrase, WordCount, Context, OnlyContext)
select ct.Id, p.Phrase, p.WordCount, p.Context, p.OnlyContext
from Pattern p
join Category c on c.Id = p.CategoryId
join Category ct on ct.ParentId = c.Id
left join Partition pt on pt.Id = c.PartitionId
where c.IsTest = 1
	and (@SubPartition is null or @SubPartition = c.PartitionId)
	and (@Partition is null or @Partition = pt.ParentId)

Insert into PatternWordRel (PatternId, WordId)
select distinct ptt.Id, pwr.WordId
from Category c 
join Category ct on ct.ParentId = c.Id
join Pattern p on c.Id = p.CategoryId
join Pattern ptt on ptt.CategoryId = ct.Id and ptt.Phrase = p.Phrase
join PatternWordRel pwr on pwr.PatternId = p.Id
left join Partition pt on pt.Id = c.PartitionId
where c.IsTest = 1
	and (@SubPartition is null or @SubPartition = c.PartitionId)
	and (@Partition is null or @Partition = pt.ParentId)

update Category Set IsAdded = 0, IsChanged = 0 
from Category c
left join Partition pt on pt.Id = c.PartitionId
where IsTest = 1
	and (@SubPartition is null or @SubPartition = c.PartitionId)
	and (@Partition is null or @Partition = pt.ParentId)

delete from WordForm
from Word w
join WordForm wf on w.Id = wf.WordId
left join PatternWordRel pw on w.Id = pw.WordId
where pw.WordId is null


delete from Word 
from Word w
left join PatternWordRel pw on w.Id = pw.WordId
where pw.WordId is null

delete from Learning 
from Learning l
left join Category c on c.OriginId = l.CategoryId and c.IsTest = 0
where l.CategoryId is not null and c.Id is null

commit transaction

RETURN 0
