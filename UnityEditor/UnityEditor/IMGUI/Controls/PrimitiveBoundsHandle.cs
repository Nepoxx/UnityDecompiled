// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  /// <summary>
  ///   <para>Base class for a compound handle to edit a bounding volume in the Scene view.</para>
  /// </summary>
  public abstract class PrimitiveBoundsHandle
  {
    private static readonly float s_DefaultMidpointHandleSize = 0.03f;
    private static readonly int[] s_NextAxis = new int[3]{ 1, 2, 0 };
    private int[] m_ControlIDs = new int[6];
    private static GUIContent s_EditModeButton;
    private Bounds m_Bounds;
    private Bounds m_BoundsOnClick;

    /// <summary>
    ///   <para>Create a new instance of the PrimitiveBoundsHandle class.</para>
    /// </summary>
    /// <param name="controlIDHint">An integer value used to generate consistent control IDs for each control handle on this instance. Avoid using the same value for all of your PrimitiveBoundsHandle instances.</param>
    [Obsolete("Use parameterless constructor instead.")]
    public PrimitiveBoundsHandle(int controlIDHint)
      : this()
    {
    }

    /// <summary>
    ///   <para>Create a new instance of the PrimitiveBoundsHandle class.</para>
    /// </summary>
    /// <param name="controlIDHint">An integer value used to generate consistent control IDs for each control handle on this instance. Avoid using the same value for all of your PrimitiveBoundsHandle instances.</param>
    public PrimitiveBoundsHandle()
    {
      this.handleColor = Color.white;
      this.wireframeColor = Color.white;
      this.axes = PrimitiveBoundsHandle.Axes.All;
    }

    internal static GUIContent editModeButton
    {
      get
      {
        if (PrimitiveBoundsHandle.s_EditModeButton == null)
          PrimitiveBoundsHandle.s_EditModeButton = new GUIContent(EditorGUIUtility.IconContent("EditCollider").image, EditorGUIUtility.TextContent("Edit bounding volume.\n\n - Hold Alt after clicking control handle to pin center in place.\n - Hold Shift after clicking control handle to scale uniformly.").text);
        return PrimitiveBoundsHandle.s_EditModeButton;
      }
    }

    private static float DefaultMidpointHandleSizeFunction(Vector3 position)
    {
      return HandleUtility.GetHandleSize(position) * PrimitiveBoundsHandle.s_DefaultMidpointHandleSize;
    }

    /// <summary>
    ///   <para>Returns or specifies the center of the bounding volume for the handle.</para>
    /// </summary>
    public Vector3 center
    {
      get
      {
        return this.m_Bounds.center;
      }
      set
      {
        this.m_Bounds.center = value;
      }
    }

    /// <summary>
    ///   <para>Flags specifying which axes should display control handles.</para>
    /// </summary>
    public PrimitiveBoundsHandle.Axes axes { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the color of the control handles.</para>
    /// </summary>
    public Color handleColor { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the color of the wireframe shape.</para>
    /// </summary>
    public Color wireframeColor { get; set; }

    /// <summary>
    ///   <para>An optional Handles.CapFunction to use when displaying the control handles. Defaults to Handles.DotHandleCap if no value is specified.</para>
    /// </summary>
    public Handles.CapFunction midpointHandleDrawFunction { get; set; }

    /// <summary>
    ///   <para>An optional Handles.SizeFunction to specify how large the control handles should be in the space of Handles.matrix. Defaults to a fixed screen-space size.</para>
    /// </summary>
    public Handles.SizeFunction midpointHandleSizeFunction { get; set; }

    /// <summary>
    ///   <para>Sets handleColor and wireframeColor to the same value.</para>
    /// </summary>
    /// <param name="color">The color to use for the control handles and the wireframe shape.</param>
    public void SetColor(Color color)
    {
      this.handleColor = color;
      this.wireframeColor = color;
    }

    /// <summary>
    ///   <para>A function to display this instance in the current handle camera using its current configuration.</para>
    /// </summary>
    public void DrawHandle()
    {
      int index1 = 0;
      for (int length = this.m_ControlIDs.Length; index1 < length; ++index1)
        this.m_ControlIDs[index1] = GUIUtility.GetControlID(this.GetHashCode(), FocusType.Passive);
      using (new Handles.DrawingScope(Handles.color * this.wireframeColor))
        this.DrawWireframe();
      if (Event.current.alt)
      {
        bool flag = true;
        foreach (int controlId in this.m_ControlIDs)
        {
          if (controlId == GUIUtility.hotControl)
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return;
      }
      Vector3 min = this.m_Bounds.min;
      Vector3 max = this.m_Bounds.max;
      int hotControl1 = GUIUtility.hotControl;
      bool isCameraInsideBox = this.m_Bounds.Contains(Handles.inverseMatrix.MultiplyPoint(Camera.current.transform.position));
      EditorGUI.BeginChangeCheck();
      using (new Handles.DrawingScope(Handles.color * this.handleColor))
        this.MidpointHandles(ref min, ref max, isCameraInsideBox);
      bool flag1 = EditorGUI.EndChangeCheck();
      if (hotControl1 != GUIUtility.hotControl && GUIUtility.hotControl != 0)
        this.m_BoundsOnClick = this.m_Bounds;
      if (!flag1)
        return;
      this.m_Bounds.center = (max + min) * 0.5f;
      this.m_Bounds.size = max - min;
      int index2 = 0;
      for (int length = this.m_ControlIDs.Length; index2 < length; ++index2)
      {
        if (GUIUtility.hotControl == this.m_ControlIDs[index2])
          this.m_Bounds = this.OnHandleChanged((PrimitiveBoundsHandle.HandleDirection) index2, this.m_BoundsOnClick, this.m_Bounds);
      }
      if (Event.current.shift)
      {
        int hotControl2 = GUIUtility.hotControl;
        Vector3 size = this.m_Bounds.size;
        int index3 = 0;
        if (hotControl2 == this.m_ControlIDs[2] || hotControl2 == this.m_ControlIDs[3])
          index3 = 1;
        if (hotControl2 == this.m_ControlIDs[4] || hotControl2 == this.m_ControlIDs[5])
          index3 = 2;
        float num = !Mathf.Approximately(this.m_BoundsOnClick.size[index3], 0.0f) ? size[index3] / this.m_BoundsOnClick.size[index3] : 1f;
        int nextAxi1 = PrimitiveBoundsHandle.s_NextAxis[index3];
        size[nextAxi1] = num * this.m_BoundsOnClick.size[nextAxi1];
        int nextAxi2 = PrimitiveBoundsHandle.s_NextAxis[nextAxi1];
        size[nextAxi2] = num * this.m_BoundsOnClick.size[nextAxi2];
        this.m_Bounds.size = size;
      }
      if (Event.current.alt)
        this.m_Bounds.center = this.m_BoundsOnClick.center;
    }

    /// <summary>
    ///   <para>Draw a wireframe shape for this instance. Subclasses must implement this method.</para>
    /// </summary>
    protected abstract void DrawWireframe();

    protected virtual Bounds OnHandleChanged(PrimitiveBoundsHandle.HandleDirection handle, Bounds boundsOnClick, Bounds newBounds)
    {
      return newBounds;
    }

    /// <summary>
    ///   <para>Gets the current size of the bounding volume for this instance.</para>
    /// </summary>
    /// <returns>
    ///   <para>The current size of the bounding volume for this instance.</para>
    /// </returns>
    protected Vector3 GetSize()
    {
      Vector3 size = this.m_Bounds.size;
      for (int vector3Axis = 0; vector3Axis < 3; ++vector3Axis)
      {
        if (!this.IsAxisEnabled(vector3Axis))
          size[vector3Axis] = 0.0f;
      }
      return size;
    }

    /// <summary>
    ///   <para>Sets the current size of the bounding volume for this instance.</para>
    /// </summary>
    /// <param name="size">A Vector3 specifying how large the bounding volume is along all of its axes.</param>
    protected void SetSize(Vector3 size)
    {
      this.m_Bounds.size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
    }

    protected bool IsAxisEnabled(PrimitiveBoundsHandle.Axes axis)
    {
      return (this.axes & axis) == axis;
    }

    /// <summary>
    ///   <para>Gets a value indicating whether the specified axis is enabled for the current instance.</para>
    /// </summary>
    /// <param name="axis">An Axes.</param>
    /// <param name="vector3Axis">An integer corresponding to an axis on a Vector3. For example, 0 is x, 1 is y, and 2 is z.</param>
    /// <returns>
    ///   <para>true if the specified axis is enabled; otherwise, false.</para>
    /// </returns>
    protected bool IsAxisEnabled(int vector3Axis)
    {
      switch (vector3Axis)
      {
        case 0:
          return this.IsAxisEnabled(PrimitiveBoundsHandle.Axes.X);
        case 1:
          return this.IsAxisEnabled(PrimitiveBoundsHandle.Axes.Y);
        case 2:
          return this.IsAxisEnabled(PrimitiveBoundsHandle.Axes.Z);
        default:
          throw new ArgumentOutOfRangeException(nameof (vector3Axis), "Must be 0, 1, or 2");
      }
    }

    private void MidpointHandles(ref Vector3 minPos, ref Vector3 maxPos, bool isCameraInsideBox)
    {
      Vector3 right = Vector3.right;
      Vector3 up = Vector3.up;
      Vector3 forward = Vector3.forward;
      Vector3 vector3_1 = (maxPos + minPos) * 0.5f;
      Vector3 localPos;
      if (this.IsAxisEnabled(PrimitiveBoundsHandle.Axes.X))
      {
        localPos = new Vector3(maxPos.x, vector3_1.y, vector3_1.z);
        Vector3 vector3_2 = this.MidpointHandle(this.m_ControlIDs[0], localPos, up, forward, isCameraInsideBox);
        maxPos.x = Mathf.Max(vector3_2.x, minPos.x);
        localPos = new Vector3(minPos.x, vector3_1.y, vector3_1.z);
        Vector3 vector3_3 = this.MidpointHandle(this.m_ControlIDs[1], localPos, up, -forward, isCameraInsideBox);
        minPos.x = Mathf.Min(vector3_3.x, maxPos.x);
      }
      if (this.IsAxisEnabled(PrimitiveBoundsHandle.Axes.Y))
      {
        localPos = new Vector3(vector3_1.x, maxPos.y, vector3_1.z);
        Vector3 vector3_2 = this.MidpointHandle(this.m_ControlIDs[2], localPos, right, -forward, isCameraInsideBox);
        maxPos.y = Mathf.Max(vector3_2.y, minPos.y);
        localPos = new Vector3(vector3_1.x, minPos.y, vector3_1.z);
        Vector3 vector3_3 = this.MidpointHandle(this.m_ControlIDs[3], localPos, right, forward, isCameraInsideBox);
        minPos.y = Mathf.Min(vector3_3.y, maxPos.y);
      }
      if (!this.IsAxisEnabled(PrimitiveBoundsHandle.Axes.Z))
        return;
      localPos = new Vector3(vector3_1.x, vector3_1.y, maxPos.z);
      Vector3 vector3_4 = this.MidpointHandle(this.m_ControlIDs[4], localPos, up, -right, isCameraInsideBox);
      maxPos.z = Mathf.Max(vector3_4.z, minPos.z);
      localPos = new Vector3(vector3_1.x, vector3_1.y, minPos.z);
      Vector3 vector3_5 = this.MidpointHandle(this.m_ControlIDs[5], localPos, up, right, isCameraInsideBox);
      minPos.z = Mathf.Min(vector3_5.z, maxPos.z);
    }

    private Vector3 MidpointHandle(int id, Vector3 localPos, Vector3 localTangent, Vector3 localBinormal, bool isCameraInsideBox)
    {
      Color color = Handles.color;
      this.AdjustMidpointHandleColor(localPos, localTangent, localBinormal, isCameraInsideBox);
      if ((double) Handles.color.a > 0.0)
      {
        Vector3 normalized = Vector3.Cross(localTangent, localBinormal).normalized;
        Handles.CapFunction capFunction1 = this.midpointHandleDrawFunction;
        if (capFunction1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          if (PrimitiveBoundsHandle.\u003C\u003Ef__mg\u0024cache0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            PrimitiveBoundsHandle.\u003C\u003Ef__mg\u0024cache0 = new Handles.CapFunction(Handles.DotHandleCap);
          }
          // ISSUE: reference to a compiler-generated field
          capFunction1 = PrimitiveBoundsHandle.\u003C\u003Ef__mg\u0024cache0;
        }
        Handles.CapFunction capFunction2 = capFunction1;
        Handles.SizeFunction sizeFunction1 = this.midpointHandleSizeFunction;
        if (sizeFunction1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          if (PrimitiveBoundsHandle.\u003C\u003Ef__mg\u0024cache1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            PrimitiveBoundsHandle.\u003C\u003Ef__mg\u0024cache1 = new Handles.SizeFunction(PrimitiveBoundsHandle.DefaultMidpointHandleSizeFunction);
          }
          // ISSUE: reference to a compiler-generated field
          sizeFunction1 = PrimitiveBoundsHandle.\u003C\u003Ef__mg\u0024cache1;
        }
        Handles.SizeFunction sizeFunction2 = sizeFunction1;
        localPos = Slider1D.Do(id, localPos, normalized, sizeFunction2(localPos), capFunction2, SnapSettings.scale);
      }
      Handles.color = color;
      return localPos;
    }

    private void AdjustMidpointHandleColor(Vector3 localPos, Vector3 localTangent, Vector3 localBinormal, bool isCameraInsideBox)
    {
      float a = 1f;
      if (!isCameraInsideBox && this.axes == PrimitiveBoundsHandle.Axes.All)
      {
        Vector3 normalized = Vector3.Cross(Handles.matrix.MultiplyVector(localTangent), Handles.matrix.MultiplyVector(localBinormal)).normalized;
        if ((!Camera.current.orthographic ? (double) Vector3.Dot((Camera.current.transform.position - Handles.matrix.MultiplyPoint(localPos)).normalized, normalized) : (double) Vector3.Dot(-Camera.current.transform.forward, normalized)) < -9.99999974737875E-05)
          a *= Handles.backfaceAlphaMultiplier;
      }
      Handles.color *= new Color(1f, 1f, 1f, a);
    }

    /// <summary>
    ///   <para>A flag enumeration for specifying which axes on a PrimitiveBoundsHandle object should be enabled.</para>
    /// </summary>
    [System.Flags]
    public enum Axes
    {
      None = 0,
      X = 1,
      Y = 2,
      Z = 4,
      All = Z | Y | X, // 0x00000007
    }

    /// <summary>
    ///   <para>An enumeration of directions the handle moves in.</para>
    /// </summary>
    protected enum HandleDirection
    {
      PositiveX,
      NegativeX,
      PositiveY,
      NegativeY,
      PositiveZ,
      NegativeZ,
    }
  }
}
