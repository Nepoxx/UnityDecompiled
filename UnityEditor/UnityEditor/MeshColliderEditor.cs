// Decompiled with JetBrains decompiler
// Type: UnityEditor.MeshColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (MeshCollider))]
  [CanEditMultipleObjects]
  internal class MeshColliderEditor : Collider3DEditorBase
  {
    private SerializedProperty m_Mesh;
    private SerializedProperty m_Convex;
    private SerializedProperty m_CookingOptions;
    private SerializedProperty m_SkinWidth;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Mesh = this.serializedObject.FindProperty("m_Mesh");
      this.m_Convex = this.serializedObject.FindProperty("m_Convex");
      this.m_CookingOptions = this.serializedObject.FindProperty("m_CookingOptions");
      this.m_SkinWidth = this.serializedObject.FindProperty("m_SkinWidth");
    }

    private MeshColliderCookingOptions GetCookingOptions()
    {
      return (MeshColliderCookingOptions) this.m_CookingOptions.intValue;
    }

    private void SetCookingOptions(MeshColliderCookingOptions cookingOptions)
    {
      this.m_CookingOptions.intValue = (int) cookingOptions;
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_Convex, MeshColliderEditor.Texts.convextText, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck() && !this.m_Convex.boolValue)
      {
        this.m_IsTrigger.boolValue = false;
        this.SetCookingOptions(this.GetCookingOptions() & ~MeshColliderCookingOptions.InflateConvexMesh);
      }
      ++EditorGUI.indentLevel;
      using (new EditorGUI.DisabledScope(!this.m_Convex.boolValue))
        EditorGUILayout.PropertyField(this.m_IsTrigger, MeshColliderEditor.Texts.isTriggerText, new GUILayoutOption[0]);
      --EditorGUI.indentLevel;
      if (this.m_Convex.boolValue && (this.GetCookingOptions() & MeshColliderCookingOptions.InflateConvexMesh) != MeshColliderCookingOptions.None)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_SkinWidth, MeshColliderEditor.Texts.skinWidthText, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
      }
      this.SetCookingOptions((MeshColliderCookingOptions) EditorGUILayout.EnumFlagsField(MeshColliderEditor.Texts.cookingOptionsText, (Enum) this.GetCookingOptions(), new GUILayoutOption[0]));
      EditorGUILayout.PropertyField(this.m_Material);
      EditorGUILayout.PropertyField(this.m_Mesh);
      this.serializedObject.ApplyModifiedProperties();
    }

    private static class Texts
    {
      public static GUIContent isTriggerText = new GUIContent("Is Trigger", "Is this collider a trigger? Triggers are only supported on convex colliders.");
      public static GUIContent convextText = new GUIContent("Convex", "Is this collider convex?");
      public static GUIContent skinWidthText = new GUIContent("Skin Width", "How far out to inflate the mesh when building collision mesh.");
      public static GUIContent cookingOptionsText = new GUIContent("Cooking Options", "Options affecting the result of the mesh processing by the physics engine.");
    }
  }
}
