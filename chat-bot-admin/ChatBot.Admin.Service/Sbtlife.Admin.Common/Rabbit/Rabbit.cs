using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using ChatBot.Admin.Common.Rabbit.Abstractions;

namespace ChatBot.Admin.Common.Rabbit
{
    public abstract class Rabbit : IRabbit
    {
        public IModel Channel { get; set; }
        public virtual string QueueName => "";
        public virtual string QueueReplyName => "";
        public virtual string TaskName => "";

        public void SendMessage(Guid id, object[] data)
        {
            IDictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("task", TaskName);
            headers.Add("id", id.ToString());

            IBasicProperties props = Channel.CreateBasicProperties();
            props.Headers = headers;
            props.CorrelationId = (string)headers["id"];
            props.ContentEncoding = "utf-8";
            props.ContentType = "application/json";
            props.ReplyTo = QueueReplyName; //# Название очереди ответа.

            object[] arguments = new object[] { data, new object(), new object() };

            var messagePublish = JsonConvert.SerializeObject(arguments);
            var body = Encoding.UTF8.GetBytes(messagePublish);

            Channel.BasicPublish(exchange: "",
                routingKey: QueueName, //# Название очереди на вход
                basicProperties: props,
                body: body);
        }

    }

    public class DictInPublisher : Rabbit, IDictInPublisher
    {
        public override string QueueName => RabbitConst.RabbitQueueName.CatalogIn;
        public override string TaskName => RabbitConst.RabbitTaskName.Catalog;
    }

    public class SpellCheckInPublisher : Rabbit, ISpellCheckInPublisher
    {
        public override string QueueName => RabbitConst.RabbitQueueName.SpellCheckIn;
        public override string QueueReplyName => RabbitConst.RabbitQueueName.SpellCheckOut;
        public override string TaskName => RabbitConst.RabbitTaskName.SpellCheck;
    }

    public class SpellCheckOutConsumer : Rabbit, ISpellCheckOutConsumer
    {
        public override string QueueName => RabbitConst.RabbitQueueName.SpellCheckOut;
        public override string TaskName => RabbitConst.RabbitTaskName.SpellCheck;
    }

    public class ModelInPublisher : Rabbit, IModelInPublisher
    {
        public override string QueueName => RabbitConst.RabbitQueueName.ModelIn;
        public override string QueueReplyName => RabbitConst.RabbitQueueName.ModelOut;
        public override string TaskName => RabbitConst.RabbitTaskName.LearnModel;
    }

    public class ModelOutConsumer : Rabbit, IModelOutConsumer
    {
        public override string QueueName => RabbitConst.RabbitQueueName.ModelOut;
        public override string TaskName => RabbitConst.RabbitTaskName.LearnModel;
    }
}
