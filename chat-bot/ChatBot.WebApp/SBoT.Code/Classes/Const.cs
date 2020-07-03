using System;
using System.Collections.Generic;
using System.Linq;
using SBoT.Code.Dto;

namespace SBoT.Code.Classes
{
    public static class Const
    {
        public static byte SbtSliceType = 1;

        public static class RabbitMQ
        {
            public static int WaitTimeout = 30;
            public static int MinWordCount = 3;
        }

        public static class Elastic
        {
            public static int MinLetterCount = 4;
        }

        public static class CategoriesReserved
        {

            // <summary>
            // Ответ по-умолчанию (ответ не найден)
            // </summary>
            public static string Default = "default";

            // <summary>
            // Фраза при исправлении вопроса (Вы имели в виду {0}?)
            // </summary>
            public static string Changed = "changed";

            // <summary>
            // Приветствие
            // </summary>
            public static string HelloMessage = "hello";

            // <summary>
            // Паттерны для отпиливания приветствия
            // </summary>
            public static string HelloPatterns = "--Привет";

            // <summary>
            // Слова, которые не надо исправлять в опечаточнике
            // </summary>
            public static string FixedWords = "--Слова";

            // <summary>
            // Текст для полезных ссылок
            // </summary>
            public static string UsefulLinks = "usefullinks";

            // <summary>
            // Текст для полезных ссылок, когда ответ не найден
            // </summary>
            public static string UsefulLinksOnly = "usefullinks_only";

            // <summary>
            // Сообщение для не-СБТ сотрудников из СберЧата
            // </summary>
            public static string NoSbt = "no_sbt";
        }

        // <summary>
        // Названия очередей в RabbitMQ
        // </summary>
        public static class RabbitQueueName
        {
            // <summary>
            // Обновление каталога 
            // </summary>
            public static string CatalogIn = "eve_words_in";

            // <summary>
            // Проверка орфографии - из Евы
            // </summary>
            public static string SpellCheckIn = "eve_sc_in";

            // <summary>
            // Проверка орфографии - возврат исправленного
            // </summary>
            public static string SpellCheckOut = "eve_sc_out";

            // <summary>
            // Проверка МТО - из Евы
            // </summary>
            public static string MtoIn = "eve_mto_in";

            // <summary>
            // Проверка МТО - возврат исправленного
            // </summary>
            public static string MtoOut = "eve_mto_out";
        }

        // <summary>
        // Названия задач в RabbitMQ
        // </summary>
        public static class RabbitTaskName
        {
            // <summary>
            // Обновление каталога 
            // </summary>
            public static string Catalog = "tasks.update_catalog";

            // <summary>
            // Проверка орфографии
            // </summary>
            public static string SpellCheck = "tasks.spell_checker";

            // <summary>
            // Проверка МТО
            // </summary>
            public static string Mto = "tasks.define_mto";

        }

        public static class Settings
        {
            // <summary>
            // Использовать ли модель ML для обработки вопросов 
            // false - не использовать
            // </summary>
            public static string UseModel = "UseModel";
            // <summary>
            // Использовать ли порог для ответов модели ML
            // true - использовать
            // </summary>
            public static string UseMLThreshold = "UseMLThreshold";
            // <summary>
            // Порог вероятности ответа МЛ
            // </summary>
            public static string MLThreshold = "MLThreshold";
            // <summary>
            // Использовать ли несколько ответов модели ML
            // true - использовать
            // </summary>
            public static string UseMLMultiAnswer = "UseMLMultiAnswer";
            // <summary>
            // Порог вероятности ответа МЛ для отбора нескольких ответов
            // </summary>
            public static string MLMultiThreshold = "MLMultiThreshold";
        }

    }
}
