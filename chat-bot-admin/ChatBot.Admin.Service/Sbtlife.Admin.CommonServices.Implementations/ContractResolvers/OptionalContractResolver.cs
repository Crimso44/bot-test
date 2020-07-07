using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ChatBot.Admin.Common.Optional;

namespace ChatBot.Admin.CommonServices.ContractResolvers
{
    internal class OptionalContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var propertyTypeInfo = property.PropertyType.GetTypeInfo();

            if (IsOptionalTypeProperty(propertyTypeInfo))
                property.ShouldSerialize = inst =>
                {
                    var opt = ((PropertyInfo)member).GetMethod.Invoke(inst, null);
                    return opt != null;
                };

            return property;
        }

        private bool IsOptionalTypeProperty(TypeInfo typeInfo)
        {
            return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Optional<>);
        }
    }
}
