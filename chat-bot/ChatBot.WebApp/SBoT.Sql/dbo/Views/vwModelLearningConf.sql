CREATE VIEW [dbo].[vwModelLearningConf]
	AS SELECT r.*, cFrom.Name CategoryName, cTo.Name ToCategoryName
	FROM [ModelLearningConf] r
	left join [Category] cFrom on r.CategoryId = cFrom.OriginId and cFrom.IsTest = 0
	left join [Category] cTo on r.ToCategoryId = cTo.OriginId and cTo.IsTest = 0

