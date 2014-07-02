using System;

namespace BunBun.Core.Commands {
  public class UpsertCourse : ICommand {
    public Guid Id { get; set; }
    public string Title { get; set; }
  }
}