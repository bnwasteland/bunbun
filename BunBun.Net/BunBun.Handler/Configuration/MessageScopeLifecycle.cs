using BunBun.Handler.Messaging;
using StructureMap.Pipeline;

namespace BunBun.Handler.Configuration {
  public class MessageScopeLifecycle : ILifecycle {
    public string Scope { get { return "Message Scope Lifecycle"; } }
    
    public void EjectAll() {
      FindCache().DisposeAndClear();
    }

    public IObjectCache FindCache() {
      return MessageScope.Current.ObjectCache;
    }
  }
}