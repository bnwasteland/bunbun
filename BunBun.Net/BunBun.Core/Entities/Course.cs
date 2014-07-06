using System;

namespace BunBun.Core.Entities {
  public class Course : IEntity {
    private Course() {}

    public Course(Guid id) {
      Id = id;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
  }
}
