using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BunBun.Handler {
  public interface ILog {
    void Log(string logMessage);
  }

  public class ConsoleLogger : ILog {
    public void Log(string logMessage) {
      Console.WriteLine(" [{1}] {0}", logMessage, Thread.CurrentThread.ManagedThreadId);
    }
  }
}
