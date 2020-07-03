using SBoT.Code.Dto;
using System;
using System.Collections.Generic;
using SBoT.Code.Classes;
using System.Threading.Tasks;

namespace SBoT.Code.Repository.Interfaces
{
    public interface ISboTRepository
    {
        List<ResponseDto> FindResponse(List<string> words, string context);
        List<ResponseDto> FindResponseByWeights(List<WeightDto> weights, int wordCount, string context);
        ResponseDto GetCategoryByName(string name, bool isTest);
        ResponseDto DefaultResponse();
        ResponseDto GetResponse(string category);
        List<ResponseDto> GetResponsesFromOriginIds(List<Guid> originIds);
        int SaveHistory(string source, string question, string origQuestion, string context, ResponseDto response, bool isButton, string type);
        void UpdateHistory(int historyId, string question, string origQuestion, string context, ResponseDto response, bool isButton, string type, bool isMto, string mtoTresholds);
        bool SetLike(int historyId, short value);
        List<ReportDto> GetReportData(DateTime from, DateTime to);
        List<string> GetTestSetData(string set);
        List<ReportStatDto> GetReportStatData(DateTime from, DateTime to);

        PatternDto GetPatternWordsById(int id);

        List<HistoryDto> GetHistoryFrame(string sigmaLogin, int beforeId, int size);
        string FindUserData(string question, Dictionary<string, RosterDto> roster);

        bool CheckHello(List<string> words);
        List<string> GetFixedWords();

        List<WordListOutDto> GetCategoriesWords();

        List<WordIndexDto> GetWordsForIndex();

        string GetSettings(string name);
    }
    public interface ISboTRepositoryTransient : ISboTRepository { };
}
