// Decompiled with JetBrains decompiler
// Type: UnityEditor.SkinnedMeshRendererEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (SkinnedMeshRenderer))]
  internal class SkinnedMeshRendererEditor : RendererEditorBase
  {
    private BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();
    private const string kDisplayLightingKey = "SkinnedMeshRendererEditor.Lighting.ShowSettings";
    private SerializedProperty m_Materials;
    private SerializedProperty m_AABB;
    private SerializedProperty m_DirtyAABB;
    private SerializedProperty m_BlendShapeWeights;
    private LightingSettingsInspector m_Lighting;
    private string[] m_ExcludedProperties;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Materials = this.serializedObject.FindProperty("m_Materials");
      this.m_BlendShapeWeights = this.serializedObject.FindProperty("m_BlendShapeWeights");
      this.m_AABB = this.serializedObject.FindProperty("m_AABB");
      this.m_DirtyAABB = this.serializedObject.FindProperty("m_DirtyAABB");
      this.m_BoundsHandle.SetColor(Handles.s_BoundingBoxHandleColor);
      this.InitializeProbeFields();
      this.InitializeLightingFields();
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) new string[8]
      {
        "m_CastShadows",
        "m_ReceiveShadows",
        "m_MotionVectors",
        "m_Materials",
        "m_BlendShapeWeights",
        "m_AABB",
        "m_LightmapParameters",
        "m_DynamicOccludee"
      });
      stringList.AddRange((IEnumerable<string>) RendererEditorBase.Probes.GetFieldsStringArray());
      this.m_ExcludedProperties = stringList.ToArray();
    }

    private void InitializeLightingFields()
    {
      this.m_Lighting = new LightingSettingsInspector(this.serializedObject);
      this.m_Lighting.showSettings = EditorPrefs.GetBool("SkinnedMeshRendererEditor.Lighting.ShowSettings", false);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.OnBlendShapeUI();
      Editor.DrawPropertiesExcluding(this.serializedObject, this.m_ExcludedProperties);
      UnityEditorInternal.EditMode.DoEditModeInspectorModeButton(UnityEditorInternal.EditMode.SceneViewEditMode.Collider, "Edit Bounds", PrimitiveBoundsHandle.editModeButton, (IToolModeOwner) this);
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_AABB, new GUIContent("Bounds"), new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_DirtyAABB.boolValue = false;
      this.LightingFieldsGUI();
      EditorGUILayout.PropertyField(this.m_Materials, true, new GUILayoutOption[0]);
      this.CullDynamicFieldGUI();
      this.serializedObject.ApplyModifiedProperties();
    }

    internal override Bounds GetWorldBoundsOfTarget(Object targetObject)
    {
      return ((Renderer) targetObject).bounds;
    }

    private void LightingFieldsGUI()
    {
      bool showSettings = this.m_Lighting.showSettings;
      if (this.m_Lighting.Begin())
      {
        this.RenderProbeFields();
        this.m_Lighting.RenderMeshSettings(false);
      }
      this.m_Lighting.End();
      if (this.m_Lighting.showSettings == showSettings)
        return;
      EditorPrefs.SetBool("SkinnedMeshRendererEditor.Lighting.ShowSettings", this.m_Lighting.showSettings);
    }

    public void OnBlendShapeUI()
    {
      SkinnedMeshRenderer target = (SkinnedMeshRenderer) this.target;
      int num1 = !((Object) target.sharedMesh == (Object) null) ? target.sharedMesh.blendShapeCount : 0;
      if (num1 == 0)
        return;
      GUIContent label = new GUIContent();
      label.text = "BlendShapes";
      EditorGUILayout.PropertyField(this.m_BlendShapeWeights, label, false, new GUILayoutOption[0]);
      if (!this.m_BlendShapeWeights.isExpanded)
        return;
      ++EditorGUI.indentLevel;
      Mesh sharedMesh = target.sharedMesh;
      int num2 = this.m_BlendShapeWeights.arraySize;
      for (int index = 0; index < num1; ++index)
      {
        label.text = sharedMesh.GetBlendShapeName(index);
        if (index < num2)
        {
          EditorGUILayout.PropertyField(this.m_BlendShapeWeights.GetArrayElementAtIndex(index), label, new GUILayoutOption[0]);
        }
        else
        {
          EditorGUI.BeginChangeCheck();
          float num3 = EditorGUILayout.FloatField(label, 0.0f, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
          {
            this.m_BlendShapeWeights.arraySize = num1;
            num2 = num1;
            this.m_BlendShapeWeights.GetArrayElementAtIndex(index).floatValue = num3;
          }
        }
      }
      --EditorGUI.indentLevel;
    }

    public void OnSceneGUI()
    {
      SkinnedMeshRenderer target = (SkinnedMeshRenderer) this.target;
      if (target.updateWhenOffscreen)
      {
        Bounds bounds = target.bounds;
        Handles.DrawWireCube(bounds.center, bounds.size);
      }
      else
      {
        using (new Handles.DrawingScope(target.actualRootBone.localToWorldMatrix))
        {
          Bounds localBounds = target.localBounds;
          this.m_BoundsHandle.center = localBounds.center;
          this.m_BoundsHandle.size = localBounds.size;
          this.m_BoundsHandle.handleColor = UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.Collider || !UnityEditorInternal.EditMode.IsOwner((Editor) this) ? Color.clear : this.m_BoundsHandle.wireframeColor;
          EditorGUI.BeginChangeCheck();
          this.m_BoundsHandle.DrawHandle();
          if (EditorGUI.EndChangeCheck())
          {
            Undo.RecordObject((Object) target, "Resize Bounds");
            target.localBounds = new Bounds(this.m_BoundsHandle.center, this.m_BoundsHandle.size);
          }
        }
      }
    }
  }
}
