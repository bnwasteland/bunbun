namespace BunBun.Core.Messaging {

  public interface IBus {
    void Send(IMessage msg);
  }
}