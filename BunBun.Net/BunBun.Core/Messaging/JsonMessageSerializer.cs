using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BunBun.Core.Messaging {
  public class JsonMessageSerializer : IDecodeTransportMessages, IEncodeTransportMessages {
    public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings {
      ContractResolver = new MessageContractResolver(),
      TypeNameHandling = TypeNameHandling.Objects,
      TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
    };

    public IMessage Decode(byte[] encodedMessage) {
      var strMsg = Encoding.UTF8.GetString(encodedMessage);
      return ParseMessageString(strMsg);
    }

    public byte[] Encode(IMessage message) {
      var strMsg = BuildMessageString(message);
      return Encoding.UTF8.GetBytes(strMsg);
    }

    public static string BuildMessageString(IMessage message) {
      return JsonConvert.SerializeObject(message, JsonSettings);
    }

    public static IMessage ParseMessageString(string strMsg) {
      return (IMessage)JsonConvert.DeserializeObject(strMsg, JsonSettings);
    }

    public class MessageContractResolver : DefaultContractResolver {
      protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        var prop = base.CreateProperty(member, memberSerialization);

        if (!prop.Writable) {
          var property = member as PropertyInfo;
          if (property != null) {
            var hasPrivateSetter = property.GetSetMethod(true) != null;
            prop.Writable = hasPrivateSetter;
          }
        }

        return prop;
      }
    }
  }
}