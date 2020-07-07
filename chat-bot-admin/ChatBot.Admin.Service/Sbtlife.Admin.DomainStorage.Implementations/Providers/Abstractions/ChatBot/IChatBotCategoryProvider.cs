using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.Common.Rabbit.Model;
using ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot;

namespace ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot
{
    public interface IChatBotCategoryProvider 
    {
        void AddCategory(CategoryDto category);
        void ModifyCategory(CategoryOptionalDto category);
        void DeleteCategory(int id);
        void DeletePattern(int patternId);
        void StorePattern(PatternDto pattern);
        bool CheckExistsAndNotDeleted(int id);
        bool CheckCaptionUnique(string caption);
        bool CheckCategoryIsEditable(int id);
        bool CheckAnyEditCategories();
        string ValidateResponse(string response);
        string ValidateDelete(int id);
        void PublishCategories(Guid? partitionId, Guid? subPartId);
        void UnpublishCategories(Guid? partitionId, Guid? subPartId);
        List<WordListOutDto> GetCategoriesWords();
        List<WordIndexDto> GetWordsForIndex();
        CategoryDto GetByOriginId(Guid originId);
        PatternDto GetTestPatternById(int patternId, int? categoryId);
    }
}
