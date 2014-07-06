using BunBun.Core.Messaging;
using BunBun.Handler.Configuration;
using StructureMap.Configuration.DSL;

namespace BunBun.Send {
  public class SenderRegistry : Registry {
    public SenderRegistry(string queueServer, params string[] queues) {
      IncludeRegistry(new RabbitMQRegistry(queueServer, queues));
      IncludeRegistry(new TypeRegistry());
    }
  }
}
