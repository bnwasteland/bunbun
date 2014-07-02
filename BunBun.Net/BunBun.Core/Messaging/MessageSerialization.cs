namespace BunBun.Core.Messaging {
  public interface IDecodeTransportMessages {
    IMessage Decode(byte[] encodedMessage);
  }

  public interface IEncodeTransportMessages {
    byte[] Encode(IMessage message);
  }
}
