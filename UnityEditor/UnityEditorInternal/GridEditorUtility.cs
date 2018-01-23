// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.GridEditorUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal static class GridEditorUtility
  {
    private const int k_GridGizmoVertexCount = 32000;
    private const float k_GridGizmoDistanceFalloff = 50f;

    public static Vector3Int ClampToGrid(Vector3Int p, Vector2Int origin, Vector2Int gridSize)
    {
      return new Vector3Int(Math.Max(Math.Min(p.x, origin.x + gridSize.x - 1), origin.x), Math.Max(Math.Min(p.y, origin.y + gridSize.y - 1), origin.y), p.z);
    }

    public static Vector3 ScreenToLocal(Transform transform, Vector2 screenPosition)
    {
      return GridEditorUtility.ScreenToLocal(transform, screenPosition, new Plane(transform.forward * -1f, transform.position));
    }

    public static Vector3 ScreenToLocal(Transform transform, Vector2 screenPosition, Plane plane)
    {
      Ray ray;
      if (Camera.current.orthographic)
      {
        Vector2 pixels = EditorGUIUtility.PointsToPixels(GUIClip.Unclip(screenPosition));
        pixels.y = (float) Screen.height - pixels.y;
        ray = new Ray(Camera.current.ScreenToWorldPoint((Vector3) pixels), Camera.current.transform.forward);
      }
      else
        ray = HandleUtility.GUIPointToWorldRay(screenPosition);
      float enter;
      plane.Raycast(ray, out enter);
      Vector3 point = ray.GetPoint(enter);
      return transform.InverseTransformPoint(point);
    }

    public static RectInt GetMarqueeRect(Vector2Int p1, Vector2Int p2)
    {
      return new RectInt(Math.Min(p1.x, p2.x), Math.Min(p1.y, p2.y), Math.Abs(p2.x - p1.x) + 1, Math.Abs(p2.y - p1.y) + 1);
    }

    public static BoundsInt GetMarqueeBounds(Vector3Int p1, Vector3Int p2)
    {
      return new BoundsInt(Math.Min(p1.x, p2.x), Math.Min(p1.y, p2.y), Math.Min(p1.z, p2.z), Math.Abs(p2.x - p1.x) + 1, Math.Abs(p2.y - p1.y) + 1, Math.Abs(p2.z - p1.z) + 1);
    }

    [DebuggerHidden]
    public static IEnumerable<Vector2Int> GetPointsOnLine(Vector2Int p1, Vector2Int p2)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GridEditorUtility.\u003CGetPointsOnLine\u003Ec__Iterator0 onLineCIterator0 = new GridEditorUtility.\u003CGetPointsOnLine\u003Ec__Iterator0() { p1 = p1, p2 = p2 };
      // ISSUE: reference to a compiler-generated field
      onLineCIterator0.\u0024PC = -2;
      return (IEnumerable<Vector2Int>) onLineCIterator0;
    }

    public static void DrawBatchedHorizontalLine(float x1, float x2, float y)
    {
      GL.Vertex3(x1, y, 0.0f);
      GL.Vertex3(x2, y, 0.0f);
      GL.Vertex3(x2, y + 1f, 0.0f);
      GL.Vertex3(x1, y + 1f, 0.0f);
    }

    public static void DrawBatchedVerticalLine(float y1, float y2, float x)
    {
      GL.Vertex3(x, y1, 0.0f);
      GL.Vertex3(x, y2, 0.0f);
      GL.Vertex3(x + 1f, y2, 0.0f);
      GL.Vertex3(x + 1f, y1, 0.0f);
    }

    public static void DrawBatchedLine(Vector3 p1, Vector3 p2)
    {
      GL.Vertex3(p1.x, p1.y, p1.z);
      GL.Vertex3(p2.x, p2.y, p2.z);
    }

    public static void DrawLine(Vector2 p1, Vector2 p2, Color color)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(GUI.matrix);
      GL.Begin(1);
      GL.Color(color);
      GridEditorUtility.DrawBatchedLine((Vector3) p1, (Vector3) p2);
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawBox(Rect r, Color color)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(GUI.matrix);
      GL.Begin(1);
      GL.Color(color);
      GridEditorUtility.DrawBatchedLine(new Vector3(r.xMin, r.yMin, 0.0f), new Vector3(r.xMax, r.yMin, 0.0f));
      GridEditorUtility.DrawBatchedLine(new Vector3(r.xMax, r.yMin, 0.0f), new Vector3(r.xMax, r.yMax, 0.0f));
      GridEditorUtility.DrawBatchedLine(new Vector3(r.xMax, r.yMax, 0.0f), new Vector3(r.xMin, r.yMax, 0.0f));
      GridEditorUtility.DrawBatchedLine(new Vector3(r.xMin, r.yMax, 0.0f), new Vector3(r.xMin, r.yMin, 0.0f));
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawFilledBox(Rect r, Color color)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(GUI.matrix);
      GL.Begin(7);
      GL.Color(color);
      GL.Vertex3(r.xMin, r.yMin, 0.0f);
      GL.Vertex3(r.xMax, r.yMin, 0.0f);
      GL.Vertex3(r.xMax, r.yMax, 0.0f);
      GL.Vertex3(r.xMin, r.yMax, 0.0f);
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawGridMarquee(GridLayout gridLayout, BoundsInt area, Color color)
    {
      Vector3 vector3 = gridLayout.cellSize + gridLayout.cellGap;
      Vector3 one = Vector3.one;
      if (!Mathf.Approximately(vector3.x, 0.0f))
        one.x = gridLayout.cellSize.x / vector3.x;
      if (!Mathf.Approximately(vector3.y, 0.0f))
        one.y = gridLayout.cellSize.y / vector3.y;
      Vector3[] vector3Array = new Vector3[4]{ gridLayout.CellToLocal(new Vector3Int(area.xMin, area.yMin, 0)), gridLayout.CellToLocalInterpolated(new Vector3((float) (area.xMax - 1) + one.x, (float) area.yMin, 0.0f)), gridLayout.CellToLocalInterpolated(new Vector3((float) (area.xMax - 1) + one.x, (float) (area.yMax - 1) + one.y, 0.0f)), gridLayout.CellToLocalInterpolated(new Vector3((float) area.xMin, (float) (area.yMax - 1) + one.y, 0.0f)) };
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(gridLayout.transform.localToWorldMatrix);
      GL.Begin(1);
      GL.Color(color);
      int index1 = 0;
      int index2 = vector3Array.Length - 1;
      for (; index1 < vector3Array.Length; index2 = index1++)
        GridEditorUtility.DrawBatchedLine(vector3Array[index2], vector3Array[index1]);
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawGridGizmo(GridLayout gridLayout, Transform transform, Color color, ref Mesh gridMesh, ref Material gridMaterial)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if ((UnityEngine.Object) gridMesh == (UnityEngine.Object) null)
        gridMesh = GridEditorUtility.GenerateCachedGridMesh(gridLayout, color);
      if ((UnityEngine.Object) gridMaterial == (UnityEngine.Object) null)
        gridMaterial = (Material) EditorGUIUtility.LoadRequired("SceneView/GridGap.mat");
      gridMaterial.SetVector("_Gap", (Vector4) gridLayout.cellSize);
      gridMaterial.SetVector("_Stride", (Vector4) (gridLayout.cellGap + gridLayout.cellSize));
      gridMaterial.SetPass(0);
      GL.PushMatrix();
      if (gridMesh.GetTopology(0) == MeshTopology.Lines)
        GL.Begin(1);
      else
        GL.Begin(7);
      Graphics.DrawMeshNow(gridMesh, transform.localToWorldMatrix);
      GL.End();
      GL.PopMatrix();
    }

    public static Vector3 GetSpriteWorldSize(Sprite sprite)
    {
      if ((UnityEngine.Object) sprite != (UnityEngine.Object) null && (double) sprite.rect.size.magnitude > 0.0)
        return new Vector3(sprite.rect.size.x / sprite.pixelsPerUnit, sprite.rect.size.y / sprite.pixelsPerUnit, 1f);
      return Vector3.one;
    }

    private static Mesh GenerateCachedGridMesh(GridLayout gridLayout, Color color)
    {
      int num1 = -1000;
      int num2 = num1 * -1 - num1;
      RectInt bounds = new RectInt(num1, num1, num2, num2);
      return GridEditorUtility.GenerateCachedGridMesh(gridLayout, color, 0.0f, bounds, MeshTopology.Lines);
    }

    public static Mesh GenerateCachedGridMesh(GridLayout gridLayout, Color color, float screenPixelSize, RectInt bounds, MeshTopology topology)
    {
      Mesh mesh = new Mesh();
      mesh.hideFlags = HideFlags.HideAndDontSave;
      int length1 = 0;
      int length2 = topology != MeshTopology.Quads ? 4 * (bounds.size.x + bounds.size.y) : 8 * (bounds.size.x + bounds.size.y);
      Vector3 vector3_1 = new Vector3(screenPixelSize, 0.0f, 0.0f);
      Vector3 vector3_2 = new Vector3(0.0f, screenPixelSize, 0.0f);
      Vector3[] vector3Array = new Vector3[length2];
      Vector2[] vector2Array1 = new Vector2[length2];
      Vector3 vector3_3 = gridLayout.cellSize + gridLayout.cellGap;
      Vector3Int cellPosition1 = new Vector3Int(0, bounds.min.y, 0);
      Vector3Int cellPosition2 = new Vector3Int(0, bounds.max.y, 0);
      Vector3 zero1 = Vector3.zero;
      if (!Mathf.Approximately(vector3_3.x, 0.0f))
        zero1.x = gridLayout.cellSize.x / vector3_3.x;
      for (int x = bounds.min.x; x < bounds.max.x; ++x)
      {
        cellPosition1.x = x;
        cellPosition2.x = x;
        vector3Array[length1] = gridLayout.CellToLocal(cellPosition1);
        vector3Array[length1 + 1] = gridLayout.CellToLocal(cellPosition2);
        if (topology == MeshTopology.Quads)
        {
          vector3Array[length1 + 2] = gridLayout.CellToLocal(cellPosition2) + vector3_1;
          vector3Array[length1 + 3] = gridLayout.CellToLocal(cellPosition1) + vector3_1;
          vector2Array1[length1] = Vector2.zero;
          vector2Array1[length1 + 1] = new Vector2(0.0f, vector3_3.y * (float) bounds.size.y);
          vector2Array1[length1 + 2] = new Vector2(0.0f, vector3_3.y * (float) bounds.size.y);
          vector2Array1[length1 + 3] = Vector2.zero;
        }
        int index = length1 + (topology != MeshTopology.Quads ? 2 : 4);
        vector3Array[index] = gridLayout.CellToLocalInterpolated((Vector3) cellPosition1 + zero1);
        vector3Array[index + 1] = gridLayout.CellToLocalInterpolated((Vector3) cellPosition2 + zero1);
        if (topology == MeshTopology.Quads)
        {
          vector3Array[index + 2] = gridLayout.CellToLocalInterpolated((Vector3) cellPosition2 + zero1) + vector3_1;
          vector3Array[index + 3] = gridLayout.CellToLocalInterpolated((Vector3) cellPosition1 + zero1) + vector3_1;
          vector2Array1[index] = Vector2.zero;
          vector2Array1[index + 1] = new Vector2(0.0f, vector3_3.y * (float) bounds.size.y);
          vector2Array1[index + 2] = new Vector2(0.0f, vector3_3.y * (float) bounds.size.y);
          vector2Array1[index + 3] = Vector2.zero;
        }
        length1 = index + (topology != MeshTopology.Quads ? 2 : 4);
      }
      cellPosition1 = new Vector3Int(bounds.min.x, 0, 0);
      cellPosition2 = new Vector3Int(bounds.max.x, 0, 0);
      Vector3 zero2 = Vector3.zero;
      if (!Mathf.Approximately(vector3_3.y, 0.0f))
        zero2.y = gridLayout.cellSize.y / vector3_3.y;
      for (int y = bounds.min.y; y < bounds.max.y; ++y)
      {
        cellPosition1.y = y;
        cellPosition2.y = y;
        vector3Array[length1] = gridLayout.CellToLocal(cellPosition1);
        vector3Array[length1 + 1] = gridLayout.CellToLocal(cellPosition2);
        if (topology == MeshTopology.Quads)
        {
          vector3Array[length1 + 2] = gridLayout.CellToLocal(cellPosition2) + vector3_2;
          vector3Array[length1 + 3] = gridLayout.CellToLocal(cellPosition1) + vector3_2;
          vector2Array1[length1] = Vector2.zero;
          vector2Array1[length1 + 1] = new Vector2(vector3_3.x * (float) bounds.size.x, 0.0f);
          vector2Array1[length1 + 2] = new Vector2(vector3_3.x * (float) bounds.size.x, 0.0f);
          vector2Array1[length1 + 3] = Vector2.zero;
        }
        int index = length1 + (topology != MeshTopology.Quads ? 2 : 4);
        vector3Array[index] = gridLayout.CellToLocalInterpolated((Vector3) cellPosition1 + zero2);
        vector3Array[index + 1] = gridLayout.CellToLocalInterpolated((Vector3) cellPosition2 + zero2);
        if (topology == MeshTopology.Quads)
        {
          vector3Array[index + 2] = gridLayout.CellToLocalInterpolated((Vector3) cellPosition2 + zero2) + vector3_2;
          vector3Array[index + 3] = gridLayout.CellToLocalInterpolated((Vector3) cellPosition1 + zero2) + vector3_2;
          vector2Array1[index] = Vector2.zero;
          vector2Array1[index + 1] = new Vector2(vector3_3.x * (float) bounds.size.x, 0.0f);
          vector2Array1[index + 2] = new Vector2(vector3_3.x * (float) bounds.size.x, 0.0f);
          vector2Array1[index + 3] = Vector2.zero;
        }
        length1 = index + (topology != MeshTopology.Quads ? 2 : 4);
      }
      Vector2 vector2 = new Vector2(50f, 0.0f);
      Vector2[] vector2Array2 = new Vector2[length1];
      int[] indices = new int[length1];
      Color[] colorArray = new Color[length1];
      for (int index = 0; index < length1; ++index)
      {
        vector2Array2[index] = vector2;
        indices[index] = index;
        colorArray[index] = color;
      }
      mesh.vertices = vector3Array;
      mesh.uv = vector2Array2;
      if (topology == MeshTopology.Quads)
        mesh.uv2 = vector2Array1;
      mesh.colors = colorArray;
      mesh.SetIndices(indices, topology, 0);
      return mesh;
    }
  }
}
