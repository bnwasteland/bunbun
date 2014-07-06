using BunBun.Core.Messaging.Topologies;
using RabbitMQ.Client;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BunBun.Core.Messaging {
  public class RabbitMqRegistry : Registry {
    public RabbitMqRegistry(string rabbitHost, string[] queues) {
      For<ConnectionFactory>()
        .Singleton()
        .Use(() => new ConnectionFactory { HostName = rabbitHost });

     For<IConnection>()
        .Singleton()
        .Use(ctx => ctx.GetInstance<ConnectionFactory>().CreateConnection());

      For<IModel>()
        .LifecycleIs(new ThreadLocalStorageLifecycle())
        .Use(ctx => ctx.GetInstance<IConnection>().CreateModel());

      For<IEncodeTransportMessages>().Use<JsonMessageSerializer>();
      For<IDecodeTransportMessages>().Use<JsonMessageSerializer>();

      For<IRabbitMqTopology>()
        .Singleton()
        .Use(ctx => new UnnamedSingleQueueRoundRobinTopology(ctx.GetInstance<HandlerMap>(), queues));

      For<IBus>()
        .Use<RabbitBus>();
    }
  }
}
