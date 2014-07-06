using System;
using System.Collections.Generic;
using System.Linq;

namespace BunBun.Core.Messaging.Topologies {
  public class UnnamedSingleQueueRoundRobinTopology : IRabbitMqTopology {
    private readonly string[] Queues;
    private readonly IEnumerator<string> QueueEnumerator;
    private readonly HandlerMap Handlers;

    public UnnamedSingleQueueRoundRobinTopology(HandlerMap handlers, params string[] queues) {
      Handlers = handlers;
      Queues = queues;
      QueueEnumerator = QueueCycle(Queues);
    }

    public IEnumerable<RabbitMqMessageTarget> GetDestinations(IMessage msg) {
      return new List<RabbitMqMessageTarget> {
        new RabbitMqMessageTarget {
          Exchange = "",
          BindingKey = NextQueue()
        }
      };
    }

    public RabbitMqDeclarations GetDeclarations() {
      return new RabbitMqDeclarations {
        Queues = Queues.Select(name => new RabbitMqQueue(name))
      };
    }

    public IEnumerable<Type> GetConsumers(Type msgType, string queue) {
      return Handlers.AllHandlers.Where(h => h.MessagesHandled.Select(m => m.Type).Contains(msgType)).Select(h => h.Type);
    }
    private string NextQueue() {
      QueueEnumerator.MoveNext();
      return QueueEnumerator.Current;
    }

    private IEnumerator<string> QueueCycle(IEnumerable<string> queues) {
      while (true) {
        foreach (var queue in queues) {
          yield return queue;
        }
      }
    } 
  }
}