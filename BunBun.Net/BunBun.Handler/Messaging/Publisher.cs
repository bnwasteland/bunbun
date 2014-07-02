using System;
using BunBun.Core.Events;

namespace BunBun.Handler.Messaging {
  public class Publisher {
    public static void Raise<T>(Action<T> initializer) where T : IEvent, new() {
      var evt = new T();
      initializer(evt);

      MessageScope.Current.OutboundMessages.Add(evt);
    }
  }
}
