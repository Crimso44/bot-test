
using System;

namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface IJsonSerializerService
    {
        string GetJsonString<T>(T data);
        T GetJsonObject<T>(string data);
        object JObjectToObject(Type type, object data);
    }
}
