using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BunBun.Core.Messaging;
using BunBun.Core.Messaging.Topologies;
using NUnit.Framework;

namespace BunBun.Handler.Tests {
  [TestFixture]
  public class RoundRobinQueues {
    [Test]
    public void SelectingQueues() {
      var topology = new UnnamedSingleQueueRoundRobinTopology(new HandlerMap(), "one", "two", "three");
      var message = new MessageEncoding.FodderMessage("blah");
      
      for (int i = 0; i < 7; i++) {
        foreach (var destination in topology.GetDestinations(message)) {
          Console.WriteLine("{0}:{1}", destination.Exchange, destination.BindingKey);
        }
        Console.WriteLine();
      }
    }
  }
}
