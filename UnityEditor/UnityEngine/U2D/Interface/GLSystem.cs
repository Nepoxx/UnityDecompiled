// Decompiled with JetBrains decompiler
// Type: UnityEngine.U2D.Interface.GLSystem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEngine.U2D.Interface
{
  internal class GLSystem : IGL
  {
    private static IGL m_GLSystem;

    internal static void SetSystem(IGL system)
    {
      GLSystem.m_GLSystem = system;
    }

    internal static IGL GetSystem()
    {
      if (GLSystem.m_GLSystem == null)
        GLSystem.m_GLSystem = (IGL) new GLSystem();
      return GLSystem.m_GLSystem;
    }

    public void PushMatrix()
    {
      GL.PushMatrix();
    }

    public void PopMatrix()
    {
      GL.PopMatrix();
    }

    public void MultMatrix(Matrix4x4 m)
    {
      GL.MultMatrix(m);
    }

    public void Begin(int mode)
    {
      GL.Begin(mode);
    }

    public void End()
    {
      GL.End();
    }

    public void Color(Color c)
    {
      GL.Color(c);
    }

    public void Vertex(Vector3 v)
    {
      GL.Vertex(v);
    }
  }
}
