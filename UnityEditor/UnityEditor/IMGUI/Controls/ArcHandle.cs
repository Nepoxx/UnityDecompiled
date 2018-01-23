// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.ArcHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  /// <summary>
  ///   <para>A class for a compound handle to edit an angle and a radius in the Scene view.</para>
  /// </summary>
  public class ArcHandle
  {
    private static readonly float s_DefaultAngleHandleSize = 0.08f;
    private static readonly float s_DefaultAngleHandleSizeRatio = 1.25f;
    private static readonly float s_DefaultRadiusHandleSize = 0.03f;
    private bool m_ControlIDsReserved = false;
    private int[] m_RadiusHandleControlIDs = new int[4];
    private Quaternion m_MostRecentValidAngleHandleOrientation = Quaternion.identity;
    private int m_AngleHandleControlID;

    /// <summary>
    ///   <para>Creates a new instance of the ArcHandle class.</para>
    /// </summary>
    public ArcHandle()
    {
      this.radius = 1f;
      this.SetColorWithoutRadiusHandle(Color.white, 0.1f);
    }

    private static float DefaultAngleHandleSizeFunction(Vector3 position)
    {
      return HandleUtility.GetHandleSize(position) * ArcHandle.s_DefaultAngleHandleSize;
    }

    private static float DefaultRadiusHandleSizeFunction(Vector3 position)
    {
      return HandleUtility.GetHandleSize(position) * ArcHandle.s_DefaultRadiusHandleSize;
    }

    private static void DefaultRadiusHandleDrawFunction(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      Handles.DotHandleCap(controlID, position, rotation, size, eventType);
    }

    /// <summary>
    ///   <para>Returns or specifies the angle of the arc for the handle.</para>
    /// </summary>
    public float angle { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the radius of the arc for the handle.</para>
    /// </summary>
    public float radius { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the color of the angle control handle.</para>
    /// </summary>
    public Color angleHandleColor { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the color of the radius control handle.</para>
    /// </summary>
    public Color radiusHandleColor { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the color of the arc shape.</para>
    /// </summary>
    public Color fillColor { get; set; }

    /// <summary>
    ///   <para>Returns or specifies the color of the curved line along the outside of the arc.</para>
    /// </summary>
    public Color wireframeColor { get; set; }

    /// <summary>
    ///   <para>An optional Handles.CapFunction to use when displaying the angle control handle. Defaults to a line terminated with Handles.CylinderHandleCap if no value is specified.</para>
    /// </summary>
    public Handles.CapFunction angleHandleDrawFunction { get; set; }

    /// <summary>
    ///   <para>An optional Handles.SizeFunction to specify how large the angle control handle should be. Defaults to a fixed screen-space size.</para>
    /// </summary>
    public Handles.SizeFunction angleHandleSizeFunction { get; set; }

    /// <summary>
    ///   <para>An optional Handles.CapFunction to use when displaying the radius control handle. Defaults to a Handles.DotHandleHandleCap along the arc if no value is specified.</para>
    /// </summary>
    public Handles.CapFunction radiusHandleDrawFunction { get; set; }

    /// <summary>
    ///   <para>An optional Handles.SizeFunction to specify how large the radius control handle should be. Defaults to a fixed screen-space size.</para>
    /// </summary>
    public Handles.SizeFunction radiusHandleSizeFunction { get; set; }

    /// <summary>
    ///   <para>Sets angleHandleColor, wireframeColor, and fillColor to the same value, where fillColor will have the specified alpha value. radiusHandleColor will be set to Color.clear and the radius handle will be disabled.</para>
    /// </summary>
    /// <param name="color">The color to use for the angle control handle and the fill shape.</param>
    /// <param name="fillColorAlpha">The alpha value to use for fillColor.</param>
    public void SetColorWithoutRadiusHandle(Color color, float fillColorAlpha)
    {
      this.SetColorWithRadiusHandle(color, fillColorAlpha);
      this.radiusHandleColor = Color.clear;
      this.wireframeColor = color;
    }

    /// <summary>
    ///   <para>Sets angleHandleColor, radiusHandleColor, wireframeColor, and fillColor to the same value, where fillColor will have the specified alpha value.</para>
    /// </summary>
    /// <param name="color">The color to use for the angle and radius control handles and the fill shape.</param>
    /// <param name="fillColorAlpha">The alpha value to use for fillColor.</param>
    public void SetColorWithRadiusHandle(Color color, float fillColorAlpha)
    {
      this.fillColor = color * new Color(1f, 1f, 1f, fillColorAlpha);
      this.angleHandleColor = color;
      this.radiusHandleColor = color;
      this.wireframeColor = color;
    }

    /// <summary>
    ///   <para>A function to display this instance in the current handle camera using its current configuration.</para>
    /// </summary>
    public void DrawHandle()
    {
      if (!this.m_ControlIDsReserved)
        this.GetControlIDs();
      this.m_ControlIDsReserved = false;
      if ((double) Handles.color.a == 0.0)
        return;
      Vector3 vector3_1 = Handles.matrix.MultiplyPoint3x4(Vector3.one) - Handles.matrix.MultiplyPoint3x4(Vector3.zero);
      if ((double) vector3_1.x == 0.0 && (double) vector3_1.z == 0.0)
        return;
      Vector3 vector3_2 = Quaternion.AngleAxis(this.angle, Vector3.up) * Vector3.forward * this.radius;
      float b = Mathf.Abs(this.angle);
      float angle = this.angle % 360f;
      using (new Handles.DrawingScope(Handles.color * this.fillColor))
      {
        if ((double) Handles.color.a > 0.0)
        {
          int num = 0;
          for (int index = (int) b / 360; num < index; ++num)
            Handles.DrawSolidArc(Vector3.zero, Vector3.up, Vector3.forward, 360f, this.radius);
          Handles.DrawSolidArc(Vector3.zero, Vector3.up, Vector3.forward, angle, this.radius);
        }
      }
      using (new Handles.DrawingScope(Handles.color * this.wireframeColor))
      {
        if ((double) Handles.color.a > 0.0)
          Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, (double) b < 360.0 ? angle : 360f, this.radius);
      }
      if (Event.current.alt)
      {
        bool flag = true;
        foreach (int radiusHandleControlId in this.m_RadiusHandleControlIDs)
        {
          if (radiusHandleControlId == GUIUtility.hotControl)
          {
            flag = false;
            break;
          }
        }
        if (flag && GUIUtility.hotControl != this.m_AngleHandleControlID)
          return;
      }
      using (new Handles.DrawingScope(Handles.color * this.radiusHandleColor))
      {
        if ((double) Handles.color.a > 0.0)
        {
          float num1 = Mathf.Sign(this.angle);
          int num2 = Mathf.Min(1 + (int) ((double) Mathf.Min(360f, b) * 0.0111111113801599), 4);
          for (int index = 0; index < num2; ++index)
          {
            using (new Handles.DrawingScope(Handles.matrix * Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis((float) index * 90f * num1, Vector3.up), Vector3.one)))
            {
              Vector3 position1 = Vector3.forward * this.radius;
              EditorGUI.BeginChangeCheck();
              float num3 = this.radiusHandleSizeFunction != null ? this.radiusHandleSizeFunction(position1) : ArcHandle.DefaultRadiusHandleSizeFunction(position1);
              int radiusHandleControlId = this.m_RadiusHandleControlIDs[index];
              Vector3 position2 = position1;
              Vector3 forward = Vector3.forward;
              double num4 = (double) num3;
              Handles.CapFunction capFunction = this.radiusHandleDrawFunction;
              if (capFunction == null)
              {
                // ISSUE: reference to a compiler-generated field
                if (ArcHandle.\u003C\u003Ef__mg\u0024cache0 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ArcHandle.\u003C\u003Ef__mg\u0024cache0 = new Handles.CapFunction(ArcHandle.DefaultRadiusHandleDrawFunction);
                }
                // ISSUE: reference to a compiler-generated field
                capFunction = ArcHandle.\u003C\u003Ef__mg\u0024cache0;
              }
              double z = (double) SnapSettings.move.z;
              Vector3 vector3_3 = Handles.Slider(radiusHandleControlId, position2, forward, (float) num4, capFunction, (float) z);
              if (EditorGUI.EndChangeCheck())
                this.radius += (vector3_3 - position1).z;
            }
          }
        }
      }
      using (new Handles.DrawingScope(Handles.color * this.angleHandleColor))
      {
        if ((double) Handles.color.a <= 0.0)
          return;
        EditorGUI.BeginChangeCheck();
        float handleSize = this.angleHandleSizeFunction != null ? this.angleHandleSizeFunction(vector3_2) : ArcHandle.DefaultAngleHandleSizeFunction(vector3_2);
        Vector3 vector3_3 = Handles.Slider2D(this.m_AngleHandleControlID, vector3_2, Vector3.up, Vector3.forward, Vector3.right, handleSize, this.angleHandleDrawFunction ?? new Handles.CapFunction(this.DefaultAngleHandleDrawFunction), Vector2.zero);
        if (EditorGUI.EndChangeCheck())
        {
          this.angle += Mathf.DeltaAngle(this.angle, Vector3.Angle(Vector3.forward, vector3_3) * Mathf.Sign(Vector3.Dot(Vector3.right, vector3_3)));
          this.angle = Handles.SnapValue(this.angle, SnapSettings.rotation);
        }
      }
    }

    internal void GetControlIDs()
    {
      this.m_AngleHandleControlID = GUIUtility.GetControlID(this.GetHashCode(), FocusType.Passive);
      for (int index = 0; index < this.m_RadiusHandleControlIDs.Length; ++index)
        this.m_RadiusHandleControlIDs[index] = GUIUtility.GetControlID(this.GetHashCode(), FocusType.Passive);
      this.m_ControlIDsReserved = true;
    }

    private void DefaultAngleHandleDrawFunction(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      Handles.DrawLine(Vector3.zero, position);
      Vector3 pos = Handles.matrix.MultiplyPoint3x4(position);
      Vector3 upwards = pos - Handles.matrix.MultiplyPoint3x4(Vector3.zero);
      Vector3 forward = Handles.matrix.MultiplyVector(Quaternion.AngleAxis(90f, Vector3.up) * position);
      this.m_MostRecentValidAngleHandleOrientation = rotation = (double) forward.sqrMagnitude != 0.0 ? Quaternion.LookRotation(forward, upwards) : this.m_MostRecentValidAngleHandleOrientation;
      using (new Handles.DrawingScope(Matrix4x4.TRS(pos, rotation, Vector3.one + Vector3.forward * ArcHandle.s_DefaultAngleHandleSizeRatio)))
        Handles.CylinderHandleCap(controlID, Vector3.zero, Quaternion.identity, size, eventType);
    }
  }
}
