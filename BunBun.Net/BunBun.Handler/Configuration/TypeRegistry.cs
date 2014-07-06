using System.Reflection;
using BunBun.Core.Messaging;
using Raven.Abstractions.Extensions;
using StructureMap.Configuration.DSL;

namespace BunBun.Handler.Configuration {
  public class TypeRegistry : Registry {
    public TypeRegistry() {
      
      For<HandlerMap>()
        .Singleton()
        .Use(() => {
          var handlerMap = new HandlerMap();
          TypeScanner.ScanHandlers(Assembly.GetExecutingAssembly()).ForEach(handlerMap.RegisterHandler);
          return handlerMap;
        });
    }
  }
}
