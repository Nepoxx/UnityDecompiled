// Decompiled with JetBrains decompiler
// Type: UnityEditor.RendererEditorBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class RendererEditorBase : Editor
  {
    private GUIContent m_DynamicOccludeeLabel = EditorGUIUtility.TextContent("Dynamic Occluded|Controls if dynamic occlusion culling should be performed for this renderer.");
    private SerializedProperty m_SortingOrder;
    private SerializedProperty m_SortingLayerID;
    private SerializedProperty m_DynamicOccludee;
    protected RendererEditorBase.Probes m_Probes;

    public virtual void OnEnable()
    {
      this.m_SortingOrder = this.serializedObject.FindProperty("m_SortingOrder");
      this.m_SortingLayerID = this.serializedObject.FindProperty("m_SortingLayerID");
      this.m_DynamicOccludee = this.serializedObject.FindProperty("m_DynamicOccludee");
    }

    protected void RenderSortingLayerFields()
    {
      EditorGUILayout.Space();
      SortingLayerEditorUtility.RenderSortingLayerFields(this.m_SortingOrder, this.m_SortingLayerID);
    }

    protected void InitializeProbeFields()
    {
      this.m_Probes = new RendererEditorBase.Probes();
      this.m_Probes.Initialize(this.serializedObject);
    }

    protected void RenderProbeFields()
    {
      this.m_Probes.OnGUI(this.targets, (Renderer) this.target, false);
    }

    protected void CullDynamicFieldGUI()
    {
      EditorGUILayout.PropertyField(this.m_DynamicOccludee, this.m_DynamicOccludeeLabel, new GUILayoutOption[0]);
    }

    protected void RenderCommonProbeFields(bool useMiniStyle)
    {
      bool isDeferredRenderingPath = SceneView.IsUsingDeferredRenderingPath();
      bool isDeferredReflections = isDeferredRenderingPath && GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections) != UnityEngine.Rendering.BuiltinShaderMode.Disabled;
      this.m_Probes.RenderReflectionProbeUsage(useMiniStyle, isDeferredRenderingPath, isDeferredReflections);
      this.m_Probes.RenderProbeAnchor(useMiniStyle);
    }

    internal class Probes
    {
      private GUIContent m_LightProbeUsageStyle = EditorGUIUtility.TextContent("Light Probes|Specifies how Light Probes will handle the interpolation of lighting and occlusion. Disabled if the object is set to Lightmap Static.");
      private GUIContent m_LightProbeVolumeOverrideStyle = EditorGUIUtility.TextContent("Proxy Volume Override|If set, the Renderer will use the Light Probe Proxy Volume component from another GameObject.");
      private GUIContent m_ReflectionProbeUsageStyle = EditorGUIUtility.TextContent("Reflection Probes|Specifies if or how the object is affected by reflections in the Scene.  This property cannot be disabled in deferred rendering modes.");
      private GUIContent m_ProbeAnchorStyle = EditorGUIUtility.TextContent("Anchor Override|Specifies the Transform position that will be used for sampling the light probes and reflection probes.");
      private GUIContent m_DeferredNote = EditorGUIUtility.TextContent("In Deferred Shading, all objects receive shadows and get per-pixel reflection probes.");
      private GUIContent m_LightProbeVolumeNote = EditorGUIUtility.TextContent("A valid Light Probe Proxy Volume component could not be found.");
      private GUIContent m_LightProbeVolumeUnsupportedNote = EditorGUIUtility.TextContent("The Light Probe Proxy Volume feature is unsupported by the current graphics hardware or API configuration. Simple 'Blend Probes' mode will be used instead.");
      private GUIContent m_LightProbeVolumeUnsupportedOnTreesNote = EditorGUIUtility.TextContent("The Light Probe Proxy Volume feature is not supported on tree rendering. Simple 'Blend Probes' mode will be used instead.");
      private GUIContent[] m_ReflectionProbeUsageOptions = ((IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (ReflectionProbeUsage))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>()).Select<string, GUIContent>((Func<string, GUIContent>) (x => new GUIContent(x))).ToArray<GUIContent>();
      private GUIContent[] m_LightProbeBlendModeOptions = ((IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (LightProbeUsage))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>()).Select<string, GUIContent>((Func<string, GUIContent>) (x => new GUIContent(x))).ToArray<GUIContent>();
      private List<ReflectionProbeBlendInfo> m_BlendInfo = new List<ReflectionProbeBlendInfo>();
      private SerializedProperty m_LightProbeUsage;
      private SerializedProperty m_LightProbeVolumeOverride;
      private SerializedProperty m_ReflectionProbeUsage;
      private SerializedProperty m_ProbeAnchor;
      private SerializedProperty m_ReceiveShadows;

      internal void Initialize(SerializedObject serializedObject)
      {
        this.m_LightProbeUsage = serializedObject.FindProperty("m_LightProbeUsage");
        this.m_LightProbeVolumeOverride = serializedObject.FindProperty("m_LightProbeVolumeOverride");
        this.m_ReflectionProbeUsage = serializedObject.FindProperty("m_ReflectionProbeUsage");
        this.m_ProbeAnchor = serializedObject.FindProperty("m_ProbeAnchor");
        this.m_ReceiveShadows = serializedObject.FindProperty("m_ReceiveShadows");
      }

      internal bool IsUsingLightProbeProxyVolume(int selectionCount)
      {
        return selectionCount == 1 && this.m_LightProbeUsage.intValue == 2 || selectionCount > 1 && !this.m_LightProbeUsage.hasMultipleDifferentValues && this.m_LightProbeUsage.intValue == 2;
      }

      internal bool HasValidLightProbeProxyVolumeOverride(Renderer renderer, int selectionCount)
      {
        LightProbeProxyVolume probeProxyVolume = !((UnityEngine.Object) renderer.lightProbeProxyVolumeOverride != (UnityEngine.Object) null) ? (LightProbeProxyVolume) null : renderer.lightProbeProxyVolumeOverride.GetComponent<LightProbeProxyVolume>();
        return this.IsUsingLightProbeProxyVolume(selectionCount) && ((UnityEngine.Object) probeProxyVolume == (UnityEngine.Object) null || probeProxyVolume.boundingBoxMode != LightProbeProxyVolume.BoundingBoxMode.AutomaticLocal);
      }

      internal void RenderLightProbeProxyVolumeWarningNote(Renderer renderer, int selectionCount)
      {
        if (!this.IsUsingLightProbeProxyVolume(selectionCount))
          return;
        if (LightProbeProxyVolume.isFeatureSupported)
        {
          if ((UnityEngine.Object) renderer.GetComponent<LightProbeProxyVolume>() == (UnityEngine.Object) null && ((UnityEngine.Object) renderer.lightProbeProxyVolumeOverride == (UnityEngine.Object) null || (UnityEngine.Object) renderer.lightProbeProxyVolumeOverride.GetComponent<LightProbeProxyVolume>() == (UnityEngine.Object) null) && LightProbes.AreLightProbesAllowed(renderer))
            EditorGUILayout.HelpBox(this.m_LightProbeVolumeNote.text, MessageType.Warning);
        }
        else
          EditorGUILayout.HelpBox(this.m_LightProbeVolumeUnsupportedNote.text, MessageType.Warning);
      }

      internal void RenderReflectionProbeUsage(bool useMiniStyle, bool isDeferredRenderingPath, bool isDeferredReflections)
      {
        using (new EditorGUI.DisabledScope(isDeferredRenderingPath))
        {
          if (!useMiniStyle)
          {
            if (isDeferredReflections)
              EditorGUILayout.EnumPopup(this.m_ReflectionProbeUsageStyle, (Enum) (ReflectionProbeUsage) (this.m_ReflectionProbeUsage.intValue == 0 ? 0 : 3), new GUILayoutOption[0]);
            else
              EditorGUILayout.Popup(this.m_ReflectionProbeUsage, this.m_ReflectionProbeUsageOptions, this.m_ReflectionProbeUsageStyle, new GUILayoutOption[0]);
          }
          else if (isDeferredReflections)
            ModuleUI.GUIPopup(this.m_ReflectionProbeUsageStyle, 3, this.m_ReflectionProbeUsageOptions);
          else
            ModuleUI.GUIPopup(this.m_ReflectionProbeUsageStyle, this.m_ReflectionProbeUsage, this.m_ReflectionProbeUsageOptions);
        }
      }

      internal void RenderLightProbeUsage(int selectionCount, Renderer renderer, bool useMiniStyle, bool lightProbeAllowed)
      {
        using (new EditorGUI.DisabledScope(!lightProbeAllowed))
        {
          if (!useMiniStyle)
          {
            if (lightProbeAllowed)
            {
              EditorGUILayout.Popup(this.m_LightProbeUsage, this.m_LightProbeBlendModeOptions, this.m_LightProbeUsageStyle, new GUILayoutOption[0]);
              if (!this.m_LightProbeUsage.hasMultipleDifferentValues)
              {
                if (this.m_LightProbeUsage.intValue == 2)
                {
                  ++EditorGUI.indentLevel;
                  EditorGUILayout.PropertyField(this.m_LightProbeVolumeOverride, this.m_LightProbeVolumeOverrideStyle, new GUILayoutOption[0]);
                  --EditorGUI.indentLevel;
                }
              }
            }
            else
              EditorGUILayout.EnumPopup(this.m_LightProbeUsageStyle, (Enum) LightProbeUsage.Off, new GUILayoutOption[0]);
          }
          else if (lightProbeAllowed)
          {
            ModuleUI.GUIPopup(this.m_LightProbeUsageStyle, this.m_LightProbeUsage, this.m_LightProbeBlendModeOptions);
            if (!this.m_LightProbeUsage.hasMultipleDifferentValues && this.m_LightProbeUsage.intValue == 2)
            {
              ++EditorGUI.indentLevel;
              ModuleUI.GUIObject(this.m_LightProbeVolumeOverrideStyle, this.m_LightProbeVolumeOverride);
              --EditorGUI.indentLevel;
            }
          }
          else
            ModuleUI.GUIPopup(this.m_LightProbeUsageStyle, 0, this.m_LightProbeBlendModeOptions);
        }
        if (!((UnityEngine.Object) renderer.GetComponent<Tree>() != (UnityEngine.Object) null) || this.m_LightProbeUsage.intValue != 2)
          return;
        ++EditorGUI.indentLevel;
        EditorGUILayout.HelpBox(this.m_LightProbeVolumeUnsupportedOnTreesNote.text, MessageType.Warning);
        --EditorGUI.indentLevel;
      }

      internal bool RenderProbeAnchor(bool useMiniStyle)
      {
        bool flag = !this.m_ReflectionProbeUsage.hasMultipleDifferentValues && this.m_ReflectionProbeUsage.intValue != 0 || !this.m_LightProbeUsage.hasMultipleDifferentValues && this.m_LightProbeUsage.intValue != 0;
        if (flag)
        {
          if (!useMiniStyle)
            EditorGUILayout.PropertyField(this.m_ProbeAnchor, this.m_ProbeAnchorStyle, new GUILayoutOption[0]);
          else
            ModuleUI.GUIObject(this.m_ProbeAnchorStyle, this.m_ProbeAnchor);
        }
        return flag;
      }

      internal void OnGUI(UnityEngine.Object[] selection, Renderer renderer, bool useMiniStyle)
      {
        int selectionCount = 1;
        bool isDeferredRenderingPath = SceneView.IsUsingDeferredRenderingPath();
        bool isDeferredReflections = isDeferredRenderingPath && GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections) != UnityEngine.Rendering.BuiltinShaderMode.Disabled;
        bool lightProbeAllowed = true;
        if (selection != null)
        {
          foreach (Renderer renderer1 in selection)
          {
            if (!LightProbes.AreLightProbesAllowed(renderer1))
            {
              lightProbeAllowed = false;
              break;
            }
          }
          selectionCount = selection.Length;
        }
        this.RenderLightProbeUsage(selectionCount, renderer, useMiniStyle, lightProbeAllowed);
        this.RenderLightProbeProxyVolumeWarningNote(renderer, selectionCount);
        this.RenderReflectionProbeUsage(useMiniStyle, isDeferredRenderingPath, isDeferredReflections);
        bool flag1 = this.RenderProbeAnchor(useMiniStyle);
        if (flag1 && (!this.m_ReflectionProbeUsage.hasMultipleDifferentValues && this.m_ReflectionProbeUsage.intValue != 0 && !isDeferredReflections))
        {
          renderer.GetClosestReflectionProbes(this.m_BlendInfo);
          RendererEditorBase.Probes.ShowClosestReflectionProbes(this.m_BlendInfo);
        }
        bool flag2 = !this.m_ReceiveShadows.hasMultipleDifferentValues && this.m_ReceiveShadows.boolValue;
        if ((!isDeferredRenderingPath || !flag2) && (!isDeferredReflections || !flag1))
          return;
        EditorGUILayout.HelpBox(this.m_DeferredNote.text, MessageType.Info);
      }

      internal static void ShowClosestReflectionProbes(List<ReflectionProbeBlendInfo> blendInfos)
      {
        float num1 = 20f;
        float num2 = 70f;
        using (new EditorGUI.DisabledScope(true))
        {
          for (int index = 0; index < blendInfos.Count; ++index)
          {
            Rect source = GUILayoutUtility.GetRect(0.0f, 16f);
            source = EditorGUI.IndentedRect(source);
            float num3 = source.width - num1 - num2;
            Rect position = source;
            position.width = num1;
            GUI.Label(position, "#" + (object) index, EditorStyles.miniLabel);
            position.x += position.width;
            position.width = num3;
            EditorGUI.ObjectField(position, (UnityEngine.Object) blendInfos[index].probe, typeof (UnityEngine.ReflectionProbe), true);
            position.x += position.width;
            position.width = num2;
            GUI.Label(position, "Weight " + blendInfos[index].weight.ToString("f2"), EditorStyles.miniLabel);
          }
        }
      }

      internal static string[] GetFieldsStringArray()
      {
        return new string[4]{ "m_LightProbeUsage", "m_LightProbeVolumeOverride", "m_ReflectionProbeUsage", "m_ProbeAnchor" };
      }
    }
  }
}
