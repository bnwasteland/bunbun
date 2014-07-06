using System;
using System.Threading;

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
