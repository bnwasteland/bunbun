using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BunBun.Core.Messaging;
using Fasterflect;

namespace BunBun.Handler.Configuration {
  public class TypeScanner {
    public static IEnumerable<Type> ScanHandlers(Assembly assembly) {
      return assembly.GetTypes().Where(t => t.Implements(typeof (IHandleMessages<>)));
    } 
  }
}
