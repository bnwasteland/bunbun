using RabbitMQ.Client;

namespace BunBun.Core.Messaging {
  public class RabbitBus : IBus {
    public IEncodeTransportMessages Encoder { get; private set; }
    public IRabbitMqTopology Topology { get; private set; }
    public IModel Channel { get; private set; }
    public IBasicProperties MessageProperties { get; private set; }

    public RabbitBus(IEncodeTransportMessages encoder, IRabbitMqTopology topology, IModel channel) {
      Encoder = encoder;
      Topology = topology;
      Channel = channel;
      MessageProperties = channel.CreateBasicProperties();
      MessageProperties.SetPersistent(true);
    }

    public void Send(IMessage msg) {
      var body = Encoder.Encode(msg);
      foreach (var dest in Topology.GetDestinations(msg)) {
        Channel.BasicPublish(dest.Exchange, dest.BindingKey, MessageProperties, body);
      }
    }
  }
}