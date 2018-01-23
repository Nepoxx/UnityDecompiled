// Decompiled with JetBrains decompiler
// Type: UnityEditor.QualitySettingsEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (QualitySettings))]
  internal class QualitySettingsEditor : ProjectSettingsBaseEditor
  {
    private readonly int m_QualityElementHash = "QualityElementHash".GetHashCode();
    private int m_DeleteLevel = -1;
    public const int kMinAsyncRingBufferSize = 2;
    public const int kMaxAsyncRingBufferSize = 512;
    public const int kMinAsyncUploadTimeSlice = 1;
    public const int kMaxAsyncUploadTimeSlice = 33;
    private SerializedObject m_QualitySettings;
    private SerializedProperty m_QualitySettingsProperty;
    private SerializedProperty m_PerPlatformDefaultQualityProperty;
    private List<BuildPlatform> m_ValidPlatforms;
    private QualitySettingsEditor.Dragging m_Dragging;
    private bool m_ShouldAddNewLevel;

    public void OnEnable()
    {
      this.m_QualitySettings = new SerializedObject(this.target);
      this.m_QualitySettingsProperty = this.m_QualitySettings.FindProperty("m_QualitySettings");
      this.m_PerPlatformDefaultQualityProperty = this.m_QualitySettings.FindProperty("m_PerPlatformDefaultQuality");
      this.m_ValidPlatforms = BuildPlatforms.instance.GetValidPlatforms();
    }

    private int DoQualityLevelSelection(int currentQualitylevel, IList<QualitySettingsEditor.QualitySetting> qualitySettings, Dictionary<string, int> platformDefaultQualitySettings)
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.BeginVertical();
      int num = currentQualitylevel;
      GUILayout.BeginHorizontal();
      Rect rect1 = GUILayoutUtility.GetRect(GUIContent.none, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[3]{ GUILayout.ExpandWidth(false), GUILayout.Width(80f), GUILayout.Height(20f) });
      rect1.x += EditorGUI.indent;
      rect1.width -= EditorGUI.indent;
      GUI.Label(rect1, "Levels", EditorStyles.boldLabel);
      foreach (BuildPlatform validPlatform in this.m_ValidPlatforms)
      {
        Rect rect2 = GUILayoutUtility.GetRect(GUIContent.none, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[3]{ GUILayout.MinWidth(15f), GUILayout.MaxWidth(20f), GUILayout.Height(20f) });
        GUIContent content = EditorGUIUtility.TempContent((Texture) validPlatform.smallIcon);
        content.tooltip = validPlatform.title.text;
        GUI.Label(rect2, content);
        content.tooltip = "";
      }
      GUILayoutUtility.GetRect(GUIContent.none, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[3]
      {
        GUILayout.MinWidth(15f),
        GUILayout.MaxWidth(20f),
        GUILayout.Height(20f)
      });
      GUILayout.EndHorizontal();
      Event current = Event.current;
      for (int index = 0; index < qualitySettings.Count; ++index)
      {
        GUILayout.BeginHorizontal();
        GUIStyle guiStyle = index % 2 != 0 ? QualitySettingsEditor.Styles.kListOddBg : QualitySettingsEditor.Styles.kListEvenBg;
        bool on = num == index;
        Rect rect2 = GUILayoutUtility.GetRect(GUIContent.none, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[2]{ GUILayout.ExpandWidth(false), GUILayout.Width(80f) });
        switch (current.type)
        {
          case EventType.MouseDown:
            if (rect2.Contains(current.mousePosition))
            {
              num = index;
              GUIUtility.keyboardControl = 0;
              GUIUtility.hotControl = this.m_QualityElementHash;
              GUI.changed = true;
              this.m_Dragging = new QualitySettingsEditor.Dragging()
              {
                m_StartPosition = index,
                m_Position = index
              };
              current.Use();
              break;
            }
            break;
          case EventType.MouseUp:
            if (GUIUtility.hotControl == this.m_QualityElementHash)
            {
              GUIUtility.hotControl = 0;
              current.Use();
              break;
            }
            break;
          case EventType.MouseDrag:
            if (GUIUtility.hotControl == this.m_QualityElementHash && rect2.Contains(current.mousePosition))
            {
              this.m_Dragging.m_Position = index;
              current.Use();
              break;
            }
            break;
          case EventType.KeyDown:
            if (current.keyCode == KeyCode.UpArrow || current.keyCode == KeyCode.DownArrow)
            {
              num = Mathf.Clamp(num + (current.keyCode != KeyCode.UpArrow ? 1 : -1), 0, qualitySettings.Count - 1);
              GUIUtility.keyboardControl = 0;
              GUI.changed = true;
              current.Use();
              break;
            }
            break;
          case EventType.Repaint:
            guiStyle.Draw(rect2, GUIContent.none, false, false, on, false);
            GUI.Label(rect2, EditorGUIUtility.TempContent(qualitySettings[index].m_Name));
            break;
        }
        foreach (BuildPlatform validPlatform in this.m_ValidPlatforms)
        {
          bool flag1 = false;
          if (platformDefaultQualitySettings.ContainsKey(validPlatform.name) && platformDefaultQualitySettings[validPlatform.name] == index)
            flag1 = true;
          Rect rect3 = GUILayoutUtility.GetRect(QualitySettingsEditor.Styles.kPlatformTooltip, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[2]{ GUILayout.MinWidth(15f), GUILayout.MaxWidth(20f) });
          if (Event.current.type == EventType.Repaint)
            guiStyle.Draw(rect3, GUIContent.none, false, false, on, false);
          Color backgroundColor = GUI.backgroundColor;
          if (flag1 && !EditorApplication.isPlayingOrWillChangePlaymode)
            GUI.backgroundColor = Color.green;
          bool flag2 = !qualitySettings[index].m_ExcludedPlatforms.Contains(validPlatform.name);
          bool flag3 = GUI.Toggle(rect3, flag2, QualitySettingsEditor.Styles.kPlatformTooltip, !flag1 ? QualitySettingsEditor.Styles.kToggle : QualitySettingsEditor.Styles.kDefaultToggle);
          if (flag2 != flag3)
          {
            if (flag3)
              qualitySettings[index].m_ExcludedPlatforms.Remove(validPlatform.name);
            else
              qualitySettings[index].m_ExcludedPlatforms.Add(validPlatform.name);
          }
          GUI.backgroundColor = backgroundColor;
        }
        Rect rect4 = GUILayoutUtility.GetRect(GUIContent.none, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[2]{ GUILayout.MinWidth(15f), GUILayout.MaxWidth(20f) });
        if (Event.current.type == EventType.Repaint)
          guiStyle.Draw(rect4, GUIContent.none, false, false, on, false);
        if (GUI.Button(rect4, QualitySettingsEditor.Styles.kIconTrash, GUIStyle.none))
          this.m_DeleteLevel = index;
        GUILayout.EndHorizontal();
      }
      GUILayout.BeginHorizontal();
      GUILayoutUtility.GetRect(GUIContent.none, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[3]
      {
        GUILayout.MinWidth(15f),
        GUILayout.MaxWidth(20f),
        GUILayout.Height(1f)
      });
      QualitySettingsEditor.DrawHorizontalDivider();
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      Rect rect5 = GUILayoutUtility.GetRect(GUIContent.none, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[3]{ GUILayout.ExpandWidth(false), GUILayout.Width(80f), GUILayout.Height(20f) });
      rect5.x += EditorGUI.indent;
      rect5.width -= EditorGUI.indent;
      GUI.Label(rect5, "Default", EditorStyles.boldLabel);
      foreach (BuildPlatform validPlatform in this.m_ValidPlatforms)
      {
        Rect rect2 = GUILayoutUtility.GetRect(GUIContent.none, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[3]{ GUILayout.MinWidth(15f), GUILayout.MaxWidth(20f), GUILayout.Height(20f) });
        int selectedIndex;
        if (!platformDefaultQualitySettings.TryGetValue(validPlatform.name, out selectedIndex))
          platformDefaultQualitySettings.Add(validPlatform.name, 0);
        selectedIndex = EditorGUI.Popup(rect2, selectedIndex, qualitySettings.Select<QualitySettingsEditor.QualitySetting, string>((Func<QualitySettingsEditor.QualitySetting, string>) (x => x.m_Name)).ToArray<string>(), QualitySettingsEditor.Styles.kDefaultDropdown);
        platformDefaultQualitySettings[validPlatform.name] = selectedIndex;
      }
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayoutUtility.GetRect(GUIContent.none, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[3]
      {
        GUILayout.MinWidth(15f),
        GUILayout.MaxWidth(20f),
        GUILayout.Height(20f)
      });
      if (GUI.Button(GUILayoutUtility.GetRect(QualitySettingsEditor.Styles.kAddQualityLevel, QualitySettingsEditor.Styles.kToggle, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) }), QualitySettingsEditor.Styles.kAddQualityLevel))
        this.m_ShouldAddNewLevel = true;
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      return num;
    }

    private List<QualitySettingsEditor.QualitySetting> GetQualitySettings()
    {
      List<QualitySettingsEditor.QualitySetting> qualitySettingList = new List<QualitySettingsEditor.QualitySetting>();
      IEnumerator enumerator1 = this.m_QualitySettingsProperty.GetEnumerator();
      try
      {
        while (enumerator1.MoveNext())
        {
          SerializedProperty current1 = (SerializedProperty) enumerator1.Current;
          QualitySettingsEditor.QualitySetting qualitySetting = new QualitySettingsEditor.QualitySetting() { m_Name = current1.FindPropertyRelative("name").stringValue, m_PropertyPath = current1.propertyPath };
          qualitySetting.m_PropertyPath = current1.propertyPath;
          List<string> stringList = new List<string>();
          IEnumerator enumerator2 = current1.FindPropertyRelative("excludedTargetPlatforms").GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              SerializedProperty current2 = (SerializedProperty) enumerator2.Current;
              stringList.Add(current2.stringValue);
            }
          }
          finally
          {
            IDisposable disposable;
            if ((disposable = enumerator2 as IDisposable) != null)
              disposable.Dispose();
          }
          qualitySetting.m_ExcludedPlatforms = stringList;
          qualitySettingList.Add(qualitySetting);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator1 as IDisposable) != null)
          disposable.Dispose();
      }
      return qualitySettingList;
    }

    private void SetQualitySettings(IEnumerable<QualitySettingsEditor.QualitySetting> settings)
    {
      foreach (QualitySettingsEditor.QualitySetting setting in settings)
      {
        SerializedProperty property = this.m_QualitySettings.FindProperty(setting.m_PropertyPath);
        if (property != null)
        {
          SerializedProperty propertyRelative = property.FindPropertyRelative("excludedTargetPlatforms");
          if (propertyRelative.arraySize != setting.m_ExcludedPlatforms.Count)
            propertyRelative.arraySize = setting.m_ExcludedPlatforms.Count;
          int index = 0;
          IEnumerator enumerator = propertyRelative.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              SerializedProperty current = (SerializedProperty) enumerator.Current;
              if (current.stringValue != setting.m_ExcludedPlatforms[index])
                current.stringValue = setting.m_ExcludedPlatforms[index];
              ++index;
            }
          }
          finally
          {
            IDisposable disposable;
            if ((disposable = enumerator as IDisposable) != null)
              disposable.Dispose();
          }
        }
      }
    }

    private void HandleAddRemoveQualitySetting(ref int selectedLevel, Dictionary<string, int> platformDefaults)
    {
      if (this.m_DeleteLevel >= 0)
      {
        if (this.m_DeleteLevel < selectedLevel || this.m_DeleteLevel == this.m_QualitySettingsProperty.arraySize - 1)
          selectedLevel = Mathf.Max(0, selectedLevel - 1);
        if (this.m_QualitySettingsProperty.arraySize > 1 && this.m_DeleteLevel >= 0 && this.m_DeleteLevel < this.m_QualitySettingsProperty.arraySize)
        {
          this.m_QualitySettingsProperty.DeleteArrayElementAtIndex(this.m_DeleteLevel);
          foreach (string index1 in new List<string>((IEnumerable<string>) platformDefaults.Keys))
          {
            int platformDefault = platformDefaults[index1];
            if (platformDefault != 0 && platformDefault >= this.m_DeleteLevel)
            {
              Dictionary<string, int> dictionary;
              string index2;
              (dictionary = platformDefaults)[index2 = index1] = dictionary[index2] - 1;
            }
          }
        }
        this.m_DeleteLevel = -1;
      }
      if (!this.m_ShouldAddNewLevel)
        return;
      ++this.m_QualitySettingsProperty.arraySize;
      this.m_QualitySettingsProperty.GetArrayElementAtIndex(this.m_QualitySettingsProperty.arraySize - 1).FindPropertyRelative("name").stringValue = "Level " + (object) (this.m_QualitySettingsProperty.arraySize - 1);
      this.m_ShouldAddNewLevel = false;
    }

    private Dictionary<string, int> GetDefaultQualityForPlatforms()
    {
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      IEnumerator enumerator = this.m_PerPlatformDefaultQualityProperty.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          SerializedProperty current = (SerializedProperty) enumerator.Current;
          dictionary.Add(current.FindPropertyRelative("first").stringValue, current.FindPropertyRelative("second").intValue);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return dictionary;
    }

    private void SetDefaultQualityForPlatforms(Dictionary<string, int> platformDefaults)
    {
      if (this.m_PerPlatformDefaultQualityProperty.arraySize != platformDefaults.Count)
        this.m_PerPlatformDefaultQualityProperty.arraySize = platformDefaults.Count;
      int index = 0;
      foreach (KeyValuePair<string, int> platformDefault in platformDefaults)
      {
        SerializedProperty arrayElementAtIndex = this.m_PerPlatformDefaultQualityProperty.GetArrayElementAtIndex(index);
        SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("first");
        SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("second");
        if (propertyRelative1.stringValue != platformDefault.Key || propertyRelative2.intValue != platformDefault.Value)
        {
          propertyRelative1.stringValue = platformDefault.Key;
          propertyRelative2.intValue = platformDefault.Value;
        }
        ++index;
      }
    }

    private static void DrawHorizontalDivider()
    {
      Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, new GUILayoutOption[2]{ GUILayout.ExpandWidth(true), GUILayout.Height(1f) });
      Color backgroundColor = GUI.backgroundColor;
      GUI.backgroundColor = !EditorGUIUtility.isProSkin ? Color.black : backgroundColor * 0.7058f;
      if (Event.current.type == EventType.Repaint)
        EditorGUIUtility.whiteTextureStyle.Draw(rect, GUIContent.none, false, false, false, false);
      GUI.backgroundColor = backgroundColor;
    }

    private void SoftParticlesHintGUI()
    {
      Camera main = Camera.main;
      if ((UnityEngine.Object) main == (UnityEngine.Object) null)
        return;
      switch (main.actualRenderingPath)
      {
        case RenderingPath.DeferredLighting:
          break;
        case RenderingPath.DeferredShading:
          break;
        default:
          if ((main.depthTextureMode & DepthTextureMode.Depth) != DepthTextureMode.None)
            break;
          EditorGUILayout.HelpBox(QualitySettingsEditor.Styles.kSoftParticlesHint.text, MessageType.Warning, false);
          break;
      }
    }

    private void DrawCascadeSplitGUI<T>(ref SerializedProperty shadowCascadeSplit)
    {
      float[] normalizedCascadePartitions = (float[]) null;
      System.Type type = typeof (T);
      if (type == typeof (float))
        normalizedCascadePartitions = new float[1]
        {
          shadowCascadeSplit.floatValue
        };
      else if (type == typeof (Vector3))
      {
        Vector3 vector3Value = shadowCascadeSplit.vector3Value;
        normalizedCascadePartitions = new float[3]
        {
          Mathf.Clamp(vector3Value[0], 0.0f, 1f),
          Mathf.Clamp(vector3Value[1] - vector3Value[0], 0.0f, 1f),
          Mathf.Clamp(vector3Value[2] - vector3Value[1], 0.0f, 1f)
        };
      }
      if (normalizedCascadePartitions == null)
        return;
      EditorGUI.BeginChangeCheck();
      ShadowCascadeSplitGUI.HandleCascadeSliderGUI(ref normalizedCascadePartitions);
      if (EditorGUI.EndChangeCheck())
      {
        if (type == typeof (float))
        {
          shadowCascadeSplit.floatValue = normalizedCascadePartitions[0];
        }
        else
        {
          Vector3 vector3 = new Vector3();
          vector3[0] = normalizedCascadePartitions[0];
          vector3[1] = vector3[0] + normalizedCascadePartitions[1];
          vector3[2] = vector3[1] + normalizedCascadePartitions[2];
          shadowCascadeSplit.vector3Value = vector3;
        }
      }
    }

    public override void OnInspectorGUI()
    {
      if (EditorApplication.isPlayingOrWillChangePlaymode)
        EditorGUILayout.HelpBox("Changes made in play mode will not be saved.", MessageType.Warning, true);
      this.m_QualitySettings.Update();
      List<QualitySettingsEditor.QualitySetting> qualitySettings = this.GetQualitySettings();
      Dictionary<string, int> qualityForPlatforms = this.GetDefaultQualityForPlatforms();
      int qualityLevel = QualitySettings.GetQualityLevel();
      EditorGUI.BeginChangeCheck();
      int selectedLevel = this.DoQualityLevelSelection(qualityLevel, (IList<QualitySettingsEditor.QualitySetting>) qualitySettings, qualityForPlatforms);
      if (EditorGUI.EndChangeCheck())
        QualitySettings.SetQualityLevel(selectedLevel);
      this.SetQualitySettings((IEnumerable<QualitySettingsEditor.QualitySetting>) qualitySettings);
      this.HandleAddRemoveQualitySetting(ref selectedLevel, qualityForPlatforms);
      this.SetDefaultQualityForPlatforms(qualityForPlatforms);
      GUILayout.Space(10f);
      QualitySettingsEditor.DrawHorizontalDivider();
      GUILayout.Space(10f);
      SerializedProperty arrayElementAtIndex = this.m_QualitySettingsProperty.GetArrayElementAtIndex(selectedLevel);
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("name");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("pixelLightCount");
      SerializedProperty propertyRelative3 = arrayElementAtIndex.FindPropertyRelative("shadows");
      SerializedProperty propertyRelative4 = arrayElementAtIndex.FindPropertyRelative("shadowResolution");
      SerializedProperty propertyRelative5 = arrayElementAtIndex.FindPropertyRelative("shadowProjection");
      SerializedProperty propertyRelative6 = arrayElementAtIndex.FindPropertyRelative("shadowCascades");
      SerializedProperty propertyRelative7 = arrayElementAtIndex.FindPropertyRelative("shadowDistance");
      SerializedProperty propertyRelative8 = arrayElementAtIndex.FindPropertyRelative("shadowNearPlaneOffset");
      SerializedProperty propertyRelative9 = arrayElementAtIndex.FindPropertyRelative("shadowCascade2Split");
      SerializedProperty propertyRelative10 = arrayElementAtIndex.FindPropertyRelative("shadowCascade4Split");
      SerializedProperty propertyRelative11 = arrayElementAtIndex.FindPropertyRelative("shadowmaskMode");
      SerializedProperty propertyRelative12 = arrayElementAtIndex.FindPropertyRelative("blendWeights");
      SerializedProperty propertyRelative13 = arrayElementAtIndex.FindPropertyRelative("textureQuality");
      SerializedProperty propertyRelative14 = arrayElementAtIndex.FindPropertyRelative("anisotropicTextures");
      SerializedProperty propertyRelative15 = arrayElementAtIndex.FindPropertyRelative("antiAliasing");
      SerializedProperty propertyRelative16 = arrayElementAtIndex.FindPropertyRelative("softParticles");
      SerializedProperty propertyRelative17 = arrayElementAtIndex.FindPropertyRelative("realtimeReflectionProbes");
      SerializedProperty propertyRelative18 = arrayElementAtIndex.FindPropertyRelative("billboardsFaceCameraPosition");
      SerializedProperty propertyRelative19 = arrayElementAtIndex.FindPropertyRelative("vSyncCount");
      SerializedProperty propertyRelative20 = arrayElementAtIndex.FindPropertyRelative("lodBias");
      SerializedProperty propertyRelative21 = arrayElementAtIndex.FindPropertyRelative("maximumLODLevel");
      SerializedProperty propertyRelative22 = arrayElementAtIndex.FindPropertyRelative("particleRaycastBudget");
      SerializedProperty propertyRelative23 = arrayElementAtIndex.FindPropertyRelative("asyncUploadTimeSlice");
      SerializedProperty propertyRelative24 = arrayElementAtIndex.FindPropertyRelative("asyncUploadBufferSize");
      SerializedProperty propertyRelative25 = arrayElementAtIndex.FindPropertyRelative("resolutionScalingFixedDPIFactor");
      bool flag = (UnityEngine.Object) GraphicsSettings.renderPipelineAsset != (UnityEngine.Object) null;
      if (string.IsNullOrEmpty(propertyRelative1.stringValue))
        propertyRelative1.stringValue = "Level " + (object) selectedLevel;
      EditorGUILayout.PropertyField(propertyRelative1);
      if (flag)
        EditorGUILayout.HelpBox("A Scriptable Render Pipeline is in use, some settings will not be used and are hidden", MessageType.Info);
      GUILayout.Space(10f);
      GUILayout.Label(EditorGUIUtility.TempContent("Rendering"), EditorStyles.boldLabel, new GUILayoutOption[0]);
      if (!flag)
        EditorGUILayout.PropertyField(propertyRelative2);
      EditorGUILayout.PropertyField(propertyRelative13);
      EditorGUILayout.PropertyField(propertyRelative14);
      if (!flag)
      {
        EditorGUILayout.PropertyField(propertyRelative15);
        EditorGUILayout.PropertyField(propertyRelative16);
        if (propertyRelative16.boolValue)
          this.SoftParticlesHintGUI();
      }
      EditorGUILayout.PropertyField(propertyRelative17);
      EditorGUILayout.PropertyField(propertyRelative18, QualitySettingsEditor.Styles.kBillboardsFaceCameraPos, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(propertyRelative25);
      GUILayout.Space(10f);
      if (!flag)
      {
        GUILayout.Label(EditorGUIUtility.TempContent("Shadows"), EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(propertyRelative3);
        EditorGUILayout.PropertyField(propertyRelative4);
        EditorGUILayout.PropertyField(propertyRelative5);
        EditorGUILayout.PropertyField(propertyRelative7);
        EditorGUILayout.PropertyField(propertyRelative11);
        EditorGUILayout.PropertyField(propertyRelative8);
        EditorGUILayout.PropertyField(propertyRelative6);
        if (propertyRelative6.intValue == 2)
          this.DrawCascadeSplitGUI<float>(ref propertyRelative9);
        else if (propertyRelative6.intValue == 4)
          this.DrawCascadeSplitGUI<Vector3>(ref propertyRelative10);
        GUILayout.Space(10f);
      }
      GUILayout.Label(EditorGUIUtility.TempContent("Other"), EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(propertyRelative12);
      EditorGUILayout.PropertyField(propertyRelative19);
      EditorGUILayout.PropertyField(propertyRelative20);
      EditorGUILayout.PropertyField(propertyRelative21);
      EditorGUILayout.PropertyField(propertyRelative22);
      EditorGUILayout.PropertyField(propertyRelative23);
      EditorGUILayout.PropertyField(propertyRelative24);
      propertyRelative23.intValue = Mathf.Clamp(propertyRelative23.intValue, 1, 33);
      propertyRelative24.intValue = Mathf.Clamp(propertyRelative24.intValue, 2, 512);
      if (this.m_Dragging != null && this.m_Dragging.m_Position != this.m_Dragging.m_StartPosition)
      {
        this.m_QualitySettingsProperty.MoveArrayElement(this.m_Dragging.m_StartPosition, this.m_Dragging.m_Position);
        this.m_Dragging.m_StartPosition = this.m_Dragging.m_Position;
        int position = this.m_Dragging.m_Position;
        this.m_QualitySettings.ApplyModifiedProperties();
        QualitySettings.SetQualityLevel(Mathf.Clamp(position, 0, this.m_QualitySettingsProperty.arraySize - 1));
      }
      this.m_QualitySettings.ApplyModifiedProperties();
    }

    private static class Styles
    {
      public static readonly GUIStyle kToggle = (GUIStyle) "OL Toggle";
      public static readonly GUIStyle kDefaultToggle = (GUIStyle) "OL ToggleWhite";
      public static readonly GUIContent kPlatformTooltip = new GUIContent("", "Allow quality setting on platform");
      public static readonly GUIContent kAddQualityLevel = new GUIContent("Add Quality Level");
      public static readonly GUIContent kIconTrash = EditorGUIUtility.IconContent("TreeEditor.Trash", "|Delete Level");
      public static readonly GUIContent kSoftParticlesHint = EditorGUIUtility.TextContent("Soft Particles require using Deferred Lighting or making camera render the depth texture.");
      public static readonly GUIContent kBillboardsFaceCameraPos = EditorGUIUtility.TextContent("Billboards Face Camera Position|Make billboards face towards camera position. Otherwise they face towards camera plane. This makes billboards look nicer when camera rotates but is more expensive to render.");
      public static readonly GUIStyle kListEvenBg = (GUIStyle) "ObjectPickerResultsOdd";
      public static readonly GUIStyle kListOddBg = (GUIStyle) "ObjectPickerResultsEven";
      public static readonly GUIStyle kDefaultDropdown = (GUIStyle) "QualitySettingsDefault";
      public const int kMinToggleWidth = 15;
      public const int kMaxToggleWidth = 20;
      public const int kHeaderRowHeight = 20;
      public const int kLabelWidth = 80;
    }

    private struct QualitySetting
    {
      public string m_Name;
      public string m_PropertyPath;
      public List<string> m_ExcludedPlatforms;
    }

    private class Dragging
    {
      public int m_StartPosition;
      public int m_Position;
    }
  }
}
