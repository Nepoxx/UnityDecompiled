// Decompiled with JetBrains decompiler
// Type: UnityEditor.ReflectionProbeEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  [CustomEditor(typeof (UnityEngine.ReflectionProbe))]
  [CanEditMultipleObjects]
  internal class ReflectionProbeEditor : Editor
  {
    internal static Color kGizmoReflectionProbe = new Color(1f, 0.8980392f, 0.5803922f, 0.5019608f);
    internal static Color kGizmoReflectionProbeDisabled = new Color(0.6f, 0.5372549f, 0.3490196f, 0.3764706f);
    internal static Color kGizmoHandleReflectionProbe = new Color(1f, 0.8980392f, 0.6666667f, 1f);
    private Matrix4x4 m_OldLocalSpace = Matrix4x4.identity;
    private float m_MipLevelPreview = 0.0f;
    private BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();
    private Hashtable m_CachedGizmoMaterials = new Hashtable();
    private readonly AnimBool m_ShowProbeModeRealtimeOptions = new AnimBool();
    private readonly AnimBool m_ShowProbeModeCustomOptions = new AnimBool();
    private readonly AnimBool m_ShowBoxOptions = new AnimBool();
    private TextureInspector m_CubemapEditor = (TextureInspector) null;
    private static ReflectionProbeEditor s_LastInteractedEditor;
    private SerializedProperty m_Mode;
    private SerializedProperty m_RefreshMode;
    private SerializedProperty m_TimeSlicingMode;
    private SerializedProperty m_Resolution;
    private SerializedProperty m_ShadowDistance;
    private SerializedProperty m_Importance;
    private SerializedProperty m_BoxSize;
    private SerializedProperty m_BoxOffset;
    private SerializedProperty m_CullingMask;
    private SerializedProperty m_ClearFlags;
    private SerializedProperty m_BackgroundColor;
    private SerializedProperty m_HDR;
    private SerializedProperty m_BoxProjection;
    private SerializedProperty m_IntensityMultiplier;
    private SerializedProperty m_BlendDistance;
    private SerializedProperty m_CustomBakedTexture;
    private SerializedProperty m_RenderDynamicObjects;
    private SerializedProperty m_UseOcclusionCulling;
    private SerializedProperty[] m_NearAndFarProperties;
    private static Mesh s_SphereMesh;
    private Material m_ReflectiveMaterial;

    public static void GetResolutionArray(ref int[] resolutionList, ref GUIContent[] resolutionStringList)
    {
      if (ReflectionProbeEditor.Styles.reflectionResolutionValuesArray == null && ReflectionProbeEditor.Styles.reflectionResolutionTextArray == null)
      {
        int num = Mathf.Max(1, UnityEngine.ReflectionProbe.minBakedCubemapResolution);
        List<int> intList = new List<int>();
        List<GUIContent> guiContentList = new List<GUIContent>();
        do
        {
          intList.Add(num);
          guiContentList.Add(new GUIContent(num.ToString()));
          num *= 2;
        }
        while (num <= UnityEngine.ReflectionProbe.maxBakedCubemapResolution);
        ReflectionProbeEditor.Styles.reflectionResolutionValuesArray = intList.ToArray();
        ReflectionProbeEditor.Styles.reflectionResolutionTextArray = guiContentList.ToArray();
      }
      resolutionList = ReflectionProbeEditor.Styles.reflectionResolutionValuesArray;
      resolutionStringList = ReflectionProbeEditor.Styles.reflectionResolutionTextArray;
    }

    private bool IsReflectionProbeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode editMode)
    {
      return editMode == UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeBox || editMode == UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeOrigin;
    }

    private bool sceneViewEditing
    {
      get
      {
        return this.IsReflectionProbeEditMode(UnityEditorInternal.EditMode.editMode) && UnityEditorInternal.EditMode.IsOwner((Editor) this);
      }
    }

    public void OnEnable()
    {
      this.m_Mode = this.serializedObject.FindProperty("m_Mode");
      this.m_RefreshMode = this.serializedObject.FindProperty("m_RefreshMode");
      this.m_TimeSlicingMode = this.serializedObject.FindProperty("m_TimeSlicingMode");
      this.m_Resolution = this.serializedObject.FindProperty("m_Resolution");
      this.m_NearAndFarProperties = new SerializedProperty[2]
      {
        this.serializedObject.FindProperty("m_NearClip"),
        this.serializedObject.FindProperty("m_FarClip")
      };
      this.m_ShadowDistance = this.serializedObject.FindProperty("m_ShadowDistance");
      this.m_Importance = this.serializedObject.FindProperty("m_Importance");
      this.m_BoxSize = this.serializedObject.FindProperty("m_BoxSize");
      this.m_BoxOffset = this.serializedObject.FindProperty("m_BoxOffset");
      this.m_CullingMask = this.serializedObject.FindProperty("m_CullingMask");
      this.m_ClearFlags = this.serializedObject.FindProperty("m_ClearFlags");
      this.m_BackgroundColor = this.serializedObject.FindProperty("m_BackGroundColor");
      this.m_HDR = this.serializedObject.FindProperty("m_HDR");
      this.m_BoxProjection = this.serializedObject.FindProperty("m_BoxProjection");
      this.m_IntensityMultiplier = this.serializedObject.FindProperty("m_IntensityMultiplier");
      this.m_BlendDistance = this.serializedObject.FindProperty("m_BlendDistance");
      this.m_CustomBakedTexture = this.serializedObject.FindProperty("m_CustomBakedTexture");
      this.m_RenderDynamicObjects = this.serializedObject.FindProperty("m_RenderDynamicObjects");
      this.m_UseOcclusionCulling = this.serializedObject.FindProperty("m_UseOcclusionCulling");
      UnityEngine.ReflectionProbe target = this.target as UnityEngine.ReflectionProbe;
      this.m_ShowProbeModeRealtimeOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowProbeModeCustomOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowBoxOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowProbeModeRealtimeOptions.value = target.mode == ReflectionProbeMode.Realtime;
      this.m_ShowProbeModeCustomOptions.value = target.mode == ReflectionProbeMode.Custom;
      this.m_ShowBoxOptions.value = true;
      this.m_BoundsHandle.handleColor = ReflectionProbeEditor.kGizmoHandleReflectionProbe;
      this.m_BoundsHandle.wireframeColor = Color.clear;
      this.UpdateOldLocalSpace();
      SceneView.onPreSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnPreSceneGUICallback);
    }

    public void OnDisable()
    {
      SceneView.onPreSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnPreSceneGUICallback);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ReflectiveMaterial);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_CubemapEditor);
      IEnumerator enumerator = this.m_CachedGizmoMaterials.Values.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) enumerator.Current);
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      this.m_CachedGizmoMaterials.Clear();
    }

    private bool IsCollidingWithOtherProbes(string targetPath, UnityEngine.ReflectionProbe targetProbe, out UnityEngine.ReflectionProbe collidingProbe)
    {
      UnityEngine.ReflectionProbe[] array = ((IEnumerable<UnityEngine.ReflectionProbe>) UnityEngine.Object.FindObjectsOfType<UnityEngine.ReflectionProbe>()).ToArray<UnityEngine.ReflectionProbe>();
      collidingProbe = (UnityEngine.ReflectionProbe) null;
      foreach (UnityEngine.ReflectionProbe reflectionProbe in array)
      {
        if (!((UnityEngine.Object) reflectionProbe == (UnityEngine.Object) targetProbe) && !((UnityEngine.Object) reflectionProbe.customBakedTexture == (UnityEngine.Object) null) && AssetDatabase.GetAssetPath((UnityEngine.Object) reflectionProbe.customBakedTexture) == targetPath)
        {
          collidingProbe = reflectionProbe;
          return true;
        }
      }
      return false;
    }

    private void BakeCustomReflectionProbe(UnityEngine.ReflectionProbe probe, bool usePreviousAssetPath)
    {
      string str1 = "";
      if (usePreviousAssetPath)
        str1 = AssetDatabase.GetAssetPath((UnityEngine.Object) probe.customBakedTexture);
      string extension = !probe.hdr ? "png" : "exr";
      if (string.IsNullOrEmpty(str1) || Path.GetExtension(str1) != "." + extension)
      {
        string str2 = FileUtil.GetPathWithoutExtension(SceneManager.GetActiveScene().path);
        if (string.IsNullOrEmpty(str2))
          str2 = "Assets";
        else if (!Directory.Exists(str2))
          Directory.CreateDirectory(str2);
        string path2 = probe.name + (!probe.hdr ? "-reflection" : "-reflectionHDR") + "." + extension;
        str1 = EditorUtility.SaveFilePanelInProject("Save reflection probe's cubemap.", Path.GetFileNameWithoutExtension(AssetDatabase.GenerateUniqueAssetPath(Path.Combine(str2, path2))), extension, "", str2);
        UnityEngine.ReflectionProbe collidingProbe;
        if (string.IsNullOrEmpty(str1) || this.IsCollidingWithOtherProbes(str1, probe, out collidingProbe) && !EditorUtility.DisplayDialog("Cubemap is used by other reflection probe", string.Format("'{0}' path is used by the game object '{1}', do you really want to overwrite it?", (object) str1, (object) collidingProbe.name), "Yes", "No"))
          return;
      }
      EditorUtility.DisplayProgressBar("Reflection Probes", "Baking " + str1, 0.5f);
      if (!Lightmapping.BakeReflectionProbe(probe, str1))
        Debug.LogError((object) ("Failed to bake reflection probe to " + str1));
      EditorUtility.ClearProgressBar();
    }

    private void OnBakeCustomButton(object data)
    {
      int num = (int) data;
      UnityEngine.ReflectionProbe target = this.target as UnityEngine.ReflectionProbe;
      if (num != 0)
        return;
      this.BakeCustomReflectionProbe(target, false);
    }

    private void OnBakeButton(object data)
    {
      if ((int) data != 0)
        return;
      Lightmapping.BakeAllReflectionProbesSnapshots();
    }

    private UnityEngine.ReflectionProbe reflectionProbeTarget
    {
      get
      {
        return (UnityEngine.ReflectionProbe) this.target;
      }
    }

    private void DoBakeButton()
    {
      if (this.reflectionProbeTarget.mode == ReflectionProbeMode.Realtime)
      {
        EditorGUILayout.HelpBox("Baking of this reflection probe should be initiated from the scripting API because the type is 'Realtime'", MessageType.Info);
        if (QualitySettings.realtimeReflectionProbes)
          return;
        EditorGUILayout.HelpBox("Realtime reflection probes are disabled in Quality Settings", MessageType.Warning);
      }
      else if (this.reflectionProbeTarget.mode == ReflectionProbeMode.Baked && Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand)
      {
        EditorGUILayout.HelpBox("Baking of this reflection probe is automatic because this probe's type is 'Baked' and the Lighting window is using 'Auto Baking'. The cubemap created is stored in the GI cache.", MessageType.Info);
      }
      else
      {
        GUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUIUtility.labelWidth);
        switch (this.reflectionProbeMode)
        {
          case ReflectionProbeMode.Baked:
            using (new EditorGUI.DisabledScope(!this.reflectionProbeTarget.enabled))
            {
              if (EditorGUI.ButtonWithDropdownList(ReflectionProbeEditor.Styles.bakeButtonText, ReflectionProbeEditor.Styles.bakeButtonsText, new GenericMenu.MenuFunction2(this.OnBakeButton)))
              {
                Lightmapping.BakeReflectionProbeSnapshot(this.reflectionProbeTarget);
                GUIUtility.ExitGUI();
                break;
              }
              break;
            }
          case ReflectionProbeMode.Custom:
            if (EditorGUI.ButtonWithDropdownList(ReflectionProbeEditor.Styles.bakeCustomButtonText, ReflectionProbeEditor.Styles.bakeCustomOptionText, new GenericMenu.MenuFunction2(this.OnBakeCustomButton)))
            {
              this.BakeCustomReflectionProbe(this.reflectionProbeTarget, true);
              GUIUtility.ExitGUI();
              break;
            }
            break;
        }
        GUILayout.EndHorizontal();
      }
    }

    private void DoToolbar()
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUI.changed = false;
      UnityEditorInternal.EditMode.SceneViewEditMode editMode = UnityEditorInternal.EditMode.editMode;
      EditorGUI.BeginChangeCheck();
      UnityEditorInternal.EditMode.DoInspectorToolbar(ReflectionProbeEditor.Styles.sceneViewEditModes, ReflectionProbeEditor.Styles.toolContents, (IToolModeOwner) this);
      if (EditorGUI.EndChangeCheck())
        ReflectionProbeEditor.s_LastInteractedEditor = this;
      if (editMode != UnityEditorInternal.EditMode.editMode)
      {
        if (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeOrigin)
          this.UpdateOldLocalSpace();
        if ((UnityEngine.Object) Toolbar.get != (UnityEngine.Object) null)
          Toolbar.get.Repaint();
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      string text = ReflectionProbeEditor.Styles.baseSceneEditingToolText;
      if (this.sceneViewEditing)
      {
        int index = ArrayUtility.IndexOf<UnityEditorInternal.EditMode.SceneViewEditMode>(ReflectionProbeEditor.Styles.sceneViewEditModes, UnityEditorInternal.EditMode.editMode);
        if (index >= 0)
          text = ReflectionProbeEditor.Styles.toolNames[index].text;
      }
      GUILayout.Label(text, ReflectionProbeEditor.Styles.richTextMiniLabel, new GUILayoutOption[0]);
      GUILayout.EndVertical();
      EditorGUILayout.Space();
    }

    private ReflectionProbeMode reflectionProbeMode
    {
      get
      {
        return this.reflectionProbeTarget.mode;
      }
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      if (this.targets.Length == 1)
        this.DoToolbar();
      this.m_ShowProbeModeRealtimeOptions.target = this.reflectionProbeMode == ReflectionProbeMode.Realtime;
      this.m_ShowProbeModeCustomOptions.target = this.reflectionProbeMode == ReflectionProbeMode.Custom;
      EditorGUILayout.IntPopup(this.m_Mode, ReflectionProbeEditor.Styles.reflectionProbeMode, ReflectionProbeEditor.Styles.reflectionProbeModeValues, ReflectionProbeEditor.Styles.typeText, new GUILayoutOption[0]);
      if (!this.m_Mode.hasMultipleDifferentValues)
      {
        ++EditorGUI.indentLevel;
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowProbeModeCustomOptions.faded))
        {
          EditorGUILayout.PropertyField(this.m_RenderDynamicObjects, ReflectionProbeEditor.Styles.renderDynamicObjects, new GUILayoutOption[0]);
          EditorGUI.BeginChangeCheck();
          EditorGUI.showMixedValue = this.m_CustomBakedTexture.hasMultipleDifferentValues;
          UnityEngine.Object @object = EditorGUILayout.ObjectField(ReflectionProbeEditor.Styles.customCubemapText, this.m_CustomBakedTexture.objectReferenceValue, typeof (Cubemap), false, new GUILayoutOption[0]);
          EditorGUI.showMixedValue = false;
          if (EditorGUI.EndChangeCheck())
            this.m_CustomBakedTexture.objectReferenceValue = @object;
        }
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowProbeModeRealtimeOptions.faded))
        {
          EditorGUILayout.PropertyField(this.m_RefreshMode, ReflectionProbeEditor.Styles.refreshMode, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_TimeSlicingMode, ReflectionProbeEditor.Styles.timeSlicing, new GUILayoutOption[0]);
          EditorGUILayout.Space();
        }
        EditorGUILayout.EndFadeGroup();
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.Space();
      GUILayout.Label(ReflectionProbeEditor.Styles.runtimeSettingsHeader);
      ++EditorGUI.indentLevel;
      EditorGUILayout.PropertyField(this.m_Importance, ReflectionProbeEditor.Styles.importanceText, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_IntensityMultiplier, ReflectionProbeEditor.Styles.intensityText, new GUILayoutOption[0]);
      if (!EditorGraphicsSettings.GetCurrentTierSettings().reflectionProbeBoxProjection)
      {
        using (new EditorGUI.DisabledScope(true))
          EditorGUILayout.Toggle(ReflectionProbeEditor.Styles.boxProjectionText, false, new GUILayoutOption[0]);
      }
      else
        EditorGUILayout.PropertyField(this.m_BoxProjection, ReflectionProbeEditor.Styles.boxProjectionText, new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(!SceneView.IsUsingDeferredRenderingPath() || GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections) == UnityEngine.Rendering.BuiltinShaderMode.Disabled))
        EditorGUILayout.PropertyField(this.m_BlendDistance, ReflectionProbeEditor.Styles.blendDistanceText, new GUILayoutOption[0]);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowBoxOptions.faded))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(this.m_BoxSize, ReflectionProbeEditor.Styles.sizeText, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_BoxOffset, ReflectionProbeEditor.Styles.centerText, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          Vector3 vector3Value1 = this.m_BoxOffset.vector3Value;
          Vector3 vector3Value2 = this.m_BoxSize.vector3Value;
          if (this.ValidateAABB(ref vector3Value1, ref vector3Value2))
          {
            this.m_BoxOffset.vector3Value = vector3Value1;
            this.m_BoxSize.vector3Value = vector3Value2;
          }
        }
      }
      EditorGUILayout.EndFadeGroup();
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
      GUILayout.Label(ReflectionProbeEditor.Styles.captureCubemapHeaderText);
      ++EditorGUI.indentLevel;
      int[] resolutionList = (int[]) null;
      GUIContent[] resolutionStringList = (GUIContent[]) null;
      ReflectionProbeEditor.GetResolutionArray(ref resolutionList, ref resolutionStringList);
      EditorGUILayout.IntPopup(this.m_Resolution, resolutionStringList, resolutionList, ReflectionProbeEditor.Styles.resolutionText, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(40f)
      });
      EditorGUILayout.PropertyField(this.m_HDR);
      EditorGUILayout.PropertyField(this.m_ShadowDistance);
      EditorGUILayout.IntPopup(this.m_ClearFlags, ReflectionProbeEditor.Styles.clearFlags, ReflectionProbeEditor.Styles.clearFlagsValues, ReflectionProbeEditor.Styles.clearFlagsText, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_BackgroundColor, ReflectionProbeEditor.Styles.backgroundColorText, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_CullingMask);
      EditorGUILayout.PropertyField(this.m_UseOcclusionCulling);
      EditorGUILayout.PropertiesField(EditorGUI.s_ClipingPlanesLabel, this.m_NearAndFarProperties, EditorGUI.s_NearAndFarLabels, 35f);
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
      if (this.targets.Length == 1)
      {
        UnityEngine.ReflectionProbe target = (UnityEngine.ReflectionProbe) this.target;
        if (target.mode == ReflectionProbeMode.Custom && (UnityEngine.Object) target.customBakedTexture != (UnityEngine.Object) null)
        {
          Cubemap customBakedTexture = target.customBakedTexture as Cubemap;
          if ((bool) ((UnityEngine.Object) customBakedTexture) && customBakedTexture.mipmapCount == 1)
            EditorGUILayout.HelpBox("No mipmaps in the cubemap, Smoothness value in Standard shader will be ignored.", MessageType.Warning);
        }
      }
      this.DoBakeButton();
      EditorGUILayout.Space();
      this.serializedObject.ApplyModifiedProperties();
    }

    internal override Bounds GetWorldBoundsOfTarget(UnityEngine.Object targetObject)
    {
      return ((UnityEngine.ReflectionProbe) targetObject).bounds;
    }

    private bool ValidPreviewSetup()
    {
      UnityEngine.ReflectionProbe target = (UnityEngine.ReflectionProbe) this.target;
      return (UnityEngine.Object) target != (UnityEngine.Object) null && (UnityEngine.Object) target.texture != (UnityEngine.Object) null;
    }

    public override bool HasPreviewGUI()
    {
      if (this.targets.Length > 1)
        return false;
      if (this.ValidPreviewSetup())
      {
        Editor cubemapEditor = (Editor) this.m_CubemapEditor;
        Editor.CreateCachedEditor((UnityEngine.Object) ((UnityEngine.ReflectionProbe) this.target).texture, (System.Type) null, ref cubemapEditor);
        this.m_CubemapEditor = cubemapEditor as TextureInspector;
      }
      return true;
    }

    public override void OnPreviewSettings()
    {
      if (!this.ValidPreviewSetup())
        return;
      this.m_CubemapEditor.mipLevel = this.m_MipLevelPreview;
      EditorGUI.BeginChangeCheck();
      this.m_CubemapEditor.OnPreviewSettings();
      if (!EditorGUI.EndChangeCheck())
        return;
      EditorApplication.SetSceneRepaintDirty();
      this.m_MipLevelPreview = this.m_CubemapEditor.mipLevel;
    }

    public override void OnPreviewGUI(Rect position, GUIStyle style)
    {
      if (!this.ValidPreviewSetup() && Event.current.type != EventType.ExecuteCommand)
      {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        Color color = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, 0.5f);
        GUILayout.Label("Reflection Probe not baked/ready yet");
        GUI.color = color;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
      }
      else
      {
        UnityEngine.ReflectionProbe target = this.target as UnityEngine.ReflectionProbe;
        if ((UnityEngine.Object) target != (UnityEngine.Object) null && (UnityEngine.Object) target.texture != (UnityEngine.Object) null && this.targets.Length == 1)
        {
          Editor cubemapEditor = (Editor) this.m_CubemapEditor;
          Editor.CreateCachedEditor((UnityEngine.Object) target.texture, (System.Type) null, ref cubemapEditor);
          this.m_CubemapEditor = cubemapEditor as TextureInspector;
        }
        if (!((UnityEngine.Object) this.m_CubemapEditor != (UnityEngine.Object) null))
          return;
        this.m_CubemapEditor.SetCubemapIntensity(this.GetProbeIntensity((UnityEngine.ReflectionProbe) this.target));
        this.m_CubemapEditor.OnPreviewGUI(position, style);
      }
    }

    private static Mesh sphereMesh
    {
      get
      {
        return ReflectionProbeEditor.s_SphereMesh ?? (ReflectionProbeEditor.s_SphereMesh = UnityEngine.Resources.GetBuiltinResource(typeof (Mesh), "New-Sphere.fbx") as Mesh);
      }
    }

    private Material reflectiveMaterial
    {
      get
      {
        if ((UnityEngine.Object) this.m_ReflectiveMaterial == (UnityEngine.Object) null)
        {
          this.m_ReflectiveMaterial = (Material) UnityEngine.Object.Instantiate(EditorGUIUtility.Load("Previews/PreviewCubemapMaterial.mat"));
          this.m_ReflectiveMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
        return this.m_ReflectiveMaterial;
      }
    }

    private float GetProbeIntensity(UnityEngine.ReflectionProbe p)
    {
      if ((UnityEngine.Object) p == (UnityEngine.Object) null || (UnityEngine.Object) p.texture == (UnityEngine.Object) null)
        return 1f;
      float num = p.intensity;
      if (TextureUtil.GetTextureColorSpaceString(p.texture) == "Linear")
        num = Mathf.LinearToGammaSpace(num);
      return num;
    }

    private void OnPreSceneGUICallback(SceneView sceneView)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      foreach (UnityEngine.ReflectionProbe target in this.targets)
      {
        if (!(bool) ((UnityEngine.Object) this.reflectiveMaterial))
          break;
        Matrix4x4 matrix = new Matrix4x4();
        if (!this.m_CachedGizmoMaterials.ContainsKey((object) target))
          this.m_CachedGizmoMaterials.Add((object) target, (object) UnityEngine.Object.Instantiate<Material>(this.reflectiveMaterial));
        Material cachedGizmoMaterial = this.m_CachedGizmoMaterials[(object) target] as Material;
        if (!(bool) ((UnityEngine.Object) cachedGizmoMaterial))
          break;
        float num1 = 0.0f;
        TextureInspector cubemapEditor = this.m_CubemapEditor;
        if ((bool) ((UnityEngine.Object) cubemapEditor))
          num1 = cubemapEditor.GetMipLevelForRendering();
        cachedGizmoMaterial.SetTexture("_MainTex", target.texture);
        cachedGizmoMaterial.SetMatrix("_CubemapRotation", Matrix4x4.identity);
        cachedGizmoMaterial.SetFloat("_Mip", num1);
        cachedGizmoMaterial.SetFloat("_Alpha", 0.0f);
        cachedGizmoMaterial.SetFloat("_Intensity", this.GetProbeIntensity(target));
        float num2 = target.transform.lossyScale.magnitude * 0.5f;
        matrix.SetTRS(target.transform.position, Quaternion.identity, new Vector3(num2, num2, num2));
        Graphics.DrawMesh(ReflectionProbeEditor.sphereMesh, matrix, cachedGizmoMaterial, 0, SceneView.currentDrawingSceneView.camera, 0);
      }
    }

    private bool ValidateAABB(ref Vector3 center, ref Vector3 size)
    {
      UnityEngine.ReflectionProbe target = (UnityEngine.ReflectionProbe) this.target;
      Vector3 point = ReflectionProbeEditor.GetLocalSpace(target).inverse.MultiplyPoint3x4(target.transform.position);
      Bounds bounds = new Bounds(center, size);
      if (bounds.Contains(point))
        return false;
      bounds.Encapsulate(point);
      center = bounds.center;
      size = bounds.size;
      return true;
    }

    [DrawGizmo(GizmoType.Active)]
    private static void RenderBoxGizmo(UnityEngine.ReflectionProbe reflectionProbe, GizmoType gizmoType)
    {
      if ((UnityEngine.Object) ReflectionProbeEditor.s_LastInteractedEditor == (UnityEngine.Object) null || !ReflectionProbeEditor.s_LastInteractedEditor.sceneViewEditing || UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeBox)
        return;
      Color color = Gizmos.color;
      Gizmos.color = ReflectionProbeEditor.kGizmoReflectionProbe;
      Gizmos.matrix = ReflectionProbeEditor.GetLocalSpace(reflectionProbe);
      Gizmos.DrawCube(reflectionProbe.center, -1f * reflectionProbe.size);
      Gizmos.matrix = Matrix4x4.identity;
      Gizmos.color = color;
    }

    [DrawGizmo(GizmoType.Selected)]
    private static void RenderBoxOutline(UnityEngine.ReflectionProbe reflectionProbe, GizmoType gizmoType)
    {
      Color color = Gizmos.color;
      Gizmos.color = !reflectionProbe.isActiveAndEnabled ? ReflectionProbeEditor.kGizmoReflectionProbeDisabled : ReflectionProbeEditor.kGizmoReflectionProbe;
      Gizmos.matrix = ReflectionProbeEditor.GetLocalSpace(reflectionProbe);
      Gizmos.DrawWireCube(reflectionProbe.center, reflectionProbe.size);
      Gizmos.matrix = Matrix4x4.identity;
      Gizmos.color = color;
    }

    public void OnSceneGUI()
    {
      if (!this.sceneViewEditing)
        return;
      switch (UnityEditorInternal.EditMode.editMode)
      {
        case UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeBox:
          this.DoBoxEditing();
          break;
        case UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeOrigin:
          this.DoOriginEditing();
          break;
      }
    }

    private void UpdateOldLocalSpace()
    {
      this.m_OldLocalSpace = ReflectionProbeEditor.GetLocalSpace((UnityEngine.ReflectionProbe) this.target);
    }

    private void DoOriginEditing()
    {
      UnityEngine.ReflectionProbe target = (UnityEngine.ReflectionProbe) this.target;
      Vector3 position = target.transform.position;
      Vector3 size = target.size;
      EditorGUI.BeginChangeCheck();
      Vector3 point1 = Handles.PositionHandle(position, ReflectionProbeEditor.GetLocalSpaceRotation(target));
      if (!EditorGUI.EndChangeCheck() && !(this.m_OldLocalSpace != ReflectionProbeEditor.GetLocalSpace((UnityEngine.ReflectionProbe) this.target)))
        return;
      Vector3 point2 = this.m_OldLocalSpace.inverse.MultiplyPoint3x4(point1);
      Vector3 point3 = new Bounds(target.center, size).ClosestPoint(point2);
      Undo.RecordObject((UnityEngine.Object) target.transform, "Modified Reflection Probe Origin");
      target.transform.position = this.m_OldLocalSpace.MultiplyPoint3x4(point3);
      Undo.RecordObject((UnityEngine.Object) target, "Modified Reflection Probe Origin");
      target.center = ReflectionProbeEditor.GetLocalSpace(target).inverse.MultiplyPoint3x4(this.m_OldLocalSpace.MultiplyPoint3x4(target.center));
      EditorUtility.SetDirty(this.target);
      this.UpdateOldLocalSpace();
    }

    private static Matrix4x4 GetLocalSpace(UnityEngine.ReflectionProbe probe)
    {
      return Matrix4x4.TRS(probe.transform.position, ReflectionProbeEditor.GetLocalSpaceRotation(probe), Vector3.one);
    }

    private static Quaternion GetLocalSpaceRotation(UnityEngine.ReflectionProbe probe)
    {
      if ((SupportedRenderingFeatures.active.reflectionProbe & SupportedRenderingFeatures.ReflectionProbe.Rotation) != SupportedRenderingFeatures.ReflectionProbe.None)
        return probe.transform.rotation;
      return Quaternion.identity;
    }

    private void DoBoxEditing()
    {
      UnityEngine.ReflectionProbe target = (UnityEngine.ReflectionProbe) this.target;
      using (new Handles.DrawingScope(ReflectionProbeEditor.GetLocalSpace(target)))
      {
        this.m_BoundsHandle.center = target.center;
        this.m_BoundsHandle.size = target.size;
        EditorGUI.BeginChangeCheck();
        this.m_BoundsHandle.DrawHandle();
        if (!EditorGUI.EndChangeCheck())
          return;
        Undo.RecordObject((UnityEngine.Object) target, "Modified Reflection Probe AABB");
        Vector3 center = this.m_BoundsHandle.center;
        Vector3 size = this.m_BoundsHandle.size;
        this.ValidateAABB(ref center, ref size);
        target.center = center;
        target.size = size;
        EditorUtility.SetDirty(this.target);
      }
    }

    internal static class Styles
    {
      public static GUIStyle richTextMiniLabel = new GUIStyle(EditorStyles.miniLabel);
      public static string bakeButtonText = "Bake";
      public static string[] bakeCustomOptionText = new string[1]{ "Bake as new Cubemap..." };
      public static string[] bakeButtonsText = new string[1]{ "Bake All Reflection Probes" };
      public static GUIContent bakeCustomButtonText = EditorGUIUtility.TextContent("Bake|Bakes Reflection Probe's cubemap, overwriting the existing cubemap texture asset (if any).");
      public static GUIContent runtimeSettingsHeader = new GUIContent("Runtime settings", "These settings are used by objects when they render with the cubemap of this probe");
      public static GUIContent backgroundColorText = new GUIContent("Background", "Camera clears the screen to this color before rendering.");
      public static GUIContent clearFlagsText = new GUIContent("Clear Flags");
      public static GUIContent intensityText = new GUIContent("Intensity");
      public static GUIContent resolutionText = new GUIContent("Resolution");
      public static GUIContent captureCubemapHeaderText = new GUIContent("Cubemap capture settings");
      public static GUIContent boxProjectionText = new GUIContent("Box Projection", "Box projection causes reflections to appear to change based on the object's position within the probe's box, while still using a single probe as the source of the reflection. This works well for reflections on objects that are moving through enclosed spaces such as corridors and rooms. Setting box projection to False and the cubemap reflection will be treated as coming from infinitely far away. Note that this feature can be globally disabled from Graphics Settings -> Tier Settings");
      public static GUIContent blendDistanceText = new GUIContent("Blend Distance", "Area around the probe where it is blended with other probes. Only used in deferred probes.");
      public static GUIContent sizeText = EditorGUIUtility.TextContent("Box Size|The size of the box in which the reflections will be applied to objects. The value is not affected by the Transform of the Game Object.");
      public static GUIContent centerText = EditorGUIUtility.TextContent("Box Offset|The center of the box in which the reflections will be applied to objects. The value is relative to the position of the Game Object.");
      public static GUIContent customCubemapText = new GUIContent("Cubemap");
      public static GUIContent importanceText = new GUIContent("Importance");
      public static GUIContent renderDynamicObjects = new GUIContent("Dynamic Objects", "If enabled dynamic objects are also rendered into the cubemap");
      public static GUIContent timeSlicing = new GUIContent("Time Slicing", "If enabled this probe will update over several frames, to help reduce the impact on the frame rate");
      public static GUIContent refreshMode = new GUIContent("Refresh Mode", "Controls how this probe refreshes in the Player");
      public static GUIContent typeText = new GUIContent("Type", "'Baked Cubemap' uses the 'Auto Baking' mode from the Lighting window. If it is enabled then baking is automatic otherwise manual bake is needed (use the bake button below). \n'Custom' can be used if a custom cubemap is wanted. \n'Realtime' can be used to dynamically re-render the cubemap during runtime (via scripting).");
      public static GUIContent[] reflectionProbeMode = new GUIContent[3]{ new GUIContent("Baked"), new GUIContent("Custom"), new GUIContent("Realtime") };
      public static int[] reflectionProbeModeValues = new int[3]{ 0, 2, 1 };
      public static int[] reflectionResolutionValuesArray = (int[]) null;
      public static GUIContent[] reflectionResolutionTextArray = (GUIContent[]) null;
      public static GUIContent[] clearFlags = new GUIContent[2]{ new GUIContent("Skybox"), new GUIContent("Solid Color") };
      public static int[] clearFlagsValues = new int[2]{ 1, 2 };
      public static GUIContent[] toolContents = new GUIContent[2]{ PrimitiveBoundsHandle.editModeButton, EditorGUIUtility.IconContent("MoveTool", "|Move the selected objects.") };
      public static UnityEditorInternal.EditMode.SceneViewEditMode[] sceneViewEditModes = new UnityEditorInternal.EditMode.SceneViewEditMode[2]{ UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeBox, UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeOrigin };
      public static string baseSceneEditingToolText = "<color=grey>Probe Scene Editing Mode:</color> ";
      public static GUIContent[] toolNames = new GUIContent[2]{ new GUIContent(ReflectionProbeEditor.Styles.baseSceneEditingToolText + "Box Projection Bounds", ""), new GUIContent(ReflectionProbeEditor.Styles.baseSceneEditingToolText + "Probe Origin", "") };

      static Styles()
      {
        ReflectionProbeEditor.Styles.richTextMiniLabel.richText = true;
      }
    }
  }
}
