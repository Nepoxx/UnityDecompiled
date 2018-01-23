// Decompiled with JetBrains decompiler
// Type: UnityEditor.Handles
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Custom 3D GUI controls and drawing in the scene view.</para>
  /// </summary>
  public sealed class Handles
  {
    internal static PrefColor s_XAxisColor = new PrefColor("Scene/X Axis", 0.8588235f, 0.2431373f, 0.1137255f, 0.93f);
    internal static PrefColor s_YAxisColor = new PrefColor("Scene/Y Axis", 0.6039216f, 0.9529412f, 0.282353f, 0.93f);
    internal static PrefColor s_ZAxisColor = new PrefColor("Scene/Z Axis", 0.227451f, 0.4784314f, 0.972549f, 0.93f);
    internal static PrefColor s_CenterColor = new PrefColor("Scene/Center Axis", 0.8f, 0.8f, 0.8f, 0.93f);
    internal static PrefColor s_SelectedColor = new PrefColor("Scene/Selected Axis", 0.9647059f, 0.9490196f, 0.1960784f, 0.89f);
    internal static PrefColor s_PreselectionColor = new PrefColor("Scene/Preselection Highlight", 0.7882353f, 0.7843137f, 0.5647059f, 0.89f);
    internal static PrefColor s_SecondaryColor = new PrefColor("Scene/Guide Line", 0.5f, 0.5f, 0.5f, 0.2f);
    internal static Color staticColor = new Color(0.5f, 0.5f, 0.5f, 0.0f);
    internal static float staticBlend = 0.6f;
    internal static float backfaceAlphaMultiplier = 0.2f;
    internal static Color s_ColliderHandleColor = new Color(145f, 244f, 139f, 210f) / (float) byte.MaxValue;
    internal static Color s_ColliderHandleColorDisabled = new Color(84f, 200f, 77f, 140f) / (float) byte.MaxValue;
    internal static Color s_BoundingBoxHandleColor = new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 150f) / (float) byte.MaxValue;
    internal static int s_SliderHash = "SliderHash".GetHashCode();
    internal static int s_Slider2DHash = "Slider2DHash".GetHashCode();
    internal static int s_FreeRotateHandleHash = "FreeRotateHandleHash".GetHashCode();
    internal static int s_RadiusHandleHash = "RadiusHandleHash".GetHashCode();
    internal static int s_xAxisMoveHandleHash = "xAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_yAxisMoveHandleHash = "yAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_zAxisMoveHandleHash = "zAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_FreeMoveHandleHash = "FreeMoveHandleHash".GetHashCode();
    internal static int s_xzAxisMoveHandleHash = "xzAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_xyAxisMoveHandleHash = "xyAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_yzAxisMoveHandleHash = "yzAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_xAxisScaleHandleHash = "xAxisScaleHandleHash".GetHashCode();
    internal static int s_yAxisScaleHandleHash = "yAxisScaleHandleHash".GetHashCode();
    internal static int s_zAxisScaleHandleHash = "zAxisScaleHandleHash".GetHashCode();
    internal static int s_ScaleSliderHash = "ScaleSliderHash".GetHashCode();
    internal static int s_ScaleValueHandleHash = "ScaleValueHandleHash".GetHashCode();
    internal static int s_DiscHash = "DiscHash".GetHashCode();
    internal static int s_ButtonHash = "ButtonHash".GetHashCode();
    private static Vector3[] s_RectangleCapPointsCache = new Vector3[5];
    private static readonly Vector3[] s_WireArcPoints = new Vector3[60];
    internal static int s_xRotateHandleHash = "xRotateHandleHash".GetHashCode();
    internal static int s_yRotateHandleHash = "yRotateHandleHash".GetHashCode();
    internal static int s_zRotateHandleHash = "zRotateHandleHash".GetHashCode();
    internal static int s_cameraAxisRotateHandleHash = "cameraAxisRotateHandleHash".GetHashCode();
    internal static int s_xyzRotateHandleHash = "xyzRotateHandleHash".GetHashCode();
    internal static int s_xScaleHandleHash = "xScaleHandleHash".GetHashCode();
    internal static int s_yScaleHandleHash = "yScaleHandleHash".GetHashCode();
    internal static int s_zScaleHandleHash = "zScaleHandleHash".GetHashCode();
    internal static int s_xyzScaleHandleHash = "xyzScaleHandleHash".GetHashCode();
    private static Color lineTransparency = new Color(1f, 1f, 1f, 0.75f);
    private static PrefColor[] s_AxisColor = new PrefColor[3]{ Handles.s_XAxisColor, Handles.s_YAxisColor, Handles.s_ZAxisColor };
    private static Vector3[] s_AxisVector = new Vector3[3]{ Vector3.right, Vector3.up, Vector3.forward };
    internal static Color s_DisabledHandleColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private static Vector3[] s_RectangleHandlePointsCache = new Vector3[5];
    private static Vector3[] verts = new Vector3[4]{ Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    private static bool s_FreeMoveMode = false;
    private static Vector3 s_PlanarHandlesOctant = Vector3.one;
    private static Vector3 s_DoPositionHandle_AxisHandlesOctant = Vector3.one;
    private static Vector3 s_DoPositionHandle_ArrowCapConeOffset = Vector3.zero;
    private static float[] s_DoPositionHandle_Internal_CameraViewLerp = new float[6];
    private static string[] s_DoPositionHandle_Internal_AxisNames = new string[3]{ "xAxis", "yAxis", "zAxis" };
    private static int[] s_DoPositionHandle_Internal_NextIndex = new int[3]{ 1, 2, 0 };
    private static int[] s_DoPositionHandle_Internal_PrevIndex = new int[3]{ 2, 0, 1 };
    private static int[] s_DoPositionHandle_Internal_PrevPlaneIndex = new int[3]{ 5, 3, 4 };
    private static readonly Color k_RotationPieColor = new Color(0.9647059f, 0.9490196f, 0.1960784f, 0.89f);
    private static Vector3 s_DoScaleHandle_AxisHandlesOctant = Vector3.one;
    private static bool s_IsHotInCameraAlignedMode = false;
    private static Dictionary<Handles.RotationHandleIds, Handles.RotationHandleData> s_TransformHandle_RotationData = new Dictionary<Handles.RotationHandleIds, Handles.RotationHandleData>();
    private const int kMaxDottedLineVertices = 1000;
    internal static Mesh s_CubeMesh;
    internal static Mesh s_SphereMesh;
    internal static Mesh s_ConeMesh;
    internal static Mesh s_CylinderMesh;
    internal static Mesh s_QuadMesh;
    internal const float kCameraViewLerpStart = 0.85f;
    internal const float kCameraViewThreshold = 0.9f;
    internal const float kCameraViewLerpSpeed = 6.666668f;
    private const float k_BoneThickness = 0.08f;
    private const float kFreeMoveHandleSizeFactor = 0.15f;

    /// <summary>
    ///   <para>Color to use for handles that manipulates the X coordinate of something.</para>
    /// </summary>
    public static Color xAxisColor
    {
      get
      {
        return (Color) Handles.s_XAxisColor;
      }
    }

    /// <summary>
    ///   <para>Color to use for handles that manipulates the Y coordinate of something.</para>
    /// </summary>
    public static Color yAxisColor
    {
      get
      {
        return (Color) Handles.s_YAxisColor;
      }
    }

    /// <summary>
    ///   <para>Color to use for handles that manipulates the Z coordinate of something.</para>
    /// </summary>
    public static Color zAxisColor
    {
      get
      {
        return (Color) Handles.s_ZAxisColor;
      }
    }

    /// <summary>
    ///   <para>Color to use for handles that represent the center of something.</para>
    /// </summary>
    public static Color centerColor
    {
      get
      {
        return (Color) Handles.s_CenterColor;
      }
    }

    /// <summary>
    ///   <para>Color to use for the currently active handle.</para>
    /// </summary>
    public static Color selectedColor
    {
      get
      {
        return (Color) Handles.s_SelectedColor;
      }
    }

    /// <summary>
    ///   <para>Color to use to highlight an unselected handle currently under the mouse pointer.</para>
    /// </summary>
    public static Color preselectionColor
    {
      get
      {
        return (Color) Handles.s_PreselectionColor;
      }
    }

    /// <summary>
    ///   <para>Soft color to use for for general things.</para>
    /// </summary>
    public static Color secondaryColor
    {
      get
      {
        return (Color) Handles.s_SecondaryColor;
      }
    }

    /// <summary>
    ///   <para>Are handles lit?</para>
    /// </summary>
    public static extern bool lighting { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Colors of the handles.</para>
    /// </summary>
    public static Color color
    {
      get
      {
        Color color;
        Handles.INTERNAL_get_color(out color);
        return color;
      }
      set
      {
        Handles.INTERNAL_set_color(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_color(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_color(ref Color value);

    /// <summary>
    ///   <para>zTest of the handles.</para>
    /// </summary>
    public static extern CompareFunction zTest { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Matrix for all handle operations.</para>
    /// </summary>
    public static Matrix4x4 matrix
    {
      get
      {
        Matrix4x4 matrix4x4;
        Handles.INTERNAL_get_matrix(out matrix4x4);
        return matrix4x4;
      }
      set
      {
        Handles.INTERNAL_set_matrix(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_matrix(out Matrix4x4 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_matrix(ref Matrix4x4 value);

    /// <summary>
    ///   <para>The inverse of the matrix for all handle operations.</para>
    /// </summary>
    public static Matrix4x4 inverseMatrix
    {
      get
      {
        Matrix4x4 matrix4x4;
        Handles.INTERNAL_get_inverseMatrix(out matrix4x4);
        return matrix4x4;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_inverseMatrix(out Matrix4x4 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ClearHandles();

    /// <summary>
    ///   <para>Make a position handle.</para>
    /// </summary>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <param name="rotation">Orientation of the handle in 3D space.</param>
    /// <returns>
    ///   <para>The new value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.</para>
    /// </returns>
    public static Vector3 PositionHandle(Vector3 position, Quaternion rotation)
    {
      return Handles.DoPositionHandle(position, rotation);
    }

    /// <summary>
    ///   <para>Make a Scene view rotation handle.</para>
    /// </summary>
    /// <param name="rotation">Orientation of the handle.</param>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <returns>
    ///   <para>The new rotation value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.</para>
    /// </returns>
    public static Quaternion RotationHandle(Quaternion rotation, Vector3 position)
    {
      return Handles.DoRotationHandle(rotation, position);
    }

    /// <summary>
    ///         <para>Make a Scene view scale handle.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </summary>
    /// <param name="scale">Scale to modify.</param>
    /// <param name="position">The position of the handle.</param>
    /// <param name="rotation">The rotation of the handle.</param>
    /// <param name="size">Allows you to scale the size of the handle on-scren.</param>
    /// <returns>
    ///   <para>The new value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.</para>
    /// </returns>
    public static Vector3 ScaleHandle(Vector3 scale, Vector3 position, Quaternion rotation, float size)
    {
      return Handles.DoScaleHandle(scale, position, rotation, size);
    }

    /// <summary>
    ///   <para>Make a Scene view radius handle.</para>
    /// </summary>
    /// <param name="rotation">Orientation of the handle.</param>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <param name="radius">Radius to modify.</param>
    /// <param name="handlesOnly">Whether to omit the circular outline of the radius and only draw the point handles.</param>
    /// <returns>
    ///         <para>The new value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </returns>
    public static float RadiusHandle(Quaternion rotation, Vector3 position, float radius, bool handlesOnly)
    {
      return Handles.DoRadiusHandle(rotation, position, radius, handlesOnly);
    }

    /// <summary>
    ///   <para>Make a Scene view radius handle.</para>
    /// </summary>
    /// <param name="rotation">Orientation of the handle.</param>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <param name="radius">Radius to modify.</param>
    /// <param name="handlesOnly">Whether to omit the circular outline of the radius and only draw the point handles.</param>
    /// <returns>
    ///         <para>The new value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </returns>
    public static float RadiusHandle(Quaternion rotation, Vector3 position, float radius)
    {
      return Handles.DoRadiusHandle(rotation, position, radius, false);
    }

    internal static Vector2 ConeHandle(Quaternion rotation, Vector3 position, Vector2 angleAndRange, float angleScale, float rangeScale, bool handlesOnly)
    {
      return Handles.DoConeHandle(rotation, position, angleAndRange, angleScale, rangeScale, handlesOnly);
    }

    internal static Vector3 ConeFrustrumHandle(Quaternion rotation, Vector3 position, Vector3 radiusAngleRange)
    {
      return Handles.DoConeFrustrumHandle(rotation, position, radiusAngleRange);
    }

    [ExcludeFromDocs]
    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, Vector2 snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, capFunction, snap, drawHelper);
    }

    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, Vector2 snap, [DefaultValue("false")] bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, capFunction, snap, drawHelper);
    }

    [ExcludeFromDocs]
    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, [DefaultValue("false")] bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [ExcludeFromDocs]
    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, Vector2 snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(handlePos, handleDir, slideDir1, slideDir2, handleSize, capFunction, snap, drawHelper);
    }

    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, Vector2 snap, [DefaultValue("false")] bool drawHelper)
    {
      return Slider2D.Do(GUIUtility.GetControlID(Handles.s_Slider2DHash, FocusType.Passive), handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, capFunction, snap, drawHelper);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    [ExcludeFromDocs]
    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(handlePos, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, [DefaultValue("false")] bool drawHelper)
    {
      return Slider2D.Do(GUIUtility.GetControlID(Handles.s_Slider2DHash, FocusType.Passive), handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [ExcludeFromDocs]
    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, Vector2 snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(id, handlePos, handleDir, slideDir1, slideDir2, handleSize, capFunction, snap, drawHelper);
    }

    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, Vector2 snap, [DefaultValue("false")] bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, capFunction, snap, drawHelper);
    }

    [ExcludeFromDocs]
    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(id, handlePos, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, [DefaultValue("false")] bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [ExcludeFromDocs]
    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, float snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(handlePos, handleDir, slideDir1, slideDir2, handleSize, capFunction, snap, drawHelper);
    }

    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, float snap, [DefaultValue("false")] bool drawHelper)
    {
      return Handles.Slider2D(GUIUtility.GetControlID(Handles.s_Slider2DHash, FocusType.Passive), handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, capFunction, new Vector2(snap, snap), drawHelper);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    [ExcludeFromDocs]
    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, float snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(handlePos, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, float snap, [DefaultValue("false")] bool drawHelper)
    {
      return Handles.Slider2D(GUIUtility.GetControlID(Handles.s_Slider2DHash, FocusType.Passive), handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, drawFunc, new Vector2(snap, snap), drawHelper);
    }

    /// <summary>
    ///   <para>Make an unconstrained rotation handle.</para>
    /// </summary>
    /// <param name="id">Control id of the handle.</param>
    /// <param name="rotation">Orientation of the handle.</param>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <param name="size">The size of the handle.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <returns>
    ///   <para>The new rotation value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.</para>
    /// </returns>
    public static Quaternion FreeRotateHandle(Quaternion rotation, Vector3 position, float size)
    {
      return FreeRotate.Do(GUIUtility.GetControlID(Handles.s_FreeRotateHandleHash, FocusType.Passive), rotation, position, size);
    }

    /// <summary>
    ///   <para>Make a directional scale slider.</para>
    /// </summary>
    /// <param name="scale">The value the user can modify.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="direction">The direction of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    /// <param name="snap">The snap increment. See Handles.SnapValue.</param>
    /// <returns>
    ///   <para>The new value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.</para>
    /// </returns>
    public static float ScaleSlider(float scale, Vector3 position, Vector3 direction, Quaternion rotation, float size, float snap)
    {
      return SliderScale.DoAxis(GUIUtility.GetControlID(Handles.s_ScaleSliderHash, FocusType.Passive), scale, position, direction, rotation, size, snap);
    }

    /// <summary>
    ///         <para>Make a 3D disc that can be dragged with the mouse.
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </summary>
    /// <param name="id">Control id of the handle.</param>
    /// <param name="rotation">The rotation of the disc.</param>
    /// <param name="position">The center of the disc.</param>
    /// <param name="axis">The axis to rotate around.</param>
    /// <param name="size">The size of the disc in world space See Also:HandleUtility.GetHandleSize.</param>
    /// <param name="cutoffPlane">If true, only the front-facing half of the circle is draw / draggable. This is useful when you have many overlapping rotation axes (like in the default rotate tool) to avoid clutter.</param>
    /// <param name="snap">The grid size to snap to.</param>
    /// <returns>
    ///   <para>The new rotation value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.</para>
    /// </returns>
    public static Quaternion Disc(Quaternion rotation, Vector3 position, Vector3 axis, float size, bool cutoffPlane, float snap)
    {
      return Disc.Do(GUIUtility.GetControlID(Handles.s_DiscHash, FocusType.Passive), rotation, position, axis, size, cutoffPlane, snap);
    }

    internal static void SetupIgnoreRaySnapObjects()
    {
      HandleUtility.ignoreRaySnapObjects = Selection.GetTransforms(SelectionMode.Deep | SelectionMode.Editable);
    }

    /// <summary>
    ///   <para>Rounds the value val to the closest multiple of snap (snap can only be positive).</para>
    /// </summary>
    /// <param name="val"></param>
    /// <param name="snap"></param>
    /// <returns>
    ///   <para>The rounded value, if snap is positive, and val otherwise.</para>
    /// </returns>
    public static float SnapValue(float val, float snap)
    {
      if (EditorGUI.actionKey && (double) snap > 0.0)
        return Mathf.Round(val / snap) * snap;
      return val;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_IsCameraDrawModeEnabled(Camera camera, int drawMode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_DrawCameraWithGrid(Camera cam, int renderMode, ref DrawGridParameters gridParam);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_DrawCamera(Camera cam, int renderMode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_FinishDrawingCamera(Camera cam, [DefaultValue("true")] bool drawGizmos);

    [ExcludeFromDocs]
    private static void Internal_FinishDrawingCamera(Camera cam)
    {
      bool drawGizmos = true;
      Handles.Internal_FinishDrawingCamera(cam, drawGizmos);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_ClearCamera(Camera cam);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_SetCurrentCamera(Camera cam);

    internal static void SetSceneViewColors(Color wire, Color wireOverlay, Color selectedOutline, Color selectedWire)
    {
      Handles.INTERNAL_CALL_SetSceneViewColors(ref wire, ref wireOverlay, ref selectedOutline, ref selectedWire);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetSceneViewColors(ref Color wire, ref Color wireOverlay, ref Color selectedOutline, ref Color selectedWire);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void EnableCameraFx(Camera cam, bool fx);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void EnableCameraFlares(Camera cam, bool flares);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void EnableCameraSkybox(Camera cam, bool skybox);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetCameraOnlyDrawMesh(Camera cam);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetupCamera(Camera cam);

    /// <summary>
    ///   <para>Setup viewport and stuff for a current camera.</para>
    /// </summary>
    public Camera currentCamera
    {
      get
      {
        return Camera.current;
      }
      set
      {
        Handles.Internal_SetCurrentCamera(value);
      }
    }

    internal static Color realHandleColor
    {
      get
      {
        return Handles.color * new Color(1f, 1f, 1f, 0.5f) + (!Handles.lighting ? new Color(0.0f, 0.0f, 0.0f, 0.0f) : new Color(0.0f, 0.0f, 0.0f, 0.5f));
      }
    }

    internal static void DrawTwoShadedWireDisc(Vector3 position, Vector3 axis, float radius)
    {
      Color color1 = Handles.color;
      Color color2 = color1;
      color1.a *= Handles.backfaceAlphaMultiplier;
      Handles.color = color1;
      Handles.DrawWireDisc(position, axis, radius);
      Handles.color = color2;
    }

    internal static void DrawTwoShadedWireDisc(Vector3 position, Vector3 axis, Vector3 from, float degrees, float radius)
    {
      Handles.DrawWireArc(position, axis, from, degrees, radius);
      Color color1 = Handles.color;
      Color color2 = color1;
      color1.a *= Handles.backfaceAlphaMultiplier;
      Handles.color = color1;
      Handles.DrawWireArc(position, axis, from, degrees - 360f, radius);
      Handles.color = color2;
    }

    internal static Matrix4x4 StartCapDraw(Vector3 position, Quaternion rotation, float size)
    {
      Shader.SetGlobalColor("_HandleColor", Handles.realHandleColor);
      Shader.SetGlobalFloat("_HandleSize", size);
      Matrix4x4 matrix4x4 = Handles.matrix * Matrix4x4.TRS(position, rotation, Vector3.one);
      Shader.SetGlobalMatrix("_ObjectToWorld", matrix4x4);
      HandleUtility.handleMaterial.SetInt("_HandleZTest", (int) Handles.zTest);
      HandleUtility.handleMaterial.SetPass(0);
      return matrix4x4;
    }

    [Obsolete("Use CubeHandleCap instead")]
    public static void CubeCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Graphics.DrawMeshNow(Handles.cubeMesh, Handles.StartCapDraw(position, rotation, size));
    }

    [Obsolete("Use SphereHandleCap instead")]
    public static void SphereCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Graphics.DrawMeshNow(Handles.sphereMesh, Handles.StartCapDraw(position, rotation, size));
    }

    [Obsolete("Use ConeHandleCap instead")]
    public static void ConeCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Graphics.DrawMeshNow(Handles.coneMesh, Handles.StartCapDraw(position, rotation, size));
    }

    [Obsolete("Use CylinderHandleCap instead")]
    public static void CylinderCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Graphics.DrawMeshNow(Handles.cylinderMesh, Handles.StartCapDraw(position, rotation, size));
    }

    [Obsolete("Use RectangleHandleCap instead")]
    public static void RectangleCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.RectangleCap(controlID, position, rotation, new Vector2(size, size));
    }

    internal static void RectangleCap(int controlID, Vector3 position, Quaternion rotation, Vector2 size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Vector3 vector3_1 = rotation * new Vector3(size.x, 0.0f, 0.0f);
      Vector3 vector3_2 = rotation * new Vector3(0.0f, size.y, 0.0f);
      Handles.s_RectangleCapPointsCache[0] = position + vector3_1 + vector3_2;
      Handles.s_RectangleCapPointsCache[1] = position + vector3_1 - vector3_2;
      Handles.s_RectangleCapPointsCache[2] = position - vector3_1 - vector3_2;
      Handles.s_RectangleCapPointsCache[3] = position - vector3_1 + vector3_2;
      Handles.s_RectangleCapPointsCache[4] = position + vector3_1 + vector3_2;
      Handles.DrawPolyLine(Handles.s_RectangleCapPointsCache);
    }

    public static void SelectionFrame(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Handles.StartCapDraw(position, rotation, size);
      Vector3 vector3_1 = rotation * new Vector3(size, 0.0f, 0.0f);
      Vector3 vector3_2 = rotation * new Vector3(0.0f, size, 0.0f);
      Vector3 vector3_3 = position - vector3_1 + vector3_2;
      Vector3 vector3_4 = position + vector3_1 + vector3_2;
      Vector3 vector3_5 = position + vector3_1 - vector3_2;
      Vector3 vector3_6 = position - vector3_1 - vector3_2;
      Handles.DrawLine(vector3_3, vector3_4);
      Handles.DrawLine(vector3_4, vector3_5);
      Handles.DrawLine(vector3_5, vector3_6);
      Handles.DrawLine(vector3_6, vector3_3);
    }

    [Obsolete("Use DotHandleCap instead")]
    public static void DotCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      position = Handles.matrix.MultiplyPoint(position);
      Vector3 vector3_1 = Camera.current.transform.right * size;
      Vector3 vector3_2 = Camera.current.transform.up * size;
      Color c = Handles.color * new Color(1f, 1f, 1f, 0.99f);
      HandleUtility.ApplyWireMaterial(Handles.zTest);
      GL.Begin(7);
      GL.Color(c);
      GL.Vertex(position + vector3_1 + vector3_2);
      GL.Vertex(position + vector3_1 - vector3_2);
      GL.Vertex(position - vector3_1 - vector3_2);
      GL.Vertex(position - vector3_1 + vector3_2);
      GL.End();
    }

    [Obsolete("Use CircleHandleCap instead")]
    public static void CircleCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Handles.StartCapDraw(position, rotation, size);
      Vector3 normal = rotation * new Vector3(0.0f, 0.0f, 1f);
      Handles.DrawWireDisc(position, normal, size);
    }

    [Obsolete("Use ArrowHandleCap instead")]
    public static void ArrowCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Vector3 forward = rotation * Vector3.forward;
      Handles.ConeCap(controlID, position + forward * size, Quaternion.LookRotation(forward), size * 0.2f);
      Handles.DrawLine(position, position + forward * size * 0.9f);
    }

    [Obsolete("DrawCylinder has been renamed to CylinderCap.")]
    public static void DrawCylinder(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.CylinderCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawSphere has been renamed to SphereCap.")]
    public static void DrawSphere(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.SphereCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawRectangle has been renamed to RectangleCap.")]
    public static void DrawRectangle(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.RectangleCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawCube has been renamed to CubeCap.")]
    public static void DrawCube(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.CubeCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawArrow has been renamed to ArrowCap.")]
    public static void DrawArrow(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.ArrowCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawCone has been renamed to ConeCap.")]
    public static void DrawCone(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.ConeCap(controlID, position, rotation, size);
    }

    internal static void DrawAAPolyLine(Color[] colors, Vector3[] points)
    {
      Handles.DoDrawAAPolyLine(colors, points, -1, (Texture2D) null, 2f, 0.75f);
    }

    internal static void DrawAAPolyLine(float width, Color[] colors, Vector3[] points)
    {
      Handles.DoDrawAAPolyLine(colors, points, -1, (Texture2D) null, width, 0.75f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, -1, (Texture2D) null, 2f, 0.75f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(float width, params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, -1, (Texture2D) null, width, 0.75f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(Texture2D lineTex, params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, -1, lineTex, (float) (lineTex.height / 2), 0.99f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(float width, int actualNumberOfPoints, params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, actualNumberOfPoints, (Texture2D) null, width, 0.75f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(Texture2D lineTex, float width, params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, -1, lineTex, width, 0.99f);
    }

    private static void DoDrawAAPolyLine(Color[] colors, Vector3[] points, int actualNumberOfPoints, Texture2D lineTex, float width, float alpha)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial(Handles.zTest);
      Color defaultColor = new Color(1f, 1f, 1f, alpha);
      if (colors != null)
      {
        for (int index = 0; index < colors.Length; ++index)
          colors[index] *= defaultColor;
      }
      else
        defaultColor *= Handles.color;
      Handles.Internal_DrawAAPolyLine(colors, points, defaultColor, actualNumberOfPoints, lineTex, width, Handles.matrix);
    }

    private static void Internal_DrawAAPolyLine(Color[] colors, Vector3[] points, Color defaultColor, int actualNumberOfPoints, Texture2D texture, float width, Matrix4x4 toWorld)
    {
      Handles.INTERNAL_CALL_Internal_DrawAAPolyLine(colors, points, ref defaultColor, actualNumberOfPoints, texture, width, ref toWorld);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawAAPolyLine(Color[] colors, Vector3[] points, ref Color defaultColor, int actualNumberOfPoints, Texture2D texture, float width, ref Matrix4x4 toWorld);

    /// <summary>
    ///   <para>Draw anti-aliased convex polygon specified with point array.</para>
    /// </summary>
    /// <param name="points">List of points describing the convex polygon.</param>
    public static void DrawAAConvexPolygon(params Vector3[] points)
    {
      Handles.DoDrawAAConvexPolygon(points, -1, 1f);
    }

    private static void DoDrawAAConvexPolygon(Vector3[] points, int actualNumberOfPoints, float alpha)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial(Handles.zTest);
      Color defaultColor = new Color(1f, 1f, 1f, alpha) * Handles.color;
      Handles.Internal_DrawAAConvexPolygon(points, defaultColor, actualNumberOfPoints, Handles.matrix);
    }

    private static void Internal_DrawAAConvexPolygon(Vector3[] points, Color defaultColor, int actualNumberOfPoints, Matrix4x4 toWorld)
    {
      Handles.INTERNAL_CALL_Internal_DrawAAConvexPolygon(points, ref defaultColor, actualNumberOfPoints, ref toWorld);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawAAConvexPolygon(Vector3[] points, ref Color defaultColor, int actualNumberOfPoints, ref Matrix4x4 toWorld);

    /// <summary>
    ///   <para>Draw textured bezier line through start and end points with the given tangents.  To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.  The bezier curve will be swept using this texture.</para>
    /// </summary>
    /// <param name="startPosition">The start point of the bezier line.</param>
    /// <param name="endPosition">The end point of the bezier line.</param>
    /// <param name="startTangent">The start tangent of the bezier line.</param>
    /// <param name="endTangent">The end tangent of the bezier line.</param>
    /// <param name="color">The color to use for the bezier line.</param>
    /// <param name="texture">The texture to use for drawing the bezier line.</param>
    /// <param name="width">The width of the bezier line.</param>
    public static void DrawBezier(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, Color color, Texture2D texture, float width)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial(Handles.zTest);
      Handles.Internal_DrawBezier(startPosition, endPosition, startTangent, endTangent, color, texture, width, Handles.matrix);
    }

    private static void Internal_DrawBezier(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, Color color, Texture2D texture, float width, Matrix4x4 toWorld)
    {
      Handles.INTERNAL_CALL_Internal_DrawBezier(ref startPosition, ref endPosition, ref startTangent, ref endTangent, ref color, texture, width, ref toWorld);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawBezier(ref Vector3 startPosition, ref Vector3 endPosition, ref Vector3 startTangent, ref Vector3 endTangent, ref Color color, Texture2D texture, float width, ref Matrix4x4 toWorld);

    /// <summary>
    ///   <para>Draw the outline of a flat disc in 3D space.</para>
    /// </summary>
    /// <param name="center">The center of the dics.</param>
    /// <param name="normal">The normal of the disc.</param>
    /// <param name="radius">The radius of the dics
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void DrawWireDisc(Vector3 center, Vector3 normal, float radius)
    {
      Vector3 from = Vector3.Cross(normal, Vector3.up);
      if ((double) from.sqrMagnitude < 1.0 / 1000.0)
        from = Vector3.Cross(normal, Vector3.right);
      Handles.DrawWireArc(center, normal, from, 360f, radius);
    }

    /// <summary>
    ///   <para>Draw a circular arc in 3D space.</para>
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="normal">The normal of the circle.</param>
    /// <param name="from">The direction of the point on the circle circumference, relative to the center, where the arc begins.</param>
    /// <param name="angle">The angle of the arc, in degrees.</param>
    /// <param name="radius">The radius of the circle
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void DrawWireArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
      Handles.SetDiscSectionPoints(Handles.s_WireArcPoints, center, normal, from, angle, radius);
      Handles.DrawPolyLine(Handles.s_WireArcPoints);
    }

    public static void DrawSolidRectangleWithOutline(Rect rectangle, Color faceColor, Color outlineColor)
    {
      Handles.DrawSolidRectangleWithOutline(new Vector3[4]
      {
        new Vector3(rectangle.xMin, rectangle.yMin, 0.0f),
        new Vector3(rectangle.xMax, rectangle.yMin, 0.0f),
        new Vector3(rectangle.xMax, rectangle.yMax, 0.0f),
        new Vector3(rectangle.xMin, rectangle.yMax, 0.0f)
      }, faceColor, outlineColor);
    }

    /// <summary>
    ///   <para>Draw a solid outlined rectangle in 3D space.</para>
    /// </summary>
    /// <param name="verts">The 4 vertices of the rectangle in world coordinates.</param>
    /// <param name="faceColor">The color of the rectangle's face.</param>
    /// <param name="outlineColor">The outline color of the rectangle.</param>
    public static void DrawSolidRectangleWithOutline(Vector3[] verts, Color faceColor, Color outlineColor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial(Handles.zTest);
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      if ((double) faceColor.a > 0.0)
      {
        Color c = faceColor * Handles.color;
        GL.Begin(4);
        for (int index = 0; index < 2; ++index)
        {
          GL.Color(c);
          GL.Vertex(verts[index * 2]);
          GL.Vertex(verts[index * 2 + 1]);
          GL.Vertex(verts[(index * 2 + 2) % 4]);
          GL.Vertex(verts[index * 2]);
          GL.Vertex(verts[(index * 2 + 2) % 4]);
          GL.Vertex(verts[index * 2 + 1]);
        }
        GL.End();
      }
      if ((double) outlineColor.a > 0.0)
      {
        Color c = outlineColor * Handles.color;
        GL.Begin(1);
        GL.Color(c);
        for (int index = 0; index < 4; ++index)
        {
          GL.Vertex(verts[index]);
          GL.Vertex(verts[(index + 1) % 4]);
        }
        GL.End();
      }
      GL.PopMatrix();
    }

    /// <summary>
    ///   <para>Draw a solid flat disc in 3D space.</para>
    /// </summary>
    /// <param name="center">The center of the dics.</param>
    /// <param name="normal">The normal of the disc.</param>
    /// <param name="radius">The radius of the dics
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void DrawSolidDisc(Vector3 center, Vector3 normal, float radius)
    {
      Vector3 from = Vector3.Cross(normal, Vector3.up);
      if ((double) from.sqrMagnitude < 1.0 / 1000.0)
        from = Vector3.Cross(normal, Vector3.right);
      Handles.DrawSolidArc(center, normal, from, 360f, radius);
    }

    /// <summary>
    ///   <para>Draw a circular sector (pie piece) in 3D space.</para>
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="normal">The normal of the circle.</param>
    /// <param name="from">The direction of the point on the circumference, relative to the center, where the sector begins.</param>
    /// <param name="angle">The angle of the sector, in degrees.</param>
    /// <param name="radius">The radius of the circle
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void DrawSolidArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Handles.SetDiscSectionPoints(Handles.s_WireArcPoints, center, normal, from, angle, radius);
      Shader.SetGlobalColor("_HandleColor", Handles.color * new Color(1f, 1f, 1f, 0.5f));
      Shader.SetGlobalFloat("_HandleSize", 1f);
      HandleUtility.ApplyWireMaterial(Handles.zTest);
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      GL.Begin(4);
      int index = 1;
      for (int length = Handles.s_WireArcPoints.Length; index < length; ++index)
      {
        GL.Color(Handles.color);
        GL.Vertex(center);
        GL.Vertex(Handles.s_WireArcPoints[index - 1]);
        GL.Vertex(Handles.s_WireArcPoints[index]);
        GL.Vertex(center);
        GL.Vertex(Handles.s_WireArcPoints[index]);
        GL.Vertex(Handles.s_WireArcPoints[index - 1]);
      }
      GL.End();
      GL.PopMatrix();
    }

    internal static void SetDiscSectionPoints(Vector3[] dest, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
      Handles.INTERNAL_CALL_SetDiscSectionPoints(dest, ref center, ref normal, ref from, angle, radius);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetDiscSectionPoints(Vector3[] dest, ref Vector3 center, ref Vector3 normal, ref Vector3 from, float angle, float radius);

    internal static void Init()
    {
      if ((bool) ((UnityEngine.Object) Handles.s_CubeMesh))
        return;
      GameObject gameObject = (GameObject) EditorGUIUtility.Load("SceneView/HandlesGO.fbx");
      if (!(bool) ((UnityEngine.Object) gameObject))
        Debug.Log((object) "Couldn't find SceneView/HandlesGO.fbx");
      gameObject.SetActive(false);
      IEnumerator enumerator = gameObject.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          MeshFilter component = current.GetComponent<MeshFilter>();
          switch (current.name)
          {
            case "Cube":
              Handles.s_CubeMesh = component.sharedMesh;
              break;
            case "Sphere":
              Handles.s_SphereMesh = component.sharedMesh;
              break;
            case "Cone":
              Handles.s_ConeMesh = component.sharedMesh;
              break;
            case "Cylinder":
              Handles.s_CylinderMesh = component.sharedMesh;
              break;
            case "Quad":
              Handles.s_QuadMesh = component.sharedMesh;
              break;
          }
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      if (Application.platform == RuntimePlatform.WindowsEditor)
      {
        Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande.ttf"));
        Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande Bold.ttf"));
        Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande Small.ttf"));
        Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande Small Bold.ttf"));
        Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande Big.ttf"));
      }
    }

    private static void ReplaceFontForWindows(Font font)
    {
      if (font.name.Contains("Bold"))
        font.fontNames = new string[2]
        {
          "Verdana Bold",
          "Tahoma Bold"
        };
      else
        font.fontNames = new string[2]
        {
          "Verdana",
          "Tahoma"
        };
      font.hideFlags = HideFlags.HideAndDontSave;
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, string text)
    {
      Handles.Label(position, EditorGUIUtility.TempContent(text), GUI.skin.label);
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, Texture image)
    {
      Handles.Label(position, EditorGUIUtility.TempContent(image), GUI.skin.label);
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, GUIContent content)
    {
      Handles.Label(position, content, GUI.skin.label);
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, string text, GUIStyle style)
    {
      Handles.Label(position, EditorGUIUtility.TempContent(text), style);
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, GUIContent content, GUIStyle style)
    {
      Handles.BeginGUI();
      GUI.Label(HandleUtility.WorldPointToSizedRect(position, content, style), content, style);
      Handles.EndGUI();
    }

    internal static Rect GetCameraRect(Rect position)
    {
      Rect rect = GUIClip.Unclip(position);
      return new Rect(rect.xMin, (float) Screen.height - rect.yMax, rect.width, rect.height);
    }

    /// <summary>
    ///   <para>Get the width and height of the main game view.</para>
    /// </summary>
    public static Vector2 GetMainGameViewSize()
    {
      return GameView.GetMainGameViewTargetSize();
    }

    internal static bool IsCameraDrawModeEnabled(Camera camera, DrawCameraMode drawMode)
    {
      return Handles.Internal_IsCameraDrawModeEnabled(camera, (int) drawMode);
    }

    /// <summary>
    ///   <para>Clears the camera.</para>
    /// </summary>
    /// <param name="position">Where in the Scene to clear.</param>
    /// <param name="camera">The camera to clear.</param>
    public static void ClearCamera(Rect position, Camera camera)
    {
      Event current = Event.current;
      if ((UnityEngine.Object) camera.targetTexture == (UnityEngine.Object) null)
      {
        Rect pixels = EditorGUIUtility.PointsToPixels(GUIClip.Unclip(position));
        Rect rect = new Rect(pixels.xMin, (float) Screen.height - pixels.yMax, pixels.width, pixels.height);
        camera.pixelRect = rect;
      }
      else
        camera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
      if (current.type == EventType.Repaint)
        Handles.Internal_ClearCamera(camera);
      else
        Handles.Internal_SetCurrentCamera(camera);
    }

    [ExcludeFromDocs]
    internal static void DrawCameraImpl(Rect position, Camera camera, DrawCameraMode drawMode, bool drawGrid, DrawGridParameters gridParam, bool finish)
    {
      bool renderGizmos = true;
      Handles.DrawCameraImpl(position, camera, drawMode, drawGrid, gridParam, finish, renderGizmos);
    }

    internal static void DrawCameraImpl(Rect position, Camera camera, DrawCameraMode drawMode, bool drawGrid, DrawGridParameters gridParam, bool finish, [DefaultValue("true")] bool renderGizmos)
    {
      if (Event.current.type == EventType.Repaint)
      {
        if ((UnityEngine.Object) camera.targetTexture == (UnityEngine.Object) null)
        {
          Rect pixels = EditorGUIUtility.PointsToPixels(GUIClip.Unclip(position));
          camera.pixelRect = new Rect(pixels.xMin, (float) Screen.height - pixels.yMax, pixels.width, pixels.height);
        }
        else
          camera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
        if (drawMode == DrawCameraMode.Normal)
        {
          RenderTexture targetTexture = camera.targetTexture;
          camera.targetTexture = RenderTexture.active;
          camera.Render();
          camera.targetTexture = targetTexture;
        }
        else
        {
          if (drawGrid)
            Handles.Internal_DrawCameraWithGrid(camera, (int) drawMode, ref gridParam);
          else
            Handles.Internal_DrawCamera(camera, (int) drawMode);
          if (finish && camera.cameraType != CameraType.VR)
            Handles.Internal_FinishDrawingCamera(camera, renderGizmos);
        }
      }
      else
        Handles.Internal_SetCurrentCamera(camera);
    }

    internal static void DrawCamera(Rect position, Camera camera, DrawCameraMode drawMode, DrawGridParameters gridParam)
    {
      Handles.DrawCameraImpl(position, camera, drawMode, true, gridParam, true);
    }

    internal static void DrawCameraStep1(Rect position, Camera camera, DrawCameraMode drawMode, DrawGridParameters gridParam)
    {
      Handles.DrawCameraImpl(position, camera, drawMode, true, gridParam, false);
    }

    internal static void DrawCameraStep2(Camera camera, DrawCameraMode drawMode)
    {
      if (Event.current.type != EventType.Repaint || drawMode == DrawCameraMode.Normal)
        return;
      Handles.Internal_FinishDrawingCamera(camera);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void EmitGUIGeometryForCamera(Camera source, Camera dest);

    /// <summary>
    ///   <para>Draws a camera inside a rectangle.</para>
    /// </summary>
    /// <param name="position">The area to draw the camera within in GUI coordinates.</param>
    /// <param name="camera">The camera to draw.</param>
    /// <param name="drawMode">How the camera is drawn (textured, wireframe, etc.).</param>
    [ExcludeFromDocs]
    public static void DrawCamera(Rect position, Camera camera)
    {
      DrawCameraMode drawMode = DrawCameraMode.Normal;
      Handles.DrawCamera(position, camera, drawMode);
    }

    /// <summary>
    ///   <para>Draws a camera inside a rectangle.</para>
    /// </summary>
    /// <param name="position">The area to draw the camera within in GUI coordinates.</param>
    /// <param name="camera">The camera to draw.</param>
    /// <param name="drawMode">How the camera is drawn (textured, wireframe, etc.).</param>
    public static void DrawCamera(Rect position, Camera camera, [DefaultValue("DrawCameraMode.Normal")] DrawCameraMode drawMode)
    {
      DrawGridParameters gridParam = new DrawGridParameters();
      Handles.DrawCameraImpl(position, camera, drawMode, false, gridParam, true);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetCameraFilterMode(Camera camera, Handles.FilterMode mode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Handles.FilterMode GetCameraFilterMode(Camera camera);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DrawCameraFade(Camera camera, float fade);

    /// <summary>
    ///   <para>Set the current camera so all Handles and Gizmos are draw with its settings.</para>
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="position"></param>
    public static void SetCamera(Camera camera)
    {
      if (Event.current.type == EventType.Repaint)
        Handles.Internal_SetupCamera(camera);
      else
        Handles.Internal_SetCurrentCamera(camera);
    }

    /// <summary>
    ///   <para>Set the current camera so all Handles and Gizmos are draw with its settings.</para>
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="position"></param>
    public static void SetCamera(Rect position, Camera camera)
    {
      Rect pixels = EditorGUIUtility.PointsToPixels(GUIClip.Unclip(position));
      Rect rect = new Rect(pixels.xMin, (float) Screen.height - pixels.yMax, pixels.width, pixels.height);
      camera.pixelRect = rect;
      if (Event.current.type == EventType.Repaint)
        Handles.Internal_SetupCamera(camera);
      else
        Handles.Internal_SetCurrentCamera(camera);
    }

    /// <summary>
    ///   <para>Begin a 2D GUI block inside the 3D handle GUI.</para>
    /// </summary>
    public static void BeginGUI()
    {
      if (!(bool) ((UnityEngine.Object) Camera.current) || Event.current.type != EventType.Repaint)
        return;
      GUIClip.Reapply();
    }

    [Obsolete("Please use BeginGUI() with GUILayout.BeginArea(position) / GUILayout.EndArea()")]
    public static void BeginGUI(Rect position)
    {
      GUILayout.BeginArea(position);
    }

    /// <summary>
    ///   <para>End a 2D GUI block and get back to the 3D handle GUI.</para>
    /// </summary>
    public static void EndGUI()
    {
      Camera current = Camera.current;
      if (!(bool) ((UnityEngine.Object) current) || Event.current.type != EventType.Repaint)
        return;
      Handles.Internal_SetupCamera(current);
    }

    internal static void ShowStaticLabelIfNeeded(Vector3 pos)
    {
      if (Tools.s_Hidden || !EditorApplication.isPlaying || !GameObjectUtility.ContainsStatic(Selection.gameObjects))
        return;
      Handles.ShowStaticLabel(pos);
    }

    internal static void ShowStaticLabel(Vector3 pos)
    {
      Handles.color = Color.white;
      Handles.zTest = CompareFunction.Always;
      GUIStyle style = (GUIStyle) "SC ViewAxisLabel";
      style.alignment = TextAnchor.MiddleLeft;
      style.fixedWidth = 0.0f;
      Handles.BeginGUI();
      Rect sizedRect = HandleUtility.WorldPointToSizedRect(pos, EditorGUIUtility.TempContent("Static"), style);
      sizedRect.x += 10f;
      sizedRect.y += 10f;
      GUI.Label(sizedRect, EditorGUIUtility.TempContent("Static"), style);
      Handles.EndGUI();
    }

    private static Vector3[] Internal_MakeBezierPoints(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, int division)
    {
      return Handles.INTERNAL_CALL_Internal_MakeBezierPoints(ref startPosition, ref endPosition, ref startTangent, ref endTangent, division);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Vector3[] INTERNAL_CALL_Internal_MakeBezierPoints(ref Vector3 startPosition, ref Vector3 endPosition, ref Vector3 startTangent, ref Vector3 endTangent, int division);

    /// <summary>
    ///   <para>Retuns an array of points to representing the bezier curve. See Handles.DrawBezier.</para>
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <param name="startTangent"></param>
    /// <param name="endTangent"></param>
    /// <param name="division"></param>
    public static Vector3[] MakeBezierPoints(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, int division)
    {
      if (division < 1)
        throw new ArgumentOutOfRangeException(nameof (division), "Must be greater than zero");
      return Handles.Internal_MakeBezierPoints(startPosition, endPosition, startTangent, endTangent, division);
    }

    private static Mesh cubeMesh
    {
      get
      {
        if ((UnityEngine.Object) Handles.s_CubeMesh == (UnityEngine.Object) null)
          Handles.Init();
        return Handles.s_CubeMesh;
      }
    }

    private static Mesh coneMesh
    {
      get
      {
        if ((UnityEngine.Object) Handles.s_ConeMesh == (UnityEngine.Object) null)
          Handles.Init();
        return Handles.s_ConeMesh;
      }
    }

    private static Mesh cylinderMesh
    {
      get
      {
        if ((UnityEngine.Object) Handles.s_CylinderMesh == (UnityEngine.Object) null)
          Handles.Init();
        return Handles.s_CylinderMesh;
      }
    }

    private static Mesh quadMesh
    {
      get
      {
        if ((UnityEngine.Object) Handles.s_QuadMesh == (UnityEngine.Object) null)
          Handles.Init();
        return Handles.s_QuadMesh;
      }
    }

    private static Mesh sphereMesh
    {
      get
      {
        if ((UnityEngine.Object) Handles.s_SphereMesh == (UnityEngine.Object) null)
          Handles.Init();
        return Handles.s_SphereMesh;
      }
    }

    internal static Color GetColorByAxis(int axis)
    {
      return (Color) Handles.s_AxisColor[axis];
    }

    private static Vector3 GetAxisVector(int axis)
    {
      return Handles.s_AxisVector[axis];
    }

    private static bool BeginLineDrawing(Matrix4x4 matrix, bool dottedLines, int mode)
    {
      if (Event.current.type != EventType.Repaint)
        return false;
      Color c = Handles.color * Handles.lineTransparency;
      if (dottedLines)
        HandleUtility.ApplyDottedWireMaterial(Handles.zTest);
      else
        HandleUtility.ApplyWireMaterial(Handles.zTest);
      GL.PushMatrix();
      GL.MultMatrix(matrix);
      GL.Begin(mode);
      GL.Color(c);
      return true;
    }

    private static void EndLineDrawing()
    {
      GL.End();
      GL.PopMatrix();
    }

    /// <summary>
    ///   <para>Draw a line going through the list of all points.</para>
    /// </summary>
    /// <param name="points"></param>
    public static void DrawPolyLine(params Vector3[] points)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, false, 2))
        return;
      for (int index = 0; index < points.Length; ++index)
        GL.Vertex(points[index]);
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a line from p1 to p2.</para>
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public static void DrawLine(Vector3 p1, Vector3 p2)
    {
      Handles.DrawLine(p1, p2, false);
    }

    internal static void DrawLine(Vector3 p1, Vector3 p2, bool dottedLine)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, dottedLine, 1))
        return;
      GL.Vertex(p1);
      GL.Vertex(p2);
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of line segments.</para>
    /// </summary>
    /// <param name="lineSegments">A list of pairs of points that represent the start and end of line segments.</param>
    public static void DrawLines(Vector3[] lineSegments)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, false, 1))
        return;
      int index = 0;
      while (index < lineSegments.Length)
      {
        Vector3 lineSegment1 = lineSegments[index];
        Vector3 lineSegment2 = lineSegments[index + 1];
        GL.Vertex(lineSegment1);
        GL.Vertex(lineSegment2);
        index += 2;
      }
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of indexed line segments.</para>
    /// </summary>
    /// <param name="points">A list of points.</param>
    /// <param name="segmentIndices">A list of pairs of indices to the start and end points of the line segments.</param>
    public static void DrawLines(Vector3[] points, int[] segmentIndices)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, false, 1))
        return;
      int index = 0;
      while (index < segmentIndices.Length)
      {
        Vector3 point1 = points[segmentIndices[index]];
        Vector3 point2 = points[segmentIndices[index + 1]];
        GL.Vertex(point1);
        GL.Vertex(point2);
        index += 2;
      }
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a dotted line from p1 to p2.</para>
    /// </summary>
    /// <param name="p1">The start point.</param>
    /// <param name="p2">The end point.</param>
    /// <param name="screenSpaceSize">The size in pixels for the lengths of the line segments and the gaps between them.</param>
    public static void DrawDottedLine(Vector3 p1, Vector3 p2, float screenSpaceSize)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, true, 1))
        return;
      float x = screenSpaceSize * EditorGUIUtility.pixelsPerPoint;
      GL.MultiTexCoord(1, p1);
      GL.MultiTexCoord2(2, x, 0.0f);
      GL.Vertex(p1);
      GL.MultiTexCoord(1, p1);
      GL.MultiTexCoord2(2, x, 0.0f);
      GL.Vertex(p2);
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of dotted line segments.</para>
    /// </summary>
    /// <param name="lineSegments">A list of pairs of points that represent the start and end of line segments.</param>
    /// <param name="screenSpaceSize">The size in pixels for the lengths of the line segments and the gaps between them.</param>
    public static void DrawDottedLines(Vector3[] lineSegments, float screenSpaceSize)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, true, 1))
        return;
      float x = screenSpaceSize * EditorGUIUtility.pixelsPerPoint;
      int index = 0;
      while (index < lineSegments.Length)
      {
        Vector3 lineSegment1 = lineSegments[index];
        Vector3 lineSegment2 = lineSegments[index + 1];
        GL.MultiTexCoord(1, lineSegment1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(lineSegment1);
        GL.MultiTexCoord(1, lineSegment1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(lineSegment2);
        index += 2;
      }
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of indexed dotted line segments.</para>
    /// </summary>
    /// <param name="points">A list of points.</param>
    /// <param name="segmentIndices">A list of pairs of indices to the start and end points of the line segments.</param>
    /// <param name="screenSpaceSize">The size in pixels for the lengths of the line segments and the gaps between them.</param>
    public static void DrawDottedLines(Vector3[] points, int[] segmentIndices, float screenSpaceSize)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, true, 1))
        return;
      float x = screenSpaceSize * EditorGUIUtility.pixelsPerPoint;
      int index = 0;
      while (index < segmentIndices.Length)
      {
        Vector3 point1 = points[segmentIndices[index]];
        Vector3 point2 = points[segmentIndices[index + 1]];
        GL.MultiTexCoord(1, point1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(point1);
        GL.MultiTexCoord(1, point1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(point2);
        index += 2;
      }
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a wireframe box with center and size.</para>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="size"></param>
    public static void DrawWireCube(Vector3 center, Vector3 size)
    {
      Vector3 vector3 = size * 0.5f;
      Vector3[] vector3Array = new Vector3[10]{ center + new Vector3(-vector3.x, -vector3.y, -vector3.z), center + new Vector3(-vector3.x, vector3.y, -vector3.z), center + new Vector3(vector3.x, vector3.y, -vector3.z), center + new Vector3(vector3.x, -vector3.y, -vector3.z), center + new Vector3(-vector3.x, -vector3.y, -vector3.z), center + new Vector3(-vector3.x, -vector3.y, vector3.z), center + new Vector3(-vector3.x, vector3.y, vector3.z), center + new Vector3(vector3.x, vector3.y, vector3.z), center + new Vector3(vector3.x, -vector3.y, vector3.z), center + new Vector3(-vector3.x, -vector3.y, vector3.z) };
      Handles.DrawPolyLine(vector3Array);
      Handles.DrawLine(vector3Array[1], vector3Array[6]);
      Handles.DrawLine(vector3Array[2], vector3Array[7]);
      Handles.DrawLine(vector3Array[3], vector3Array[8]);
    }

    /// <summary>
    ///         <para>Make a 3D disc that can be dragged with the mouse.
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </summary>
    /// <param name="id">Control id of the handle.</param>
    /// <param name="rotation">The rotation of the disc.</param>
    /// <param name="position">The center of the disc.</param>
    /// <param name="axis">The axis to rotate around.</param>
    /// <param name="size">The size of the disc in world space See Also:HandleUtility.GetHandleSize.</param>
    /// <param name="cutoffPlane">If true, only the front-facing half of the circle is draw / draggable. This is useful when you have many overlapping rotation axes (like in the default rotate tool) to avoid clutter.</param>
    /// <param name="snap">The grid size to snap to.</param>
    /// <returns>
    ///   <para>The new rotation value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.</para>
    /// </returns>
    public static Quaternion Disc(int id, Quaternion rotation, Vector3 position, Vector3 axis, float size, bool cutoffPlane, float snap)
    {
      return Disc.Do(id, rotation, position, axis, size, cutoffPlane, snap);
    }

    /// <summary>
    ///   <para>Make an unconstrained rotation handle.</para>
    /// </summary>
    /// <param name="id">Control id of the handle.</param>
    /// <param name="rotation">Orientation of the handle.</param>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <param name="size">The size of the handle.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <returns>
    ///   <para>The new rotation value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the same value as you passed into the function.</para>
    /// </returns>
    public static Quaternion FreeRotateHandle(int id, Quaternion rotation, Vector3 position, float size)
    {
      return FreeRotate.Do(id, rotation, position, size);
    }

    /// <summary>
    ///   <para>Make a 3D slider that moves along one axis.</para>
    /// </summary>
    /// <param name="position">The position of the current point in the space of Handles.matrix.</param>
    /// <param name="direction">The direction axis of the slider in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    /// <param name="snap">The snap increment. See Handles.SnapValue.</param>
    /// <param name="capFunction">The function to call for doing the actual drawing. By default it is Handles.ArrowHandleCap, but any function that has the same signature can be used.</param>
    /// <returns>
    ///   <para>The new value modified by the user's interaction with the handle. If the user has not moved the handle, it will return the position value passed into the function.</para>
    /// </returns>
    public static Vector3 Slider(Vector3 position, Vector3 direction)
    {
      Vector3 position1 = position;
      Vector3 direction1 = direction;
      double handleSize = (double) HandleUtility.GetHandleSize(position);
      // ISSUE: reference to a compiler-generated field
      if (Handles.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Handles.\u003C\u003Ef__mg\u0024cache0 = new Handles.CapFunction(Handles.ArrowHandleCap);
      }
      // ISSUE: reference to a compiler-generated field
      Handles.CapFunction fMgCache0 = Handles.\u003C\u003Ef__mg\u0024cache0;
      double num = -1.0;
      return Handles.Slider(position1, direction1, (float) handleSize, fMgCache0, (float) num);
    }

    public static Vector3 Slider(Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction, float snap)
    {
      return Slider1D.Do(GUIUtility.GetControlID(Handles.s_SliderHash, FocusType.Passive), position, direction, size, capFunction, snap);
    }

    public static Vector3 Slider(int controlID, Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction, float snap)
    {
      return Slider1D.Do(controlID, position, direction, size, capFunction, snap);
    }

    public static Vector3 Slider(int controlID, Vector3 position, Vector3 offset, Vector3 direction, float size, Handles.CapFunction capFunction, float snap)
    {
      return Slider1D.Do(controlID, position, offset, direction, direction, size, capFunction, snap);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Slider(Vector3 position, Vector3 direction, float size, Handles.DrawCapFunction drawFunc, float snap)
    {
      return Slider1D.Do(GUIUtility.GetControlID(Handles.s_SliderHash, FocusType.Passive), position, direction, size, drawFunc, snap);
    }

    public static Vector3 FreeMoveHandle(Vector3 position, Quaternion rotation, float size, Vector3 snap, Handles.CapFunction capFunction)
    {
      return FreeMove.Do(GUIUtility.GetControlID(Handles.s_FreeMoveHandleHash, FocusType.Passive), position, rotation, size, snap, capFunction);
    }

    public static Vector3 FreeMoveHandle(int controlID, Vector3 position, Quaternion rotation, float size, Vector3 snap, Handles.CapFunction capFunction)
    {
      return FreeMove.Do(controlID, position, rotation, size, snap, capFunction);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 FreeMoveHandle(Vector3 position, Quaternion rotation, float size, Vector3 snap, Handles.DrawCapFunction capFunc)
    {
      return FreeMove.Do(GUIUtility.GetControlID(Handles.s_FreeMoveHandleHash, FocusType.Passive), position, rotation, size, snap, capFunc);
    }

    public static float ScaleValueHandle(float value, Vector3 position, Quaternion rotation, float size, Handles.CapFunction capFunction, float snap)
    {
      return SliderScale.DoCenter(GUIUtility.GetControlID(Handles.s_ScaleValueHandleHash, FocusType.Passive), value, position, rotation, size, capFunction, snap);
    }

    public static float ScaleValueHandle(int controlID, float value, Vector3 position, Quaternion rotation, float size, Handles.CapFunction capFunction, float snap)
    {
      return SliderScale.DoCenter(controlID, value, position, rotation, size, capFunction, snap);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static float ScaleValueHandle(float value, Vector3 position, Quaternion rotation, float size, Handles.DrawCapFunction capFunc, float snap)
    {
      return SliderScale.DoCenter(GUIUtility.GetControlID(Handles.s_ScaleValueHandleHash, FocusType.Passive), value, position, rotation, size, capFunc, snap);
    }

    public static bool Button(Vector3 position, Quaternion direction, float size, float pickSize, Handles.CapFunction capFunction)
    {
      return Button.Do(GUIUtility.GetControlID(Handles.s_ButtonHash, FocusType.Passive), position, direction, size, pickSize, capFunction);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static bool Button(Vector3 position, Quaternion direction, float size, float pickSize, Handles.DrawCapFunction capFunc)
    {
      return Button.Do(GUIUtility.GetControlID(Handles.s_ButtonHash, FocusType.Passive), position, direction, size, pickSize, capFunc);
    }

    internal static bool Button(int controlID, Vector3 position, Quaternion direction, float size, float pickSize, Handles.CapFunction capFunction)
    {
      return Button.Do(controlID, position, direction, size, pickSize, capFunction);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    internal static bool Button(int controlID, Vector3 position, Quaternion direction, float size, float pickSize, Handles.DrawCapFunction capFunc)
    {
      return Button.Do(controlID, position, direction, size, pickSize, capFunc);
    }

    /// <summary>
    ///   <para>Draw a cube handle. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout and EventType.Repaint events.</param>
    public static void CubeHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (eventType != EventType.Layout)
      {
        if (eventType != EventType.Repaint)
          return;
        Graphics.DrawMeshNow(Handles.cubeMesh, Handles.StartCapDraw(position, rotation, size));
      }
      else
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToCircle(position, size));
    }

    /// <summary>
    ///   <para>Draw a sphere handle. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout and EventType.Repaint events.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    public static void SphereHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (eventType != EventType.Layout)
      {
        if (eventType != EventType.Repaint)
          return;
        Graphics.DrawMeshNow(Handles.sphereMesh, Handles.StartCapDraw(position, rotation, size));
      }
      else
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToCircle(position, size));
    }

    /// <summary>
    ///   <para>Draw a cone handle. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout and EventType.Repaint events.</param>
    public static void ConeHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (eventType != EventType.Layout)
      {
        if (eventType != EventType.Repaint)
          return;
        Graphics.DrawMeshNow(Handles.coneMesh, Handles.StartCapDraw(position, rotation, size));
      }
      else
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToCircle(position, size));
    }

    /// <summary>
    ///   <para>Draw a cylinder handle. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout and EventType.Repaint events.</param>
    public static void CylinderHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (eventType != EventType.Layout)
      {
        if (eventType != EventType.Repaint)
          return;
        Graphics.DrawMeshNow(Handles.cylinderMesh, Handles.StartCapDraw(position, rotation, size));
      }
      else
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToCircle(position, size));
    }

    /// <summary>
    ///   <para>Draw a rectangle handle. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout and EventType.Repaint events.</param>
    public static void RectangleHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      Handles.RectangleHandleCap(controlID, position, rotation, new Vector2(size, size), eventType);
    }

    internal static void RectangleHandleCap(int controlID, Vector3 position, Quaternion rotation, Vector2 size, EventType eventType)
    {
      if (eventType != EventType.Layout)
      {
        if (eventType != EventType.Repaint)
          return;
        Vector3 vector3_1 = rotation * new Vector3(size.x, 0.0f, 0.0f);
        Vector3 vector3_2 = rotation * new Vector3(0.0f, size.y, 0.0f);
        Handles.s_RectangleHandlePointsCache[0] = position + vector3_1 + vector3_2;
        Handles.s_RectangleHandlePointsCache[1] = position + vector3_1 - vector3_2;
        Handles.s_RectangleHandlePointsCache[2] = position - vector3_1 - vector3_2;
        Handles.s_RectangleHandlePointsCache[3] = position - vector3_1 + vector3_2;
        Handles.s_RectangleHandlePointsCache[4] = position + vector3_1 + vector3_2;
        Handles.DrawPolyLine(Handles.s_RectangleHandlePointsCache);
      }
      else
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToRectangleInternal(position, rotation, size));
    }

    /// <summary>
    ///   <para>Draw a dot handle. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout and EventType.Repaint events.</param>
    public static void DotHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (eventType != EventType.Layout)
      {
        if (eventType != EventType.Repaint)
          return;
        position = Handles.matrix.MultiplyPoint(position);
        Vector3 vector3_1 = Camera.current.transform.right * size;
        Vector3 vector3_2 = Camera.current.transform.up * size;
        Color c = Handles.color * new Color(1f, 1f, 1f, 0.99f);
        HandleUtility.ApplyWireMaterial();
        GL.Begin(7);
        GL.Color(c);
        GL.Vertex(position + vector3_1 + vector3_2);
        GL.Vertex(position + vector3_1 - vector3_2);
        GL.Vertex(position - vector3_1 - vector3_2);
        GL.Vertex(position - vector3_1 + vector3_2);
        GL.End();
      }
      else
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToRectangle(position, rotation, size));
    }

    /// <summary>
    ///   <para>Draw a circle handle. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout and EventType.Repaint events.</param>
    public static void CircleHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (eventType != EventType.Layout)
      {
        if (eventType != EventType.Repaint)
          return;
        Handles.StartCapDraw(position, rotation, size);
        Vector3 normal = rotation * new Vector3(0.0f, 0.0f, 1f);
        Handles.DrawWireDisc(position, normal, size);
      }
      else
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToRectangle(position, rotation, size));
    }

    /// <summary>
    ///   <para>Draw an arrow like those used by the move tool.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in the space of Handles.matrix. Use HandleUtility.GetHandleSize if you want a constant screen-space size.</param>
    /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout and EventType.Repaint events.</param>
    public static void ArrowHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      Handles.ArrowHandleCap(controlID, position, rotation, size, eventType, Vector3.zero);
    }

    internal static void ArrowHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType, Vector3 coneOffset)
    {
      if (eventType != EventType.Layout)
      {
        if (eventType != EventType.Repaint)
          return;
        Vector3 forward = rotation * Vector3.forward;
        Handles.ConeHandleCap(controlID, position + (forward + coneOffset) * size, Quaternion.LookRotation(forward), size * 0.2f, eventType);
        Handles.DrawLine(position, position + (forward + coneOffset) * size * 0.9f, false);
      }
      else
      {
        Vector3 vector3 = rotation * Vector3.forward;
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToLine(position, position + (vector3 + coneOffset) * size * 0.9f));
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToCircle(position + (vector3 + coneOffset) * size, size * 0.2f));
      }
    }

    /// <summary>
    ///   <para>Draw a camera facing selection frame.</para>
    /// </summary>
    /// <param name="controlID"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="size"></param>
    /// <param name="eventType"></param>
    public static void DrawSelectionFrame(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (eventType != EventType.Repaint)
        return;
      Handles.StartCapDraw(position, rotation, size);
      Vector3 vector3_1 = rotation * new Vector3(size, 0.0f, 0.0f);
      Vector3 vector3_2 = rotation * new Vector3(0.0f, size, 0.0f);
      Vector3 vector3_3 = position - vector3_1 + vector3_2;
      Vector3 vector3_4 = position + vector3_1 + vector3_2;
      Vector3 vector3_5 = position + vector3_1 - vector3_2;
      Vector3 vector3_6 = position - vector3_1 - vector3_2;
      Handles.DrawLine(vector3_3, vector3_4);
      Handles.DrawLine(vector3_4, vector3_5);
      Handles.DrawLine(vector3_5, vector3_6);
      Handles.DrawLine(vector3_6, vector3_3);
    }

    internal static float GetCameraViewLerpForWorldAxis(Vector3 viewVector, Vector3 axis)
    {
      return Mathf.Clamp01((float) (6.66666793823242 * ((double) Mathf.Abs(Vector3.Dot(viewVector, axis)) - 0.850000023841858)));
    }

    internal static Vector3 GetCameraViewFrom(Vector3 position, Matrix4x4 matrix)
    {
      Camera current = Camera.current;
      return !current.orthographic ? matrix.MultiplyVector(position - current.transform.position).normalized : matrix.MultiplyVector(-current.transform.forward).normalized;
    }

    internal static float DistanceToPolygone(Vector3[] vertices)
    {
      return HandleUtility.DistanceToPolyLine(vertices);
    }

    internal static void DoBoneHandle(Transform target)
    {
      Handles.DoBoneHandle(target, (Dictionary<Transform, bool>) null);
    }

    internal static void DoBoneHandle(Transform target, Dictionary<Transform, bool> validBones)
    {
      int hashCode = target.name.GetHashCode();
      Event current1 = Event.current;
      bool flag = false;
      if (validBones != null)
      {
        IEnumerator enumerator = target.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            Transform current2 = (Transform) enumerator.Current;
            if (validBones.ContainsKey(current2))
            {
              flag = true;
              break;
            }
          }
        }
        finally
        {
          IDisposable disposable;
          if ((disposable = enumerator as IDisposable) != null)
            disposable.Dispose();
        }
      }
      Vector3 position = target.position;
      List<Vector3> vector3List = new List<Vector3>();
      if (!flag && (UnityEngine.Object) target.parent != (UnityEngine.Object) null)
      {
        vector3List.Add(target.position + (target.position - target.parent.position) * 0.4f);
      }
      else
      {
        IEnumerator enumerator = target.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            Transform current2 = (Transform) enumerator.Current;
            if (validBones == null || validBones.ContainsKey(current2))
              vector3List.Add(current2.position);
          }
        }
        finally
        {
          IDisposable disposable;
          if ((disposable = enumerator as IDisposable) != null)
            disposable.Dispose();
        }
      }
      for (int index = 0; index < vector3List.Count; ++index)
      {
        Vector3 endPoint = vector3List[index];
        switch (current1.GetTypeForControl(hashCode))
        {
          case EventType.MouseDown:
            if (!current1.alt && HandleUtility.nearestControl == hashCode && current1.button == 0)
            {
              GUIUtility.hotControl = hashCode;
              if (current1.shift)
              {
                UnityEngine.Object[] objects = Selection.objects;
                if (!ArrayUtility.Contains<UnityEngine.Object>(objects, (UnityEngine.Object) target))
                {
                  ArrayUtility.Add<UnityEngine.Object>(ref objects, (UnityEngine.Object) target);
                  Selection.objects = objects;
                }
              }
              else
                Selection.activeObject = (UnityEngine.Object) target;
              EditorGUIUtility.PingObject((UnityEngine.Object) target);
              current1.Use();
              break;
            }
            break;
          case EventType.MouseUp:
            if (GUIUtility.hotControl == hashCode && (current1.button == 0 || current1.button == 2))
            {
              GUIUtility.hotControl = 0;
              current1.Use();
              break;
            }
            break;
          case EventType.MouseMove:
            if (hashCode == HandleUtility.nearestControl)
            {
              HandleUtility.Repaint();
              break;
            }
            break;
          case EventType.MouseDrag:
            if (!current1.alt && GUIUtility.hotControl == hashCode)
            {
              DragAndDrop.PrepareStartDrag();
              DragAndDrop.objectReferences = new UnityEngine.Object[1]
              {
                (UnityEngine.Object) target
              };
              DragAndDrop.StartDrag(ObjectNames.GetDragAndDropTitle((UnityEngine.Object) target));
              GUIUtility.hotControl = 0;
              current1.Use();
              break;
            }
            break;
          case EventType.Repaint:
            float num = Vector3.Magnitude(endPoint - position);
            if ((double) num > 0.0)
            {
              Handles.color = GUIUtility.hotControl != 0 || HandleUtility.nearestControl != hashCode ? Handles.color : Handles.preselectionColor;
              float size = num * 0.08f;
              if (flag)
                Handles.DrawBone(endPoint, position, size);
              else
                Handles.SphereHandleCap(hashCode, position, target.rotation, size * 0.2f, EventType.Repaint);
              break;
            }
            break;
          case EventType.Layout:
            float radius = Vector3.Magnitude(endPoint - position) * 0.08f;
            Vector3[] boneVertices = Handles.GetBoneVertices(endPoint, position, radius);
            HandleUtility.AddControl(hashCode, Handles.DistanceToPolygone(boneVertices));
            break;
        }
      }
    }

    internal static void DrawBone(Vector3 endPoint, Vector3 basePoint, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Vector3[] boneVertices = Handles.GetBoneVertices(endPoint, basePoint, size);
      HandleUtility.ApplyWireMaterial();
      GL.Begin(4);
      GL.Color(Handles.color);
      for (int index = 0; index < 3; ++index)
      {
        GL.Vertex(boneVertices[index * 6]);
        GL.Vertex(boneVertices[index * 6 + 1]);
        GL.Vertex(boneVertices[index * 6 + 2]);
        GL.Vertex(boneVertices[index * 6 + 3]);
        GL.Vertex(boneVertices[index * 6 + 4]);
        GL.Vertex(boneVertices[index * 6 + 5]);
      }
      GL.End();
      GL.Begin(1);
      GL.Color(Handles.color * new Color(1f, 1f, 1f, 0.0f) + new Color(0.0f, 0.0f, 0.0f, 1f));
      for (int index = 0; index < 3; ++index)
      {
        GL.Vertex(boneVertices[index * 6]);
        GL.Vertex(boneVertices[index * 6 + 1]);
        GL.Vertex(boneVertices[index * 6 + 1]);
        GL.Vertex(boneVertices[index * 6 + 2]);
      }
      GL.End();
    }

    internal static Vector3[] GetBoneVertices(Vector3 endPoint, Vector3 basePoint, float radius)
    {
      Vector3 lhs = Vector3.Normalize(endPoint - basePoint);
      Vector3 vector3_1 = Vector3.Cross(lhs, Vector3.up);
      if ((double) Vector3.SqrMagnitude(vector3_1) < 0.100000001490116)
        vector3_1 = Vector3.Cross(lhs, Vector3.right);
      vector3_1.Normalize();
      Vector3 vector3_2 = Vector3.Cross(lhs, vector3_1);
      Vector3[] vector3Array = new Vector3[18];
      float f = 0.0f;
      for (int index = 0; index < 3; ++index)
      {
        float num1 = Mathf.Cos(f);
        float num2 = Mathf.Sin(f);
        float num3 = Mathf.Cos(f + 2.094395f);
        float num4 = Mathf.Sin(f + 2.094395f);
        Vector3 vector3_3 = basePoint + vector3_1 * (num1 * radius) + vector3_2 * (num2 * radius);
        Vector3 vector3_4 = basePoint + vector3_1 * (num3 * radius) + vector3_2 * (num4 * radius);
        vector3Array[index * 6] = endPoint;
        vector3Array[index * 6 + 1] = vector3_3;
        vector3Array[index * 6 + 2] = vector3_4;
        vector3Array[index * 6 + 3] = basePoint;
        vector3Array[index * 6 + 4] = vector3_4;
        vector3Array[index * 6 + 5] = vector3_3;
        f += 2.094395f;
      }
      return vector3Array;
    }

    internal static Vector3 DoConeFrustrumHandle(Quaternion rotation, Vector3 position, Vector3 radiusAngleRange)
    {
      Vector3 vector3 = rotation * Vector3.forward;
      Vector3 d1 = rotation * Vector3.up;
      Vector3 d2 = rotation * Vector3.right;
      float x = radiusAngleRange.x;
      float y1 = radiusAngleRange.y;
      float z = radiusAngleRange.z;
      float y2 = Mathf.Max(0.0f, y1);
      bool changed1 = GUI.changed;
      float num1 = Handles.SizeSlider(position, vector3, z);
      GUI.changed |= changed1;
      bool changed2 = GUI.changed;
      GUI.changed = false;
      float r1 = Handles.SizeSlider(position, d1, x);
      float r2 = Handles.SizeSlider(position, -d1, r1);
      float r3 = Handles.SizeSlider(position, d2, r2);
      float num2 = Handles.SizeSlider(position, -d2, r3);
      if (GUI.changed)
        num2 = Mathf.Max(0.0f, num2);
      GUI.changed |= changed2;
      bool changed3 = GUI.changed;
      GUI.changed = false;
      float r4 = Mathf.Min(1000f, Mathf.Abs(num1 * Mathf.Tan((float) Math.PI / 180f * y2)) + num2);
      float r5 = Handles.SizeSlider(position + vector3 * num1, d1, r4);
      float r6 = Handles.SizeSlider(position + vector3 * num1, -d1, r5);
      float r7 = Handles.SizeSlider(position + vector3 * num1, d2, r6);
      float radius = Handles.SizeSlider(position + vector3 * num1, -d2, r7);
      if (GUI.changed)
        y2 = Mathf.Clamp(57.29578f * Mathf.Atan((radius - num2) / Mathf.Abs(num1)), 0.0f, 90f);
      GUI.changed |= changed3;
      if ((double) num2 > 0.0)
        Handles.DrawWireDisc(position, vector3, num2);
      if ((double) radius > 0.0)
        Handles.DrawWireDisc(position + num1 * vector3, vector3, radius);
      Handles.DrawLine(position + d1 * num2, position + vector3 * num1 + d1 * radius);
      Handles.DrawLine(position - d1 * num2, position + vector3 * num1 - d1 * radius);
      Handles.DrawLine(position + d2 * num2, position + vector3 * num1 + d2 * radius);
      Handles.DrawLine(position - d2 * num2, position + vector3 * num1 - d2 * radius);
      return new Vector3(num2, y2, num1);
    }

    internal static Vector2 DoConeHandle(Quaternion rotation, Vector3 position, Vector2 angleAndRange, float angleScale, float rangeScale, bool handlesOnly)
    {
      float x = angleAndRange.x;
      float y = angleAndRange.y;
      float r1 = y * rangeScale;
      Vector3 vector3 = rotation * Vector3.forward;
      Vector3 d1 = rotation * Vector3.up;
      Vector3 d2 = rotation * Vector3.right;
      bool changed1 = GUI.changed;
      GUI.changed = false;
      float num = Handles.SizeSlider(position, vector3, r1);
      if (GUI.changed)
        y = Mathf.Max(0.0f, num / rangeScale);
      GUI.changed |= changed1;
      bool changed2 = GUI.changed;
      GUI.changed = false;
      float r2 = num * Mathf.Tan((float) (Math.PI / 180.0 * (double) x / 2.0)) * angleScale;
      float r3 = Handles.SizeSlider(position + vector3 * num, d1, r2);
      float r4 = Handles.SizeSlider(position + vector3 * num, -d1, r3);
      float r5 = Handles.SizeSlider(position + vector3 * num, d2, r4);
      float radius = Handles.SizeSlider(position + vector3 * num, -d2, r5);
      if (GUI.changed)
        x = Mathf.Clamp((float) (57.2957801818848 * (double) Mathf.Atan(radius / (num * angleScale)) * 2.0), 0.0f, 179f);
      GUI.changed |= changed2;
      if (!handlesOnly)
      {
        Handles.DrawLine(position, position + vector3 * num + d1 * radius);
        Handles.DrawLine(position, position + vector3 * num - d1 * radius);
        Handles.DrawLine(position, position + vector3 * num + d2 * radius);
        Handles.DrawLine(position, position + vector3 * num - d2 * radius);
        Handles.DrawWireDisc(position + num * vector3, vector3, radius);
      }
      return new Vector2(x, y);
    }

    private static float SizeSlider(Vector3 p, Vector3 d, float r)
    {
      Vector3 position1 = p + d * r;
      float handleSize = HandleUtility.GetHandleSize(position1);
      bool changed = GUI.changed;
      GUI.changed = false;
      Vector3 position2 = position1;
      Vector3 direction = d;
      double num1 = (double) handleSize * 0.0299999993294477;
      // ISSUE: reference to a compiler-generated field
      if (Handles.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Handles.\u003C\u003Ef__mg\u0024cache1 = new Handles.CapFunction(Handles.DotHandleCap);
      }
      // ISSUE: reference to a compiler-generated field
      Handles.CapFunction fMgCache1 = Handles.\u003C\u003Ef__mg\u0024cache1;
      double num2 = 0.0;
      Vector3 vector3 = Handles.Slider(position2, direction, (float) num1, fMgCache1, (float) num2);
      if (GUI.changed)
        r = Vector3.Dot(vector3 - p, d);
      GUI.changed |= changed;
      return r;
    }

    private static bool currentlyDragging
    {
      get
      {
        return GUIUtility.hotControl != 0;
      }
    }

    public static Vector3 DoPositionHandle(Vector3 position, Quaternion rotation)
    {
      return Handles.DoPositionHandle(Handles.PositionHandleIds.@default, position, rotation);
    }

    internal static Vector3 DoPositionHandle(Handles.PositionHandleIds ids, Vector3 position, Quaternion rotation)
    {
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.KeyDown:
          if (current.keyCode == KeyCode.V && !Handles.currentlyDragging)
          {
            Handles.s_FreeMoveMode = true;
            break;
          }
          break;
        case EventType.KeyUp:
          position = Handles.DoPositionHandle_Internal(ids, position, rotation, Handles.PositionHandleParam.DefaultHandle);
          if (current.keyCode == KeyCode.V && !current.shift && !Handles.currentlyDragging)
            Handles.s_FreeMoveMode = false;
          return position;
        case EventType.Layout:
          if (!Handles.currentlyDragging && !Tools.vertexDragging)
          {
            Handles.s_FreeMoveMode = current.shift;
            break;
          }
          break;
      }
      Handles.PositionHandleParam positionHandleParam = !Handles.s_FreeMoveMode ? Handles.PositionHandleParam.DefaultHandle : Handles.PositionHandleParam.DefaultFreeMoveHandle;
      return Handles.DoPositionHandle_Internal(ids, position, rotation, positionHandleParam);
    }

    private static Vector3 DoPositionHandle_Internal(Handles.PositionHandleIds ids, Vector3 position, Quaternion rotation, Handles.PositionHandleParam param)
    {
      Color color = Handles.color;
      bool flag1 = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
      Vector3 cameraViewFrom = Handles.GetCameraViewFrom(Handles.matrix.MultiplyPoint3x4(position), (Handles.matrix * Matrix4x4.TRS(position, rotation, Vector3.one)).inverse);
      float handleSize = HandleUtility.GetHandleSize(position);
      for (int axis = 0; axis < 3; ++axis)
        Handles.s_DoPositionHandle_Internal_CameraViewLerp[axis] = ids[axis] != GUIUtility.hotControl ? Handles.GetCameraViewLerpForWorldAxis(cameraViewFrom, Handles.GetAxisVector(axis)) : 0.0f;
      for (int index = 0; index < 3; ++index)
        Handles.s_DoPositionHandle_Internal_CameraViewLerp[3 + index] = Mathf.Max(Handles.s_DoPositionHandle_Internal_CameraViewLerp[index], Handles.s_DoPositionHandle_Internal_CameraViewLerp[(index + 1) % 3]);
      bool flag2 = ids.Has(GUIUtility.hotControl);
      Vector3 vector3_1 = param.axisOffset;
      Vector3 vector3_2 = param.planeOffset;
      if (flag2)
      {
        vector3_1 = Vector3.zero;
        vector3_2 = Vector3.zero;
      }
      Vector3 vector3_3 = !flag2 ? param.planeSize : param.planeSize + param.planeOffset;
      for (int planePrimaryAxis = 0; planePrimaryAxis < 3; ++planePrimaryAxis)
      {
        if (param.ShouldShow(3 + planePrimaryAxis) && (!flag2 || ids[3 + planePrimaryAxis] == GUIUtility.hotControl))
        {
          float cameraLerp = !flag2 ? Handles.s_DoPositionHandle_Internal_CameraViewLerp[3 + planePrimaryAxis] : 0.0f;
          if ((double) cameraLerp <= 0.899999976158142)
          {
            Vector3 offset = vector3_2 * handleSize;
            offset[Handles.s_DoPositionHandle_Internal_PrevIndex[planePrimaryAxis]] = 0.0f;
            float num = Mathf.Max(vector3_3[planePrimaryAxis], vector3_3[Handles.s_DoPositionHandle_Internal_NextIndex[planePrimaryAxis]]);
            position = Handles.DoPlanarHandle(ids[3 + planePrimaryAxis], planePrimaryAxis, position, offset, rotation, handleSize * num, cameraLerp, cameraViewFrom, param.planeOrientation);
          }
        }
      }
      for (int axis = 0; axis < 3; ++axis)
      {
        if (param.ShouldShow(axis))
        {
          if (!Handles.currentlyDragging)
          {
            switch (param.axesOrientation)
            {
              case Handles.PositionHandleParam.Orientation.Signed:
                Handles.s_DoPositionHandle_AxisHandlesOctant[axis] = 1f;
                break;
              case Handles.PositionHandleParam.Orientation.Camera:
                Handles.s_DoPositionHandle_AxisHandlesOctant[axis] = (double) cameraViewFrom[axis] <= 0.00999999977648258 ? 1f : -1f;
                break;
            }
          }
          bool flag3 = flag2 && ids[axis] == GUIUtility.hotControl;
          Color colorByAxis = Handles.GetColorByAxis(axis);
          Handles.color = !flag1 ? colorByAxis : Color.Lerp(colorByAxis, Handles.staticColor, Handles.staticBlend);
          GUI.SetNextControlName(Handles.s_DoPositionHandle_Internal_AxisNames[axis]);
          float t = !flag3 ? Handles.s_DoPositionHandle_Internal_CameraViewLerp[axis] : 0.0f;
          if ((double) t <= 0.899999976158142)
          {
            Handles.color = Color.Lerp(Handles.color, Color.clear, t);
            Vector3 axisVector = Handles.GetAxisVector(axis);
            Vector3 vector3_4 = rotation * axisVector;
            Vector3 vector3_5 = vector3_4 * vector3_1[axis] * handleSize;
            Vector3 vector3_6 = vector3_4 * Handles.s_DoPositionHandle_AxisHandlesOctant[axis];
            Vector3 vector3_7 = vector3_5 * Handles.s_DoPositionHandle_AxisHandlesOctant[axis];
            if (flag2 && !flag3)
              Handles.color = Handles.s_DisabledHandleColor;
            if (flag2 && (ids[Handles.s_DoPositionHandle_Internal_PrevPlaneIndex[axis]] == GUIUtility.hotControl || ids[axis + 3] == GUIUtility.hotControl))
              Handles.color = Handles.selectedColor;
            Handles.s_DoPositionHandle_ArrowCapConeOffset = !flag2 ? Vector3.zero : rotation * Vector3.Scale(Vector3.Scale(axisVector, param.axisOffset), Handles.s_DoPositionHandle_AxisHandlesOctant);
            int id = ids[axis];
            Vector3 position1 = position;
            Vector3 offset = vector3_7;
            Vector3 direction = vector3_6;
            double num1 = (double) handleSize * (double) param.axisSize[axis];
            // ISSUE: reference to a compiler-generated field
            if (Handles.\u003C\u003Ef__mg\u0024cache2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Handles.\u003C\u003Ef__mg\u0024cache2 = new Handles.CapFunction(Handles.DoPositionHandle_ArrowCap);
            }
            // ISSUE: reference to a compiler-generated field
            Handles.CapFunction fMgCache2 = Handles.\u003C\u003Ef__mg\u0024cache2;
            double num2 = !GridSnapping.active ? (double) SnapSettings.move[axis] : 0.0;
            position = Handles.Slider(id, position1, offset, direction, (float) num1, fMgCache2, (float) num2);
          }
        }
      }
      VertexSnapping.HandleKeyAndMouseMove(ids.xyz);
      if (param.ShouldShow(Handles.PositionHandleParam.Handle.XYZ) && (flag2 && ids.xyz == GUIUtility.hotControl || !flag2))
      {
        Handles.color = Handles.centerColor;
        GUI.SetNextControlName("FreeMoveAxis");
        int xyz = ids.xyz;
        Vector3 position1 = position;
        Quaternion rotation1 = rotation;
        double num = (double) handleSize * 0.150000005960464;
        Vector3 snap = !GridSnapping.active ? SnapSettings.move : Vector3.zero;
        // ISSUE: reference to a compiler-generated field
        if (Handles.\u003C\u003Ef__mg\u0024cache3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Handles.\u003C\u003Ef__mg\u0024cache3 = new Handles.CapFunction(Handles.RectangleHandleCap);
        }
        // ISSUE: reference to a compiler-generated field
        Handles.CapFunction fMgCache3 = Handles.\u003C\u003Ef__mg\u0024cache3;
        position = Handles.FreeMoveHandle(xyz, position1, rotation1, (float) num, snap, fMgCache3);
      }
      Handles.color = color;
      if (GridSnapping.active)
        position = GridSnapping.Snap(position);
      return position;
    }

    private static Vector3 DoPlanarHandle(int id, int planePrimaryAxis, Vector3 position, Vector3 offset, Quaternion rotation, float handleSize, float cameraLerp, Vector3 viewVectorDrawSpace, Handles.PositionHandleParam.Orientation orientation)
    {
      Vector3 a = offset;
      int index1 = planePrimaryAxis;
      int index2 = (index1 + 1) % 3;
      int axis = (index1 + 2) % 3;
      Color color = Handles.color;
      Handles.color = Tools.s_Hidden || !EditorApplication.isPlaying || !GameObjectUtility.ContainsStatic(Selection.gameObjects) ? Handles.GetColorByAxis(axis) : Handles.staticColor;
      Handles.color = Color.Lerp(Handles.color, Color.clear, cameraLerp);
      bool flag = false;
      if (GUIUtility.hotControl == id)
        Handles.color = Handles.selectedColor;
      else if (HandleUtility.nearestControl == id)
        Handles.color = Handles.preselectionColor;
      else
        flag = true;
      if (!Handles.currentlyDragging)
      {
        if (orientation != Handles.PositionHandleParam.Orientation.Camera)
        {
          if (orientation == Handles.PositionHandleParam.Orientation.Signed)
          {
            Handles.s_PlanarHandlesOctant[index1] = 1f;
            Handles.s_PlanarHandlesOctant[index2] = 1f;
          }
        }
        else
        {
          Handles.s_PlanarHandlesOctant[index1] = (double) viewVectorDrawSpace[index1] <= 0.00999999977648258 ? 1f : -1f;
          Handles.s_PlanarHandlesOctant[index2] = (double) viewVectorDrawSpace[index2] <= 0.00999999977648258 ? 1f : -1f;
        }
      }
      Vector3 b = Handles.s_PlanarHandlesOctant;
      b[axis] = 0.0f;
      Vector3 vector3_1 = rotation * Vector3.Scale(a, b);
      b = rotation * (b * handleSize * 0.5f);
      Vector3 zero = Vector3.zero;
      Vector3 vector3_2 = Vector3.zero;
      Vector3 vector3_3 = Vector3.zero;
      zero[index1] = 1f;
      vector3_2[index2] = 1f;
      vector3_3[axis] = 1f;
      Vector3 vector3_4 = rotation * zero;
      vector3_2 = rotation * vector3_2;
      vector3_3 = rotation * vector3_3;
      Handles.verts[0] = position + vector3_1 + b + (vector3_4 + vector3_2) * handleSize * 0.5f;
      Handles.verts[1] = position + vector3_1 + b + (-vector3_4 + vector3_2) * handleSize * 0.5f;
      Handles.verts[2] = position + vector3_1 + b + (-vector3_4 - vector3_2) * handleSize * 0.5f;
      Handles.verts[3] = position + vector3_1 + b + (vector3_4 - vector3_2) * handleSize * 0.5f;
      Handles.DrawSolidRectangleWithOutline(Handles.verts, !flag ? Handles.color : new Color(Handles.color.r, Handles.color.g, Handles.color.b, 0.1f), Color.clear);
      int id1 = id;
      Vector3 handlePos = position;
      Vector3 offset1 = b + vector3_1;
      Vector3 handleDir = vector3_3;
      Vector3 slideDir1 = vector3_4;
      Vector3 slideDir2 = vector3_2;
      double num1 = (double) handleSize * 0.5;
      // ISSUE: reference to a compiler-generated field
      if (Handles.\u003C\u003Ef__mg\u0024cache4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Handles.\u003C\u003Ef__mg\u0024cache4 = new Handles.CapFunction(Handles.RectangleHandleCap);
      }
      // ISSUE: reference to a compiler-generated field
      Handles.CapFunction fMgCache4 = Handles.\u003C\u003Ef__mg\u0024cache4;
      Vector2 snap = !GridSnapping.active ? new Vector2(SnapSettings.move[index1], SnapSettings.move[index2]) : Vector2.zero;
      int num2 = 0;
      position = Handles.Slider2D(id1, handlePos, offset1, handleDir, slideDir1, slideDir2, (float) num1, fMgCache4, snap, num2 != 0);
      Handles.color = color;
      return position;
    }

    private static void DoPositionHandle_ArrowCap(int controlId, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      Handles.ArrowHandleCap(controlId, position, rotation, size, eventType, Handles.s_DoPositionHandle_ArrowCapConeOffset);
    }

    internal static float DoRadiusHandle(Quaternion rotation, Vector3 position, float radius, bool handlesOnly)
    {
      float num1 = 90f;
      Vector3[] vector3Array = new Vector3[6]{ rotation * Vector3.right, rotation * Vector3.up, rotation * Vector3.forward, rotation * -Vector3.right, rotation * -Vector3.up, rotation * -Vector3.forward };
      Vector3 vector3;
      if (Camera.current.orthographic)
      {
        vector3 = Camera.current.transform.forward;
        if (!handlesOnly)
        {
          Handles.DrawWireDisc(position, vector3, radius);
          for (int index = 0; index < 3; ++index)
          {
            Vector3 normalized = Vector3.Cross(vector3Array[index], vector3).normalized;
            Handles.DrawTwoShadedWireDisc(position, vector3Array[index], normalized, 180f, radius);
          }
        }
      }
      else
      {
        Matrix4x4 matrix4x4 = Matrix4x4.Inverse(Handles.matrix);
        vector3 = position - matrix4x4.MultiplyPoint(Camera.current.transform.position);
        float sqrMagnitude = vector3.sqrMagnitude;
        float num2 = radius * radius;
        float f1 = num2 * num2 / sqrMagnitude;
        float num3 = f1 / num2;
        if ((double) num3 < 1.0)
        {
          float num4 = Mathf.Sqrt(num2 - f1);
          num1 = Mathf.Atan2(num4, Mathf.Sqrt(f1)) * 57.29578f;
          if (!handlesOnly)
            Handles.DrawWireDisc(position - num2 * vector3 / sqrMagnitude, vector3, num4);
        }
        else
          num1 = -1000f;
        if (!handlesOnly)
        {
          for (int index = 0; index < 3; ++index)
          {
            if ((double) num3 < 1.0)
            {
              float a = Vector3.Angle(vector3, vector3Array[index]);
              float num4 = Mathf.Tan((90f - Mathf.Min(a, 180f - a)) * ((float) Math.PI / 180f));
              float f2 = Mathf.Sqrt(f1 + num4 * num4 * f1) / radius;
              if ((double) f2 < 1.0)
              {
                float angle = Mathf.Asin(f2) * 57.29578f;
                Vector3 normalized = Vector3.Cross(vector3Array[index], vector3).normalized;
                Vector3 from = Quaternion.AngleAxis(angle, vector3Array[index]) * normalized;
                Handles.DrawTwoShadedWireDisc(position, vector3Array[index], from, (float) ((90.0 - (double) angle) * 2.0), radius);
              }
              else
                Handles.DrawTwoShadedWireDisc(position, vector3Array[index], radius);
            }
            else
              Handles.DrawTwoShadedWireDisc(position, vector3Array[index], radius);
          }
        }
      }
      Color color1 = Handles.color;
      for (int index = 0; index < 6; ++index)
      {
        int controlId = GUIUtility.GetControlID(Handles.s_RadiusHandleHash, FocusType.Passive);
        float num2 = Vector3.Angle(vector3Array[index], -vector3);
        if ((double) num2 > 5.0 && (double) num2 < 175.0 || GUIUtility.hotControl == controlId)
        {
          Color color2 = color1;
          color2.a = (double) num2 <= (double) num1 + 5.0 ? Mathf.Clamp01(color1.a * 2f) : Mathf.Clamp01((float) ((double) Handles.backfaceAlphaMultiplier * (double) color1.a * 2.0));
          Handles.color = color2;
          Vector3 position1 = position + radius * vector3Array[index];
          bool changed = GUI.changed;
          GUI.changed = false;
          int id = controlId;
          Vector3 position2 = position1;
          Vector3 direction = vector3Array[index];
          double num3 = (double) HandleUtility.GetHandleSize(position1) * 0.0299999993294477;
          // ISSUE: reference to a compiler-generated field
          if (Handles.\u003C\u003Ef__mg\u0024cache5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Handles.\u003C\u003Ef__mg\u0024cache5 = new Handles.CapFunction(Handles.DotHandleCap);
          }
          // ISSUE: reference to a compiler-generated field
          Handles.CapFunction fMgCache5 = Handles.\u003C\u003Ef__mg\u0024cache5;
          double num4 = 0.0;
          Vector3 a = Slider1D.Do(id, position2, direction, (float) num3, fMgCache5, (float) num4);
          if (GUI.changed)
            radius = Vector3.Distance(a, position);
          GUI.changed |= changed;
        }
      }
      Handles.color = color1;
      return radius;
    }

    internal static Vector2 DoRectHandles(Quaternion rotation, Vector3 position, Vector2 size)
    {
      Vector3 vector3_1 = rotation * Vector3.forward;
      Vector3 d1 = rotation * Vector3.up;
      Vector3 d2 = rotation * Vector3.right;
      float r1 = 0.5f * size.x;
      float r2 = 0.5f * size.y;
      Vector3 vector3_2 = position + d1 * r2 + d2 * r1;
      Vector3 vector3_3 = position - d1 * r2 + d2 * r1;
      Vector3 vector3_4 = position - d1 * r2 - d2 * r1;
      Vector3 vector3_5 = position + d1 * r2 - d2 * r1;
      Handles.DrawLine(vector3_2, vector3_3);
      Handles.DrawLine(vector3_3, vector3_4);
      Handles.DrawLine(vector3_4, vector3_5);
      Handles.DrawLine(vector3_5, vector3_2);
      Color color = Handles.color;
      color.a = Mathf.Clamp01(color.a * 2f);
      Handles.color = color;
      float r3 = Handles.SizeSlider(position, d1, r2);
      float num1 = Handles.SizeSlider(position, -d1, r3);
      float r4 = Handles.SizeSlider(position, d2, r1);
      float num2 = Handles.SizeSlider(position, -d2, r4);
      if (Tools.current != Tool.Move && Tools.current != Tool.Scale || Tools.pivotRotation != PivotRotation.Local)
        Handles.DrawLine(position, position + vector3_1);
      size.x = 2f * num2;
      size.y = 2f * num1;
      return size;
    }

    public static Quaternion DoRotationHandle(Quaternion rotation, Vector3 position)
    {
      return Handles.DoRotationHandle(Handles.RotationHandleIds.@default, rotation, position, Handles.RotationHandleParam.Default);
    }

    internal static Quaternion DoRotationHandle(Handles.RotationHandleIds ids, Quaternion rotation, Vector3 position, Handles.RotationHandleParam param)
    {
      Event current = Event.current;
      Transform transform = Camera.current.transform;
      float handleSize = HandleUtility.GetHandleSize(position);
      Color color = Handles.color;
      bool flag1 = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
      bool flag2 = ids.Has(GUIUtility.hotControl);
      if (!flag1 && param.ShouldShow(Handles.RotationHandleParam.Handle.XYZ) && (flag2 && ids.xyz == GUIUtility.hotControl || !flag2))
      {
        Handles.color = Handles.centerColor;
        rotation = FreeRotate.Do(ids.xyz, rotation, position, handleSize * param.xyzSize, param.displayXYZCircle);
      }
      float num1 = -1f;
      for (int axis = 0; axis < 3; ++axis)
      {
        if (param.ShouldShow(axis))
        {
          Color colorByAxis = Handles.GetColorByAxis(axis);
          Handles.color = !flag1 ? colorByAxis : Color.Lerp(colorByAxis, Handles.staticColor, Handles.staticBlend);
          Vector3 axisVector = Handles.GetAxisVector(axis);
          float num2 = handleSize * param.axisSize[axis];
          num1 = Mathf.Max(num2, num1);
          rotation = Disc.Do(ids[axis], rotation, position, rotation * axisVector, num2, true, SnapSettings.rotation, param.enableRayDrag, true, Handles.k_RotationPieColor);
        }
      }
      if ((double) num1 > 0.0 && current.type == EventType.Repaint)
      {
        Handles.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        Handles.DrawWireDisc(position, transform.forward, num1);
      }
      if (flag2 && current.type == EventType.Repaint)
      {
        Handles.color = Handles.s_DisabledHandleColor;
        Handles.DrawWireDisc(position, transform.forward, handleSize * param.axisSize[0]);
      }
      if (!flag1 && param.ShouldShow(Handles.RotationHandleParam.Handle.CameraAxis) && (flag2 && ids.cameraAxis == GUIUtility.hotControl || !flag2))
      {
        Handles.color = Handles.centerColor;
        rotation = Disc.Do(ids.cameraAxis, rotation, position, Camera.current.transform.forward, handleSize * param.cameraAxisSize, false, 0.0f, param.enableRayDrag, true, Handles.k_RotationPieColor);
      }
      Handles.color = color;
      return rotation;
    }

    public static Vector3 DoScaleHandle(Vector3 scale, Vector3 position, Quaternion rotation, float size)
    {
      return Handles.DoScaleHandle(Handles.ScaleHandleIds.@default, scale, position, rotation, size, Handles.ScaleHandleParam.Default);
    }

    internal static Vector3 DoScaleHandle(Handles.ScaleHandleIds ids, Vector3 scale, Vector3 position, Quaternion rotation, float handleSize, Handles.ScaleHandleParam param)
    {
      Vector3 cameraViewFrom = Handles.GetCameraViewFrom(Handles.matrix.MultiplyPoint3x4(position), (Handles.matrix * Matrix4x4.TRS(position, rotation, Vector3.one)).inverse);
      bool flag1 = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
      bool flag2 = ids.Has(GUIUtility.hotControl);
      Vector3 vector3_1 = param.axisOffset;
      Vector3 axisLineScale = param.axisLineScale;
      if (flag2)
      {
        axisLineScale += vector3_1;
        vector3_1 = Vector3.zero;
      }
      bool flag3 = ids.xyz == GUIUtility.hotControl;
      for (int axis = 0; axis < 3; ++axis)
      {
        if (param.ShouldShow(axis))
        {
          if (!Handles.currentlyDragging)
          {
            switch (param.orientation)
            {
              case Handles.ScaleHandleParam.Orientation.Signed:
                Handles.s_DoScaleHandle_AxisHandlesOctant[axis] = 1f;
                break;
              case Handles.ScaleHandleParam.Orientation.Camera:
                Handles.s_DoScaleHandle_AxisHandlesOctant[axis] = (double) cameraViewFrom[axis] <= 0.00999999977648258 ? 1f : -1f;
                break;
            }
          }
          int id = ids[axis];
          bool flag4 = flag2 && id == GUIUtility.hotControl;
          Vector3 axisVector = Handles.GetAxisVector(axis);
          Color colorByAxis = Handles.GetColorByAxis(axis);
          float handleOffset = vector3_1[axis];
          float num = id != GUIUtility.hotControl ? Handles.GetCameraViewLerpForWorldAxis(cameraViewFrom, axisVector) : 0.0f;
          float t = !flag2 ? num : 0.0f;
          Handles.color = !flag1 ? colorByAxis : Color.Lerp(colorByAxis, Handles.staticColor, Handles.staticBlend);
          Vector3 vector3_2 = axisVector * Handles.s_DoScaleHandle_AxisHandlesOctant[axis];
          if ((double) t <= 0.899999976158142)
          {
            Handles.color = Color.Lerp(Handles.color, Color.clear, t);
            if (flag2 && !flag4)
              Handles.color = Handles.s_DisabledHandleColor;
            if (flag3)
              Handles.color = Handles.selectedColor;
            scale[axis] = SliderScale.DoAxis(id, scale[axis], position, rotation * vector3_2, rotation, handleSize * param.axisSize[axis], SnapSettings.scale, handleOffset, axisLineScale[axis]);
          }
        }
      }
      if (param.ShouldShow(Handles.ScaleHandleParam.Handle.XYZ) && (flag2 && ids.xyz == GUIUtility.hotControl || !flag2))
      {
        Handles.color = Handles.centerColor;
        EditorGUI.BeginChangeCheck();
        int xyz = ids.xyz;
        double x = (double) scale.x;
        Vector3 position1 = position;
        Quaternion rotation1 = rotation;
        double num1 = (double) handleSize * (double) param.xyzSize;
        // ISSUE: reference to a compiler-generated field
        if (Handles.\u003C\u003Ef__mg\u0024cache6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Handles.\u003C\u003Ef__mg\u0024cache6 = new Handles.CapFunction(Handles.CubeHandleCap);
        }
        // ISSUE: reference to a compiler-generated field
        Handles.CapFunction fMgCache6 = Handles.\u003C\u003Ef__mg\u0024cache6;
        double scale1 = (double) SnapSettings.scale;
        float num2 = Handles.ScaleValueHandle(xyz, (float) x, position1, rotation1, (float) num1, fMgCache6, (float) scale1);
        if (EditorGUI.EndChangeCheck() && !Mathf.Approximately(scale.x, 0.0f))
        {
          float num3 = num2 / scale.x;
          scale.x = num2;
          scale.y *= num3;
          scale.z *= num3;
        }
      }
      return scale;
    }

    internal static float DoSimpleEdgeHandle(Quaternion rotation, Vector3 position, float radius)
    {
      Vector3 d = rotation * Vector3.right;
      EditorGUI.BeginChangeCheck();
      radius = Handles.SizeSlider(position, d, radius);
      radius = Handles.SizeSlider(position, -d, radius);
      if (EditorGUI.EndChangeCheck())
        radius = Mathf.Max(0.0f, radius);
      if ((double) radius > 0.0)
        Handles.DrawLine(position - d * radius, position + d * radius);
      return radius;
    }

    internal static float DoSimpleRadiusHandle(Quaternion rotation, Vector3 position, float radius, bool hemisphere)
    {
      Vector3 vector3_1 = rotation * Vector3.forward;
      Vector3 vector3_2 = rotation * Vector3.up;
      Vector3 vector3_3 = rotation * Vector3.right;
      bool changed1 = GUI.changed;
      GUI.changed = false;
      radius = Handles.SizeSlider(position, vector3_1, radius);
      if (!hemisphere)
        radius = Handles.SizeSlider(position, -vector3_1, radius);
      if (GUI.changed)
        radius = Mathf.Max(0.0f, radius);
      GUI.changed |= changed1;
      bool changed2 = GUI.changed;
      GUI.changed = false;
      radius = Handles.SizeSlider(position, vector3_2, radius);
      radius = Handles.SizeSlider(position, -vector3_2, radius);
      radius = Handles.SizeSlider(position, vector3_3, radius);
      radius = Handles.SizeSlider(position, -vector3_3, radius);
      if (GUI.changed)
        radius = Mathf.Max(0.0f, radius);
      GUI.changed |= changed2;
      if ((double) radius > 0.0)
      {
        Handles.DrawWireDisc(position, vector3_1, radius);
        Handles.DrawWireArc(position, vector3_2, -vector3_3, !hemisphere ? 360f : 180f, radius);
        Handles.DrawWireArc(position, vector3_3, vector3_2, !hemisphere ? 360f : 180f, radius);
      }
      return radius;
    }

    internal static void TransformHandle(Handles.TransformHandleIds ids, ref Vector3 position, ref Quaternion rotation, ref Vector3 scale, Handles.TransformHandleParam param)
    {
      Quaternion rotation1 = rotation;
      Handles.PositionHandleParam positionHandleParam = param.position;
      Handles.RotationHandleParam rotationHandleParam = param.rotation;
      Handles.ScaleHandleParam scaleHandleParam = param.scale;
      bool flag1 = ids.Has(GUIUtility.hotControl);
      bool flag2 = flag1 && Handles.s_IsHotInCameraAlignedMode || !flag1 && Event.current.shift;
      if (Tools.vertexDragging)
      {
        positionHandleParam = param.vertexSnappingPosition;
        rotationHandleParam = param.vertexSnappingRotation;
        scaleHandleParam = param.vertexSnappingScale;
      }
      else if (flag2)
      {
        rotation1 = Camera.current.transform.rotation;
        positionHandleParam = param.cameraAlignedPosition;
        rotationHandleParam = param.cameraAlignedRotation;
        scaleHandleParam = param.cameraAlignedScale;
      }
      else if (Tools.pivotRotation == PivotRotation.Local)
      {
        positionHandleParam = param.localPosition;
        rotationHandleParam = param.localRotation;
        scaleHandleParam = param.localScale;
      }
      if (ids.Has(GUIUtility.hotControl))
      {
        if (ids.position.Has(GUIUtility.hotControl))
          position = Handles.DoPositionHandle_Internal(ids.position, position, rotation1, positionHandleParam);
        else if (ids.rotation.Has(GUIUtility.hotControl))
        {
          Quaternion quaternion1 = Handles.DoRotationHandle(ids.rotation, rotation1, position, rotationHandleParam);
          if (flag2)
          {
            if (!Handles.s_TransformHandle_RotationData.ContainsKey(ids.rotation))
              Handles.s_TransformHandle_RotationData[ids.rotation] = new Handles.RotationHandleData()
              {
                rotationStarted = true,
                initialRotation = rotation
              };
            Quaternion quaternion2 = ids.rotation.xyz == GUIUtility.hotControl ? rotation : Handles.s_TransformHandle_RotationData[ids.rotation].initialRotation;
            float angle;
            Vector3 axis;
            (quaternion1 * Quaternion.Inverse(rotation1)).ToAngleAxis(out angle, out axis);
            rotation = Quaternion.AngleAxis(angle, axis) * quaternion2;
          }
          else
            rotation = quaternion1;
        }
        else if (ids.scale.Has(GUIUtility.hotControl))
          scale = Handles.DoScaleHandle(ids.scale, scale, position, rotation1, HandleUtility.GetHandleSize(position), scaleHandleParam);
      }
      else
      {
        if (Handles.s_TransformHandle_RotationData.ContainsKey(ids.rotation))
        {
          Handles.RotationHandleData rotationHandleData = Handles.s_TransformHandle_RotationData[ids.rotation];
          rotationHandleData.rotationStarted = false;
          Handles.s_TransformHandle_RotationData[ids.rotation] = rotationHandleData;
        }
        Handles.DoRotationHandle(ids.rotation, rotation1, position, rotationHandleParam);
        Handles.DoPositionHandle_Internal(ids.position, position, rotation1, positionHandleParam);
        Handles.DoScaleHandle(ids.scale, scale, position, rotation1, HandleUtility.GetHandleSize(position), scaleHandleParam);
      }
      bool flag3 = ids.Has(GUIUtility.hotControl);
      if (flag3 && !flag1 && flag2)
      {
        Handles.s_IsHotInCameraAlignedMode = true;
      }
      else
      {
        if (!Handles.s_IsHotInCameraAlignedMode || flag1 && flag3)
          return;
        Handles.s_IsHotInCameraAlignedMode = false;
      }
    }

    internal enum FilterMode
    {
      Off,
      ShowFiltered,
      ShowRest,
    }

    /// <summary>
    ///   <para>Disposable helper struct for automatically setting and reverting Handles.color and/or Handles.matrix.</para>
    /// </summary>
    public struct DrawingScope : IDisposable
    {
      private bool m_Disposed;
      private Color m_OriginalColor;
      private Matrix4x4 m_OriginalMatrix;

      /// <summary>
      ///   <para>Create a new DrawingScope and set Handles.color and/or Handles.matrix to the specified values.</para>
      /// </summary>
      /// <param name="matrix">The matrix to use for displaying Handles inside the scope block.</param>
      /// <param name="color">The color to use for displaying Handles inside the scope block.</param>
      public DrawingScope(Color color)
      {
        this = new Handles.DrawingScope(color, Handles.matrix);
      }

      /// <summary>
      ///   <para>Create a new DrawingScope and set Handles.color and/or Handles.matrix to the specified values.</para>
      /// </summary>
      /// <param name="matrix">The matrix to use for displaying Handles inside the scope block.</param>
      /// <param name="color">The color to use for displaying Handles inside the scope block.</param>
      public DrawingScope(Matrix4x4 matrix)
      {
        this = new Handles.DrawingScope(Handles.color, matrix);
      }

      /// <summary>
      ///   <para>Create a new DrawingScope and set Handles.color and/or Handles.matrix to the specified values.</para>
      /// </summary>
      /// <param name="matrix">The matrix to use for displaying Handles inside the scope block.</param>
      /// <param name="color">The color to use for displaying Handles inside the scope block.</param>
      public DrawingScope(Color color, Matrix4x4 matrix)
      {
        this.m_Disposed = false;
        this.m_OriginalColor = Handles.color;
        this.m_OriginalMatrix = Handles.matrix;
        Handles.matrix = matrix;
        Handles.color = color;
      }

      /// <summary>
      ///   <para>The value of Handles.color at the time this DrawingScope was created.</para>
      /// </summary>
      public Color originalColor
      {
        get
        {
          return this.m_OriginalColor;
        }
      }

      /// <summary>
      ///   <para>The value of Handles.matrix at the time this DrawingScope was created.</para>
      /// </summary>
      public Matrix4x4 originalMatrix
      {
        get
        {
          return this.m_OriginalMatrix;
        }
      }

      /// <summary>
      ///   <para>Automatically reverts Handles.color and Handles.matrix to their values prior to entering the scope, when the scope is exited. You do not need to call this method manually.</para>
      /// </summary>
      public void Dispose()
      {
        if (this.m_Disposed)
          return;
        this.m_Disposed = true;
        Handles.color = this.m_OriginalColor;
        Handles.matrix = this.m_OriginalMatrix;
      }
    }

    /// <summary>
    ///   <para>The function to use for drawing the handle e.g. Handles.RectangleCap.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
    /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
    /// <param name="size">The size of the handle in world-space units.</param>
    /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout and EventType.Repaint events.</param>
    public delegate void CapFunction(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType);

    [Obsolete("This delegate is obsolete. Use CapFunction instead.")]
    public delegate void DrawCapFunction(int controlID, Vector3 position, Quaternion rotation, float size);

    /// <summary>
    ///   <para>A delegate type for getting a handle's size based on its current position.</para>
    /// </summary>
    /// <param name="position">The current position of the handle in the space of Handles.matrix.</param>
    public delegate float SizeFunction(Vector3 position);

    internal struct PositionHandleIds
    {
      public readonly int x;
      public readonly int y;
      public readonly int z;
      public readonly int xy;
      public readonly int yz;
      public readonly int xz;
      public readonly int xyz;

      public PositionHandleIds(int x, int y, int z, int xy, int xz, int yz, int xyz)
      {
        this.x = x;
        this.y = y;
        this.z = z;
        this.xy = xy;
        this.yz = yz;
        this.xz = xz;
        this.xyz = xyz;
      }

      public static Handles.PositionHandleIds @default
      {
        get
        {
          return new Handles.PositionHandleIds(GUIUtility.GetControlID(Handles.s_xAxisMoveHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_yAxisMoveHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_zAxisMoveHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_xyAxisMoveHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_xzAxisMoveHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_yzAxisMoveHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_FreeMoveHandleHash, FocusType.Passive));
        }
      }

      public int this[int index]
      {
        get
        {
          switch (index)
          {
            case 0:
              return this.x;
            case 1:
              return this.y;
            case 2:
              return this.z;
            case 3:
              return this.xy;
            case 4:
              return this.yz;
            case 5:
              return this.xz;
            case 6:
              return this.xyz;
            default:
              return -1;
          }
        }
      }

      public bool Has(int id)
      {
        return this.x == id || this.y == id || (this.z == id || this.xy == id) || (this.yz == id || this.xz == id) || this.xyz == id;
      }

      public override int GetHashCode()
      {
        return this.x ^ this.y ^ this.z ^ this.xy ^ this.xz ^ this.yz ^ this.xyz;
      }

      public override bool Equals(object obj)
      {
        if (!(obj is Handles.PositionHandleIds))
          return false;
        Handles.PositionHandleIds positionHandleIds = (Handles.PositionHandleIds) obj;
        return positionHandleIds.x == this.x && positionHandleIds.y == this.y && (positionHandleIds.z == this.z && positionHandleIds.xy == this.xy) && (positionHandleIds.xz == this.xz && positionHandleIds.yz == this.yz) && positionHandleIds.xyz == this.xyz;
      }
    }

    internal struct PositionHandleParam
    {
      public static Handles.PositionHandleParam DefaultHandle = new Handles.PositionHandleParam(Handles.PositionHandleParam.Handle.X | Handles.PositionHandleParam.Handle.Y | Handles.PositionHandleParam.Handle.Z | Handles.PositionHandleParam.Handle.XY | Handles.PositionHandleParam.Handle.YZ | Handles.PositionHandleParam.Handle.XZ, Vector3.zero, Vector3.one, Vector3.zero, Vector3.one * 0.25f, Handles.PositionHandleParam.Orientation.Signed, Handles.PositionHandleParam.Orientation.Camera);
      public static Handles.PositionHandleParam DefaultFreeMoveHandle = new Handles.PositionHandleParam(Handles.PositionHandleParam.Handle.X | Handles.PositionHandleParam.Handle.Y | Handles.PositionHandleParam.Handle.Z | Handles.PositionHandleParam.Handle.XYZ, Vector3.zero, Vector3.one, Vector3.zero, Vector3.one * 0.25f, Handles.PositionHandleParam.Orientation.Signed, Handles.PositionHandleParam.Orientation.Signed);
      public readonly Vector3 axisOffset;
      public readonly Vector3 axisSize;
      public readonly Vector3 planeOffset;
      public readonly Vector3 planeSize;
      public readonly Handles.PositionHandleParam.Handle handles;
      public readonly Handles.PositionHandleParam.Orientation axesOrientation;
      public readonly Handles.PositionHandleParam.Orientation planeOrientation;

      public PositionHandleParam(Handles.PositionHandleParam.Handle handles, Vector3 axisOffset, Vector3 axisSize, Vector3 planeOffset, Vector3 planeSize, Handles.PositionHandleParam.Orientation axesOrientation, Handles.PositionHandleParam.Orientation planeOrientation)
      {
        this.axisOffset = axisOffset;
        this.axisSize = axisSize;
        this.planeOffset = planeOffset;
        this.planeSize = planeSize;
        this.handles = handles;
        this.axesOrientation = axesOrientation;
        this.planeOrientation = planeOrientation;
      }

      public bool ShouldShow(int axis)
      {
        return (this.handles & (Handles.PositionHandleParam.Handle) (1 << axis)) != (Handles.PositionHandleParam.Handle) 0;
      }

      public bool ShouldShow(Handles.PositionHandleParam.Handle handle)
      {
        return (this.handles & handle) != (Handles.PositionHandleParam.Handle) 0;
      }

      [System.Flags]
      public enum Handle
      {
        X = 1,
        Y = 2,
        Z = 4,
        XY = 8,
        YZ = 16, // 0x00000010
        XZ = 32, // 0x00000020
        XYZ = 64, // 0x00000040
      }

      public enum Orientation
      {
        Signed,
        Camera,
      }
    }

    internal struct RotationHandleIds
    {
      public readonly int x;
      public readonly int y;
      public readonly int z;
      public readonly int cameraAxis;
      public readonly int xyz;

      public RotationHandleIds(int x, int y, int z, int cameraAxis, int xyz)
      {
        this.x = x;
        this.y = y;
        this.z = z;
        this.cameraAxis = cameraAxis;
        this.xyz = xyz;
      }

      public static Handles.RotationHandleIds @default
      {
        get
        {
          return new Handles.RotationHandleIds(GUIUtility.GetControlID(Handles.s_xRotateHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_yRotateHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_zRotateHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_cameraAxisRotateHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_xyzRotateHandleHash, FocusType.Passive));
        }
      }

      public int this[int index]
      {
        get
        {
          switch (index)
          {
            case 0:
              return this.x;
            case 1:
              return this.y;
            case 2:
              return this.z;
            case 3:
              return this.cameraAxis;
            case 4:
              return this.xyz;
            default:
              return -1;
          }
        }
      }

      public bool Has(int id)
      {
        return this.x == id || this.y == id || (this.z == id || this.cameraAxis == id) || this.xyz == id;
      }

      public override int GetHashCode()
      {
        return this.x ^ this.y ^ this.z ^ this.cameraAxis ^ this.xyz;
      }

      public override bool Equals(object obj)
      {
        if (!(obj is Handles.RotationHandleIds))
          return false;
        Handles.RotationHandleIds rotationHandleIds = (Handles.RotationHandleIds) obj;
        return rotationHandleIds.x == this.x && rotationHandleIds.y == this.y && (rotationHandleIds.z == this.z && rotationHandleIds.cameraAxis == this.cameraAxis) && rotationHandleIds.xyz == this.xyz;
      }
    }

    internal struct RotationHandleParam
    {
      private static Handles.RotationHandleParam s_Default = new Handles.RotationHandleParam((Handles.RotationHandleParam.Handle) -1, Vector3.one, 1f, 1.1f, true, false);
      public readonly Vector3 axisSize;
      public readonly float cameraAxisSize;
      public readonly float xyzSize;
      public readonly Handles.RotationHandleParam.Handle handles;
      public readonly bool enableRayDrag;
      public readonly bool displayXYZCircle;

      public RotationHandleParam(Handles.RotationHandleParam.Handle handles, Vector3 axisSize, float xyzSize, float cameraAxisSize, bool enableRayDrag, bool displayXYZCircle)
      {
        this.axisSize = axisSize;
        this.xyzSize = xyzSize;
        this.handles = handles;
        this.cameraAxisSize = cameraAxisSize;
        this.enableRayDrag = enableRayDrag;
        this.displayXYZCircle = displayXYZCircle;
      }

      public static Handles.RotationHandleParam Default
      {
        get
        {
          return Handles.RotationHandleParam.s_Default;
        }
        set
        {
          Handles.RotationHandleParam.s_Default = value;
        }
      }

      public bool ShouldShow(int axis)
      {
        return (this.handles & (Handles.RotationHandleParam.Handle) (1 << axis)) != (Handles.RotationHandleParam.Handle) 0;
      }

      public bool ShouldShow(Handles.RotationHandleParam.Handle handle)
      {
        return (this.handles & handle) != (Handles.RotationHandleParam.Handle) 0;
      }

      [System.Flags]
      public enum Handle
      {
        X = 1,
        Y = 2,
        Z = 4,
        CameraAxis = 8,
        XYZ = 16, // 0x00000010
      }
    }

    internal struct ScaleHandleIds
    {
      public readonly int x;
      public readonly int y;
      public readonly int z;
      public readonly int xyz;

      public ScaleHandleIds(int x, int y, int z, int xyz)
      {
        this.x = x;
        this.y = y;
        this.z = z;
        this.xyz = xyz;
      }

      public static Handles.ScaleHandleIds @default
      {
        get
        {
          return new Handles.ScaleHandleIds(GUIUtility.GetControlID(Handles.s_xScaleHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_yScaleHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_zScaleHandleHash, FocusType.Passive), GUIUtility.GetControlID(Handles.s_xyzScaleHandleHash, FocusType.Passive));
        }
      }

      public int this[int index]
      {
        get
        {
          switch (index)
          {
            case 0:
              return this.x;
            case 1:
              return this.y;
            case 2:
              return this.z;
            case 3:
              return this.xyz;
            default:
              return -1;
          }
        }
      }

      public bool Has(int id)
      {
        return this.x == id || this.y == id || this.z == id || this.xyz == id;
      }

      public override int GetHashCode()
      {
        return this.x ^ this.y ^ this.z ^ this.xyz;
      }

      public override bool Equals(object obj)
      {
        if (!(obj is Handles.ScaleHandleIds))
          return false;
        Handles.ScaleHandleIds scaleHandleIds = (Handles.ScaleHandleIds) obj;
        return scaleHandleIds.x == this.x && scaleHandleIds.y == this.y && scaleHandleIds.z == this.z && scaleHandleIds.xyz == this.xyz;
      }
    }

    internal struct ScaleHandleParam
    {
      private static Handles.ScaleHandleParam s_Default = new Handles.ScaleHandleParam((Handles.ScaleHandleParam.Handle) -1, Vector3.zero, Vector3.one, Vector3.one, 1f, Handles.ScaleHandleParam.Orientation.Signed);
      public readonly Vector3 axisOffset;
      public readonly Vector3 axisSize;
      public readonly Vector3 axisLineScale;
      public readonly float xyzSize;
      public readonly Handles.ScaleHandleParam.Handle handles;
      public readonly Handles.ScaleHandleParam.Orientation orientation;

      public ScaleHandleParam(Handles.ScaleHandleParam.Handle handles, Vector3 axisOffset, Vector3 axisSize, Vector3 axisLineScale, float xyzSize, Handles.ScaleHandleParam.Orientation orientation)
      {
        this.axisOffset = axisOffset;
        this.axisSize = axisSize;
        this.axisLineScale = axisLineScale;
        this.xyzSize = xyzSize;
        this.handles = handles;
        this.orientation = orientation;
      }

      public static Handles.ScaleHandleParam Default
      {
        get
        {
          return Handles.ScaleHandleParam.s_Default;
        }
        set
        {
          Handles.ScaleHandleParam.s_Default = value;
        }
      }

      public bool ShouldShow(int axis)
      {
        return (this.handles & (Handles.ScaleHandleParam.Handle) (1 << axis)) != (Handles.ScaleHandleParam.Handle) 0;
      }

      public bool ShouldShow(Handles.ScaleHandleParam.Handle handle)
      {
        return (this.handles & handle) != (Handles.ScaleHandleParam.Handle) 0;
      }

      [System.Flags]
      public enum Handle
      {
        X = 1,
        Y = 2,
        Z = 4,
        XYZ = 8,
      }

      public enum Orientation
      {
        Signed,
        Camera,
      }
    }

    internal struct TransformHandleIds
    {
      private static readonly int s_TransformTranslationXHash = "TransformTranslationXHash".GetHashCode();
      private static readonly int s_TransformTranslationYHash = "TransformTranslationYHash".GetHashCode();
      private static readonly int s_TransformTranslationZHash = "TransformTranslationZHash".GetHashCode();
      private static readonly int s_TransformTranslationXYHash = "TransformTranslationXYHash".GetHashCode();
      private static readonly int s_TransformTranslationXZHash = "TransformTranslationXZHash".GetHashCode();
      private static readonly int s_TransformTranslationYZHash = "TransformTranslationYZHash".GetHashCode();
      private static readonly int s_TransformTranslationXYZHash = "TransformTranslationXYZHash".GetHashCode();
      private static readonly int s_TransformRotationXHash = "TransformRotationXHash".GetHashCode();
      private static readonly int s_TransformRotationYHash = "TransformRotationYHash".GetHashCode();
      private static readonly int s_TransformRotationZHash = "TransformRotationZHash".GetHashCode();
      private static readonly int s_TransformRotationCameraAxisHash = "TransformRotationCameraAxisHash".GetHashCode();
      private static readonly int s_TransformRotationXYZHash = "TransformRotationXYZHash".GetHashCode();
      private static readonly int s_TransformScaleXHash = "TransformScaleXHash".GetHashCode();
      private static readonly int s_TransformScaleYHash = "TransformScaleYHash".GetHashCode();
      private static readonly int s_TransformScaleZHash = "TransformScaleZHash".GetHashCode();
      private static readonly int s_TransformScaleXYZHash = "TransformScaleXYZHash".GetHashCode();
      public readonly Handles.PositionHandleIds position;
      public readonly Handles.RotationHandleIds rotation;
      public readonly Handles.ScaleHandleIds scale;

      public TransformHandleIds(Handles.PositionHandleIds position, Handles.RotationHandleIds rotation, Handles.ScaleHandleIds scale)
      {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
      }

      public static Handles.TransformHandleIds Default
      {
        get
        {
          return new Handles.TransformHandleIds(new Handles.PositionHandleIds(GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformTranslationXHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformTranslationYHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformTranslationZHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformTranslationXYHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformTranslationXZHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformTranslationYZHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformTranslationXYZHash, FocusType.Passive)), new Handles.RotationHandleIds(GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformRotationXHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformRotationYHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformRotationZHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformRotationCameraAxisHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformRotationXYZHash, FocusType.Passive)), new Handles.ScaleHandleIds(GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformScaleXHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformScaleYHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformScaleZHash, FocusType.Passive), GUIUtility.GetControlID(Handles.TransformHandleIds.s_TransformScaleXYZHash, FocusType.Passive)));
        }
      }

      public bool Has(int id)
      {
        return this.position.Has(id) || this.rotation.Has(id) || this.scale.Has(id);
      }
    }

    internal struct TransformHandleParam
    {
      private static Handles.TransformHandleParam s_Default = new Handles.TransformHandleParam(new Handles.PositionHandleParam(Handles.PositionHandleParam.Handle.X | Handles.PositionHandleParam.Handle.Y | Handles.PositionHandleParam.Handle.Z | Handles.PositionHandleParam.Handle.XY | Handles.PositionHandleParam.Handle.YZ | Handles.PositionHandleParam.Handle.XZ, Vector3.one * 0.15f, Vector3.one, Vector3.zero, Vector3.one * 0.375f, Handles.PositionHandleParam.Orientation.Signed, Handles.PositionHandleParam.Orientation.Camera), new Handles.RotationHandleParam(Handles.RotationHandleParam.Handle.X | Handles.RotationHandleParam.Handle.Y | Handles.RotationHandleParam.Handle.Z | Handles.RotationHandleParam.Handle.CameraAxis | Handles.RotationHandleParam.Handle.XYZ, Vector3.one * 1.4f, 1.4f, 1.5f, false, false), new Handles.ScaleHandleParam(Handles.ScaleHandleParam.Handle.XYZ, Vector3.zero, Vector3.one, Vector3.one, 1f, Handles.ScaleHandleParam.Orientation.Signed), new Handles.PositionHandleParam(Handles.PositionHandleParam.Handle.X | Handles.PositionHandleParam.Handle.Y | Handles.PositionHandleParam.Handle.XY, Vector3.one * 0.15f, Vector3.one, Vector3.zero, Vector3.one * 0.375f, Handles.PositionHandleParam.Orientation.Signed, Handles.PositionHandleParam.Orientation.Signed), new Handles.RotationHandleParam(Handles.RotationHandleParam.Handle.Z | Handles.RotationHandleParam.Handle.XYZ, Vector3.one * 1.4f, 1.4f, 1.5f, false, false), new Handles.ScaleHandleParam(Handles.ScaleHandleParam.Handle.XYZ, Vector3.zero, Vector3.one, Vector3.one, 1f, Handles.ScaleHandleParam.Orientation.Signed), new Handles.PositionHandleParam(Handles.PositionHandleParam.Handle.X | Handles.PositionHandleParam.Handle.Y | Handles.PositionHandleParam.Handle.Z | Handles.PositionHandleParam.Handle.XY | Handles.PositionHandleParam.Handle.YZ | Handles.PositionHandleParam.Handle.XZ, Vector3.one * 0.15f, Vector3.one, Vector3.zero, Vector3.one * 0.375f, Handles.PositionHandleParam.Orientation.Signed, Handles.PositionHandleParam.Orientation.Camera), new Handles.RotationHandleParam(Handles.RotationHandleParam.Handle.X | Handles.RotationHandleParam.Handle.Y | Handles.RotationHandleParam.Handle.Z | Handles.RotationHandleParam.Handle.CameraAxis | Handles.RotationHandleParam.Handle.XYZ, Vector3.one * 1.4f, 1.4f, 1.5f, false, false), new Handles.ScaleHandleParam(Handles.ScaleHandleParam.Handle.X | Handles.ScaleHandleParam.Handle.Y | Handles.ScaleHandleParam.Handle.Z | Handles.ScaleHandleParam.Handle.XYZ, Vector3.one * 1.5f, Vector3.one, Vector3.one * 0.25f, 1f, Handles.ScaleHandleParam.Orientation.Signed), new Handles.PositionHandleParam(Handles.PositionHandleParam.Handle.X | Handles.PositionHandleParam.Handle.Y | Handles.PositionHandleParam.Handle.Z | Handles.PositionHandleParam.Handle.XYZ, Vector3.one * 0.15f, Vector3.one, Vector3.zero, Vector3.one * 0.375f, Handles.PositionHandleParam.Orientation.Signed, Handles.PositionHandleParam.Orientation.Signed), new Handles.RotationHandleParam((Handles.RotationHandleParam.Handle) 0, Vector3.one * 1.4f, 1.4f, 1.5f, false, false), new Handles.ScaleHandleParam((Handles.ScaleHandleParam.Handle) 0, Vector3.one * 1.5f, Vector3.one, Vector3.one * 0.25f, 1f, Handles.ScaleHandleParam.Orientation.Signed));
      public readonly Handles.PositionHandleParam position;
      public readonly Handles.RotationHandleParam rotation;
      public readonly Handles.ScaleHandleParam scale;
      public readonly Handles.PositionHandleParam cameraAlignedPosition;
      public readonly Handles.RotationHandleParam cameraAlignedRotation;
      public readonly Handles.ScaleHandleParam cameraAlignedScale;
      public readonly Handles.PositionHandleParam localPosition;
      public readonly Handles.RotationHandleParam localRotation;
      public readonly Handles.ScaleHandleParam localScale;
      public readonly Handles.PositionHandleParam vertexSnappingPosition;
      public readonly Handles.RotationHandleParam vertexSnappingRotation;
      public readonly Handles.ScaleHandleParam vertexSnappingScale;

      public TransformHandleParam(Handles.PositionHandleParam position, Handles.RotationHandleParam rotation, Handles.ScaleHandleParam scale, Handles.PositionHandleParam cameraAlignedPosition, Handles.RotationHandleParam cameraAlignedRotation, Handles.ScaleHandleParam cameraAlignedScale, Handles.PositionHandleParam localPosition, Handles.RotationHandleParam localRotation, Handles.ScaleHandleParam localScale, Handles.PositionHandleParam vertexSnappingPosition, Handles.RotationHandleParam vertexSnappingRotation, Handles.ScaleHandleParam vertexSnappingScale)
      {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        this.cameraAlignedPosition = cameraAlignedPosition;
        this.cameraAlignedRotation = cameraAlignedRotation;
        this.cameraAlignedScale = cameraAlignedScale;
        this.localPosition = localPosition;
        this.localRotation = localRotation;
        this.localScale = localScale;
        this.vertexSnappingPosition = vertexSnappingPosition;
        this.vertexSnappingRotation = vertexSnappingRotation;
        this.vertexSnappingScale = vertexSnappingScale;
      }

      public static Handles.TransformHandleParam Default
      {
        get
        {
          return Handles.TransformHandleParam.s_Default;
        }
        set
        {
          Handles.TransformHandleParam.s_Default = value;
        }
      }
    }

    private struct RotationHandleData
    {
      public bool rotationStarted;
      public Quaternion initialRotation;
    }
  }
}
