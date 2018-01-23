// Decompiled with JetBrains decompiler
// Type: UnityEditor.NormalCurveRenderer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class NormalCurveRenderer : CurveRenderer
  {
    private float m_CustomRangeStart = 0.0f;
    private float m_CustomRangeEnd = 0.0f;
    private WrapMode preWrapMode = WrapMode.Once;
    private WrapMode postWrapMode = WrapMode.Once;
    private const float kSegmentWindowResolution = 1000f;
    private const int kMaximumSampleCount = 50;
    private const int kMaximumLoops = 100;
    private const string kCurveRendererMeshName = "NormalCurveRendererMesh";
    private AnimationCurve m_Curve;
    private Bounds? m_Bounds;
    private Mesh m_CurveMesh;
    private static Material s_CurveMaterial;

    public NormalCurveRenderer(AnimationCurve curve)
    {
      this.m_Curve = curve;
      if (this.m_Curve != null)
        return;
      this.m_Curve = new AnimationCurve();
    }

    private float rangeStart
    {
      get
      {
        return (double) this.m_CustomRangeStart != 0.0 || (double) this.m_CustomRangeEnd != 0.0 || this.m_Curve.length <= 0 ? this.m_CustomRangeStart : this.m_Curve.keys[0].time;
      }
    }

    private float rangeEnd
    {
      get
      {
        return (double) this.m_CustomRangeStart != 0.0 || (double) this.m_CustomRangeEnd != 0.0 || this.m_Curve.length <= 0 ? this.m_CustomRangeEnd : this.m_Curve.keys[this.m_Curve.length - 1].time;
      }
    }

    public static Material curveMaterial
    {
      get
      {
        if (!(bool) ((Object) NormalCurveRenderer.s_CurveMaterial))
          NormalCurveRenderer.s_CurveMaterial = new Material((Shader) EditorGUIUtility.LoadRequired("Editors/AnimationWindow/Curve.shader"));
        return NormalCurveRenderer.s_CurveMaterial;
      }
    }

    public AnimationCurve GetCurve()
    {
      return this.m_Curve;
    }

    public float RangeStart()
    {
      return this.rangeStart;
    }

    public float RangeEnd()
    {
      return this.rangeEnd;
    }

    public void SetWrap(WrapMode wrap)
    {
      this.preWrapMode = wrap;
      this.postWrapMode = wrap;
    }

    public void SetWrap(WrapMode preWrap, WrapMode postWrap)
    {
      this.preWrapMode = preWrap;
      this.postWrapMode = postWrap;
    }

    public void SetCustomRange(float start, float end)
    {
      this.m_CustomRangeStart = start;
      this.m_CustomRangeEnd = end;
    }

    public virtual float ClampedValue(float value)
    {
      return value;
    }

    public virtual float EvaluateCurveSlow(float time)
    {
      return this.m_Curve.Evaluate(time);
    }

    public float EvaluateCurveDeltaSlow(float time)
    {
      float num = 0.0001f;
      return (float) (((double) this.EvaluateCurveSlow(time + num) - (double) this.EvaluateCurveSlow(time - num)) / ((double) num * 2.0));
    }

    private Vector3[] GetPoints()
    {
      return this.GetPoints(this.rangeStart, this.rangeEnd);
    }

    private Vector3[] GetPoints(float minTime, float maxTime)
    {
      List<Vector3> points = new List<Vector3>();
      if (this.m_Curve.length == 0)
        return points.ToArray();
      points.Capacity = 1000 + this.m_Curve.length;
      float[,] ranges = NormalCurveRenderer.CalculateRanges(minTime, maxTime, this.rangeStart, this.rangeEnd, this.preWrapMode, this.postWrapMode);
      for (int index = 0; index < ranges.GetLength(0); ++index)
        this.AddPoints(ref points, ranges[index, 0], ranges[index, 1], minTime, maxTime);
      if (points.Count > 0)
      {
        for (int index = 1; index < points.Count; ++index)
        {
          if ((double) points[index].x < (double) points[index - 1].x)
          {
            points.RemoveAt(index);
            --index;
          }
        }
      }
      return points.ToArray();
    }

    public static float[,] CalculateRanges(float minTime, float maxTime, float rangeStart, float rangeEnd, WrapMode preWrapMode, WrapMode postWrapMode)
    {
      WrapMode wrapMode = preWrapMode;
      if (postWrapMode != wrapMode)
        return new float[1, 2]{ { rangeStart, rangeEnd } };
      switch (wrapMode)
      {
        case WrapMode.Loop:
          if ((double) maxTime - (double) minTime > (double) rangeEnd - (double) rangeStart)
            return new float[1, 2]{ { rangeStart, rangeEnd } };
          minTime = Mathf.Repeat(minTime - rangeStart, rangeEnd - rangeStart) + rangeStart;
          maxTime = Mathf.Repeat(maxTime - rangeStart, rangeEnd - rangeStart) + rangeStart;
          if ((double) minTime < (double) maxTime)
            return new float[1, 2]{ { minTime, maxTime } };
          return new float[2, 2]{ { rangeStart, maxTime }, { minTime, rangeEnd } };
        case WrapMode.PingPong:
          return new float[1, 2]{ { rangeStart, rangeEnd } };
        default:
          return new float[1, 2]{ { minTime, maxTime } };
      }
    }

    protected virtual int GetSegmentResolution(float minTime, float maxTime, float keyTime, float nextKeyTime)
    {
      float num = maxTime - minTime;
      return Mathf.Clamp(Mathf.RoundToInt((float) (1000.0 * ((double) (nextKeyTime - keyTime) / (double) num))), 1, 50);
    }

    protected virtual void AddPoint(ref List<Vector3> points, ref float lastTime, float sampleTime, ref float lastValue, float sampleValue)
    {
      points.Add(new Vector3(sampleTime, sampleValue));
      lastTime = sampleTime;
      lastValue = sampleValue;
    }

    private void AddPoints(ref List<Vector3> points, float minTime, float maxTime, float visibleMinTime, float visibleMaxTime)
    {
      if ((double) this.m_Curve[0].time >= (double) minTime)
      {
        points.Add(new Vector3(this.rangeStart, this.ClampedValue(this.m_Curve[0].value)));
        points.Add(new Vector3(this.m_Curve[0].time, this.ClampedValue(this.m_Curve[0].value)));
      }
      for (int index = 0; index < this.m_Curve.length - 1; ++index)
      {
        Keyframe keyframe1 = this.m_Curve[index];
        Keyframe keyframe2 = this.m_Curve[index + 1];
        if ((double) keyframe2.time >= (double) minTime && (double) keyframe1.time <= (double) maxTime)
        {
          points.Add(new Vector3(keyframe1.time, keyframe1.value));
          int segmentResolution = this.GetSegmentResolution(visibleMinTime, visibleMaxTime, keyframe1.time, keyframe2.time);
          float num1 = Mathf.Lerp(keyframe1.time, keyframe2.time, 1f / 1000f / (float) segmentResolution);
          float time1 = keyframe1.time;
          float lastValue = this.ClampedValue(keyframe1.value);
          float curveSlow1 = this.EvaluateCurveSlow(num1);
          this.AddPoint(ref points, ref time1, num1, ref lastValue, curveSlow1);
          for (float num2 = 1f; (double) num2 < (double) segmentResolution; ++num2)
          {
            float num3 = Mathf.Lerp(keyframe1.time, keyframe2.time, num2 / (float) segmentResolution);
            float curveSlow2 = this.EvaluateCurveSlow(num3);
            this.AddPoint(ref points, ref time1, num3, ref lastValue, curveSlow2);
          }
          float num4 = Mathf.Lerp(keyframe1.time, keyframe2.time, (float) (1.0 - 1.0 / 1000.0 / (double) segmentResolution));
          float curveSlow3 = this.EvaluateCurveSlow(num4);
          this.AddPoint(ref points, ref time1, num4, ref lastValue, curveSlow3);
          float time2 = keyframe2.time;
          this.AddPoint(ref points, ref time1, time2, ref lastValue, curveSlow3);
        }
      }
      if ((double) this.m_Curve[this.m_Curve.length - 1].time > (double) maxTime)
        return;
      float y = this.ClampedValue(this.m_Curve[this.m_Curve.length - 1].value);
      points.Add(new Vector3(this.m_Curve[this.m_Curve.length - 1].time, y));
      points.Add(new Vector3(this.rangeEnd, y));
    }

    private void BuildCurveMesh()
    {
      if ((Object) this.m_CurveMesh != (Object) null)
        return;
      Vector3[] points = this.GetPoints();
      this.m_CurveMesh = new Mesh();
      this.m_CurveMesh.name = "NormalCurveRendererMesh";
      this.m_CurveMesh.hideFlags |= HideFlags.DontSave;
      this.m_CurveMesh.vertices = points;
      if (points.Length <= 0)
        return;
      int num1 = points.Length - 1;
      int num2 = 0;
      List<int> intList = new List<int>(num1 * 2);
      for (; num2 < num1; intList.Add(++num2))
        intList.Add(num2);
      this.m_CurveMesh.SetIndices(intList.ToArray(), MeshTopology.Lines, 0);
    }

    public void DrawCurve(float minTime, float maxTime, Color color, Matrix4x4 transform, Color wrapColor)
    {
      this.BuildCurveMesh();
      Keyframe[] keys = this.m_Curve.keys;
      if (keys.Length <= 0)
        return;
      Vector3 firstPoint = new Vector3(this.rangeStart, ((IEnumerable<Keyframe>) keys).First<Keyframe>().value);
      Vector3 lastPoint = new Vector3(this.rangeEnd, ((IEnumerable<Keyframe>) keys).Last<Keyframe>().value);
      NormalCurveRenderer.DrawCurveWrapped(minTime, maxTime, this.rangeStart, this.rangeEnd, this.preWrapMode, this.postWrapMode, this.m_CurveMesh, firstPoint, lastPoint, transform, color, wrapColor);
    }

    public static void DrawPolyLine(Matrix4x4 transform, float minDistance, params Vector3[] points)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color c = Handles.color * new Color(1f, 1f, 1f, 0.75f);
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      GL.Begin(1);
      GL.Color(c);
      Vector3 v1 = transform.MultiplyPoint(points[0]);
      for (int index = 1; index < points.Length; ++index)
      {
        Vector3 v2 = transform.MultiplyPoint(points[index]);
        if ((double) (v1 - v2).magnitude > (double) minDistance)
        {
          GL.Vertex(v1);
          GL.Vertex(v2);
          v1 = v2;
        }
      }
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawCurveWrapped(float minTime, float maxTime, float rangeStart, float rangeEnd, WrapMode preWrap, WrapMode postWrap, Mesh mesh, Vector3 firstPoint, Vector3 lastPoint, Matrix4x4 transform, Color color, Color wrapColor)
    {
      if (mesh.vertexCount == 0 || Event.current.type != EventType.Repaint)
        return;
      int num1;
      int num2;
      if ((double) rangeEnd - (double) rangeStart != 0.0)
      {
        num1 = Mathf.FloorToInt((float) (((double) minTime - (double) rangeStart) / ((double) rangeEnd - (double) rangeStart)));
        num2 = Mathf.CeilToInt((float) (((double) maxTime - (double) rangeEnd) / ((double) rangeEnd - (double) rangeStart)));
        if (num1 < -100)
          preWrap = WrapMode.Once;
        if (num2 > 100)
          postWrap = WrapMode.Once;
      }
      else
      {
        preWrap = WrapMode.Once;
        postWrap = WrapMode.Once;
        num1 = (double) minTime >= (double) rangeStart ? 0 : -1;
        num2 = (double) maxTime <= (double) rangeEnd ? 0 : 1;
      }
      Material curveMaterial = NormalCurveRenderer.curveMaterial;
      curveMaterial.SetColor("_Color", color);
      Handles.color = color;
      curveMaterial.SetPass(0);
      Graphics.DrawMeshNow(mesh, Handles.matrix * transform);
      curveMaterial.SetColor("_Color", new Color(wrapColor.r, wrapColor.g, wrapColor.b, wrapColor.a * color.a));
      Handles.color = new Color(wrapColor.r, wrapColor.g, wrapColor.b, wrapColor.a * color.a);
      switch (preWrap)
      {
        case WrapMode.Loop:
          Matrix4x4 matrix4x4_1 = Handles.matrix * transform * Matrix4x4.TRS(new Vector3((float) num1 * (rangeEnd - rangeStart), 0.0f, 0.0f), Quaternion.identity, Vector3.one);
          Matrix4x4 matrix4x4_2 = Matrix4x4.TRS(new Vector3(rangeEnd - rangeStart, 0.0f, 0.0f), Quaternion.identity, Vector3.one);
          curveMaterial.SetPass(0);
          Matrix4x4 matrix1 = matrix4x4_1;
          for (int index = num1; index < 0; ++index)
          {
            Graphics.DrawMeshNow(mesh, matrix1);
            matrix1 *= matrix4x4_2;
          }
          Matrix4x4 matrix4x4_3 = matrix4x4_1;
          for (int index = num1; index < 0; ++index)
          {
            Matrix4x4 matrix4x4_4 = matrix4x4_3 * matrix4x4_2;
            Handles.DrawLine(matrix4x4_3.MultiplyPoint(lastPoint), matrix4x4_4.MultiplyPoint(firstPoint));
            matrix4x4_3 = matrix4x4_4;
          }
          break;
        case WrapMode.PingPong:
          curveMaterial.SetPass(0);
          for (int index = num1; index < 0; ++index)
          {
            if (index % 2 == 0)
            {
              Matrix4x4 matrix4x4_4 = Matrix4x4.TRS(new Vector3((float) index * (rangeEnd - rangeStart), 0.0f, 0.0f), Quaternion.identity, Vector3.one);
              Graphics.DrawMeshNow(mesh, Handles.matrix * transform * matrix4x4_4);
            }
            else
            {
              Matrix4x4 matrix4x4_4 = Matrix4x4.TRS(new Vector3((float) ((double) (index + 1) * ((double) rangeEnd - (double) rangeStart) + (double) rangeStart * 2.0), 0.0f, 0.0f), Quaternion.identity, new Vector3(-1f, 1f, 1f));
              Graphics.DrawMeshNow(mesh, Handles.matrix * transform * matrix4x4_4);
            }
          }
          break;
        default:
          if (num1 < 0)
            Handles.DrawLine(transform.MultiplyPoint(new Vector3(minTime, firstPoint.y, 0.0f)), transform.MultiplyPoint(new Vector3(Mathf.Min(maxTime, firstPoint.x), firstPoint.y, 0.0f)));
          break;
      }
      switch (postWrap)
      {
        case WrapMode.Loop:
          Matrix4x4 matrix4x4_5 = Handles.matrix * transform;
          Matrix4x4 matrix4x4_6 = Matrix4x4.TRS(new Vector3(rangeEnd - rangeStart, 0.0f, 0.0f), Quaternion.identity, Vector3.one);
          Matrix4x4 matrix4x4_7 = matrix4x4_5;
          for (int index = 1; index <= num2; ++index)
          {
            Matrix4x4 matrix4x4_4 = matrix4x4_7 * matrix4x4_6;
            Handles.DrawLine(matrix4x4_7.MultiplyPoint(lastPoint), matrix4x4_4.MultiplyPoint(firstPoint));
            matrix4x4_7 = matrix4x4_4;
          }
          curveMaterial.SetPass(0);
          Matrix4x4 matrix4x4_8 = matrix4x4_5;
          for (int index = 1; index <= num2; ++index)
          {
            Matrix4x4 matrix2 = matrix4x4_8 * matrix4x4_6;
            Graphics.DrawMeshNow(mesh, matrix2);
            matrix4x4_8 = matrix2;
          }
          break;
        case WrapMode.PingPong:
          curveMaterial.SetPass(0);
          for (int index = 1; index <= num2; ++index)
          {
            if (index % 2 == 0)
            {
              Matrix4x4 matrix4x4_4 = Matrix4x4.TRS(new Vector3((float) index * (rangeEnd - rangeStart), 0.0f, 0.0f), Quaternion.identity, Vector3.one);
              Graphics.DrawMeshNow(mesh, Handles.matrix * transform * matrix4x4_4);
            }
            else
            {
              Matrix4x4 matrix4x4_4 = Matrix4x4.TRS(new Vector3((float) ((double) (index + 1) * ((double) rangeEnd - (double) rangeStart) + (double) rangeStart * 2.0), 0.0f, 0.0f), Quaternion.identity, new Vector3(-1f, 1f, 1f));
              Graphics.DrawMeshNow(mesh, Handles.matrix * transform * matrix4x4_4);
            }
          }
          break;
        default:
          if (num2 > 0)
            Handles.DrawLine(transform.MultiplyPoint(new Vector3(Mathf.Max(minTime, lastPoint.x), lastPoint.y, 0.0f)), transform.MultiplyPoint(new Vector3(maxTime, lastPoint.y, 0.0f)));
          break;
      }
    }

    public static void DrawCurveWrapped(float minTime, float maxTime, float rangeStart, float rangeEnd, WrapMode preWrap, WrapMode postWrap, Color color, Matrix4x4 transform, Vector3[] points, Color wrapColor)
    {
      if (points.Length == 0)
        return;
      int num1;
      int num2;
      if ((double) rangeEnd - (double) rangeStart != 0.0)
      {
        num1 = Mathf.FloorToInt((float) (((double) minTime - (double) rangeStart) / ((double) rangeEnd - (double) rangeStart)));
        num2 = Mathf.CeilToInt((float) (((double) maxTime - (double) rangeEnd) / ((double) rangeEnd - (double) rangeStart)));
        if (num1 < -100)
          preWrap = WrapMode.Once;
        if (num2 > 100)
          postWrap = WrapMode.Once;
      }
      else
      {
        preWrap = WrapMode.Once;
        postWrap = WrapMode.Once;
        num1 = (double) minTime >= (double) rangeStart ? 0 : -1;
        num2 = (double) maxTime <= (double) rangeEnd ? 0 : 1;
      }
      int index1 = points.Length - 1;
      Handles.color = color;
      List<Vector3> vector3List1 = new List<Vector3>();
      if (num1 <= 0 && num2 >= 0)
        NormalCurveRenderer.DrawPolyLine(transform, 2f, points);
      else
        Handles.DrawPolyLine(points);
      Handles.color = new Color(wrapColor.r, wrapColor.g, wrapColor.b, wrapColor.a * color.a);
      switch (preWrap)
      {
        case WrapMode.Loop:
          List<Vector3> vector3List2 = new List<Vector3>();
          for (int index2 = num1; index2 < 0; ++index2)
          {
            for (int index3 = 0; index3 < points.Length; ++index3)
            {
              Vector3 point = points[index3];
              point.x += (float) index2 * (rangeEnd - rangeStart);
              Vector3 vector3 = transform.MultiplyPoint(point);
              vector3List2.Add(vector3);
            }
          }
          vector3List2.Add(transform.MultiplyPoint(points[0]));
          Handles.DrawPolyLine(vector3List2.ToArray());
          break;
        case WrapMode.PingPong:
          List<Vector3> vector3List3 = new List<Vector3>();
          for (int index2 = num1; index2 < 0; ++index2)
          {
            for (int index3 = 0; index3 < points.Length; ++index3)
            {
              if ((double) (index2 / 2) == (double) index2 / 2.0)
              {
                Vector3 point = points[index3];
                point.x += (float) index2 * (rangeEnd - rangeStart);
                Vector3 vector3 = transform.MultiplyPoint(point);
                vector3List3.Add(vector3);
              }
              else
              {
                Vector3 point = points[index1 - index3];
                point.x = (float) (-(double) point.x + (double) (index2 + 1) * ((double) rangeEnd - (double) rangeStart) + (double) rangeStart * 2.0);
                Vector3 vector3 = transform.MultiplyPoint(point);
                vector3List3.Add(vector3);
              }
            }
          }
          Handles.DrawPolyLine(vector3List3.ToArray());
          break;
        default:
          if (num1 < 0)
            Handles.DrawPolyLine(transform.MultiplyPoint(new Vector3(minTime, points[0].y, 0.0f)), transform.MultiplyPoint(new Vector3(Mathf.Min(maxTime, points[0].x), points[0].y, 0.0f)));
          break;
      }
      switch (postWrap)
      {
        case WrapMode.Loop:
          List<Vector3> vector3List4 = new List<Vector3>();
          vector3List4.Add(transform.MultiplyPoint(points[index1]));
          for (int index2 = 1; index2 <= num2; ++index2)
          {
            for (int index3 = 0; index3 < points.Length; ++index3)
            {
              Vector3 point = points[index3];
              point.x += (float) index2 * (rangeEnd - rangeStart);
              Vector3 vector3 = transform.MultiplyPoint(point);
              vector3List4.Add(vector3);
            }
          }
          Handles.DrawPolyLine(vector3List4.ToArray());
          break;
        case WrapMode.PingPong:
          List<Vector3> vector3List5 = new List<Vector3>();
          for (int index2 = 1; index2 <= num2; ++index2)
          {
            for (int index3 = 0; index3 < points.Length; ++index3)
            {
              if ((double) (index2 / 2) == (double) index2 / 2.0)
              {
                Vector3 point = points[index3];
                point.x += (float) index2 * (rangeEnd - rangeStart);
                Vector3 vector3 = transform.MultiplyPoint(point);
                vector3List5.Add(vector3);
              }
              else
              {
                Vector3 point = points[index1 - index3];
                point.x = (float) (-(double) point.x + (double) (index2 + 1) * ((double) rangeEnd - (double) rangeStart) + (double) rangeStart * 2.0);
                Vector3 vector3 = transform.MultiplyPoint(point);
                vector3List5.Add(vector3);
              }
            }
          }
          Handles.DrawPolyLine(vector3List5.ToArray());
          break;
        default:
          if (num2 > 0)
            Handles.DrawPolyLine(transform.MultiplyPoint(new Vector3(Mathf.Max(minTime, points[index1].x), points[index1].y, 0.0f)), transform.MultiplyPoint(new Vector3(maxTime, points[index1].y, 0.0f)));
          break;
      }
    }

    public Bounds GetBounds()
    {
      this.BuildCurveMesh();
      if (!this.m_Bounds.HasValue)
        this.m_Bounds = new Bounds?(this.m_CurveMesh.bounds);
      return this.m_Bounds.Value;
    }

    public Bounds GetBounds(float minTime, float maxTime)
    {
      Vector3[] points = this.GetPoints(minTime, maxTime);
      float num1 = float.PositiveInfinity;
      float num2 = float.NegativeInfinity;
      for (int index = 0; index < points.Length; ++index)
      {
        Vector3 vector3 = points[index];
        if ((double) vector3.y > (double) num2)
          num2 = vector3.y;
        if ((double) vector3.y < (double) num1)
          num1 = vector3.y;
      }
      if ((double) num1 == double.PositiveInfinity)
      {
        num1 = 0.0f;
        num2 = 0.0f;
      }
      return new Bounds(new Vector3((float) (((double) maxTime + (double) minTime) * 0.5), (float) (((double) num2 + (double) num1) * 0.5), 0.0f), new Vector3(maxTime - minTime, num2 - num1, 0.0f));
    }

    public void FlushCache()
    {
      Object.DestroyImmediate((Object) this.m_CurveMesh);
    }
  }
}
