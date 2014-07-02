using System;
using BunBun.Core.Commands;
using BunBun.Core.Entities;
using BunBun.Core.Events;
using BunBun.Handler.Messaging;
using Raven.Client;

namespace BunBun.Handler.Learning {
  public class UpsertCourseHandler : IHandleMessages<UpsertCourse> {
    private readonly IDocumentSession DocumentSession;

    public UpsertCourseHandler(IDocumentSession documentSession) {
      DocumentSession = documentSession;
    }

    public void Handle(UpsertCourse message) {
      var course = DocumentSession.Load<Course>(message.Id) ?? new Course(message.Id).Store(DocumentSession);

      if (course.Title != message.Title) {
        Publisher.Raise<CourseTitleChanged>(e => {
          e.CourseId = course.Id;
          e.OldTitle = course.Title;
          e.NewTitle = message.Title;
        });

        course.Title = message.Title;
      }

      DocumentSession.SaveChanges();
      MessageScope.Current.Commit();
    }
  }
}
