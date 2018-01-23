// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.BoxBoundsHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  /// <summary>
  ///   <para>A compound handle to edit a box-shaped bounding volume in the Scene view.</para>
  /// </summary>
  public class BoxBoundsHandle : PrimitiveBoundsHandle
  {
    /// <summary>
    ///   <para>Create a new instance of the BoxBoundsHandle class.</para>
    /// </summary>
    /// <param name="controlIDHint">An integer value used to generate consistent control IDs for each control handle on this instance. Avoid using the same value for all of your BoxBoundsHandle instances.</param>
    [Obsolete("Use parameterless constructor instead.")]
    public BoxBoundsHandle(int controlIDHint)
      : base(controlIDHint)
    {
    }

    /// <summary>
    ///   <para>Create a new instance of the BoxBoundsHandle class.</para>
    /// </summary>
    /// <param name="controlIDHint">An integer value used to generate consistent control IDs for each control handle on this instance. Avoid using the same value for all of your BoxBoundsHandle instances.</param>
    public BoxBoundsHandle()
    {
    }

    /// <summary>
    ///   <para>Returns or specifies the size of the bounding box.</para>
    /// </summary>
    public Vector3 size
    {
      get
      {
        return this.GetSize();
      }
      set
      {
        this.SetSize(value);
      }
    }

    /// <summary>
    ///   <para>Draw a wireframe box for this instance.</para>
    /// </summary>
    protected override void DrawWireframe()
    {
      Handles.DrawWireCube(this.center, this.size);
    }
  }
}
