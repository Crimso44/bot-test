using System;
using RabbitMQ.Client;

namespace ChatBot.Admin.Common.Rabbit.Abstractions
{
    public interface IRabbit
    {
        IModel Channel { get; set; }
        string QueueName { get; }
        string QueueReplyName { get; }
        string TaskName { get; }
        void SendMessage(Guid id, object[] data);
    }

    public interface IDictInPublisher : IRabbit
    {
    }

    public interface ISpellCheckInPublisher : IRabbit
    {
    }

    public interface ISpellCheckOutConsumer : IRabbit
    {
    }

    public interface IModelInPublisher : IRabbit
    {
    }

    public interface IModelOutConsumer : IRabbit
    {
    }


}
