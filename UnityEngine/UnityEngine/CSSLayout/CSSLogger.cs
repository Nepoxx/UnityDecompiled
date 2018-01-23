// Decompiled with JetBrains decompiler
// Type: UnityEngine.CSSLayout.CSSLogger
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;

namespace UnityEngine.CSSLayout
{
  internal static class CSSLogger
  {
    public static CSSLogger.Func Logger = (CSSLogger.Func) null;

    public static void Initialize()
    {
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Func(CSSLogLevel level, string message);
  }
}
