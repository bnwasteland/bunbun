using BunBun.Core.Commands;
using BunBun.Core.Entities;
using BunBun.Core.Events;
using BunBun.Core.Messaging;
using Raven.Client;

namespace BunBun.Handler.Learning {
  [HandlerQueue("learning")]
  public class UpsertCourseHandler : IHandleMessages<UpsertCourse> {
    private readonly IDocumentSession DocumentSession;
    private readonly IBus Bus;

    public UpsertCourseHandler(IDocumentSession documentSession, IBus bus) {
      DocumentSession = documentSession;
      Bus = bus;
    }

    public void Handle(UpsertCourse message) {
      var course = DocumentSession.Load<Course>(message.Id) ?? new Course(message.Id).Store(DocumentSession);

      if (course.Title != message.Title) {
        Bus.Send(new CourseTitleChanged {
          CourseId = course.Id,
          OldTitle = course.Title,
          NewTitle = message.Title,
        });

        course.Title = message.Title;
      }

      DocumentSession.SaveChanges();
      MessageScope.Current.Commit();
    }
  }
}
