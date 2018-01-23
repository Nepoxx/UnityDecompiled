// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRCustomOptionsDaydream
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.VR
{
  internal class VRCustomOptionsDaydream : VRCustomOptionsGoogleVR
  {
    private static GUIContent s_ForegroundIconLabel = EditorGUIUtility.TextContent("Foreground Icon|Icon should be a Texture with dimensions of 512px by 512px and a 1:1 aspect ratio.");
    private static GUIContent s_BackgroundIconLabel = EditorGUIUtility.TextContent("Background Icon|Icon should be a Texture with dimensions of 512px by 512px and a 1:1 aspect ratio.");
    private static GUIContent s_SustainedPerformanceModeLabel = EditorGUIUtility.TextContent("Sustained Performance|Sustained Performance mode is intended to provide a consistent level of performance for a prolonged amount of time");
    private static GUIContent s_EnableVideoLayer = EditorGUIUtility.TextContent("Video Surface|Enable the use of the video surface integrated with Daydream asynchronous reprojection.");
    private static GUIContent s_UseProtectedVideoMemoryLabel = EditorGUIUtility.TextContent("Protected Memory|Enable the use of DRM protection. Only usable if all content is DRM Protected.");
    private static GUIContent s_MinimumTargetHeadTracking = EditorGUIUtility.TextContent("Positional Head Tracking|Requested head tracking support of target devices to run the application on.");
    private static GUIContent[] s_TargetHeadTrackingOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Disabled|Will run on any device and provides no head tracking."), EditorGUIUtility.TextContent("Supported|Will run on any device and will provide head tracking on devices that support head tracking."), EditorGUIUtility.TextContent("Required|Will only run on devices with full 6 DoF head tracking support.") };
    private const int kDisabled = 0;
    private const int kSupported = 1;
    private const int kRequired = 2;
    private const int kThreeDoFHeadTracking = 0;
    private const int kSixDoFHeadTracking = 1;
    private const float s_Indent = 10f;
    private SerializedProperty m_DaydreamIcon;
    private SerializedProperty m_DaydreamIconBackground;
    private SerializedProperty m_DaydreamUseSustainedPerformanceMode;
    private SerializedProperty m_DaydreamEnableVideoLayer;
    private SerializedProperty m_DaydreamUseProtectedMemory;
    private SerializedProperty m_MinimumSupportedHeadTracking;
    private SerializedProperty m_MaximumSupportedHeadTracking;

    public override void Initialize(SerializedObject settings)
    {
      this.Initialize(settings, "daydream");
    }

    public override void Initialize(SerializedObject settings, string propertyName)
    {
      base.Initialize(settings, propertyName);
      this.m_DaydreamIcon = this.FindPropertyAssert("daydreamIconForeground");
      this.m_DaydreamIconBackground = this.FindPropertyAssert("daydreamIconBackground");
      this.m_DaydreamUseSustainedPerformanceMode = this.FindPropertyAssert("useSustainedPerformanceMode");
      this.m_DaydreamEnableVideoLayer = this.FindPropertyAssert("enableVideoLayer");
      this.m_DaydreamUseProtectedMemory = this.FindPropertyAssert("useProtectedVideoMemory");
      this.m_MinimumSupportedHeadTracking = this.FindPropertyAssert("minimumSupportedHeadTracking");
      this.m_MaximumSupportedHeadTracking = this.FindPropertyAssert("maximumSupportedHeadTracking");
    }

    private Rect DrawTextureUI(Rect rect, GUIContent propLabel, SerializedProperty prop)
    {
      rect.height = 64f;
      GUIContent label = EditorGUI.BeginProperty(rect, propLabel, prop);
      EditorGUI.BeginChangeCheck();
      Texture2D texture2D = EditorGUI.ObjectField(rect, label, prop.objectReferenceValue, typeof (Texture2D), false) as Texture2D;
      if (EditorGUI.EndChangeCheck())
        prop.objectReferenceValue = (Object) texture2D;
      EditorGUI.EndProperty();
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      return rect;
    }

    private int GetHeadTrackingValue()
    {
      int num = 0;
      if (this.m_MinimumSupportedHeadTracking.intValue == 0 && this.m_MaximumSupportedHeadTracking.intValue == 0)
        num = 0;
      else if (this.m_MinimumSupportedHeadTracking.intValue == 0 && this.m_MaximumSupportedHeadTracking.intValue == 1)
        num = 1;
      else if (this.m_MinimumSupportedHeadTracking.intValue == 1 && this.m_MaximumSupportedHeadTracking.intValue == 1)
        num = 2;
      return num;
    }

    private void SetHeadTrackingValue(int headTrackingValue)
    {
      switch (headTrackingValue)
      {
        case 0:
          this.m_MinimumSupportedHeadTracking.intValue = 0;
          this.m_MaximumSupportedHeadTracking.intValue = 0;
          break;
        case 1:
          this.m_MinimumSupportedHeadTracking.intValue = 0;
          this.m_MaximumSupportedHeadTracking.intValue = 1;
          break;
        case 2:
          this.m_MinimumSupportedHeadTracking.intValue = 1;
          this.m_MaximumSupportedHeadTracking.intValue = 1;
          break;
      }
    }

    public override Rect Draw(Rect rect)
    {
      rect = base.Draw(rect);
      rect = this.DrawTextureUI(rect, VRCustomOptionsDaydream.s_ForegroundIconLabel, this.m_DaydreamIcon);
      rect = this.DrawTextureUI(rect, VRCustomOptionsDaydream.s_BackgroundIconLabel, this.m_DaydreamIconBackground);
      rect.height = EditorGUIUtility.singleLineHeight;
      GUIContent label1 = EditorGUI.BeginProperty(rect, VRCustomOptionsDaydream.s_SustainedPerformanceModeLabel, this.m_DaydreamUseSustainedPerformanceMode);
      EditorGUI.BeginChangeCheck();
      bool flag1 = EditorGUI.Toggle(rect, label1, this.m_DaydreamUseSustainedPerformanceMode.boolValue);
      if (EditorGUI.EndChangeCheck())
        this.m_DaydreamUseSustainedPerformanceMode.boolValue = flag1;
      EditorGUI.EndProperty();
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      rect.height = EditorGUIUtility.singleLineHeight;
      GUIContent label2 = EditorGUI.BeginProperty(rect, VRCustomOptionsDaydream.s_EnableVideoLayer, this.m_DaydreamEnableVideoLayer);
      EditorGUI.BeginChangeCheck();
      bool flag2 = EditorGUI.Toggle(rect, label2, this.m_DaydreamEnableVideoLayer.boolValue);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_DaydreamEnableVideoLayer.boolValue = flag2;
        if (!flag2)
          this.m_DaydreamUseProtectedMemory.boolValue = false;
      }
      EditorGUI.EndProperty();
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      if (this.m_DaydreamEnableVideoLayer.boolValue)
      {
        rect.x += 10f;
        rect.width -= 10f;
        rect.height = EditorGUIUtility.singleLineHeight;
        GUIContent label3 = EditorGUI.BeginProperty(rect, VRCustomOptionsDaydream.s_UseProtectedVideoMemoryLabel, this.m_DaydreamUseProtectedMemory);
        EditorGUI.BeginChangeCheck();
        bool flag3 = EditorGUI.Toggle(rect, label3, this.m_DaydreamUseProtectedMemory.boolValue);
        if (EditorGUI.EndChangeCheck())
          this.m_DaydreamUseProtectedMemory.boolValue = flag3;
        EditorGUI.EndProperty();
        rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
        rect.x -= 10f;
        rect.width += 10f;
      }
      rect.y += EditorGUIUtility.standardVerticalSpacing;
      rect.height = EditorGUIUtility.singleLineHeight;
      EditorGUI.BeginChangeCheck();
      int headTrackingValue = EditorGUI.Popup(rect, VRCustomOptionsDaydream.s_MinimumTargetHeadTracking, this.GetHeadTrackingValue(), VRCustomOptionsDaydream.s_TargetHeadTrackingOptions);
      if (EditorGUI.EndChangeCheck())
        this.SetHeadTrackingValue(headTrackingValue);
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      return rect;
    }

    public override float GetHeight()
    {
      float num1 = 5f;
      float num2 = 2f;
      float num3 = 4f;
      if (this.m_DaydreamEnableVideoLayer.boolValue)
      {
        ++num1;
        ++num3;
      }
      return (float) ((double) base.GetHeight() + (double) EditorGUIUtility.singleLineHeight * (double) num1 + 64.0 * (double) num2 + (double) EditorGUIUtility.standardVerticalSpacing * (double) num3);
    }
  }
}
