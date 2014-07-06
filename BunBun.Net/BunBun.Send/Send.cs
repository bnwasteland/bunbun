using System;
using BunBun.Core.Commands;
using BunBun.Core.Messaging;
using RabbitMQ.Client;
using StructureMap;

namespace BunBun.Send {
  class Send {
    static void Main() {
      ObjectFactory.Configure(config => config.AddRegistry(new SenderRegistry("caprice", "learning")));
      
      var channel = ObjectFactory.GetInstance<IModel>();
      var topology = ObjectFactory.GetInstance<IRabbitMqTopology>();
      
      channel.Declare(topology.GetDeclarations());

      var bus = ObjectFactory.GetInstance<IBus>();

      bus.Send(new UpsertCourse { Id = Guid.NewGuid(), Title = "A Course of Two Cities" });  
      
      ObjectFactory.Container.Dispose();
    }
  }
}
