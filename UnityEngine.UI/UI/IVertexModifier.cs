// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IVertexModifier
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
  [Obsolete("Use IMeshModifier instead", true)]
  public interface IVertexModifier
  {
    [Obsolete("use IMeshModifier.ModifyMesh (VertexHelper verts)  instead", true)]
    void ModifyVertices(List<UIVertex> verts);
  }
}
