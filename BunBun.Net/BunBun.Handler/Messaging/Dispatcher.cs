using System;
using BunBun.Core.Messaging;
using StructureMap;

namespace BunBun.Handler {
  public interface IDispatchDomainMessages {
    void Dispatch(IMessage message);
  }
  
  public class Dispatcher : IDispatchDomainMessages {
    private readonly IRabbitMqTopology Topology;
    private readonly ILog Logger;

    public Dispatcher(IRabbitMqTopology topology, ILog logger) {
      Topology = topology;
      Logger = logger;
    }

    public void Dispatch(IMessage message) {
      var handlerTypes = Topology.GetConsumers(message.GetType(), MessageScope.Current.Queue);
      var closedHandlerInterface = typeof(IHandleMessages<>).MakeGenericType(message.GetType());
      var handlerMethod = closedHandlerInterface.GetMethod("Handle");
      
      foreach (var type in handlerTypes) {
        var handler = ObjectFactory.GetInstance(type);
        Logger.Log("Activating handler '{0}'".FormatWith(handler.GetType()));
        handlerMethod.Invoke(handler, new object[] { message });
      }
    }
  }
}
