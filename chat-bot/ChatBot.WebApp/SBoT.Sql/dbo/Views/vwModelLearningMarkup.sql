CREATE VIEW [dbo].[vwModelLearningMarkup]
	AS SELECT r.*, cFrom.Name CategoryName, cTo.Name ToCategoryName
	FROM [ModelLearningMarkup] r
	left join [Category] cFrom on r.CategoryFrom = cFrom.OriginId and cFrom.IsTest = 0
	left join [Category] cTo on r.CategoryTo = cTo.OriginId and cTo.IsTest = 0

