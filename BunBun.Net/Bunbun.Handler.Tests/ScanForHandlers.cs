using System;
using System.Linq;
using System.Reflection;
using BunBun.Core.Messaging;
using BunBun.Handler.Configuration;
using NUnit.Framework;
using SharpTestsEx;

namespace BunBun.Handler.Tests {
  [TestFixture]
  public class ScanForHandlers {

    [Test]
    public void ScanningForHandlerTypes() {
      var handlerMap = new HandlerMap();
      var types = TypeScanner.ScanHandlers(Assembly.GetAssembly(typeof(HandlerHost))).ToList();

      types.Should().Not.Be.Empty();
      foreach (var handlerType in types) {
        handlerMap.RegisterHandler(handlerType);
      }

      foreach (var handler in handlerMap.AllHandlers) {
        Console.WriteLine("Registered type: {0} in '{1}'", handler.Type, handler.Queue);
        
        handler.MessagesHandled.Should().Not.Be.Empty();
        handler.MessagesHandled.First().Type.Should().Not.Be.Null();

        foreach (var record in handler.MessagesHandled) {
            Console.WriteLine("\t- handles {0}", record.Type);
        }
      }

    }

  }
}
