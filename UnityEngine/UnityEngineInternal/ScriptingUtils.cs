// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.ScriptingUtils
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Reflection;

namespace UnityEngineInternal
{
  public class ScriptingUtils
  {
    public static Delegate CreateDelegate(Type type, MethodInfo methodInfo)
    {
      return Delegate.CreateDelegate(type, methodInfo);
    }
  }
}
