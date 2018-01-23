// Decompiled with JetBrains decompiler
// Type: UnityEngine.CreateAssetMenuAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class CreateAssetMenuAttribute : Attribute
  {
    public string menuName { get; set; }

    public string fileName { get; set; }

    public int order { get; set; }
  }
}
