// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidJavaException
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  public sealed class AndroidJavaException : Exception
  {
    private string mJavaStackTrace;

    internal AndroidJavaException(string message, string javaStackTrace)
      : base(message)
    {
      this.mJavaStackTrace = javaStackTrace;
    }

    public override string StackTrace
    {
      get
      {
        return this.mJavaStackTrace + base.StackTrace;
      }
    }
  }
}
