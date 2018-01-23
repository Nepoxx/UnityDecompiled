// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.PlayerSettingsEditorVR
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditorInternal.VR
{
  internal class PlayerSettingsEditorVR
  {
    private Dictionary<BuildTargetGroup, VRDeviceInfoEditor[]> m_AllVRDevicesForBuildTarget = new Dictionary<BuildTargetGroup, VRDeviceInfoEditor[]>();
    private Dictionary<BuildTargetGroup, ReorderableList> m_VRDeviceActiveUI = new Dictionary<BuildTargetGroup, ReorderableList>();
    private Dictionary<string, string> m_MapVRDeviceKeyToUIString = new Dictionary<string, string>();
    private Dictionary<string, string> m_MapVRUIStringToDeviceKey = new Dictionary<string, string>();
    private Dictionary<string, VRCustomOptions> m_CustomOptions = new Dictionary<string, VRCustomOptions>();
    private bool m_InstallsRequired = false;
    private bool m_VuforiaInstalled = false;
    private PlayerSettingsEditor m_Settings;
    private SerializedProperty m_StereoRenderingPath;
    private SerializedProperty m_AndroidEnableTango;

    public PlayerSettingsEditorVR(PlayerSettingsEditor settingsEditor)
    {
      this.m_Settings = settingsEditor;
      this.m_StereoRenderingPath = this.m_Settings.serializedObject.FindProperty(nameof (m_StereoRenderingPath));
      this.m_AndroidEnableTango = this.m_Settings.FindPropertyAssert("AndroidEnableTango");
    }

    internal int GUISectionIndex { get; set; }

    private void RefreshVRDeviceList(BuildTargetGroup targetGroup)
    {
      VRDeviceInfoEditor[] array = ((IEnumerable<VRDeviceInfoEditor>) VREditor.GetAllVRDeviceInfo(targetGroup)).OrderBy<VRDeviceInfoEditor, string>((Func<VRDeviceInfoEditor, string>) (d => d.deviceNameUI)).ToArray<VRDeviceInfoEditor>();
      this.m_AllVRDevicesForBuildTarget[targetGroup] = array;
      for (int index = 0; index < array.Length; ++index)
      {
        VRDeviceInfoEditor deviceInfoEditor = array[index];
        this.m_MapVRDeviceKeyToUIString[deviceInfoEditor.deviceNameKey] = deviceInfoEditor.deviceNameUI;
        this.m_MapVRUIStringToDeviceKey[deviceInfoEditor.deviceNameUI] = deviceInfoEditor.deviceNameKey;
        VRCustomOptions vrCustomOptions;
        if (!this.m_CustomOptions.TryGetValue(deviceInfoEditor.deviceNameKey, out vrCustomOptions))
        {
          System.Type type = System.Type.GetType("UnityEditorInternal.VR.VRCustomOptions" + deviceInfoEditor.deviceNameKey, false, true);
          vrCustomOptions = type == null ? (VRCustomOptions) new VRCustomOptionsNone() : (VRCustomOptions) Activator.CreateInstance(type);
          vrCustomOptions.Initialize(this.m_Settings.serializedObject);
          this.m_CustomOptions.Add(deviceInfoEditor.deviceNameKey, vrCustomOptions);
        }
      }
    }

    internal bool TargetGroupSupportsVirtualReality(BuildTargetGroup targetGroup)
    {
      if (!this.m_AllVRDevicesForBuildTarget.ContainsKey(targetGroup))
        this.RefreshVRDeviceList(targetGroup);
      return this.m_AllVRDevicesForBuildTarget[targetGroup].Length > 0;
    }

    internal bool TargetGroupSupportsAugmentedReality(BuildTargetGroup targetGroup)
    {
      return this.TargetGroupSupportsTango(targetGroup) || this.TargetGroupSupportsVuforia(targetGroup);
    }

    internal void XRSectionGUI(BuildTargetGroup targetGroup, int sectionIndex)
    {
      this.GUISectionIndex = sectionIndex;
      if (!this.TargetGroupSupportsVirtualReality(targetGroup) && !this.TargetGroupSupportsAugmentedReality(targetGroup))
        return;
      if (VREditor.IsDeviceListDirty(targetGroup))
      {
        VREditor.ClearDeviceListDirty(targetGroup);
        this.m_VRDeviceActiveUI[targetGroup].list = (IList) VREditor.GetVREnabledDevicesOnTargetGroup(targetGroup);
      }
      this.CheckDevicesRequireInstall(targetGroup);
      if (this.m_Settings.BeginSettingsBox(sectionIndex, PlayerSettingsEditorVR.Styles.xrSettingsTitle))
      {
        if (EditorApplication.isPlaying)
          EditorGUILayout.HelpBox("Changing XRSettings in not allowed in play mode.", MessageType.Info);
        using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
        {
          this.DevicesGUI(targetGroup);
          this.ErrorOnVRDeviceIncompatibility(targetGroup);
          this.SinglePassStereoGUI(targetGroup, this.m_StereoRenderingPath);
          this.TangoGUI(targetGroup);
          this.VuforiaGUI(targetGroup);
          this.ErrorOnARDeviceIncompatibility(targetGroup);
        }
        this.InstallGUI(targetGroup);
      }
      this.m_Settings.EndSettingsBox();
    }

    private void DevicesGUI(BuildTargetGroup targetGroup)
    {
      if (!this.TargetGroupSupportsVirtualReality(targetGroup))
        return;
      bool enabledOnTargetGroup = VREditor.GetVREnabledOnTargetGroup(targetGroup);
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle(PlayerSettingsEditorVR.Styles.supportedCheckbox, enabledOnTargetGroup, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        VREditor.SetVREnabledOnTargetGroup(targetGroup, flag);
      if (!flag)
        return;
      this.VRDevicesGUIOneBuildTarget(targetGroup);
    }

    private void InstallGUI(BuildTargetGroup targetGroup)
    {
      if (!this.m_InstallsRequired)
        return;
      EditorGUILayout.Space();
      GUILayout.Label("XR Support Installers", EditorStyles.boldLabel, new GUILayoutOption[0]);
      ++EditorGUI.indentLevel;
      if (!this.m_VuforiaInstalled && EditorGUILayout.LinkLabel("Vuforia Augmented Reality"))
        Application.OpenURL(BuildPlayerWindow.GetPlaybackEngineDownloadURL("Vuforia-AR"));
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
    }

    private void CheckDevicesRequireInstall(BuildTargetGroup targetGroup)
    {
      this.m_InstallsRequired = false;
      if (this.m_VuforiaInstalled)
        return;
      foreach (VRDeviceInfoEditor deviceInfoEditor in VREditor.GetAllVRDeviceInfo(targetGroup))
      {
        if (deviceInfoEditor.deviceNameKey.ToLower() == "vuforia")
        {
          this.m_VuforiaInstalled = true;
          break;
        }
      }
      if (!this.m_VuforiaInstalled)
        this.m_InstallsRequired = true;
    }

    private static bool TargetSupportsSinglePassStereoRendering(BuildTargetGroup targetGroup)
    {
      return targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.Android || (targetGroup == BuildTargetGroup.WSA || targetGroup == BuildTargetGroup.PS4);
    }

    private static bool TargetSupportsStereoInstancingRendering(BuildTargetGroup targetGroup)
    {
      return targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.WSA || targetGroup == BuildTargetGroup.PS4;
    }

    private static GUIContent[] GetStereoRenderingPaths(BuildTargetGroup targetGroup)
    {
      return targetGroup != BuildTargetGroup.Android ? PlayerSettingsEditorVR.Styles.kDefaultStereoRenderingPaths : PlayerSettingsEditorVR.Styles.kAndroidStereoRenderingPaths;
    }

    private void SinglePassStereoGUI(BuildTargetGroup targetGroup, SerializedProperty stereoRenderingPath)
    {
      if (!PlayerSettings.virtualRealitySupported)
        return;
      bool flag1 = PlayerSettingsEditorVR.TargetSupportsSinglePassStereoRendering(targetGroup);
      bool flag2 = PlayerSettingsEditorVR.TargetSupportsStereoInstancingRendering(targetGroup);
      int length = 1 + (!flag1 ? 0 : 1) + (!flag2 ? 0 : 1);
      GUIContent[] displayedOptions = new GUIContent[length];
      int[] optionValues = new int[length];
      GUIContent[] stereoRenderingPaths = PlayerSettingsEditorVR.GetStereoRenderingPaths(targetGroup);
      int index1 = 0;
      displayedOptions[index1] = stereoRenderingPaths[0];
      int[] numArray1 = optionValues;
      int index2 = index1;
      int index3 = index2 + 1;
      int num1 = 0;
      numArray1[index2] = num1;
      if (flag1)
      {
        displayedOptions[index3] = stereoRenderingPaths[1];
        optionValues[index3++] = 1;
      }
      if (flag2)
      {
        displayedOptions[index3] = stereoRenderingPaths[2];
        int[] numArray2 = optionValues;
        int index4 = index3;
        int num2 = index4 + 1;
        int num3 = 2;
        numArray2[index4] = num3;
      }
      if (!flag2 && stereoRenderingPath.intValue == 2)
        stereoRenderingPath.intValue = 1;
      if (!flag1 && stereoRenderingPath.intValue == 1)
        stereoRenderingPath.intValue = 0;
      EditorGUILayout.IntPopup(stereoRenderingPath, displayedOptions, optionValues, EditorGUIUtility.TextContent("Stereo Rendering Method*"), new GUILayoutOption[0]);
      if (stereoRenderingPath.intValue == 1 && targetGroup == BuildTargetGroup.Android)
      {
        GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
        if (graphicsApIs.Length > 0 && graphicsApIs[0] == GraphicsDeviceType.OpenGLES3)
          EditorGUILayout.HelpBox(PlayerSettingsEditorVR.Styles.singlepassAndroidWarning2.text, MessageType.Info);
        else
          EditorGUILayout.HelpBox(PlayerSettingsEditorVR.Styles.singlepassAndroidWarning.text, MessageType.Warning);
      }
      else
      {
        if (stereoRenderingPath.intValue != 2 || targetGroup != BuildTargetGroup.Standalone)
          return;
        EditorGUILayout.HelpBox(PlayerSettingsEditorVR.Styles.singlePassInstancedWarning.text, MessageType.Warning);
      }
    }

    private void AddVRDeviceMenuSelected(object userData, string[] options, int selected)
    {
      BuildTargetGroup buildTargetGroup = (BuildTargetGroup) userData;
      List<string> list = ((IEnumerable<string>) VREditor.GetVREnabledDevicesOnTargetGroup(buildTargetGroup)).ToList<string>();
      string option;
      if (!this.m_MapVRUIStringToDeviceKey.TryGetValue(options[selected], out option))
        option = options[selected];
      list.Add(option);
      this.ApplyChangedVRDeviceList(buildTargetGroup, list.ToArray());
    }

    private void AddVRDeviceElement(BuildTargetGroup target, Rect rect, ReorderableList list)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PlayerSettingsEditorVR.\u003CAddVRDeviceElement\u003Ec__AnonStorey0 elementCAnonStorey0 = new PlayerSettingsEditorVR.\u003CAddVRDeviceElement\u003Ec__AnonStorey0();
      VRDeviceInfoEditor[] deviceInfoEditorArray = this.m_AllVRDevicesForBuildTarget[target];
      // ISSUE: reference to a compiler-generated field
      elementCAnonStorey0.enabledDevices = ((IEnumerable<string>) VREditor.GetVREnabledDevicesOnTargetGroup(target)).ToList<string>();
      string[] array1 = ((IEnumerable<VRDeviceInfoEditor>) deviceInfoEditorArray).Select<VRDeviceInfoEditor, string>((Func<VRDeviceInfoEditor, string>) (d => d.deviceNameUI)).ToArray<string>();
      // ISSUE: reference to a compiler-generated method
      bool[] array2 = ((IEnumerable<VRDeviceInfoEditor>) deviceInfoEditorArray).Select<VRDeviceInfoEditor, bool>(new Func<VRDeviceInfoEditor, bool>(elementCAnonStorey0.\u003C\u003Em__0)).ToArray<bool>();
      EditorUtility.DisplayCustomMenu(rect, array1, array2, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.AddVRDeviceMenuSelected), (object) target);
    }

    private void RemoveVRDeviceElement(BuildTargetGroup target, ReorderableList list)
    {
      List<string> list1 = ((IEnumerable<string>) VREditor.GetVREnabledDevicesOnTargetGroup(target)).ToList<string>();
      list1.RemoveAt(list.index);
      this.ApplyChangedVRDeviceList(target, list1.ToArray());
    }

    private void ReorderVRDeviceElement(BuildTargetGroup target, ReorderableList list)
    {
      string[] array = list.list.Cast<string>().ToArray<string>();
      this.ApplyChangedVRDeviceList(target, array);
    }

    private void ApplyChangedVRDeviceList(BuildTargetGroup target, string[] devices)
    {
      if (!this.m_VRDeviceActiveUI.ContainsKey(target))
        return;
      if (target == BuildTargetGroup.iPhone && ((IEnumerable<string>) devices).Contains<string>("cardboard") && PlayerSettings.iOS.cameraUsageDescription == "")
        PlayerSettings.iOS.cameraUsageDescription = "Used to scan QR codes";
      VREditor.SetVREnabledDevicesOnTargetGroup(target, devices);
      this.m_VRDeviceActiveUI[target].list = (IList) devices;
    }

    private void DrawVRDeviceElement(BuildTargetGroup target, Rect rect, int index, bool selected, bool focused)
    {
      string key = (string) this.m_VRDeviceActiveUI[target].list[index];
      string text;
      if (!this.m_MapVRDeviceKeyToUIString.TryGetValue(key, out text))
        text = key + " (missing from build)";
      VRCustomOptions vrCustomOptions;
      if (this.m_CustomOptions.TryGetValue(key, out vrCustomOptions) && !(vrCustomOptions is VRCustomOptionsNone))
      {
        Rect position = new Rect(rect);
        position.width = (float) EditorStyles.foldout.border.left;
        position.height = (float) EditorStyles.foldout.border.top;
        bool hierarchyMode = EditorGUIUtility.hierarchyMode;
        EditorGUIUtility.hierarchyMode = false;
        vrCustomOptions.IsExpanded = EditorGUI.Foldout(position, vrCustomOptions.IsExpanded, "", false, EditorStyles.foldout);
        EditorGUIUtility.hierarchyMode = hierarchyMode;
      }
      rect.xMin += (float) EditorStyles.foldout.border.left;
      GUI.Label(rect, text, EditorStyles.label);
      rect.y += EditorGUIUtility.singleLineHeight + 2f;
      if (vrCustomOptions == null || !vrCustomOptions.IsExpanded)
        return;
      vrCustomOptions.Draw(rect);
    }

    private float GetVRDeviceElementHeight(BuildTargetGroup target, int index)
    {
      ReorderableList reorderableList = this.m_VRDeviceActiveUI[target];
      string key = (string) reorderableList.list[index];
      float num = 0.0f;
      VRCustomOptions vrCustomOptions;
      if (this.m_CustomOptions.TryGetValue(key, out vrCustomOptions))
        num = !vrCustomOptions.IsExpanded ? 0.0f : vrCustomOptions.GetHeight() + 2f;
      return reorderableList.elementHeight + num;
    }

    private void SelectVRDeviceElement(BuildTargetGroup target, ReorderableList list)
    {
      VRCustomOptions vrCustomOptions;
      if (!this.m_CustomOptions.TryGetValue((string) this.m_VRDeviceActiveUI[target].list[list.index], out vrCustomOptions))
        return;
      vrCustomOptions.IsExpanded = false;
    }

    private bool GetVRDeviceElementIsInList(BuildTargetGroup target, string deviceName)
    {
      return ((IEnumerable<string>) VREditor.GetVREnabledDevicesOnTargetGroup(target)).Contains<string>(deviceName);
    }

    private void VRDevicesGUIOneBuildTarget(BuildTargetGroup targetGroup)
    {
      if (!this.m_VRDeviceActiveUI.ContainsKey(targetGroup))
        this.m_VRDeviceActiveUI.Add(targetGroup, new ReorderableList((IList) VREditor.GetVREnabledDevicesOnTargetGroup(targetGroup), typeof (VRDeviceInfoEditor), true, true, true, true)
        {
          onAddDropdownCallback = (ReorderableList.AddDropdownCallbackDelegate) ((rect, list) => this.AddVRDeviceElement(targetGroup, rect, list)),
          onRemoveCallback = (ReorderableList.RemoveCallbackDelegate) (list => this.RemoveVRDeviceElement(targetGroup, list)),
          onReorderCallback = (ReorderableList.ReorderCallbackDelegate) (list => this.ReorderVRDeviceElement(targetGroup, list)),
          drawElementCallback = (ReorderableList.ElementCallbackDelegate) ((rect, index, isActive, isFocused) => this.DrawVRDeviceElement(targetGroup, rect, index, isActive, isFocused)),
          drawHeaderCallback = (ReorderableList.HeaderCallbackDelegate) (rect => GUI.Label(rect, PlayerSettingsEditorVR.Styles.listHeader, EditorStyles.label)),
          elementHeightCallback = (ReorderableList.ElementHeightCallbackDelegate) (index => this.GetVRDeviceElementHeight(targetGroup, index)),
          onSelectCallback = (ReorderableList.SelectCallbackDelegate) (list => this.SelectVRDeviceElement(targetGroup, list))
        });
      this.m_VRDeviceActiveUI[targetGroup].DoLayoutList();
      if (this.m_VRDeviceActiveUI[targetGroup].list.Count != 0)
        return;
      EditorGUILayout.HelpBox("Must add at least one Virtual Reality SDK.", MessageType.Warning);
    }

    private void ErrorOnVRDeviceIncompatibility(BuildTargetGroup targetGroup)
    {
      if (!PlayerSettings.GetVirtualRealitySupported(targetGroup) || targetGroup != BuildTargetGroup.Android)
        return;
      List<string> list = ((IEnumerable<string>) VREditor.GetVREnabledDevicesOnTargetGroup(targetGroup)).ToList<string>();
      if (list.Contains("Oculus") && list.Contains("daydream"))
        EditorGUILayout.HelpBox("To avoid initialization conflicts on devices which support both Daydream and Oculus based VR, build separate APKs with different package names, targeting only the Daydream or Oculus VR SDK in the respective APK.", MessageType.Warning);
    }

    private void ErrorOnARDeviceIncompatibility(BuildTargetGroup targetGroup)
    {
      if (targetGroup != BuildTargetGroup.Android || !PlayerSettings.Android.androidTangoEnabled || !PlayerSettings.GetPlatformVuforiaEnabled(targetGroup))
        return;
      EditorGUILayout.HelpBox("Both ARCore and Vuforia XR Device support cannot be selected at the same time. Please select only one XR Device that will manage the Android device.", MessageType.Error);
    }

    internal bool TargetGroupSupportsTango(BuildTargetGroup targetGroup)
    {
      return targetGroup == BuildTargetGroup.Android;
    }

    internal bool TargetGroupSupportsVuforia(BuildTargetGroup targetGroup)
    {
      return targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.WSA;
    }

    internal void TangoGUI(BuildTargetGroup targetGroup)
    {
      if (!this.TargetGroupSupportsTango(targetGroup))
        return;
      EditorGUILayout.PropertyField(this.m_AndroidEnableTango, EditorGUIUtility.TextContent("ARCore Supported"), new GUILayoutOption[0]);
      if (!PlayerSettings.Android.androidTangoEnabled)
        return;
      ++EditorGUI.indentLevel;
      if (PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel24)
        EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("ARCore requires 'Minimum API Level' to be at least Android 7.0").text, MessageType.Warning);
      --EditorGUI.indentLevel;
    }

    internal void VuforiaGUI(BuildTargetGroup targetGroup)
    {
      if (!this.TargetGroupSupportsVuforia(targetGroup) || !this.m_VuforiaInstalled)
        return;
      bool disabled = VREditor.GetVREnabledOnTargetGroup(targetGroup) && this.GetVRDeviceElementIsInList(targetGroup, "Vuforia");
      using (new EditorGUI.DisabledScope(disabled))
      {
        if (disabled && !PlayerSettings.GetPlatformVuforiaEnabled(targetGroup))
          PlayerSettings.SetPlatformVuforiaEnabled(targetGroup, true);
        bool platformVuforiaEnabled = PlayerSettings.GetPlatformVuforiaEnabled(targetGroup);
        EditorGUI.BeginChangeCheck();
        bool enabled = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Vuforia Augmented Reality Supported"), platformVuforiaEnabled, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
          PlayerSettings.SetPlatformVuforiaEnabled(targetGroup, enabled);
      }
      if (!disabled)
        return;
      EditorGUILayout.HelpBox("Vuforia Augmented Reality is required when using the Vuforia Virtual Reality SDK.", MessageType.Info);
    }

    private static class Styles
    {
      public static readonly GUIContent singlepassAndroidWarning = EditorGUIUtility.TextContent("Single Pass stereo rendering requires OpenGL ES 3. Please make sure that it's the first one listed under Graphics APIs.");
      public static readonly GUIContent singlepassAndroidWarning2 = EditorGUIUtility.TextContent("Multi Pass will be used on Android devices that don't support Single Pass.");
      public static readonly GUIContent singlePassInstancedWarning = EditorGUIUtility.TextContent("Single Pass Instanced is only supported on Windows. Multi Pass will be used on other platforms.");
      public static readonly GUIContent[] kDefaultStereoRenderingPaths = new GUIContent[3]{ new GUIContent("Multi Pass"), new GUIContent("Single Pass"), new GUIContent("Single Pass Instanced (Preview)") };
      public static readonly GUIContent[] kAndroidStereoRenderingPaths = new GUIContent[2]{ new GUIContent("Multi Pass"), new GUIContent("Single Pass (Preview)") };
      public static readonly GUIContent xrSettingsTitle = EditorGUIUtility.TextContent("XR Settings");
      public static readonly GUIContent supportedCheckbox = EditorGUIUtility.TextContent("Virtual Reality Supported");
      public static readonly GUIContent listHeader = EditorGUIUtility.TextContent("Virtual Reality SDKs");
    }
  }
}
