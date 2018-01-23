// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.SphereBoundsHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  /// <summary>
  ///   <para>A compound handle to edit a sphere-shaped bounding volume in the Scene view.</para>
  /// </summary>
  public class SphereBoundsHandle : PrimitiveBoundsHandle
  {
    /// <summary>
    ///   <para>Create a new instance of the SphereBoundsHandle class.</para>
    /// </summary>
    /// <param name="controlIDHint">An integer value used to generate consistent control IDs for each control handle on this instance. Avoid using the same value for all of your SphereBoundsHandle instances.</param>
    [Obsolete("Use parameterless constructor instead.")]
    public SphereBoundsHandle(int controlIDHint)
      : base(controlIDHint)
    {
    }

    /// <summary>
    ///   <para>Create a new instance of the SphereBoundsHandle class.</para>
    /// </summary>
    /// <param name="controlIDHint">An integer value used to generate consistent control IDs for each control handle on this instance. Avoid using the same value for all of your SphereBoundsHandle instances.</param>
    public SphereBoundsHandle()
    {
    }

    /// <summary>
    ///   <para>Returns or specifies the radius of the capsule bounding volume.</para>
    /// </summary>
    public float radius
    {
      get
      {
        Vector3 size = this.GetSize();
        float a = 0.0f;
        for (int vector3Axis = 0; vector3Axis < 3; ++vector3Axis)
        {
          if (this.IsAxisEnabled(vector3Axis))
            a = Mathf.Max(a, Mathf.Abs(size[vector3Axis]));
        }
        return a * 0.5f;
      }
      set
      {
        this.SetSize(2f * value * Vector3.one);
      }
    }

    /// <summary>
    ///   <para>Draw a wireframe sphere for this instance.</para>
    /// </summary>
    protected override void DrawWireframe()
    {
      bool flag1 = this.IsAxisEnabled(PrimitiveBoundsHandle.Axes.X);
      bool flag2 = this.IsAxisEnabled(PrimitiveBoundsHandle.Axes.Y);
      bool flag3 = this.IsAxisEnabled(PrimitiveBoundsHandle.Axes.Z);
      if (flag1 && flag2)
        Handles.DrawWireArc(this.center, Vector3.forward, Vector3.up, 360f, this.radius);
      if (flag1 && flag3)
        Handles.DrawWireArc(this.center, Vector3.up, Vector3.right, 360f, this.radius);
      if (flag2 && flag3)
        Handles.DrawWireArc(this.center, Vector3.right, Vector3.forward, 360f, this.radius);
      if (flag1 && !flag2 && !flag3)
        Handles.DrawLine(Vector3.right * this.radius, Vector3.left * this.radius);
      if (!flag1 && flag2 && !flag3)
        Handles.DrawLine(Vector3.up * this.radius, Vector3.down * this.radius);
      if (flag1 || flag2 || !flag3)
        return;
      Handles.DrawLine(Vector3.forward * this.radius, Vector3.back * this.radius);
    }

    protected override Bounds OnHandleChanged(PrimitiveBoundsHandle.HandleDirection handle, Bounds boundsOnClick, Bounds newBounds)
    {
      Vector3 max = newBounds.max;
      Vector3 min = newBounds.min;
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
      float num = (float) (0.5 * ((double) max[index1] - (double) min[index1]));
      for (int index2 = 0; index2 < 3; ++index2)
      {
        if (index2 != index1)
        {
          min[index2] = this.center[index2] - num;
          max[index2] = this.center[index2] + num;
        }
      }
      return new Bounds((max + min) * 0.5f, max - min);
    }
  }
}
