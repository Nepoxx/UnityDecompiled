// Decompiled with JetBrains decompiler
// Type: UnityEditor.PhysicsManagerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (PhysicsManager))]
  internal class PhysicsManagerInspector : ProjectSettingsBaseEditor
  {
    private bool show = true;
    private Vector2 scrollPos;

    private bool GetValue(int layerA, int layerB)
    {
      return !Physics.GetIgnoreLayerCollision(layerA, layerB);
    }

    private void SetValue(int layerA, int layerB, bool val)
    {
      Physics.IgnoreLayerCollision(layerA, layerB, !val);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      Editor.DrawPropertiesExcluding(this.serializedObject, "m_ClothInterCollisionDistance", "m_ClothInterCollisionStiffness", "m_ClothInterCollisionSettingsToggle");
      LayerMatrixGUI.DoGUI("Layer Collision Matrix", ref this.show, ref this.scrollPos, new LayerMatrixGUI.GetValueFunc(this.GetValue), new LayerMatrixGUI.SetValueFunc(this.SetValue));
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle(PhysicsManagerInspector.Styles.interCollisionPropertiesLabel, Physics.interCollisionSettingsToggle, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        Physics.interCollisionSettingsToggle = flag;
      if (Physics.interCollisionSettingsToggle)
      {
        ++EditorGUI.indentLevel;
        EditorGUI.BeginChangeCheck();
        float num1 = EditorGUILayout.FloatField(PhysicsManagerInspector.Styles.interCollisionDistanceLabel, Physics.interCollisionDistance, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          if ((double) num1 < 0.0)
            num1 = 0.0f;
          Physics.interCollisionDistance = num1;
        }
        EditorGUI.BeginChangeCheck();
        float num2 = EditorGUILayout.FloatField(PhysicsManagerInspector.Styles.interCollisionStiffnessLabel, Physics.interCollisionStiffness, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          if ((double) num2 < 0.0)
            num2 = 0.0f;
          Physics.interCollisionStiffness = num2;
        }
        --EditorGUI.indentLevel;
      }
      this.serializedObject.ApplyModifiedProperties();
    }

    private static class Styles
    {
      public static readonly GUIContent interCollisionPropertiesLabel = EditorGUIUtility.TextContent("Cloth Inter-Collision");
      public static readonly GUIContent interCollisionDistanceLabel = EditorGUIUtility.TextContent("Distance");
      public static readonly GUIContent interCollisionStiffnessLabel = EditorGUIUtility.TextContent("Stiffness");
    }
  }
}
