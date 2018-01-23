// Decompiled with JetBrains decompiler
// Type: UnityEditor.CompositeCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (CompositeCollider2D))]
  [CanEditMultipleObjects]
  internal class CompositeCollider2DEditor : Collider2DEditorBase
  {
    private readonly AnimBool m_ShowEdgeRadius = new AnimBool();
    private readonly AnimBool m_ShowManualGenerationButton = new AnimBool();
    private SerializedProperty m_GeometryType;
    private SerializedProperty m_GenerationType;
    private SerializedProperty m_VertexDistance;
    private SerializedProperty m_EdgeRadius;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_GeometryType = this.serializedObject.FindProperty("m_GeometryType");
      this.m_GenerationType = this.serializedObject.FindProperty("m_GenerationType");
      this.m_VertexDistance = this.serializedObject.FindProperty("m_VertexDistance");
      this.m_EdgeRadius = this.serializedObject.FindProperty("m_EdgeRadius");
      this.m_ShowEdgeRadius.value = ((IEnumerable<UnityEngine.Object>) this.targets).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => (x as CompositeCollider2D).geometryType == CompositeCollider2D.GeometryType.Polygons)).Count<UnityEngine.Object>() == 0;
      this.m_ShowEdgeRadius.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowManualGenerationButton.value = ((IEnumerable<UnityEngine.Object>) this.targets).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => (x as CompositeCollider2D).generationType != CompositeCollider2D.GenerationType.Manual)).Count<UnityEngine.Object>() == 0;
      this.m_ShowManualGenerationButton.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnDisable()
    {
      this.m_ShowEdgeRadius.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowManualGenerationButton.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      base.OnInspectorGUI();
      EditorGUILayout.PropertyField(this.m_GeometryType);
      EditorGUILayout.PropertyField(this.m_GenerationType);
      EditorGUILayout.PropertyField(this.m_VertexDistance);
      this.m_ShowManualGenerationButton.target = ((IEnumerable<UnityEngine.Object>) this.targets).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => (x as CompositeCollider2D).generationType != CompositeCollider2D.GenerationType.Manual)).Count<UnityEngine.Object>() == 0;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowManualGenerationButton.faded) && GUILayout.Button("Regenerate Collider"))
      {
        foreach (UnityEngine.Object target in this.targets)
          (target as CompositeCollider2D).GenerateGeometry();
      }
      EditorGUILayout.EndFadeGroup();
      this.m_ShowEdgeRadius.target = ((IEnumerable<UnityEngine.Object>) this.targets).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => (x as CompositeCollider2D).geometryType == CompositeCollider2D.GeometryType.Polygons)).Count<UnityEngine.Object>() == 0;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowEdgeRadius.faded))
        EditorGUILayout.PropertyField(this.m_EdgeRadius);
      EditorGUILayout.EndFadeGroup();
      if (((IEnumerable<UnityEngine.Object>) this.targets).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => (x as CompositeCollider2D).geometryType == CompositeCollider2D.GeometryType.Outlines && (UnityEngine.Object) (x as CompositeCollider2D).attachedRigidbody != (UnityEngine.Object) null && (x as CompositeCollider2D).attachedRigidbody.bodyType == RigidbodyType2D.Dynamic)).Count<UnityEngine.Object>() > 0)
        EditorGUILayout.HelpBox("Outline geometry is composed of edges and will not preserve the original collider's center-of-mass or rotational inertia.  The CompositeCollider2D is attached to a Dynamic Rigidbody2D so you may need to explicitly set these if they are required.", MessageType.Info);
      this.serializedObject.ApplyModifiedProperties();
      this.FinalizeInspectorGUI();
    }
  }
}
