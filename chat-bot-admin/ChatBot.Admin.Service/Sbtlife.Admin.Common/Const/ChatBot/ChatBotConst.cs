using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatBot.Admin.Common.Model.ChatBot;

namespace ChatBot.Admin.Common.Const.ChatBot
{
    public class ChatBotConst
    {
        public enum WordType
        {
            //None = 0,

            // <summary>
            // существительное
            // </summary>
            Noun = 1,

            // <summary>
            // прилагательное
            // </summary>
            Adjective = 2,

            // <summary>
            // глагол
            // </summary>
            Verb = 0, //3,

            // <summary>
            // наречие
            // </summary>
            Adverb = 4,

            // <summary>
            // притяжательное местоимение
            // </summary>
            Possessive = 5,

            // <summary>
            // притяжательное возвратное местоимение
            // </summary>
            PossessiveReflexive = 6
        }


        public static class Settings
        {
            // <summary>
            // Использовать ли модель ML для обработки вопросов 
            // false - не использовать
            // </summary>
            public static string UseModel = "UseModel";
        }

    }

}
