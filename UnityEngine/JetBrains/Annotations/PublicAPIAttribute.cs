// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.PublicAPIAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace JetBrains.Annotations
{
  [MeansImplicitUse]
  public sealed class PublicAPIAttribute : Attribute
  {
    public PublicAPIAttribute()
    {
    }

    public PublicAPIAttribute([NotNull] string comment)
    {
      this.Comment = comment;
    }

    [NotNull]
    public string Comment { get; private set; }
  }
}
