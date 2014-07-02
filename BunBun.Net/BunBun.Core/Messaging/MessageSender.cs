using RabbitMQ.Client;

namespace BunBun.Core.Messaging {
  public interface ISendMessages {
    void Send(string queue, IMessage msg);
  }

  public class MessageSender : ISendMessages {
    public IEncodeTransportMessages Encoder { get; private set; }
    public IModel Channel { get; private set; }
    public IBasicProperties MessageProperties { get; private set; }

    public MessageSender(IEncodeTransportMessages encoder, IModel channel) {
      Encoder = encoder;
      Channel = channel;
      MessageProperties = channel.CreateBasicProperties();
      MessageProperties.SetPersistent(true);
    }

    public void Send(string queue, IMessage msg) {
      Channel.BasicPublish("", queue, MessageProperties, Encoder.Encode(msg));
    }
  }
}