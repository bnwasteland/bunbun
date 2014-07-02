using System;
using System.Threading;
using BunBun.Handler.Configuration;
using StructureMap;

namespace BunBun.Handler {
  public class HandlerHost {
    private static void Main(string[] args) {
      Console.CancelKeyPress += ConsoleOnCancelKeyPress;
      
      ObjectFactory.Configure(config => config.AddRegistry(new HandlerRegistry()));

      StartThread("tnw.learning");
      StartThread("tnw.wall");
    }
    
    private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs consoleCancelEventArgs) {
      Console.WriteLine(" Shutting down.");
      Environment.Exit(0);
    }

    private static void StartThread(string queueName) {
      var worker = new Thread(() => {
        var loop = ObjectFactory.GetInstance<MessageLoop>();
        loop.Run(queueName);
      });
      worker.Start();
    }
  }
}
