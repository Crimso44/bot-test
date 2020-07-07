using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.Common.Rabbit
{
    public class RabbitConst
    {
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
            // Команды на обучение модели
            // </summary>
            public static string ModelIn = "eve_model_in";

            // <summary>
            // Получение обученных моделей
            // </summary>
            public static string ModelOut = "eve_model_out";

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
            // Оюучение модели
            // </summary>
            public static string LearnModel = "tasks.learn_model";
        }


        public static class RabbitMQ
        {
            public static int WaitTimeout = 60;
            public static int MinWordCount = 3;
        }


    }
}
