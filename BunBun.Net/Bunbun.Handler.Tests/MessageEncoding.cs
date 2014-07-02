using System;
using BunBun.Core.Messaging;
using NUnit.Framework;
using SharpTestsEx;

namespace BunBun.Handler.Tests
{
  [TestFixture]
  public class MessageEncoding
  {
    [Test]
    public void Encoding() {
      var msg = new FodderMessage("bloaw.");
      var strMsg = JsonMessageSerializer.BuildMessageString(msg);
      
      Console.WriteLine(strMsg);

      var decoded = JsonMessageSerializer.ParseMessageString(strMsg);
      decoded.Should().Be.OfType<FodderMessage>();
      var recvd = (FodderMessage) decoded;
      recvd.Value.Should().Be.EqualTo("bloaw.");
    }


    public class FodderMessage : IMessage {
      public FodderMessage(string val) {
        Value = val;
      }

      public string Value { get; private set; }
    }
  }
}
