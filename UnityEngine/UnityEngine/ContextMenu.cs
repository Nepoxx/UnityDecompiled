// Decompiled with JetBrains decompiler
// Type: UnityEngine.ContextMenu
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  [RequiredByNativeCode]
  public sealed class ContextMenu : Attribute
  {
    public readonly string menuItem;
    public readonly bool validate;
    public readonly int priority;

    public ContextMenu(string itemName)
      : this(itemName, false)
    {
    }

    public ContextMenu(string itemName, bool isValidateFunction)
      : this(itemName, isValidateFunction, 1000000)
    {
    }

    public ContextMenu(string itemName, bool isValidateFunction, int priority)
    {
      this.menuItem = itemName;
      this.validate = isValidateFunction;
      this.priority = priority;
    }
  }
}
