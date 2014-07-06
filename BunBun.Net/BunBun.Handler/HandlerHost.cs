using System;
using BunBun.Core.Messaging;
using BunBun.Handler.Configuration;
using BunBun.Handler.Messaging;
using RabbitMQ.Client;
using StructureMap;

namespace BunBun.Handler {
  public class HandlerHost {
    private static void Main() {
      Console.CancelKeyPress += ConsoleOnCancelKeyPress;
      
      ObjectFactory.Configure(config => config.AddRegistry(new HandlerRegistry("caprice", "learning")));

      var channel = ObjectFactory.GetInstance<IModel>();
      var topology = ObjectFactory.GetInstance<IRabbitMqTopology>();

      channel.Declare(topology.GetDeclarations());

      ObjectFactory.GetInstance<MessageLoop>().Run("learning");
    }
    
    private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs consoleCancelEventArgs) {
      Console.WriteLine(" Shutting down.");
      Environment.Exit(0);
    }
  }
}
