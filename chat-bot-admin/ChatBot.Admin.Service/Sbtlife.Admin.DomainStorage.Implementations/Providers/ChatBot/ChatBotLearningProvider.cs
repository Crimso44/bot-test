using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChatBot.Admin.Common.Model.ChatBot;
using ChatBot.Admin.Common.Rabbit.Model;
using ChatBot.Admin.CommonServices.Rabbit.Abstractions;
using ChatBot.Admin.DomainStorage.Const;
using ChatBot.Admin.DomainStorage.Contexts;
using ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.ChatBot;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.DocumentStorage;
using Microsoft.Extensions.Logging;
using System.Globalization;
using SBoT.Connect.Abstractions.Interfaces;

namespace ChatBot.Admin.DomainStorage.Providers.ChatBot
{
    class ChatBotLearningProvider : ProviderChatBot, IChatBotLearningProvider
    {
        private IConfiguration _config { get; set; }
        private IRabbitWorker _rabbitWorker { get; set; }
        private ChatBotContext _storage { get; set; }
        private IServiceProvider _service { get; set; }
        private IDocumentStorageProvider _docStore { get; set; }
        private IUser _user { get; set; }
        private readonly ILogger<ChatBotLearningProvider> _logger;

        public ChatBotLearningProvider(IServiceProvider service, ChatBotContext storage, IConfiguration config, IRabbitWorker rabbitWorker, 
            IDocumentStorageProvider docStore, IUser user, ILogger<ChatBotLearningProvider> logger)
            : base(storage)
        {
            _user = user;
            _service = service;
            _config = config;
            _rabbitWorker = rabbitWorker;
            _storage = storage;
            _docStore = docStore;
            _logger = logger;
        }

        public  void AddLearning(LearningDto learning)
        {
            var entity = Mapper.Map<Learning>(learning);

            Category category = null;
            if (learning.CategoryId.HasValue)
                category =  Context.Categorys.FirstOrDefault(x => !(x.IsTest ?? false) && x.OriginId == learning.CategoryId.Value);
            entity.CategoryId = category?.OriginId;
            entity.PartitionId = category?.PartitionId;

             Context.Learnings.Add(entity);
             Context.SaveChanges();

             UpdateTokens(entity);
        }

        public  void ModifyLearning(LearningDto learning)
        {
            var entity =  Context.Learnings.FirstOrDefault(x => x.Id == learning.Id.Value);

            var needSpelling = string.IsNullOrEmpty(entity.Tokens) || entity.Question != learning.Question;
            entity.Question = learning.Question;
            if (needSpelling)
            {
                entity.Tokens = "";
            }

            Category category = null;
            if (learning.CategoryId.HasValue)
                category =  Context.Categorys.FirstOrDefault(x => !(x.IsTest ?? false) && x.OriginId == learning.CategoryId.Value);
            entity.CategoryId = category?.OriginId;
            entity.PartitionId = category?.PartitionId;

             Context.SaveChanges();

            if (needSpelling)
            {
                 UpdateTokens(entity);
            }
        }

        private  void UpdateTokens(Learning entity)
        {
            if (Convert.ToBoolean(_config["Config:IsRabbitMQ"].ToLower()))
            {
                var tc = new ThreadTokens(_service, entity.Id, entity.Question);
                var t = new Thread(new ThreadStart(tc.ThreadProc));
                t.Start();
            }
        }

        public  void DeleteLearning(int id)
        {
            var entity = Context.Learnings.First(x => x.Id == id);
            Context.Learnings.Remove(entity);
             Context.SaveChanges();
        }

        public  LearningDto GetByIdAndQuestion(int? id, string question)
        {
            Learning entity = null;
            if (id.HasValue)
            {
                entity =  Context.Learnings.FirstOrDefault(x => x.Id == id);
            }

            if (entity == null && !string.IsNullOrEmpty(question))
            {
                entity =  Context.Learnings.FirstOrDefault(x => x.Question == question);
            }
            return Mapper.Map<LearningDto>(entity);
        }

        public  void RecalcLearningTokens(bool isFullRecalc)
        {
            var qry = Context.Learnings.AsQueryable();
            if (!isFullRecalc)
            {
                qry = qry.Where(x => string.IsNullOrEmpty(x.Tokens));
            }
            var entities =  qry.ToList();
            foreach (var entity in entities) {
                var rabbitId = Guid.NewGuid();
                if (_rabbitWorker.SendQuestion(rabbitId, new List<string>() { entity.Question }))
                {
                    var fixedWords = _rabbitWorker.ReceiveAnswers(rabbitId);
                    if (!string.IsNullOrEmpty(fixedWords))
                    {
                        entity.Tokens = fixedWords;
                         Context.SaveChanges();
                    }
                }
            }
        }

        public  Guid RunLearningCommand(string command)
        {
            var rabbitId = Guid.NewGuid();
            if (_rabbitWorker.SendModelCommand(rabbitId, command, ""))
            {
                var entity = new ModelLearning() { Id = rabbitId, CreateDate = DateTime.Now, Command = command };
                 Context.ModelLearnings.Add(entity);
                 Context.SaveChanges();
            }
            return rabbitId;
        }


        public  ModelLearningDto GetModelLearning(Guid id)
        {
            var entity =  Context.ModelLearnings.FirstOrDefault(x => x.Id == id);
            return Mapper.Map<ModelLearningDto>(entity);
        }

