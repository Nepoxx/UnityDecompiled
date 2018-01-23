// Decompiled with JetBrains decompiler
// Type: UnityEditor.MeshRendererEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (MeshRenderer))]
  [CanEditMultipleObjects]
  internal class MeshRendererEditor : RendererEditorBase
  {
    private SerializedProperty m_Materials;
    private LightingSettingsInspector m_Lighting;
    private const string kDisplayLightingKey = "MeshRendererEditor.Lighting.ShowSettings";
    private const string kDisplayLightmapKey = "MeshRendererEditor.Lighting.ShowLightmapSettings";
    private const string kDisplayChartingKey = "MeshRendererEditor.Lighting.ShowChartingSettings";
    private SerializedObject m_GameObjectsSerializedObject;
    private SerializedProperty m_GameObjectStaticFlags;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Materials = this.serializedObject.FindProperty("m_Materials");
      this.m_GameObjectsSerializedObject = new SerializedObject((UnityEngine.Object[]) ((IEnumerable<UnityEngine.Object>) this.targets).Select<UnityEngine.Object, GameObject>((Func<UnityEngine.Object, GameObject>) (t => ((Component) t).gameObject)).ToArray<GameObject>());
      this.m_GameObjectStaticFlags = this.m_GameObjectsSerializedObject.FindProperty("m_StaticEditorFlags");
      this.InitializeProbeFields();
      this.InitializeLightingFields();
    }

    private void InitializeLightingFields()
    {
      this.m_Lighting = new LightingSettingsInspector(this.serializedObject);
      this.m_Lighting.showSettings = EditorPrefs.GetBool("MeshRendererEditor.Lighting.ShowSettings", false);
      this.m_Lighting.showChartingSettings = SessionState.GetBool("MeshRendererEditor.Lighting.ShowChartingSettings", true);
      this.m_Lighting.showLightmapSettings = SessionState.GetBool("MeshRendererEditor.Lighting.ShowLightmapSettings", true);
    }

    private void LightingFieldsGUI()
    {
      bool showSettings = this.m_Lighting.showSettings;
      bool chartingSettings = this.m_Lighting.showChartingSettings;
      bool lightmapSettings = this.m_Lighting.showLightmapSettings;
      if (this.m_Lighting.Begin())
      {
        this.RenderProbeFields();
        this.m_Lighting.RenderMeshSettings(true);
      }
      this.m_Lighting.End();
      if (this.m_Lighting.showSettings != showSettings)
        EditorPrefs.SetBool("MeshRendererEditor.Lighting.ShowSettings", this.m_Lighting.showSettings);
      if (this.m_Lighting.showChartingSettings != chartingSettings)
        SessionState.SetBool("MeshRendererEditor.Lighting.ShowChartingSettings", this.m_Lighting.showChartingSettings);
      if (this.m_Lighting.showLightmapSettings == lightmapSettings)
        return;
      SessionState.SetBool("MeshRendererEditor.Lighting.ShowLightmapSettings", this.m_Lighting.showLightmapSettings);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.LightingFieldsGUI();
      bool flag = false;
      if (!this.m_Materials.hasMultipleDifferentValues)
      {
        MeshFilter component = ((Component) this.serializedObject.targetObject).GetComponent<MeshFilter>();
        flag = (UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.sharedMesh != (UnityEngine.Object) null && this.m_Materials.arraySize > component.sharedMesh.subMeshCount;
      }
      EditorGUILayout.PropertyField(this.m_Materials, true, new GUILayoutOption[0]);
      if (!this.m_Materials.hasMultipleDifferentValues && flag)
        EditorGUILayout.HelpBox(MeshRendererEditor.Styles.MaterialWarning, MessageType.Warning, true);
      if (ShaderUtil.MaterialsUseInstancingShader(this.m_Materials))
      {
        this.m_GameObjectsSerializedObject.Update();
        if (!this.m_GameObjectStaticFlags.hasMultipleDifferentValues && (this.m_GameObjectStaticFlags.intValue & 4) != 0)
          EditorGUILayout.HelpBox(MeshRendererEditor.Styles.StaticBatchingWarning, MessageType.Warning, true);
      }
      this.CullDynamicFieldGUI();
      this.serializedObject.ApplyModifiedProperties();
    }

    private class Styles
    {
      public static readonly string MaterialWarning = "This renderer has more materials than the Mesh has submeshes. Multiple materials will be applied to the same submesh, which costs performance. Consider using multiple shader passes.";
      public static readonly string StaticBatchingWarning = "This renderer is statically batched and uses an instanced shader at the same time. Instancing will be disabled in such a case. Consider disabling static batching if you want it to be instanced.";
    }
  }
}
