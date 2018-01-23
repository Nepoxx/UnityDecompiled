// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.CapsuleBoundsHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  /// <summary>
  ///   <para>A compound handle to edit a capsule-shaped bounding volume in the Scene view.</para>
  /// </summary>
  public class CapsuleBoundsHandle : PrimitiveBoundsHandle
  {
    private static readonly Vector3[] s_HeightAxes = new Vector3[3]{ Vector3.right, Vector3.up, Vector3.forward };
    private static readonly int[] s_NextAxis = new int[3]{ 1, 2, 0 };
    private int m_HeightAxis = 1;
    private const int k_DirectionX = 0;
    private const int k_DirectionY = 1;
    private const int k_DirectionZ = 2;

    /// <summary>
    ///   <para>Create a new instance of the CapsuleBoundsHandle class.</para>
    /// </summary>
    /// <param name="controlIDHint">An integer value used to generate consistent control IDs for each control handle on this instance. Avoid using the same value for all of your CapsuleBoundsHandle instances.</param>
    [Obsolete("Use parameterless constructor instead.")]
    public CapsuleBoundsHandle(int controlIDHint)
      : base(controlIDHint)
    {
    }

    /// <summary>
    ///   <para>Create a new instance of the CapsuleBoundsHandle class.</para>
    /// </summary>
    /// <param name="controlIDHint">An integer value used to generate consistent control IDs for each control handle on this instance. Avoid using the same value for all of your CapsuleBoundsHandle instances.</param>
    public CapsuleBoundsHandle()
    {
    }

    /// <summary>
    ///   <para>Returns or specifies the axis in the handle's space to which height maps. The radius maps to the remaining axes.</para>
    /// </summary>
    public CapsuleBoundsHandle.HeightAxis heightAxis
    {
      get
      {
        return (CapsuleBoundsHandle.HeightAxis) this.m_HeightAxis;
      }
      set
      {
        int index = (int) value;
        if (this.m_HeightAxis == index)
          return;
        Vector3 size = Vector3.one * this.radius * 2f;
        size[index] = this.GetSize()[this.m_HeightAxis];
        this.m_HeightAxis = index;
        this.SetSize(size);
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the height of the capsule bounding volume.</para>
    /// </summary>
    public float height
    {
      get
      {
        return this.IsAxisEnabled(this.m_HeightAxis) ? Mathf.Max(this.GetSize()[this.m_HeightAxis], 2f * this.radius) : 0.0f;
      }
      set
      {
        value = Mathf.Max(Mathf.Abs(value), 2f * this.radius);
        if ((double) this.height == (double) value)
          return;
        Vector3 size = this.GetSize();
        size[this.m_HeightAxis] = value;
        this.SetSize(size);
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the radius of the capsule bounding volume.</para>
    /// </summary>
    public float radius
    {
      get
      {
        int radiusAxis;
        if (this.GetRadiusAxis(out radiusAxis) || this.IsAxisEnabled(this.m_HeightAxis))
          return 0.5f * this.GetSize()[radiusAxis];
        return 0.0f;
      }
      set
      {
        Vector3 size = this.GetSize();
        float b = 2f * value;
        for (int index = 0; index < 3; ++index)
          size[index] = index != this.m_HeightAxis ? b : Mathf.Max(size[index], b);
        this.SetSize(size);
      }
    }

    /// <summary>
    ///   <para>Draw a wireframe capsule for this instance.</para>
    /// </summary>
    protected override void DrawWireframe()
    {
      CapsuleBoundsHandle.HeightAxis heightAxis1 = CapsuleBoundsHandle.HeightAxis.Y;
      CapsuleBoundsHandle.HeightAxis heightAxis2 = CapsuleBoundsHandle.HeightAxis.Z;
      switch (this.heightAxis)
      {
        case CapsuleBoundsHandle.HeightAxis.Y:
          heightAxis1 = CapsuleBoundsHandle.HeightAxis.Z;
          heightAxis2 = CapsuleBoundsHandle.HeightAxis.X;
          break;
        case CapsuleBoundsHandle.HeightAxis.Z:
          heightAxis1 = CapsuleBoundsHandle.HeightAxis.X;
          heightAxis2 = CapsuleBoundsHandle.HeightAxis.Y;
          break;
      }
      bool flag1 = this.IsAxisEnabled((int) this.heightAxis);
      bool flag2 = this.IsAxisEnabled((int) heightAxis1);
      bool flag3 = this.IsAxisEnabled((int) heightAxis2);
      Vector3 heightAx1 = CapsuleBoundsHandle.s_HeightAxes[this.m_HeightAxis];
      Vector3 heightAx2 = CapsuleBoundsHandle.s_HeightAxes[CapsuleBoundsHandle.s_NextAxis[this.m_HeightAxis]];
      Vector3 heightAx3 = CapsuleBoundsHandle.s_HeightAxes[CapsuleBoundsHandle.s_NextAxis[CapsuleBoundsHandle.s_NextAxis[this.m_HeightAxis]]];
      float radius = this.radius;
      float height = this.height;
      Vector3 center1 = this.center + heightAx1 * (height * 0.5f - radius);
      Vector3 center2 = this.center - heightAx1 * (height * 0.5f - radius);
      if (flag1)
      {
        if (flag3)
        {
          Handles.DrawWireArc(center1, heightAx2, heightAx3, 180f, radius);
          Handles.DrawWireArc(center2, heightAx2, heightAx3, -180f, radius);
          Handles.DrawLine(center1 + heightAx3 * radius, center2 + heightAx3 * radius);
          Handles.DrawLine(center1 - heightAx3 * radius, center2 - heightAx3 * radius);
        }
        if (flag2)
        {
          Handles.DrawWireArc(center1, heightAx3, heightAx2, -180f, radius);
          Handles.DrawWireArc(center2, heightAx3, heightAx2, 180f, radius);
          Handles.DrawLine(center1 + heightAx2 * radius, center2 + heightAx2 * radius);
          Handles.DrawLine(center1 - heightAx2 * radius, center2 - heightAx2 * radius);
        }
      }
      if (!flag2 || !flag3)
        return;
      Handles.DrawWireArc(center1, heightAx1, heightAx2, 360f, radius);
      Handles.DrawWireArc(center2, heightAx1, heightAx2, -360f, radius);
    }

    protected override Bounds OnHandleChanged(PrimitiveBoundsHandle.HandleDirection handle, Bounds boundsOnClick, Bounds newBounds)
    {
      int index1 = 0;
      switch (handle)
      {
        case PrimitiveBoundsHandle.HandleDirection.PositiveY:
        case PrimitiveBoundsHandle.HandleDirection.NegativeY:
          index1 = 1;
          break;
        case PrimitiveBoundsHandle.HandleDirection.PositiveZ:
        case PrimitiveBoundsHandle.HandleDirection.NegativeZ:
          index1 = 2;
          break;
      }
      Vector3 max = newBounds.max;
      Vector3 min = newBounds.min;
      if (index1 == this.m_HeightAxis)
      {
        int radiusAxis;
        this.GetRadiusAxis(out radiusAxis);
        float num = max[radiusAxis] - min[radiusAxis];
        if ((double) (max[this.m_HeightAxis] - min[this.m_HeightAxis]) < (double) num)
        {
          if (handle == PrimitiveBoundsHandle.HandleDirection.PositiveX || handle == PrimitiveBoundsHandle.HandleDirection.PositiveY || handle == PrimitiveBoundsHandle.HandleDirection.PositiveZ)
            max[this.m_HeightAxis] = min[this.m_HeightAxis] + num;
          else
            min[this.m_HeightAxis] = max[this.m_HeightAxis] - num;
        }
      }
      else
      {
        max[this.m_HeightAxis] = boundsOnClick.center[this.m_HeightAxis] + 0.5f * boundsOnClick.size[this.m_HeightAxis];
        min[this.m_HeightAxis] = boundsOnClick.center[this.m_HeightAxis] - 0.5f * boundsOnClick.size[this.m_HeightAxis];
        float b = (float) (0.5 * ((double) max[index1] - (double) min[index1]));
        float a = (float) (0.5 * ((double) max[this.m_HeightAxis] - (double) min[this.m_HeightAxis]));
        for (int index2 = 0; index2 < 3; ++index2)
        {
          if (index2 != index1)
          {
            float num = index2 != this.m_HeightAxis ? b : Mathf.Max(a, b);
            min[index2] = this.center[index2] - num;
            max[index2] = this.center[index2] + num;
          }
        }
      }
      return new Bounds((max + min) * 0.5f, max - min);
    }

    private bool GetRadiusAxis(out int radiusAxis)
    {
      radiusAxis = CapsuleBoundsHandle.s_NextAxis[this.m_HeightAxis];
      if (this.IsAxisEnabled(radiusAxis))
        return this.IsAxisEnabled(CapsuleBoundsHandle.s_NextAxis[radiusAxis]);
      radiusAxis = CapsuleBoundsHandle.s_NextAxis[radiusAxis];
      return false;
    }

    /// <summary>
    ///   <para>An enumeration for specifying which axis on a CapsuleBoundsHandle object maps to the CapsuleBoundsHandle.height parameter.</para>
    /// </summary>
    public enum HeightAxis
    {
      X,
      Y,
      Z,
    }
  }
}
