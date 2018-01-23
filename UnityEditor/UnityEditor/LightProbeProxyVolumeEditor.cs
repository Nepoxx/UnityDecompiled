// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightProbeProxyVolumeEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (LightProbeProxyVolume))]
  [CanEditMultipleObjects]
  internal class LightProbeProxyVolumeEditor : Editor
  {
    internal static Color kGizmoLightProbeProxyVolumeColor = new Color(1f, 0.8980392f, 0.5803922f, 0.5019608f);
    internal static Color kGizmoLightProbeProxyVolumeHandleColor = new Color(1f, 0.8980392f, 0.6666667f, 1f);
    private BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();
    private AnimBool m_ShowBoundingBoxOptions = new AnimBool();
    private AnimBool m_ShowComponentUnusedWarning = new AnimBool();
    private AnimBool m_ShowResolutionXYZOptions = new AnimBool();
    private AnimBool m_ShowResolutionProbesOption = new AnimBool();
    private AnimBool m_ShowNoRendererWarning = new AnimBool();
    private AnimBool m_ShowNoLightProbesWarning = new AnimBool();
    private static LightProbeProxyVolumeEditor s_LastInteractedEditor;
    private SerializedProperty m_ResolutionX;
    private SerializedProperty m_ResolutionY;
    private SerializedProperty m_ResolutionZ;
    private SerializedProperty m_BoundingBoxSize;
    private SerializedProperty m_BoundingBoxOrigin;
    private SerializedProperty m_BoundingBoxMode;
    private SerializedProperty m_ResolutionMode;
    private SerializedProperty m_ResolutionProbesPerUnit;
    private SerializedProperty m_ProbePositionMode;
    private SerializedProperty m_RefreshMode;

    private bool IsLightProbeVolumeProxyEditMode(UnityEditorInternal.EditMode.SceneViewEditMode editMode)
    {
      return editMode == UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeProxyVolumeBox || editMode == UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeProxyVolumeOrigin;
    }

    private bool sceneViewEditing
    {
      get
      {
        return this.IsLightProbeVolumeProxyEditMode(UnityEditorInternal.EditMode.editMode) && UnityEditorInternal.EditMode.IsOwner((Editor) this);
      }
    }

    private bool boundingBoxOptionsValue
    {
      get
      {
        return !this.m_BoundingBoxMode.hasMultipleDifferentValues && this.m_BoundingBoxMode.intValue == 2;
      }
    }

    private bool resolutionXYZOptionValue
    {
      get
      {
        return !this.m_ResolutionMode.hasMultipleDifferentValues && this.m_ResolutionMode.intValue == 1;
      }
    }

    private bool resolutionProbesOptionValue
    {
      get
      {
        return !this.m_ResolutionMode.hasMultipleDifferentValues && this.m_ResolutionMode.intValue == 0;
      }
    }

    private bool noLightProbesWarningValue
    {
      get
      {
        return (UnityEngine.Object) LightmapSettings.lightProbes == (UnityEngine.Object) null || LightmapSettings.lightProbes.count == 0;
      }
    }

    private bool componentUnusedWarningValue
    {
      get
      {
        Renderer component = ((Component) this.target).GetComponent(typeof (Renderer)) as Renderer;
        bool flag = (UnityEngine.Object) component != (UnityEngine.Object) null && LightProbes.AreLightProbesAllowed(component);
        return (UnityEngine.Object) component != (UnityEngine.Object) null && this.targets.Length == 1 && (component.lightProbeUsage != LightProbeUsage.UseProxyVolume || !flag);
      }
    }

    private bool noRendererWarningValue
    {
      get
      {
        return (UnityEngine.Object) (((Component) this.target).GetComponent(typeof (Renderer)) as Renderer) == (UnityEngine.Object) null && this.targets.Length == 1;
      }
    }

    private void SetOptions(AnimBool animBool, bool initialize, bool targetValue)
    {
      if (initialize)
      {
        animBool.value = targetValue;
        animBool.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      }
      else
        animBool.target = targetValue;
    }

    private void UpdateShowOptions(bool initialize)
    {
      this.SetOptions(this.m_ShowBoundingBoxOptions, initialize, this.boundingBoxOptionsValue);
      this.SetOptions(this.m_ShowComponentUnusedWarning, initialize, this.componentUnusedWarningValue);
      this.SetOptions(this.m_ShowResolutionXYZOptions, initialize, this.resolutionXYZOptionValue);
      this.SetOptions(this.m_ShowResolutionProbesOption, initialize, this.resolutionProbesOptionValue);
      this.SetOptions(this.m_ShowNoRendererWarning, initialize, this.noRendererWarningValue);
      this.SetOptions(this.m_ShowNoLightProbesWarning, initialize, this.noLightProbesWarningValue);
    }

    public void OnEnable()
    {
      this.m_ResolutionX = this.serializedObject.FindProperty("m_ResolutionX");
      this.m_ResolutionY = this.serializedObject.FindProperty("m_ResolutionY");
      this.m_ResolutionZ = this.serializedObject.FindProperty("m_ResolutionZ");
      this.m_BoundingBoxSize = this.serializedObject.FindProperty("m_BoundingBoxSize");
      this.m_BoundingBoxOrigin = this.serializedObject.FindProperty("m_BoundingBoxOrigin");
      this.m_BoundingBoxMode = this.serializedObject.FindProperty("m_BoundingBoxMode");
      this.m_ResolutionMode = this.serializedObject.FindProperty("m_ResolutionMode");
      this.m_ResolutionProbesPerUnit = this.serializedObject.FindProperty("m_ResolutionProbesPerUnit");
      this.m_ProbePositionMode = this.serializedObject.FindProperty("m_ProbePositionMode");
      this.m_RefreshMode = this.serializedObject.FindProperty("m_RefreshMode");
      this.m_BoundsHandle.handleColor = LightProbeProxyVolumeEditor.kGizmoLightProbeProxyVolumeHandleColor;
      this.m_BoundsHandle.wireframeColor = Color.clear;
      this.UpdateShowOptions(true);
    }

    internal override Bounds GetWorldBoundsOfTarget(UnityEngine.Object targetObject)
    {
      return ((LightProbeProxyVolume) this.target).boundsGlobal;
    }

    private void DoToolbar()
    {
      using (new EditorGUI.DisabledScope(this.m_BoundingBoxMode.intValue != 2))
      {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        UnityEditorInternal.EditMode.SceneViewEditMode editMode = UnityEditorInternal.EditMode.editMode;
        EditorGUI.BeginChangeCheck();
        UnityEditorInternal.EditMode.DoInspectorToolbar(LightProbeProxyVolumeEditor.Styles.sceneViewEditModes, LightProbeProxyVolumeEditor.Styles.toolContents, (IToolModeOwner) this);
        if (EditorGUI.EndChangeCheck())
          LightProbeProxyVolumeEditor.s_LastInteractedEditor = this;
        if (editMode != UnityEditorInternal.EditMode.editMode && (UnityEngine.Object) Toolbar.get != (UnityEngine.Object) null)
          Toolbar.get.Repaint();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
        string text = LightProbeProxyVolumeEditor.Styles.baseSceneEditingToolText;
        if (this.sceneViewEditing)
        {
          int index = ArrayUtility.IndexOf<UnityEditorInternal.EditMode.SceneViewEditMode>(LightProbeProxyVolumeEditor.Styles.sceneViewEditModes, UnityEditorInternal.EditMode.editMode);
          if (index >= 0)
            text = LightProbeProxyVolumeEditor.Styles.toolNames[index].text;
        }
        GUILayout.Label(text, LightProbeProxyVolumeEditor.Styles.richTextMiniLabel, new GUILayoutOption[0]);
        GUILayout.EndVertical();
        EditorGUILayout.Space();
      }
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.UpdateShowOptions(false);
      if ((UnityEngine.Object) ((Component) this.target).GetComponent<Tree>() != (UnityEngine.Object) null)
      {
        EditorGUILayout.HelpBox(LightProbeProxyVolumeEditor.Styles.componentUnsuportedOnTreesNote.text, MessageType.Info);
      }
      else
      {
        EditorGUILayout.Space();
        EditorGUILayout.Popup(this.m_RefreshMode, LightProbeProxyVolumeEditor.Styles.refreshMode, LightProbeProxyVolumeEditor.Styles.refreshModeText, new GUILayoutOption[0]);
        EditorGUILayout.Popup(this.m_BoundingBoxMode, LightProbeProxyVolumeEditor.Styles.bbMode, LightProbeProxyVolumeEditor.Styles.bbModeText, new GUILayoutOption[0]);
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowBoundingBoxOptions.faded))
        {
          if (this.targets.Length == 1)
            this.DoToolbar();
          GUILayout.Label(LightProbeProxyVolumeEditor.Styles.bbSettingsText);
          ++EditorGUI.indentLevel;
          EditorGUILayout.PropertyField(this.m_BoundingBoxSize, LightProbeProxyVolumeEditor.Styles.sizeText, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_BoundingBoxOrigin, LightProbeProxyVolumeEditor.Styles.originText, new GUILayoutOption[0]);
          --EditorGUI.indentLevel;
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.Space();
        GUILayout.Label(LightProbeProxyVolumeEditor.Styles.volumeResolutionText);
        ++EditorGUI.indentLevel;
        EditorGUILayout.Popup(this.m_ResolutionMode, LightProbeProxyVolumeEditor.Styles.resMode, LightProbeProxyVolumeEditor.Styles.resModeText, new GUILayoutOption[0]);
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowResolutionXYZOptions.faded))
        {
          EditorGUILayout.IntPopup(this.m_ResolutionX, LightProbeProxyVolumeEditor.Styles.volTextureSizes, LightProbeProxyVolumeEditor.Styles.volTextureSizesValues, LightProbeProxyVolumeEditor.Styles.resolutionXText, new GUILayoutOption[1]
          {
            GUILayout.MinWidth(40f)
          });
          EditorGUILayout.IntPopup(this.m_ResolutionY, LightProbeProxyVolumeEditor.Styles.volTextureSizes, LightProbeProxyVolumeEditor.Styles.volTextureSizesValues, LightProbeProxyVolumeEditor.Styles.resolutionYText, new GUILayoutOption[1]
          {
            GUILayout.MinWidth(40f)
          });
          EditorGUILayout.IntPopup(this.m_ResolutionZ, LightProbeProxyVolumeEditor.Styles.volTextureSizes, LightProbeProxyVolumeEditor.Styles.volTextureSizesValues, LightProbeProxyVolumeEditor.Styles.resolutionZText, new GUILayoutOption[1]
          {
            GUILayout.MinWidth(40f)
          });
        }
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowResolutionProbesOption.faded))
        {
          GUILayout.BeginHorizontal();
          EditorGUILayout.PropertyField(this.m_ResolutionProbesPerUnit, LightProbeProxyVolumeEditor.Styles.resProbesPerUnit, new GUILayoutOption[0]);
          GUILayout.Label(" probes per unit", EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
          GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFadeGroup();
        --EditorGUI.indentLevel;
        EditorGUILayout.Space();
        EditorGUILayout.Popup(this.m_ProbePositionMode, LightProbeProxyVolumeEditor.Styles.probePositionMode, LightProbeProxyVolumeEditor.Styles.probePositionText, new GUILayoutOption[0]);
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowComponentUnusedWarning.faded) && LightProbeProxyVolume.isFeatureSupported)
          EditorGUILayout.HelpBox(LightProbeProxyVolumeEditor.Styles.componentUnusedNote.text, MessageType.Warning);
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowNoRendererWarning.faded))
          EditorGUILayout.HelpBox(LightProbeProxyVolumeEditor.Styles.noRendererNode.text, MessageType.Info);
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowNoLightProbesWarning.faded))
          EditorGUILayout.HelpBox(LightProbeProxyVolumeEditor.Styles.noLightProbes.text, MessageType.Info);
        EditorGUILayout.EndFadeGroup();
        this.serializedObject.ApplyModifiedProperties();
      }
    }

    [DrawGizmo(GizmoType.Active)]
    private static void RenderBoxGizmo(LightProbeProxyVolume probeProxyVolume, GizmoType gizmoType)
    {
      if ((UnityEngine.Object) LightProbeProxyVolumeEditor.s_LastInteractedEditor == (UnityEngine.Object) null || !LightProbeProxyVolumeEditor.s_LastInteractedEditor.sceneViewEditing || UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeProxyVolumeBox)
        return;
      Color color = Gizmos.color;
      Gizmos.color = LightProbeProxyVolumeEditor.kGizmoLightProbeProxyVolumeColor;
      Vector3 originCustom = probeProxyVolume.originCustom;
      Matrix4x4 matrix = Gizmos.matrix;
      Gizmos.matrix = probeProxyVolume.transform.localToWorldMatrix;
      Gizmos.DrawCube(originCustom, -1f * probeProxyVolume.sizeCustom);
      Gizmos.matrix = matrix;
      Gizmos.color = color;
    }

    public void OnSceneGUI()
    {
      if (!this.sceneViewEditing)
        return;
      if (this.m_BoundingBoxMode.intValue != 2)
        UnityEditorInternal.EditMode.QuitEditMode();
      switch (UnityEditorInternal.EditMode.editMode)
      {
        case UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeProxyVolumeBox:
          this.DoBoxEditing();
          break;
        case UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeProxyVolumeOrigin:
          this.DoOriginEditing();
          break;
      }
    }

    private void DoOriginEditing()
    {
      LightProbeProxyVolume target = (LightProbeProxyVolume) this.target;
      Vector3 position1 = target.transform.TransformPoint(target.originCustom);
      EditorGUI.BeginChangeCheck();
      Vector3 position2 = Handles.PositionHandle(position1, target.transform.rotation);
      if (!EditorGUI.EndChangeCheck())
        return;
      Undo.RecordObject((UnityEngine.Object) target, "Modified Light Probe Proxy Volume Box Origin");
      target.originCustom = target.transform.InverseTransformPoint(position2);
      EditorUtility.SetDirty(this.target);
    }

    private void DoBoxEditing()
    {
      LightProbeProxyVolume target = (LightProbeProxyVolume) this.target;
      using (new Handles.DrawingScope(target.transform.localToWorldMatrix))
      {
        this.m_BoundsHandle.center = target.originCustom;
        this.m_BoundsHandle.size = target.sizeCustom;
        EditorGUI.BeginChangeCheck();
        this.m_BoundsHandle.DrawHandle();
        if (!EditorGUI.EndChangeCheck())
          return;
        Undo.RecordObject((UnityEngine.Object) target, "Modified Light Probe Proxy Volume AABB");
        target.originCustom = this.m_BoundsHandle.center;
        target.sizeCustom = this.m_BoundsHandle.size;
        EditorUtility.SetDirty(this.target);
      }
    }

    private static class Styles
    {
      public static GUIStyle richTextMiniLabel = new GUIStyle(EditorStyles.miniLabel);
      public static GUIContent volumeResolutionText = EditorGUIUtility.TextContent("Proxy Volume Resolution|Specifies the resolution of the 3D grid of interpolated light probes. Higher resolution/density means better lighting but the CPU cost will increase.");
      public static GUIContent resolutionXText = new GUIContent("X");
      public static GUIContent resolutionYText = new GUIContent("Y");
      public static GUIContent resolutionZText = new GUIContent("Z");
      public static GUIContent sizeText = EditorGUIUtility.TextContent("Size");
      public static GUIContent bbSettingsText = EditorGUIUtility.TextContent("Bounding Box Settings");
      public static GUIContent originText = EditorGUIUtility.TextContent("Origin");
      public static GUIContent bbModeText = EditorGUIUtility.TextContent("Bounding Box Mode|The mode in which the bounding box is computed. A 3D grid of interpolated light probes will be generated inside this bounding box.\n\nAutomatic Local - the local-space bounding box of the Renderer is used.\n\nAutomatic Global - a bounding box is computed which encloses the current Renderer and all the Renderers down the hierarchy that have the Light Probes property set to Use Proxy Volume. The bounding box will be world-space aligned.\n\nCustom - a custom bounding box is used. The bounding box is specified in the local-space of the game object.");
      public static GUIContent resModeText = EditorGUIUtility.TextContent("Resolution Mode|The mode in which the resolution of the 3D grid of interpolated light probes is specified:\n\nAutomatic - the resolution on each axis is computed using a user-specified number of interpolated light probes per unit area(Density).\n\nCustom - the user can specify a different resolution on each axis.");
      public static GUIContent probePositionText = EditorGUIUtility.TextContent("Probe Position Mode|The mode in which the interpolated probe positions are generated.\n\nCellCorner - divide the volume in cells and generate interpolated probe positions in the corner/edge of the cells.\n\nCellCenter - divide the volume in cells and generate interpolated probe positions in the center of the cells.");
      public static GUIContent refreshModeText = EditorGUIUtility.TextContent("Refresh Mode");
      public static GUIContent[] bbMode = ((IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (LightProbeProxyVolume.BoundingBoxMode))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>()).Select<string, GUIContent>((Func<string, GUIContent>) (x => new GUIContent(x))).ToArray<GUIContent>();
      public static GUIContent[] resMode = ((IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (LightProbeProxyVolume.ResolutionMode))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>()).Select<string, GUIContent>((Func<string, GUIContent>) (x => new GUIContent(x))).ToArray<GUIContent>();
      public static GUIContent[] probePositionMode = ((IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (LightProbeProxyVolume.ProbePositionMode))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>()).Select<string, GUIContent>((Func<string, GUIContent>) (x => new GUIContent(x))).ToArray<GUIContent>();
      public static GUIContent[] refreshMode = ((IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (LightProbeProxyVolume.RefreshMode))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>()).Select<string, GUIContent>((Func<string, GUIContent>) (x => new GUIContent(x))).ToArray<GUIContent>();
      public static GUIContent resProbesPerUnit = EditorGUIUtility.TextContent("Density|Density in probes per world unit.");
      public static GUIContent componentUnusedNote = EditorGUIUtility.TextContent("In order to use the component on this game object, the Light Probes property should be set to 'Use Proxy Volume' in Renderer.");
      public static GUIContent noRendererNode = EditorGUIUtility.TextContent("The component is unused by this game object because there is no Renderer component attached.");
      public static GUIContent noLightProbes = EditorGUIUtility.TextContent("The scene doesn't contain any light probes. Add light probes using Light Probe Group components (menu: Component->Rendering->Light Probe Group).");
      public static GUIContent componentUnsuportedOnTreesNote = EditorGUIUtility.TextContent("Tree rendering doesn't support Light Probe Proxy Volume components.");
      public static int[] volTextureSizesValues = new int[6]{ 1, 2, 4, 8, 16, 32 };
      public static GUIContent[] volTextureSizes = ((IEnumerable<int>) LightProbeProxyVolumeEditor.Styles.volTextureSizesValues).Select<int, GUIContent>((Func<int, GUIContent>) (n => new GUIContent(n.ToString()))).ToArray<GUIContent>();
      public static GUIContent[] toolContents = new GUIContent[2]{ PrimitiveBoundsHandle.editModeButton, EditorGUIUtility.IconContent("MoveTool", "|Move the selected objects.") };
      public static UnityEditorInternal.EditMode.SceneViewEditMode[] sceneViewEditModes = new UnityEditorInternal.EditMode.SceneViewEditMode[2]{ UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeProxyVolumeBox, UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeProxyVolumeOrigin };
      public static string baseSceneEditingToolText = "<color=grey>Light Probe Proxy Volume Scene Editing Mode:</color> ";
      public static GUIContent[] toolNames = new GUIContent[2]{ new GUIContent(LightProbeProxyVolumeEditor.Styles.baseSceneEditingToolText + "Box Bounds", ""), new GUIContent(LightProbeProxyVolumeEditor.Styles.baseSceneEditingToolText + "Box Origin", "") };

      static Styles()
      {
        LightProbeProxyVolumeEditor.Styles.richTextMiniLabel.richText = true;
      }
    }
  }
}
