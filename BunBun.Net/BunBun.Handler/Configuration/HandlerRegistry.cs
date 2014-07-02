using BunBun.Core.Messaging;
using Raven.Client;
using Raven.Client.Document;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace BunBun.Handler.Configuration {
  public class HandlerRegistry : Registry {
    public HandlerRegistry() {
      Scan(scan => {
        scan.WithDefaultConventions();
        scan.SingleImplementationsOfInterface();
        scan.TheCallingAssembly();
      });
      
      IncludeRegistry(new RabbitMQRegistry("localhost"));

      var store = new DocumentStore() {
        Url = "http://localhost:8080",
        DefaultDatabase = "BunBun"
      };
      store.Initialize();

      For<IDocumentStore>()
        .Singleton()
        .Use(() => store);

      For<IDocumentSession>()
        .LifecycleIs(new MessageScopeLifecycle())
        .Use(ctx => ctx.GetInstance<IDocumentStore>().OpenSession());
      
    }
  }
}
