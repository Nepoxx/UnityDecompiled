// Decompiled with JetBrains decompiler
// Type: UnityEditor.ControlPointRenderer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class ControlPointRenderer
  {
    private List<ControlPointRenderer.RenderChunk> m_RenderChunks = new List<ControlPointRenderer.RenderChunk>();
    private Texture2D m_Icon;
    private const int kMaxVertices = 65000;
    private const string kControlPointRendererMeshName = "ControlPointRendererMesh";
    private static Material s_Material;

    public ControlPointRenderer(Texture2D icon)
    {
      this.m_Icon = icon;
    }

    public static Material material
    {
      get
      {
        if (!(bool) ((Object) ControlPointRenderer.s_Material))
          ControlPointRenderer.s_Material = new Material((Shader) EditorGUIUtility.LoadRequired("Editors/AnimationWindow/ControlPoint.shader"));
        return ControlPointRenderer.s_Material;
      }
    }

    public void FlushCache()
    {
      for (int index = 0; index < this.m_RenderChunks.Count; ++index)
        Object.DestroyImmediate((Object) this.m_RenderChunks[index].mesh);
      this.m_RenderChunks.Clear();
    }

    public void Clear()
    {
      for (int index = 0; index < this.m_RenderChunks.Count; ++index)
      {
        ControlPointRenderer.RenderChunk renderChunk = this.m_RenderChunks[index];
        renderChunk.mesh.Clear();
        renderChunk.vertices.Clear();
        renderChunk.colors.Clear();
        renderChunk.uvs.Clear();
        renderChunk.indices.Clear();
        renderChunk.isDirty = true;
      }
    }

    public void Render()
    {
      Material material = ControlPointRenderer.material;
      material.SetTexture("_MainTex", (Texture) this.m_Icon);
      material.SetPass(0);
      for (int index = 0; index < this.m_RenderChunks.Count; ++index)
      {
        ControlPointRenderer.RenderChunk renderChunk = this.m_RenderChunks[index];
        if (renderChunk.isDirty)
        {
          renderChunk.mesh.vertices = renderChunk.vertices.ToArray();
          renderChunk.mesh.colors32 = renderChunk.colors.ToArray();
          renderChunk.mesh.uv = renderChunk.uvs.ToArray();
          renderChunk.mesh.SetIndices(renderChunk.indices.ToArray(), MeshTopology.Triangles, 0);
          renderChunk.isDirty = false;
        }
        Graphics.DrawMeshNow(renderChunk.mesh, Handles.matrix);
      }
    }

    public void AddPoint(Rect rect, Color color)
    {
      ControlPointRenderer.RenderChunk renderChunk = this.GetRenderChunk();
      int count = renderChunk.vertices.Count;
      renderChunk.vertices.Add(new Vector3(rect.xMin, rect.yMin, 0.0f));
      renderChunk.vertices.Add(new Vector3(rect.xMax, rect.yMin, 0.0f));
      renderChunk.vertices.Add(new Vector3(rect.xMax, rect.yMax, 0.0f));
      renderChunk.vertices.Add(new Vector3(rect.xMin, rect.yMax, 0.0f));
      renderChunk.colors.Add((Color32) color);
      renderChunk.colors.Add((Color32) color);
      renderChunk.colors.Add((Color32) color);
      renderChunk.colors.Add((Color32) color);
      renderChunk.uvs.Add(new Vector2(0.0f, 0.0f));
      renderChunk.uvs.Add(new Vector2(1f, 0.0f));
      renderChunk.uvs.Add(new Vector2(1f, 1f));
      renderChunk.uvs.Add(new Vector2(0.0f, 1f));
      renderChunk.indices.Add(count);
      renderChunk.indices.Add(count + 1);
      renderChunk.indices.Add(count + 2);
      renderChunk.indices.Add(count);
      renderChunk.indices.Add(count + 2);
      renderChunk.indices.Add(count + 3);
      renderChunk.isDirty = true;
    }

    private ControlPointRenderer.RenderChunk GetRenderChunk()
    {
      ControlPointRenderer.RenderChunk renderChunk;
      if (this.m_RenderChunks.Count > 0)
      {
        renderChunk = this.m_RenderChunks.Last<ControlPointRenderer.RenderChunk>();
        if (renderChunk.vertices.Count + 4 > 65000)
          renderChunk = this.CreateRenderChunk();
      }
      else
        renderChunk = this.CreateRenderChunk();
      return renderChunk;
    }

    private ControlPointRenderer.RenderChunk CreateRenderChunk()
    {
      ControlPointRenderer.RenderChunk renderChunk = new ControlPointRenderer.RenderChunk();
      renderChunk.mesh = new Mesh();
      renderChunk.mesh.name = "ControlPointRendererMesh";
      renderChunk.mesh.hideFlags |= HideFlags.DontSave;
      renderChunk.vertices = new List<Vector3>();
      renderChunk.colors = new List<Color32>();
      renderChunk.uvs = new List<Vector2>();
      renderChunk.indices = new List<int>();
      this.m_RenderChunks.Add(renderChunk);
      return renderChunk;
    }

    private class RenderChunk
    {
      public bool isDirty = true;
      public Mesh mesh;
      public List<Vector3> vertices;
      public List<Color32> colors;
      public List<Vector2> uvs;
      public List<int> indices;
    }
  }
}
