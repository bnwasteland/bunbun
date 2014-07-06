using BunBun.Core.Events;
using BunBun.Core.Messaging;

namespace BunBun.Handler.Learning {
  [HandlerQueue("wall")]
  public class CourseTitleChangedHandler : IHandleMessages<CourseTitleChanged> {
    public void Handle(CourseTitleChanged message) {
      //Do Something
    }
  }
}