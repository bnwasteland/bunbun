using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System {
  public static class SystemExtensions {
    public static string FormatWith(this string formatString, params object[] parms) {
      return string.Format(formatString, parms);
    }
  }
}
