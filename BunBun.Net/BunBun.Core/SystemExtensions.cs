// ReSharper disable once CheckNamespace
namespace System {
  public static class SystemExtensions {
    public static string FormatWith(this string formatString, params object[] parms) {
      return string.Format(formatString, parms);
    }
  }
}
