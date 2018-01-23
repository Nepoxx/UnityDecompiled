// Decompiled with JetBrains decompiler
// Type: UnityEngine.TextAreaAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public sealed class TextAreaAttribute : PropertyAttribute
  {
    public readonly int minLines;
    public readonly int maxLines;

    public TextAreaAttribute()
    {
      this.minLines = 3;
      this.maxLines = 3;
    }

    public TextAreaAttribute(int minLines, int maxLines)
    {
      this.minLines = minLines;
      this.maxLines = maxLines;
    }
  }
}
