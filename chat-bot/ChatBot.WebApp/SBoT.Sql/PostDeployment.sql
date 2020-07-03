update History set OriginalQuestion = Question where OriginalQuestion is null

update History set AnswerType = 'ManyAnswers' 
where AnswerText Like '%Уточните, пожалуйста%' and IsNull(AnswerType,'') = ''

update History set AnswerType = 'NoAnswer' 
where not (AnswerText Like '%Уточните, пожалуйста%') and not (AnswerText Like 'Вы хотели узнать%') and Answer is null and IsNull(AnswerType,'') = ''

update Category set OriginId = NewId() where IsTest = 0 and OriginId is null

update Category Set OriginId = cc.OriginId
from Category c join (select * from Category) cc on c.ParentId = cc.Id or cc.ParentId = c.Id
where c.IsTest = 1 and c.OriginId is null

update Category set OriginId = NewId() where OriginId is null

update Category set IsIneligible = case when Name like 'Матерные%' then 1 else 0 end where IsIneligible is null

insert into Config (Name, Value)
select 'UseModel', 'true' where not exists (select 1 from Config where Name = 'UseModel')