        public  void RunPublishCommand(string command, Guid modelId)
        {
            var entity =  Context.ModelLearnings.First(x => x.Id == modelId);
            //var doc =  _docStore.GetFile(entity.ModelDocumentId.Value);
            //var model = Encoding.UTF8.GetString(doc.Body);
            var rabbitId = Guid.NewGuid();
            _rabbitWorker.SendModelCommand(rabbitId, command, modelId.ToString());

            var activeEntities =  Context.ModelLearnings.Where(x => x.IsActive).ToList();
            foreach(var ae in activeEntities)
            {
                ae.IsActive = false;
            }
            entity.IsActive = true;
             Context.SaveChanges();
        }

        public  void StoreLearningReport(Guid id, LearningModelAnswerDto report, string fullAnswer)
        {
             _docStore.CheckSbtLifeCatalog();

            var model = Encoding.UTF8.GetBytes(report.Model);
            var docId =  _docStore.StoreFile(_user.Id, CommonConst.DocumentStorage.CatalogId, CommonConst.DocumentStorage.CatalogName, model);

            var entity =  Context.ModelLearnings.First(x => x.Id == id);
            entity.AnswerDate = DateTime.Now;
            entity.Markup = report.Markup;
            entity.Accuracy = report.Quality_metrics.Accuracy;
            entity.Precision = report.Quality_metrics.Precision_macro_average;
            entity.Recall = report.Quality_metrics.Recall_macro_average;
            entity.F1 = report.Quality_metrics.Macro_f1;
            entity.ModelDocumentId = docId;
            entity.FullAnswer = fullAnswer;

            for (var i = 1; i < report.Quality_metrics.Classes_report.Length; i++)
            {
                var repSrc = report.Quality_metrics.Classes_report[i];
                var rep = new ModelLearningReport()
                {
                    ModelLearningId = id,
                    CategoryId = repSrc[0] == "0" ? (Guid?)null : Guid.Parse(repSrc[0]),
                    Precision = float.Parse(repSrc[1], CultureInfo.InvariantCulture),
                    Recall = float.Parse(repSrc[2], CultureInfo.InvariantCulture),
                    F1 = float.Parse(repSrc[3], CultureInfo.InvariantCulture),
                    Markup = int.Parse(repSrc[4], CultureInfo.InvariantCulture)
                };
                 Context.ModelLearningReports.Add(rep);
            }

            for (var i = 0; i < report.Conf_matrix.Length; i++)
            {
                var confs = report.Conf_matrix[i];
                for (var j = 0; j < confs.Length; j++)
                {
                    var conf = confs[j];
                    if (double.TryParse(conf, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                    {
                        if (value > 0)
                        {
                            var confEntity = new ModelLearningConf()
                            {
                                ModelLearningId = id,
                                CategoryId = report.Conf_matrix_labels[i] == "0" ? (Guid?) null : Guid.Parse(report.Conf_matrix_labels[i]),
                                ToCategoryId = report.Conf_matrix_labels[j] == "0" ? (Guid?)null : Guid.Parse(report.Conf_matrix_labels[j]),
                                Confusion = value
                            };
                             Context.ModelLearningConfs.Add(confEntity);
                        }
                    }
                }
            }

            for (var i = 0; i < report.Conf_markup.Question_lemmas.Length; i++)
            {
                var confEntity = new ModelLearningMarkup()
                {
                    ModelLearningId = id,
                    Question = report.Conf_markup.Question_lemmas[i],
                    CategoryFrom = report.Conf_markup.Answer_id[i] == "0" ? (Guid?) null : Guid.Parse(report.Conf_markup.Answer_id[i]),
                    CategoryTo = report.Conf_markup.Answer_id_predicted[i] == "0" ? (Guid?) null : Guid.Parse(report.Conf_markup.Answer_id_predicted[i])
                };
                 Context.ModelLearningMarkups.Add(confEntity);
            }

             Context.SaveChanges();
        }
        
        public  void CopyPatternToLearn(int categoryId)
        {
            var categ =  Context.Categorys.Single(x => x.Id == categoryId);
            var patterns =  Context.Patterns.Where(x => x.CategoryId == categoryId).ToList();
            var learns =  Context.Learnings.Where(x => x.CategoryId == categ.OriginId).ToList();
            var learnsToAdd = patterns
                .Select(x => x.Phrase
                    .Replace("<", " ").Replace(">", " ").Replace("(", " ").Replace(")", " ").Replace("  ", " "))
                .Where(x => learns.All(y => y.Question != x)).Select(x => new Learning()
            {
                CategoryId = categ.OriginId, 
                Question = x
                    
            }).ToList();
             Context.Learnings.AddRange(learnsToAdd);
             Context.SaveChanges();
            foreach (var learn in learnsToAdd)
            {
                 UpdateTokens(learn);
            }
        }

        
    }

    class ThreadTokens
    {
        private IServiceProvider _service { get; set; }
        private IRabbitWorker _rabbitWorker { get; set; }
        private ChatBotContext _storage { get; set; }
        private int _id;
        private string _question;

        public ThreadTokens(IServiceProvider service, int id, string question)
        {
            _service = service;
            _id = id;
            _question = question;
        }

        public void ThreadProc()
        {
            var scopeFactory = _service.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                _storage = (ChatBotContext)scope.ServiceProvider.GetService(typeof(ChatBotContext));
                _rabbitWorker = (IRabbitWorker)scope.ServiceProvider.GetService(typeof(IRabbitWorker));
                var rabbitId = Guid.NewGuid();
                if (_rabbitWorker.SendQuestion(rabbitId, new List<string>() { _question }))
                {
                    var fixedWords = _rabbitWorker.ReceiveAnswers(rabbitId);
                    if (!string.IsNullOrEmpty(fixedWords))
                    {
                        var entity = _storage.Learnings.FirstOrDefault(x => x.Id == _id);
                        if (entity != null)
                        {
                            entity.Tokens = fixedWords;
                            _storage.SaveChanges();
                        }
                        entity = null;
                    }
                }
            }
        }
    }
}
