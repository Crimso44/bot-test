using System;
using System.Threading.Tasks;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.Common.Rabbit.Model;

namespace ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot
{
    public interface IChatBotLearningProvider 
    {
        void AddLearning(LearningDto learning);
        void ModifyLearning(LearningDto learning);
        void DeleteLearning(int id);
        LearningDto GetByIdAndQuestion(int? id, string question);
        void RecalcLearningTokens(bool isFullRecalc);
        Guid RunLearningCommand(string command);
        void RunPublishCommand(string command, Guid modelId);
        ModelLearningDto GetModelLearning(Guid id);
        void StoreLearningReport(Guid id, LearningModelAnswerDto report, string fullAnswer);
        void CopyPatternToLearn(int categoryId);
    }
}
