using System;
using BunBun.Core.Messaging;
using StructureMap;

namespace BunBun.Handler {
  public interface IDispatchDomainMessages {
    void Dispatch(IMessage message);
  }
  
  public class Dispatcher : IDispatchDomainMessages {
    private readonly ILog Logger;

    public Dispatcher(ILog logger) {
      Logger = logger;
    }

    public void Dispatch(IMessage message) {
      var closedHandlerInterface = typeof (IHandleMessages<>).MakeGenericType(message.GetType());
      var handlerMethod = closedHandlerInterface.GetMethod("Handle");
      var handler = ObjectFactory.GetInstance(closedHandlerInterface);
      Logger.Log("Activating handler '{0}'".FormatWith(handler.GetType()));
      handlerMethod.Invoke(handler, new object[] {message});
    }
  }
}
