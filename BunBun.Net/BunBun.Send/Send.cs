using System;
using System.Diagnostics;
using BunBun.Core.Commands;
using BunBun.Core.Messaging;
using RabbitMQ.Client;
using StructureMap;

namespace BunBun.Send {
  class Send {
    static void Main() {
      ObjectFactory.Configure(config => config.AddRegistry(new RabbitMQRegistry("localhost")));

      var channel = ObjectFactory.GetInstance<IModel>();
      channel.QueueDeclare("tnw.learning", true, false, false, null);
      channel.QueueDeclare("tnw.wall", true, false, false, null);

      var sender = ObjectFactory.GetInstance<MessageSender>();
      
      var id = new Guid("918A92E8-DBAE-43AA-856C-9B191613DBFF");

      for (var i = 0; i < 5; i++) {
        sender.Send("tnw.learning", new UpsertCourse {Id = id, Title = "Rugby"});
        sender.Send("tnw.wall", new UpsertCourse {Id = id, Title = "Futbol"});
      }

      ObjectFactory.Container.Dispose();
    }
  }
}
