namespace BunBun.Core.Messaging {
  public interface IHandleMessages<T> where T : IMessage {
    void Handle(T message);
  }
}