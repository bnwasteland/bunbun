using System;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using BunBun.Core.Messaging;
using StructureMap.Pipeline;

namespace BunBun.Handler.Messaging {
  public class MessageScope : IDisposable {
    public readonly string Queue;
    
    public TransactionScope TxScope = new TransactionScope(TransactionScopeOption.RequiresNew);
    public IObjectCache ObjectCache = new MainObjectCache();
    public IList<IMessage> OutboundMessages = new List<IMessage>();

    public MessageScope(string queue) {
      if (Current != null) throw new InvalidOperationException("Message scopes do not nest.");
      Current = this;
      Queue = queue;
    }
    
    public void Commit() {
      TxScope.Complete();
    }
    
    public void Dispose() {
      ObjectCache.DisposeAndClear();
      TxScope.Dispose();

      Current = null;
    }

    private static readonly ThreadLocal<MessageScope> Container = new ThreadLocal<MessageScope>(() => null);

    public static MessageScope Current {
      get { return Container.Value; }
      private set { Container.Value = value; }
    }

  }
}
