using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Fasterflect;

namespace BunBun.Core.Messaging {
  public class HandlerMap {
    private readonly IDictionary<string, HandlerRecord> Handlers = new Dictionary<string, HandlerRecord>();

    public void RegisterHandler<THandler>() {
      RegisterHandler(typeof(THandler));
    }

    public void RegisterHandler(Type handlerType) {
      if (!Handlers.ContainsKey(handlerType.FullName)) {
        Handlers[handlerType.FullName] = new HandlerRecord(handlerType);
      }
    }

    public IEnumerable<HandlerRecord> AllHandlers { get { return Handlers.Values; } }

    public class HandlerRecord {
      public HandlerRecord(Type handlerType) {
        Type = handlerType;
        Queue = HandlerQueueAttribute.GetQueue(handlerType);
        MessagesHandled = GetMessagesHandled(handlerType);
      }

      public string Queue { get; set; }
      public Type Type { get; set; }
      public IEnumerable<MessageRecord> MessagesHandled { get; set; }

      private static IEnumerable<MessageRecord> GetMessagesHandled(Type handlerType) {
        return handlerType.GetInterfaces()
          .Where(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IHandleMessages<>))
          .Select(t => new MessageRecord { Type = t.GenericTypeArguments[0]});
      }
    }

    public class MessageRecord {
      public Type Type { get; set; }
    }
  }
  
  [AttributeUsage(AttributeTargets.Class)]
  public class HandlerQueueAttribute : Attribute {
    public string Queue { get; set; }

    public HandlerQueueAttribute(string queue) {
      Queue = queue;
    }

    public static string GetQueue(Type handlerType) {
      var attrs = handlerType.Attributes<HandlerQueueAttribute>();
      if (attrs.Count == 0)
        return null;
      
      if (attrs.Count() > 1) 
        throw new Exception("{0} ambiguously declared to be in queues {1}".FormatWith(handlerType, string.Join(",", attrs.Select(a => a.Queue))));

      return attrs.Single().Queue;
    }
  }
}
