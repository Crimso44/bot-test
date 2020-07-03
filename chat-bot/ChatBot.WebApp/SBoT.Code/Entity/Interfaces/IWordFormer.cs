using SBoT.Code.Classes;
using SBoT.Code.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace SBoT.Code.Entity.Interfaces
{
    public interface IWordFormer
    {
        WordListDto GetWordsFromPhrase(string phrase, bool isUnYo, bool isDistinct, bool isForTranslit);
        FileDto GetReport(DateTime from, DateTime to, string rootPath);

        bool IsLetter(char c, bool isForTranslit);

        ///<summary>
        /// Обработка заданных обязательных слов ( (... ...) и &lt;... ...&gt; в паттернах )
        ///</summary>
        List<ResponseDto> CheckSequences(string question, List<string> words, List<ResponseDto> pts);
    }
}
