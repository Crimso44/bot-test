using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl;
using SBoT.Code.Classes;
using SBoT.Code.Dto;
using SBoT.Code.Entity.Interfaces;

namespace SBoT.Code.Entity
{
    public abstract class Rabbit
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

            object[] arguments = new object[] { data, new object(), new object()};

            var messagePublish = JsonConvert.SerializeObject(arguments);
            var body = Encoding.UTF8.GetBytes(messagePublish);

            Channel.BasicPublish(exchange: "",
                routingKey: QueueName, //# Название очереди на вход
                basicProperties: props,
                body: body);
        }

        public void SendMessageAnswer(Guid id, string[] data) 
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

            var answ = new RabbitAnswerDto<string[]>()
            {
                task_id = id,
                result = data,
                status = "SUCCESS"
            };

            var messagePublish = JsonConvert.SerializeObject(answ);
            var body = Encoding.UTF8.GetBytes(messagePublish);

            Channel.BasicPublish(exchange: "",
                routingKey: QueueName, //# Название очереди на вход
                basicProperties: props,
                body: body);
        }
    }


    public class SpellCheckInPublisher : Rabbit, ISpellCheckInPublisher
    {
        public override string QueueName => Const.RabbitQueueName.SpellCheckIn;
        public override string QueueReplyName => Const.RabbitQueueName.SpellCheckOut;
        public override string TaskName => Const.RabbitTaskName.SpellCheck;
    }

    public class SpellCheckInConsumer : Rabbit, ISpellCheckInConsumer
    {
        public override string QueueName => Const.RabbitQueueName.SpellCheckIn;
        public override string QueueReplyName => Const.RabbitQueueName.SpellCheckOut;
        public override string TaskName => Const.RabbitTaskName.SpellCheck;
    }

    public class SpellCheckOutPublisher : Rabbit, ISpellCheckOutPublisher
    {
        public override string QueueName => Const.RabbitQueueName.SpellCheckOut;
        public override string TaskName => Const.RabbitTaskName.SpellCheck;
    }

    public class SpellCheckOutConsumer : Rabbit, ISpellCheckOutConsumer
    {
        public override string QueueName => Const.RabbitQueueName.SpellCheckOut;
        public override string TaskName => Const.RabbitTaskName.SpellCheck;
    }

    public class DictInPublisher : Rabbit, IDictInPublisher
    {
        public override string QueueName => Const.RabbitQueueName.CatalogIn;
        public override string TaskName => Const.RabbitTaskName.Catalog;
    }

    public class MtoInPublisher : Rabbit, IMtoInPublisher
    {
        public override string QueueName => Const.RabbitQueueName.MtoIn;
        public override string TaskName => Const.RabbitTaskName.Mto;
        public override string QueueReplyName => Const.RabbitQueueName.MtoOut;
    }

    public class MtoOutConsumer : Rabbit, IMtoOutConsumer
    {
        public override string QueueName => Const.RabbitQueueName.MtoOut;
        public override string TaskName => Const.RabbitTaskName.Mto;
    }

}
