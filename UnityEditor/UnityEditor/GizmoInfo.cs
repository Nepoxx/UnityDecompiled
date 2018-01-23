// Decompiled with JetBrains decompiler
// Type: UnityEditor.GizmoInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class GizmoInfo
  {
    [SerializeField]
    private Vector2 m_Center = new Vector2(0.0f, 0.0f);
    [SerializeField]
    private float m_Angle = 0.0f;
    [SerializeField]
    private float m_Length = 0.2f;
    [SerializeField]
    private Vector2 m_Point1;
    [SerializeField]
    private Vector2 m_Point2;
    [SerializeField]
    private Vector4 m_Plane;
    [SerializeField]
    private Vector4 m_PlaneOrtho;

    public GizmoInfo()
    {
      this.Update(this.m_Center, this.m_Length, this.m_Angle);
    }

    public Vector2 point1
    {
      get
      {
        return this.m_Point1;
      }
    }

    public Vector2 point2
    {
      get
      {
        return this.m_Point2;
      }
    }

    public Vector2 center
    {
      get
      {
        return this.m_Center;
      }
    }

    public float angle
    {
      get
      {
        return this.m_Angle;
      }
    }

    public float length
    {
      get
      {
        return this.m_Length;
      }
    }

    public Vector4 plane
    {
      get
      {
        return this.m_Plane;
      }
    }

    public Vector4 planeOrtho
    {
      get
      {
        return this.m_PlaneOrtho;
      }
    }

    private Vector4 Get2DPlane(Vector2 firstPoint, float angle)
    {
      Vector4 vector4 = new Vector4();
      angle %= 6.283185f;
      Vector2 vector2 = new Vector2(firstPoint.x + Mathf.Sin(angle), firstPoint.y + Mathf.Cos(angle)) - firstPoint;
      if ((double) Mathf.Abs(vector2.x) < 1E-05)
      {
        vector4.Set(-1f, 0.0f, firstPoint.x, 0.0f);
        float num = (double) Mathf.Cos(angle) <= 0.0 ? -1f : 1f;
        vector4 *= num;
      }
      else
      {
        float num = vector2.y / vector2.x;
        vector4.Set(-num, 1f, (float) -((double) firstPoint.y - (double) num * (double) firstPoint.x), 0.0f);
      }
      if ((double) angle > 3.14159274101257)
        vector4 = -vector4;
      float num1 = Mathf.Sqrt((float) ((double) vector4.x * (double) vector4.x + (double) vector4.y * (double) vector4.y));
      vector4 /= num1;
      return vector4;
    }

    public void Update(Vector2 point1, Vector2 point2)
    {
      this.m_Point1 = point1;
      this.m_Point2 = point2;
      this.m_Center = (point1 + point2) * 0.5f;
      this.m_Length = (point2 - point1).magnitude * 0.5f;
      Vector3 rhs = (Vector3) this.Get2DPlane(this.m_Center, 0.0f);
      float num = Vector3.Dot(new Vector3(point1.x, point1.y, 1f), rhs);
      this.m_Angle = (float) Math.PI / 180f * Vector2.Angle(new Vector2(0.0f, 1f), (point1 - point2).normalized);
      if ((double) num > 0.0)
        this.m_Angle = 6.283185f - this.m_Angle;
      this.m_Plane = this.Get2DPlane(this.m_Center, this.m_Angle);
      this.m_PlaneOrtho = this.Get2DPlane(this.m_Center, this.m_Angle + 1.570796f);
    }

    public void Update(Vector2 center, float length, float angle)
    {
      this.m_Center = center;
      this.m_Length = length;
      this.m_Angle = angle;
      this.m_Plane = this.Get2DPlane(this.m_Center, this.m_Angle);
      this.m_PlaneOrtho = this.Get2DPlane(this.m_Center, this.m_Angle + 1.570796f);
      Vector2 vector2 = new Vector2(this.m_PlaneOrtho.x, this.m_PlaneOrtho.y);
      this.m_Point1 = this.m_Center + vector2 * this.m_Length;
      this.m_Point2 = this.m_Center - vector2 * this.m_Length;
    }
  }
}
