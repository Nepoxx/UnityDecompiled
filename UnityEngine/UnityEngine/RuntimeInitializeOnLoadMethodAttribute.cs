// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeInitializeOnLoadMethodAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class RuntimeInitializeOnLoadMethodAttribute : PreserveAttribute
  {
    public RuntimeInitializeOnLoadMethodAttribute()
    {
      this.loadType = RuntimeInitializeLoadType.AfterSceneLoad;
    }

    public RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType loadType)
    {
      this.loadType = loadType;
    }

    public RuntimeInitializeLoadType loadType { get; private set; }
  }
}
