using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace BunBun.Core.Messaging {
  public interface IRabbitMqTopology {
    RabbitMqDeclarations GetDeclarations();

    IEnumerable<RabbitMqMessageTarget> GetDestinations(IMessage msg);
    IEnumerable<Type> GetConsumers(Type msgType, string queue);
  }

  public class RabbitMqMessageTarget {
    public string Exchange { get; set; }
    public string BindingKey { get; set; }
  }

  public class RabbitMqDeclarations {
    public RabbitMqDeclarations() {
      Exchanges = new List<RabbitMqExchange>();
      Queues = new List<RabbitMqQueue>();
      Bindings = new List<RabbitMqBinding>();
    }

    public IEnumerable<RabbitMqExchange> Exchanges { get; set; }
    public IEnumerable<RabbitMqQueue> Queues { get; set; }
    public IEnumerable<RabbitMqBinding> Bindings { get; set; } 
  }

  public class RabbitMqExchange {
    public RabbitMqExchange() {
      Durable = true;
    }

    public RabbitMqExchange(string name, string type) : this() {
      Name = name;
      Type = type;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public bool Durable { get; set; }
    public bool AutoDelete { get; set; }
    public IDictionary<string, object> Args { get; set; }
  }

  public class RabbitMqQueue {
    public RabbitMqQueue(string name, bool durable = true) {
      Name = name;
      Durable = durable;
    }

    public string Name { get; set; }
    public bool Durable { get; set; }
    public bool Exclusive { get; set; }
    public bool AutoDelete { get; set; }
    public IDictionary<string, object> Args { get; set; }
  }

  public class RabbitMqBinding {
    public string Exchange { get; set; }
    public string Queue { get; set; }
    public string RoutingKey { get; set; }
    public IDictionary<string, object> Args { get; set; }
  }

  public static class ChannelExtensions {
    public static void Declare(this IModel channel, RabbitMqDeclarations declarations) {
      foreach (var exchange in declarations.Exchanges) {
        channel.ExchangeDeclare(exchange.Name, exchange.Type, exchange.Durable, exchange.AutoDelete, exchange.Args);
      }

      foreach (var queue in declarations.Queues) {
        channel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete, queue.Args);
      }

      foreach (var binding in declarations.Bindings) {
        channel.QueueBind(binding.Queue, binding.Exchange, binding.RoutingKey, binding.Args);
      }
    }
  }
}