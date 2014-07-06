using System;
using System.Collections.Generic;
using System.Linq;

namespace BunBun.Core.Messaging.Topologies {
  public class DirectToRegisteredHandlersTopology : IRabbitMqTopology {
    private readonly HandlerMap Handlers;
    private readonly RabbitMqExchange Exchange;

    public DirectToRegisteredHandlersTopology(HandlerMap handlers, string exchange = "") {
      Handlers = handlers;
      Exchange = new RabbitMqExchange(exchange, "direct");
    }

    public IEnumerable<RabbitMqMessageTarget> GetDestinations(IMessage msg) {
      return Handlers.AllHandlers
        .Where(h => h.MessagesHandled
          .Select(m => m.Type)
          .Contains(msg.GetType()))
        .Select(h => h.Queue)
        .Distinct()
        .Select(q => new RabbitMqMessageTarget {Exchange = Exchange.Name, BindingKey = q});
    }

    public RabbitMqDeclarations GetDeclarations() {
      return new RabbitMqDeclarations {
        Exchanges = Exchange.Name == "" ? new RabbitMqExchange[0] : new[] { Exchange },
        Queues = RequiredQueues(),
        Bindings = RequiredBindings()
      };
    }

    public IEnumerable<Type> GetConsumers(Type msgType, string queue) {
      return Handlers.AllHandlers.Where(h => h.Queue == queue && h.MessagesHandled.Select(m => m.Type).Contains(msgType)).Select(h => h.Type);
    }

    private IEnumerable<RabbitMqQueue> RequiredQueues() {
      return Handlers.AllHandlers
        .Select(h => h.Queue)
        .Distinct()
        .Select(queue =>
          new RabbitMqQueue(queue));
    }

    private IEnumerable<RabbitMqBinding> RequiredBindings() {
      return RequiredQueues()
        .Select(q => new RabbitMqBinding { Queue = q.Name, Exchange = Exchange.Name, RoutingKey = q.Name })
        .Where(b => b.Exchange != "");
    }
  }
}