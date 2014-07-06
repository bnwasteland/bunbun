using BunBun.Core.Messaging;
using Raven.Client;
using Raven.Client.Document;
using StructureMap.Configuration.DSL;

namespace BunBun.Handler.Configuration {
  public class HandlerRegistry : Registry {
    public HandlerRegistry(string queueServer, params string[] queues) {
      Scan(scan => {
        scan.WithDefaultConventions();
        scan.SingleImplementationsOfInterface();
        scan.TheCallingAssembly();
      });
      
      IncludeRegistry(new RabbitMqRegistry(queueServer, queues));
      IncludeRegistry(new TypeRegistry());
      
      For<IDocumentStore>()
        .Singleton()
        .Use(() => {
          var store = new DocumentStore {
            Url = "http://localhost:8080",
            DefaultDatabase = "BunBun"
          };
          store.Initialize();
          return store;
        });

      For<IDocumentSession>()
        .LifecycleIs(new MessageScopeLifecycle())
        .Use(ctx => ctx.GetInstance<IDocumentStore>().OpenSession());
      
    }
  }
}
