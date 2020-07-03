using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RabbitMQ.Client;
using SBoT.Code.Dto;

namespace SBoT.Code.Entity.Interfaces
{
    public interface IRabbit
    {
        IModel Channel { get; set; }
        string QueueName { get; }
        string QueueReplyName { get; }
        string TaskName { get; }
        void SendMessage(Guid id, object[] data);
        void SendMessageAnswer(Guid id, string[] data);
    }

    public interface ISpellCheckInPublisher : IRabbit
    {
    }

    public interface ISpellCheckInConsumer : IRabbit
    {
    }

    public interface ISpellCheckOutPublisher : IRabbit
    {
    }

    public interface ISpellCheckOutConsumer : IRabbit
    {
    }

    public interface IMtoInPublisher : IRabbit
    {
    }

    public interface IMtoOutConsumer : IRabbit
    {
    }

    public interface IDictInPublisher : IRabbit
    {
    }


}
