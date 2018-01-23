// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.U2D.SpriteDataBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
  internal abstract class SpriteDataBase
  {
    public abstract string name { get; set; }

    public abstract Rect rect { get; set; }

    public abstract SpriteAlignment alignment { get; set; }

    public abstract Vector2 pivot { get; set; }

    public abstract Vector4 border { get; set; }

    public abstract float tessellationDetail { get; set; }

    public abstract List<Vector2[]> outline { get; set; }

    public abstract List<Vector2[]> physicsShape { get; set; }
  }
}
