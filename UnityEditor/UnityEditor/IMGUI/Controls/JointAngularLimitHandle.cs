// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.JointAngularLimitHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  /// <summary>
  ///   <para>A class for a compound handle to edit multiaxial angular motion limits in the Scene view.</para>
  /// </summary>
  public class JointAngularLimitHandle
  {
    private static readonly Matrix4x4 s_XHandleOffset = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(90f, Vector3.forward), Vector3.one);
    private static readonly Matrix4x4 s_ZHandleOffset = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(90f, Vector3.left), Vector3.one);
    private static readonly float s_LockedColorAmount = 0.5f;
    private static readonly Color s_LockedColor = new Color(0.5f, 0.5f, 0.5f, 0.0f);
    private List<KeyValuePair<Action, float>> m_HandleFunctionDistances = new List<KeyValuePair<Action, float>>(6);
    private bool m_XHandleColorInitialized = false;
    private bool m_YHandleColorInitialized = false;
    private bool m_ZHandleColorInitialized = false;
    private ArcHandle m_XMinHandle;
    private ArcHandle m_XMaxHandle;
    private ArcHandle m_YMinHandle;
    private ArcHandle m_YMaxHandle;
    private ArcHandle m_ZMinHandle;
    private ArcHandle m_ZMaxHandle;
    private Matrix4x4 m_SecondaryAxesMatrix;

    /// <summary>
    ///   <para>Creates a new instance of the JointAngularLimitHandle class.</para>
    /// </summary>
    public JointAngularLimitHandle()
    {
      this.m_XMinHandle = new ArcHandle();
      this.m_XMaxHandle = new ArcHandle();
      this.m_YMinHandle = new ArcHandle();
      this.m_YMaxHandle = new ArcHandle();
      this.m_ZMinHandle = new ArcHandle();
      this.m_ZMaxHandle = new ArcHandle();
      ConfigurableJointMotion configurableJointMotion1 = ConfigurableJointMotion.Limited;
      this.zMotion = configurableJointMotion1;
      ConfigurableJointMotion configurableJointMotion2 = configurableJointMotion1;
      this.yMotion = configurableJointMotion2;
      this.xMotion = configurableJointMotion2;
      this.radius = 1f;
      this.fillAlpha = 0.1f;
      this.wireframeAlpha = 1f;
      Vector2 vector2_1 = new Vector2(-180f, 180f);
      this.zRange = vector2_1;
      Vector2 vector2_2 = vector2_1;
      this.yRange = vector2_2;
      this.xRange = vector2_2;
    }

    private static float GetSortingDistance(ArcHandle handle)
    {
      Vector3 rhs = Handles.matrix.MultiplyPoint3x4(Quaternion.AngleAxis(handle.angle, Vector3.up) * Vector3.forward * handle.radius) - Camera.current.transform.position;
      if (Camera.current.orthographic)
      {
        Vector3 forward = Camera.current.transform.forward;
        rhs = forward * Vector3.Dot(forward, rhs);
      }
      return rhs.sqrMagnitude;
    }

    private static int CompareHandleFunctionsByDistance(KeyValuePair<Action, float> func1, KeyValuePair<Action, float> func2)
    {
      return func2.Value.CompareTo(func1.Value);
    }

    /// <summary>
    ///   <para>Returns or specifies the minimum angular motion about the x-axis.</para>
    /// </summary>
    public float xMin
    {
      get
      {
        switch (this.xMotion)
        {
          case ConfigurableJointMotion.Locked:
            return 0.0f;
          case ConfigurableJointMotion.Free:
            return this.xRange.x;
          default:
            return Mathf.Clamp(this.m_XMinHandle.angle, this.xRange.x, this.m_XMaxHandle.angle);
        }
      }
      set
      {
        this.m_XMinHandle.angle = value;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the maximum angular motion about the x-axis.</para>
    /// </summary>
    public float xMax
    {
      get
      {
        switch (this.xMotion)
        {
          case ConfigurableJointMotion.Locked:
            return 0.0f;
          case ConfigurableJointMotion.Free:
            return this.xRange.y;
          default:
            return Mathf.Clamp(this.m_XMaxHandle.angle, this.m_XMinHandle.angle, this.xRange.y);
        }
      }
      set
      {
        this.m_XMaxHandle.angle = value;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the minimum angular motion about the y-axis.</para>
    /// </summary>
    public float yMin
    {
      get
      {
        switch (this.yMotion)
        {
          case ConfigurableJointMotion.Locked:
            return 0.0f;
          case ConfigurableJointMotion.Free:
            return this.yRange.x;
          default:
            return Mathf.Clamp(this.m_YMinHandle.angle, this.yRange.x, this.m_YMaxHandle.angle);
        }
      }
      set
      {
        this.m_YMinHandle.angle = value;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the maximum angular motion about the y-axis.</para>
    /// </summary>
    public float yMax
    {
      get
      {
        switch (this.yMotion)
        {
          case ConfigurableJointMotion.Locked:
            return 0.0f;
          case ConfigurableJointMotion.Free:
            return this.yRange.y;
          default:
            return Mathf.Clamp(this.m_YMaxHandle.angle, this.m_YMinHandle.angle, this.yRange.y);
        }
      }
      set
      {
        this.m_YMaxHandle.angle = value;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the minimum angular motion about the z-axis.</para>
    /// </summary>
    public float zMin
    {
      get
      {
        switch (this.zMotion)
        {
          case ConfigurableJointMotion.Locked:
            return 0.0f;
          case ConfigurableJointMotion.Free:
            return this.zRange.x;
          default:
            return Mathf.Clamp(this.m_ZMinHandle.angle, this.zRange.x, this.m_ZMaxHandle.angle);
        }
      }
      set
      {
        this.m_ZMinHandle.angle = value;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the maximum angular motion about the z-axis.</para>
    /// </summary>
    public float zMax
    {
      get
      {
        switch (this.zMotion)
        {
          case ConfigurableJointMotion.Locked:
            return 0.0f;
          case ConfigurableJointMotion.Free:
            return this.zRange.y;
          default:
            return Mathf.Clamp(this.m_ZMaxHandle.angle, this.m_ZMinHandle.angle, this.zRange.y);
        }
      }
      set
      {
        this.m_ZMaxHandle.angle = value;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the range of valid values for angular motion about the x-axis. Defaults to [-180.0, 180.0].</para>
    /// </summary>
    public Vector2 xRange { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the range of valid values for angular motion about the y-axis. Defaults to [-180.0, 180.0].</para>
    /// </summary>
    public Vector2 yRange { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the range of valid values for angular motion about the z-axis. Defaults to [-180.0, 180.0].</para>
    /// </summary>
    public Vector2 zRange { get; set; }

    /// <summary>
    ///   <para>Returns or specifies how angular motion is limited about the x-axis. Defaults to ConfigurableJointMotion.Limited.</para>
    /// </summary>
    public ConfigurableJointMotion xMotion { get; set; }

    /// <summary>
    ///   <para>Returns or specifies how angular motion is limited about the y-axis. Defaults to ConfigurableJointMotion.Limited.</para>
    /// </summary>
    public ConfigurableJointMotion yMotion { get; set; }

    /// <summary>
    ///   <para>Returns or specifies how angular motion is limited about the z-axis. Defaults to ConfigurableJointMotion.Limited.</para>
    /// </summary>
    public ConfigurableJointMotion zMotion { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the color to use for the handle limiting motion around the x-axis. Defaults to Handles.xAxisColor.</para>
    /// </summary>
    public Color xHandleColor
    {
      get
      {
        if (!this.m_XHandleColorInitialized)
          this.xHandleColor = Handles.xAxisColor;
        return this.m_XMinHandle.angleHandleColor;
      }
      set
      {
        this.m_XMinHandle.SetColorWithoutRadiusHandle(value, this.fillAlpha);
        this.m_XMaxHandle.SetColorWithoutRadiusHandle(value, this.fillAlpha);
        this.m_XHandleColorInitialized = true;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the color to use for the handle limiting motion around the y-axis. Defaults to Handles.yAxisColor.</para>
    /// </summary>
    public Color yHandleColor
    {
      get
      {
        if (!this.m_YHandleColorInitialized)
          this.yHandleColor = Handles.yAxisColor;
        return this.m_YMinHandle.angleHandleColor;
      }
      set
      {
        this.m_YMinHandle.SetColorWithoutRadiusHandle(value, this.fillAlpha);
        this.m_YMaxHandle.SetColorWithoutRadiusHandle(value, this.fillAlpha);
        this.m_YHandleColorInitialized = true;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the color to use for the handle limiting motion around the z-axis. Defaults to Handles.zAxisColor.</para>
    /// </summary>
    public Color zHandleColor
    {
      get
      {
        if (!this.m_ZHandleColorInitialized)
          this.zHandleColor = Handles.zAxisColor;
        return this.m_ZMinHandle.angleHandleColor;
      }
      set
      {
        this.m_ZMinHandle.SetColorWithoutRadiusHandle(value, this.fillAlpha);
        this.m_ZMaxHandle.SetColorWithoutRadiusHandle(value, this.fillAlpha);
        this.m_ZHandleColorInitialized = true;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the radius of the arc for the handle. Defaults to 1.0.</para>
    /// </summary>
    public float radius
    {
      get
      {
        return this.m_XMinHandle.radius;
      }
      set
      {
        this.m_XMinHandle.radius = value;
        this.m_XMaxHandle.radius = value;
        this.m_YMinHandle.radius = value;
        this.m_YMaxHandle.radius = value;
        this.m_ZMinHandle.radius = value;
        this.m_ZMaxHandle.radius = value;
      }
    }

    /// <summary>
    ///   <para>Returns or specifies the opacity to use when rendering fill shapes for the range of motion for each axis. Defaults to 0.1.</para>
    /// </summary>
    public float fillAlpha { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the opacity to use for the curved lines along the outside of the arcs of motion. Defaults to 1.0.</para>
    /// </summary>
    public float wireframeAlpha { get; set; }

    /// <summary>
    ///   <para>An optional Handles.CapFunction to use for displaying the control handles. Defaults to a line terminated with Handles.CylinderHandleCap if no value is specified.</para>
    /// </summary>
    public Handles.CapFunction angleHandleDrawFunction
    {
      get
      {
        return this.m_XMinHandle.angleHandleDrawFunction;
      }
      set
      {
        this.m_XMinHandle.angleHandleDrawFunction = value;
        this.m_XMaxHandle.angleHandleDrawFunction = value;
        this.m_YMinHandle.angleHandleDrawFunction = value;
        this.m_YMaxHandle.angleHandleDrawFunction = value;
        this.m_ZMinHandle.angleHandleDrawFunction = value;
        this.m_ZMaxHandle.angleHandleDrawFunction = value;
      }
    }

    /// <summary>
    ///   <para>An optional Handles.SizeFunction to specify how large the control handles should be. Defaults to a fixed screen-space size.</para>
    /// </summary>
    public Handles.SizeFunction angleHandleSizeFunction
    {
      get
      {
        return this.m_XMinHandle.angleHandleSizeFunction;
      }
      set
      {
        this.m_XMinHandle.angleHandleSizeFunction = value;
        this.m_XMaxHandle.angleHandleSizeFunction = value;
        this.m_YMinHandle.angleHandleSizeFunction = value;
        this.m_YMaxHandle.angleHandleSizeFunction = value;
        this.m_ZMinHandle.angleHandleSizeFunction = value;
        this.m_ZMaxHandle.angleHandleSizeFunction = value;
      }
    }

    /// <summary>
    ///   <para>A function to display this instance in the current handle camera using its current configuration.</para>
    /// </summary>
    public void DrawHandle()
    {
      this.m_SecondaryAxesMatrix = Handles.matrix;
      this.xHandleColor = this.xHandleColor;
      this.yHandleColor = this.yHandleColor;
      this.zHandleColor = this.zHandleColor;
      ArcHandle xminHandle = this.m_XMinHandle;
      Color clear1 = Color.clear;
      this.m_XMinHandle.wireframeColor = clear1;
      Color color1 = clear1;
      xminHandle.fillColor = color1;
      ArcHandle xmaxHandle = this.m_XMaxHandle;
      Color clear2 = Color.clear;
      this.m_XMaxHandle.wireframeColor = clear2;
      Color color2 = clear2;
      xmaxHandle.fillColor = color2;
      ArcHandle yminHandle = this.m_YMinHandle;
      Color clear3 = Color.clear;
      this.m_YMinHandle.wireframeColor = clear3;
      Color color3 = clear3;
      yminHandle.fillColor = color3;
      ArcHandle ymaxHandle = this.m_YMaxHandle;
      Color clear4 = Color.clear;
      this.m_YMaxHandle.wireframeColor = clear4;
      Color color4 = clear4;
      ymaxHandle.fillColor = color4;
      ArcHandle zminHandle = this.m_ZMinHandle;
      Color clear5 = Color.clear;
      this.m_ZMinHandle.wireframeColor = clear5;
      Color color5 = clear5;
      zminHandle.fillColor = color5;
      ArcHandle zmaxHandle = this.m_ZMaxHandle;
      Color clear6 = Color.clear;
      this.m_ZMaxHandle.wireframeColor = clear6;
      Color color6 = clear6;
      zmaxHandle.fillColor = color6;
      Color color7 = new Color(1f, 1f, 1f, this.fillAlpha);
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      switch (this.xMotion)
      {
        case ConfigurableJointMotion.Locked:
          using (new Handles.DrawingScope(Handles.color * Color.Lerp(this.xHandleColor, JointAngularLimitHandle.s_LockedColor, JointAngularLimitHandle.s_LockedColorAmount)))
          {
            Handles.DrawWireDisc(Vector3.zero, Vector3.right, this.radius);
            break;
          }
        case ConfigurableJointMotion.Limited:
          flag1 = true;
          this.m_SecondaryAxesMatrix *= Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis((float) (((double) this.xMin + (double) this.xMax) * 0.5), Vector3.left), Vector3.one);
          if (this.yMotion == ConfigurableJointMotion.Limited)
          {
            this.DrawMultiaxialFillShape();
            break;
          }
          using (new Handles.DrawingScope(Handles.matrix * JointAngularLimitHandle.s_XHandleOffset))
          {
            this.DrawArc(this.m_XMinHandle, this.m_XMaxHandle, this.xHandleColor * color7, JointAngularLimitHandle.ArcType.Solid);
            break;
          }
        case ConfigurableJointMotion.Free:
          using (new Handles.DrawingScope(Handles.color * this.xHandleColor))
          {
            Handles.DrawWireDisc(Vector3.zero, Vector3.right, this.radius);
            Handles.color *= color7;
            Handles.DrawSolidDisc(Vector3.zero, Vector3.right, this.radius);
            break;
          }
      }
      using (new Handles.DrawingScope(this.m_SecondaryAxesMatrix))
      {
        switch (this.yMotion)
        {
          case ConfigurableJointMotion.Locked:
            using (new Handles.DrawingScope(Handles.color * Color.Lerp(this.yHandleColor, JointAngularLimitHandle.s_LockedColor, JointAngularLimitHandle.s_LockedColorAmount)))
            {
              Handles.DrawWireDisc(Vector3.zero, Vector3.up, this.radius);
              break;
            }
          case ConfigurableJointMotion.Limited:
            flag2 = true;
            if (this.xMotion != ConfigurableJointMotion.Limited)
            {
              this.DrawArc(this.m_YMinHandle, this.m_YMaxHandle, this.yHandleColor * color7, JointAngularLimitHandle.ArcType.Solid);
              break;
            }
            break;
          case ConfigurableJointMotion.Free:
            using (new Handles.DrawingScope(Handles.color * this.yHandleColor))
            {
              Handles.DrawWireDisc(Vector3.zero, Vector3.up, this.radius);
              Handles.color *= color7;
              Handles.DrawSolidDisc(Vector3.zero, Vector3.up, this.radius);
              break;
            }
        }
        switch (this.zMotion)
        {
          case ConfigurableJointMotion.Locked:
            using (new Handles.DrawingScope(Handles.color * Color.Lerp(this.zHandleColor, JointAngularLimitHandle.s_LockedColor, JointAngularLimitHandle.s_LockedColorAmount)))
            {
              Handles.DrawWireDisc(Vector3.zero, Vector3.forward, this.radius);
              break;
            }
          case ConfigurableJointMotion.Limited:
            using (new Handles.DrawingScope(Handles.matrix * JointAngularLimitHandle.s_ZHandleOffset))
              this.DrawArc(this.m_ZMinHandle, this.m_ZMaxHandle, this.zHandleColor * color7, JointAngularLimitHandle.ArcType.Solid);
            flag3 = true;
            break;
          case ConfigurableJointMotion.Free:
            using (new Handles.DrawingScope(Handles.color * this.zHandleColor))
            {
              Handles.DrawWireDisc(Vector3.zero, Vector3.forward, this.radius);
              Handles.color *= color7;
              Handles.DrawSolidDisc(Vector3.zero, Vector3.forward, this.radius);
              break;
            }
        }
      }
      this.m_HandleFunctionDistances.Clear();
      this.m_XMinHandle.GetControlIDs();
      this.m_XMaxHandle.GetControlIDs();
      this.m_YMinHandle.GetControlIDs();
      this.m_YMaxHandle.GetControlIDs();
      this.m_ZMinHandle.GetControlIDs();
      this.m_ZMaxHandle.GetControlIDs();
      if (flag1)
      {
        using (new Handles.DrawingScope(Handles.matrix * JointAngularLimitHandle.s_XHandleOffset))
        {
          this.DrawArc(this.m_XMinHandle, this.m_XMaxHandle, this.xHandleColor, JointAngularLimitHandle.ArcType.Wire);
          this.m_HandleFunctionDistances.Add(new KeyValuePair<Action, float>(new Action(this.DrawXMinHandle), JointAngularLimitHandle.GetSortingDistance(this.m_XMinHandle)));
          this.m_HandleFunctionDistances.Add(new KeyValuePair<Action, float>(new Action(this.DrawXMaxHandle), JointAngularLimitHandle.GetSortingDistance(this.m_XMaxHandle)));
        }
      }
      using (new Handles.DrawingScope(this.m_SecondaryAxesMatrix))
      {
        if (flag2)
        {
          this.DrawArc(this.m_YMinHandle, this.m_YMaxHandle, this.yHandleColor, JointAngularLimitHandle.ArcType.Wire);
          this.m_HandleFunctionDistances.Add(new KeyValuePair<Action, float>(new Action(this.DrawYMinHandle), JointAngularLimitHandle.GetSortingDistance(this.m_YMinHandle)));
          this.m_HandleFunctionDistances.Add(new KeyValuePair<Action, float>(new Action(this.DrawYMaxHandle), JointAngularLimitHandle.GetSortingDistance(this.m_YMaxHandle)));
        }
        if (flag3)
        {
          using (new Handles.DrawingScope(Handles.matrix * JointAngularLimitHandle.s_ZHandleOffset))
          {
            this.DrawArc(this.m_ZMinHandle, this.m_ZMaxHandle, this.zHandleColor, JointAngularLimitHandle.ArcType.Wire);
            this.m_HandleFunctionDistances.Add(new KeyValuePair<Action, float>(new Action(this.DrawZMinHandle), JointAngularLimitHandle.GetSortingDistance(this.m_ZMinHandle)));
            this.m_HandleFunctionDistances.Add(new KeyValuePair<Action, float>(new Action(this.DrawZMaxHandle), JointAngularLimitHandle.GetSortingDistance(this.m_ZMaxHandle)));
          }
        }
      }
      List<KeyValuePair<Action, float>> functionDistances = this.m_HandleFunctionDistances;
      // ISSUE: reference to a compiler-generated field
      if (JointAngularLimitHandle.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        JointAngularLimitHandle.\u003C\u003Ef__mg\u0024cache0 = new Comparison<KeyValuePair<Action, float>>(JointAngularLimitHandle.CompareHandleFunctionsByDistance);
      }
      // ISSUE: reference to a compiler-generated field
      Comparison<KeyValuePair<Action, float>> fMgCache0 = JointAngularLimitHandle.\u003C\u003Ef__mg\u0024cache0;
      functionDistances.Sort(fMgCache0);
      foreach (KeyValuePair<Action, float> functionDistance in this.m_HandleFunctionDistances)
        functionDistance.Key();
    }

    private void DrawArc(ArcHandle minHandle, ArcHandle maxHandle, Color color, JointAngularLimitHandle.ArcType arcType)
    {
      float f = maxHandle.angle - minHandle.angle;
      Vector3 from = Quaternion.AngleAxis(minHandle.angle, Vector3.up) * Vector3.forward;
      using (new Handles.DrawingScope(Handles.color * color))
      {
        if (arcType == JointAngularLimitHandle.ArcType.Solid)
        {
          int num = 0;
          for (int index = (int) Mathf.Abs(f) / 360; num < index; ++num)
            Handles.DrawSolidArc(Vector3.zero, Vector3.up, Vector3.forward, 360f, this.radius);
          Handles.DrawSolidArc(Vector3.zero, Vector3.up, from, f % 360f, this.radius);
        }
        else
        {
          int num = 0;
          for (int index = (int) Mathf.Abs(f) / 360; num < index; ++num)
            Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, 360f, this.radius);
          Handles.DrawWireArc(Vector3.zero, Vector3.up, from, f % 360f, this.radius);
        }
      }
    }

    private void DrawMultiaxialFillShape()
    {
      Quaternion quaternion1 = Quaternion.AngleAxis(this.xMin, Vector3.left);
      Quaternion quaternion2 = Quaternion.AngleAxis(this.xMax, Vector3.left);
      Quaternion quaternion3 = Quaternion.AngleAxis(this.yMin, Vector3.up);
      Quaternion quaternion4 = Quaternion.AngleAxis(this.yMax, Vector3.up);
      Color color = new Color(1f, 1f, 1f, this.fillAlpha);
      using (new Handles.DrawingScope(Handles.color * (this.yHandleColor * color)))
      {
        float angle = this.yMax - this.yMin;
        Vector3 from1 = quaternion2 * quaternion4 * Vector3.forward;
        Handles.DrawSolidArc(Vector3.zero, quaternion2 * Vector3.down, from1, angle, this.radius);
        Vector3 from2 = quaternion1 * quaternion4 * Vector3.forward;
        Handles.DrawSolidArc(Vector3.zero, quaternion1 * Vector3.down, from2, angle, this.radius);
      }
      using (new Handles.DrawingScope(Handles.color * (this.xHandleColor * color)))
      {
        float angle = this.xMax - this.xMin;
        Handles.DrawSolidArc(Vector3.zero, Vector3.right, quaternion2 * quaternion4 * Vector3.forward, angle, this.radius);
        Handles.DrawSolidArc(Vector3.zero, Vector3.right, quaternion2 * quaternion3 * Vector3.forward, angle, this.radius);
      }
    }

    private void DrawXMinHandle()
    {
      using (new Handles.DrawingScope(Handles.matrix * JointAngularLimitHandle.s_XHandleOffset))
      {
        this.m_XMinHandle.DrawHandle();
        this.m_XMinHandle.angle = Mathf.Clamp(this.m_XMinHandle.angle, this.xRange.x, this.m_XMaxHandle.angle);
      }
    }

    private void DrawXMaxHandle()
    {
      using (new Handles.DrawingScope(Handles.matrix * JointAngularLimitHandle.s_XHandleOffset))
      {
        this.m_XMaxHandle.DrawHandle();
        this.m_XMaxHandle.angle = Mathf.Clamp(this.m_XMaxHandle.angle, this.m_XMinHandle.angle, this.xRange.y);
      }
    }

    private void DrawYMinHandle()
    {
      using (new Handles.DrawingScope(this.m_SecondaryAxesMatrix))
      {
        this.m_YMinHandle.DrawHandle();
        this.m_YMinHandle.angle = Mathf.Clamp(this.m_YMinHandle.angle, this.yRange.x, this.m_YMaxHandle.angle);
      }
    }

    private void DrawYMaxHandle()
    {
      using (new Handles.DrawingScope(this.m_SecondaryAxesMatrix))
      {
        this.m_YMaxHandle.DrawHandle();
        this.m_YMaxHandle.angle = Mathf.Clamp(this.m_YMaxHandle.angle, this.m_YMinHandle.angle, this.yRange.y);
      }
    }

    private void DrawZMinHandle()
    {
      using (new Handles.DrawingScope(this.m_SecondaryAxesMatrix * JointAngularLimitHandle.s_ZHandleOffset))
      {
        this.m_ZMinHandle.DrawHandle();
        this.m_ZMinHandle.angle = Mathf.Clamp(this.m_ZMinHandle.angle, this.zRange.x, this.m_ZMaxHandle.angle);
      }
    }

    private void DrawZMaxHandle()
    {
      using (new Handles.DrawingScope(this.m_SecondaryAxesMatrix * JointAngularLimitHandle.s_ZHandleOffset))
      {
        this.m_ZMaxHandle.DrawHandle();
        this.m_ZMaxHandle.angle = Mathf.Clamp(this.m_ZMaxHandle.angle, this.m_ZMinHandle.angle, this.zRange.y);
      }
    }

    private enum ArcType
    {
      Solid,
      Wire,
    }
  }
}
