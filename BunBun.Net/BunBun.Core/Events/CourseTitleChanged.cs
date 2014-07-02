using System;

namespace BunBun.Core.Events {
  public class CourseTitleChanged : IEvent {
    public Guid CourseId { get; set; }
    public string OldTitle { get; set; }
    public string NewTitle { get; set; }
  }
}
