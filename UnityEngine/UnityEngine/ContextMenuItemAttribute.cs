// Decompiled with JetBrains decompiler
// Type: UnityEngine.ContextMenuItemAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
  public class ContextMenuItemAttribute : PropertyAttribute
  {
    public readonly string name;
    public readonly string function;

    public ContextMenuItemAttribute(string name, string function)
    {
      this.name = name;
      this.function = function;
    }
  }
}
