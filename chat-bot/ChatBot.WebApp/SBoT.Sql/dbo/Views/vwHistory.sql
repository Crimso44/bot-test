CREATE VIEW [dbo].[vwHistory] as
select 
	h.Id, h.QuestionDate, h.Question, h.Rate, h.Context, h.SetContext, 
	h.IsButton, h.[Like], h.AnswerText, h.OriginalQuestion, h.AnswerType, h.CategoryOriginId, h.IsMto,
	h.SigmaLogin, h.UserName, 
	IsNull(c.Name + ' - ' + cpp.Name + ' (' + cp.Name + ')', h.Answer) as Answer,
	l.Id as LearnId, l.CategoryId as LearnCategoryId,
	cast(case when c.OriginId is null or 
		exists (select 1 from Learning ll where ll.Question = ltrim(rtrim(h.Question)) and ll.CategoryId = h.CategoryOriginId) or
		not exists (select 1 from Learning ll where ll.Question = ltrim(rtrim(h.Question)) and ll.CategoryId is not null) 
	then 1 else 0 end as bit) as AnswerGood,
	h.MtoThresholds, 
	case when h.QuestionDate >= convert(datetime,'29.04.2020', 104) and h.Source is null then 'Win' else h.Source end Source
from History h
left join Category c on c.OriginId = h.CategoryOriginId and c.IsTest = 0
left join Partition cp on cp.Id = c.PartitionId
left join Partition cpp on cpp.Id = cp.ParentId
left join (
	select Max(lx.Id) Id, Max(lx.CategoryId) CategoryId, lx.Question From Learning lx group by lx.Question
) l on l.Question = ltrim(rtrim(h.Question))
