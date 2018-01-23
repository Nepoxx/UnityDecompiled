// Decompiled with JetBrains decompiler
// Type: UnityEngine.U2D.Interface.IGL
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEngine.U2D.Interface
{
  internal interface IGL
  {
    void PushMatrix();

    void PopMatrix();

    void MultMatrix(Matrix4x4 m);

    void Begin(int mode);

    void End();

    void Color(Color c);

    void Vertex(Vector3 v);
  }
}
