using Raven.Client;

namespace BunBun.Core.Entities {
  public static class EntityExtensions {
    public static T Store<T>(this T entity, IDocumentSession session) where T : IEntity {
      session.Store(entity);
      return entity;
    }
  }
}
