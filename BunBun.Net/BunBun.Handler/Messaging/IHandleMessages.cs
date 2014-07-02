using BunBun.Core.Messaging;

namespace BunBun.Handler {
  public interface IHandleMessages<T> where T : IMessage {
    void Handle(T message);
  }
}