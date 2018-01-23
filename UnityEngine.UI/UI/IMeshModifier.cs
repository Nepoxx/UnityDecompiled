// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IMeshModifier
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;

namespace UnityEngine.UI
{
  public interface IMeshModifier
  {
    /// <summary>
    ///   <para>Call used to modify mesh.</para>
    /// </summary>
    /// <param name="mesh"></param>
    [Obsolete("use IMeshModifier.ModifyMesh (VertexHelper verts) instead", false)]
    void ModifyMesh(Mesh mesh);

    void ModifyMesh(VertexHelper verts);
  }
}
