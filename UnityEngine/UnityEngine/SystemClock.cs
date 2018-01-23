// Decompiled with JetBrains decompiler
// Type: UnityEngine.SystemClock
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  internal class SystemClock
  {
    private static readonly DateTime s_Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static DateTime now
    {
      get
      {
        return DateTime.Now;
      }
    }

    public static long ToUnixTimeMilliseconds(DateTime date)
    {
      return Convert.ToInt64((date.ToUniversalTime() - SystemClock.s_Epoch).TotalMilliseconds);
    }

    public static long ToUnixTimeSeconds(DateTime date)
    {
      return Convert.ToInt64((date.ToUniversalTime() - SystemClock.s_Epoch).TotalSeconds);
    }
  }
}
