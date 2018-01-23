// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.Interface.ITextureImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.U2D.Interface
{
  internal abstract class ITextureImporter
  {
    public abstract void GetWidthAndHeight(ref int width, ref int height);

    public abstract SpriteImportMode spriteImportMode { get; }

    public abstract Vector4 spriteBorder { get; }

    public abstract Vector2 spritePivot { get; }

    public abstract string assetPath { get; }

    public static bool operator ==(ITextureImporter t1, ITextureImporter t2)
    {
      if (object.ReferenceEquals((object) t1, (object) null))
        return object.ReferenceEquals((object) t2, (object) null) || t2 == (ITextureImporter) null;
      return t1.Equals((object) t2);
    }

    public static bool operator !=(ITextureImporter t1, ITextureImporter t2)
    {
      if (object.ReferenceEquals((object) t1, (object) null))
        return !object.ReferenceEquals((object) t2, (object) null) && t2 != (ITextureImporter) null;
      return !t1.Equals((object) t2);
    }

    public override bool Equals(object other)
    {
      throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
      throw new NotImplementedException();
    }
  }
}
