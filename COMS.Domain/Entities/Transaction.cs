using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace COMS.Domain.Entities
{
    //[Serializable]
    //[XmlInclude(typeof(Customer))]
    //[KnownType(typeof(Customer))]

    //[XmlInclude(typeof(Order))]
    //[KnownType(typeof(Order))]

    //[JsonConverter(typeof(JsonInheritanceConverter), "TypeName")]

    public class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string ModelRequest { get; set; }

        //protected T GetModelRequest<T>()
        //{
        //    return CustomDeserializeObject<T>(ModelRequest);
        //}

        public void SetModelRequest<T>(T model)
        {
            // Configuração para ser aceito na Ingram (Eles não utilizam CamelCase)
            ModelRequest = CustomSerializeObject(model);
        }

        public override string ToString()
        {
            return $"[{Id}][{Status}] {Name}";
        }

        public string CustomSerializeObject<T>(T objeto)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CustomContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            return JsonConvert.SerializeObject(objeto, settings);
        }

        //public T CustomDeserializeObject<T>(string model)
        //{
        //    if (string.IsNullOrEmpty(model))
        //    {
        //        return default(T);
        //    }

        //    var jsonSerializerSettings = new JsonSerializerSettings
        //    {
        //        ContractResolver = new CustomContractResolver(),
        //        NullValueHandling = NullValueHandling.Ignore,
        //        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        //    };
        //    jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

        //    try
        //    {
        //        return JsonConvert.DeserializeObject<T>(model, jsonSerializerSettings);
        //    }
        //    catch (JsonSerializationException ex)
        //    {
        //        throw new InvalidOperationException($"Failed to deserialize model to type {typeof(T)}", ex);
        //    }
        //}
    }

    public class CustomContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            return property;
        }
    }
}
