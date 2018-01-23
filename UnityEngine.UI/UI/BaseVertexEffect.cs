// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.BaseVertexEffect
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Base class for effects that modify the the generated Vertex Buffers.</para>
  /// </summary>
  [Obsolete("Use BaseMeshEffect instead", true)]
  public abstract class BaseVertexEffect
  {
    [Obsolete("Use BaseMeshEffect.ModifyMeshes instead", true)]
    public abstract void ModifyVertices(List<UIVertex> vertices);
  }
}
