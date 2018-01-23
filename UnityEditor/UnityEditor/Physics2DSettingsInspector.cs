// Decompiled with JetBrains decompiler
// Type: UnityEditor.Physics2DSettingsInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (Physics2DSettings))]
  internal class Physics2DSettingsInspector : ProjectSettingsBaseEditor
  {
    private bool m_ShowLayerCollisionMatrix = true;
    private readonly AnimBool m_GizmoSettingsFade = new AnimBool();
    private Vector2 m_LayerCollisionMatrixScrollPos;
    private static bool s_ShowGizmoSettings;
    private SerializedProperty m_AlwaysShowColliders;
    private SerializedProperty m_ShowColliderSleep;
    private SerializedProperty m_ShowColliderContacts;
    private SerializedProperty m_ShowColliderAABB;
    private SerializedProperty m_ContactArrowScale;
    private SerializedProperty m_ColliderAwakeColor;
    private SerializedProperty m_ColliderAsleepColor;
    private SerializedProperty m_ColliderContactColor;
    private SerializedProperty m_ColliderAABBColor;

    public void OnEnable()
    {
      this.m_AlwaysShowColliders = this.serializedObject.FindProperty("m_AlwaysShowColliders");
      this.m_ShowColliderSleep = this.serializedObject.FindProperty("m_ShowColliderSleep");
      this.m_ShowColliderContacts = this.serializedObject.FindProperty("m_ShowColliderContacts");
      this.m_ShowColliderAABB = this.serializedObject.FindProperty("m_ShowColliderAABB");
      this.m_ContactArrowScale = this.serializedObject.FindProperty("m_ContactArrowScale");
      this.m_ColliderAwakeColor = this.serializedObject.FindProperty("m_ColliderAwakeColor");
      this.m_ColliderAsleepColor = this.serializedObject.FindProperty("m_ColliderAsleepColor");
      this.m_ColliderContactColor = this.serializedObject.FindProperty("m_ColliderContactColor");
      this.m_ColliderAABBColor = this.serializedObject.FindProperty("m_ColliderAABBColor");
      this.m_GizmoSettingsFade.value = Physics2DSettingsInspector.s_ShowGizmoSettings;
      this.m_GizmoSettingsFade.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    public void OnDisable()
    {
      this.m_GizmoSettingsFade.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      this.DrawDefaultInspector();
      Physics2DSettingsInspector.s_ShowGizmoSettings = EditorGUILayout.Foldout(Physics2DSettingsInspector.s_ShowGizmoSettings, "Gizmos", true);
      this.m_GizmoSettingsFade.target = Physics2DSettingsInspector.s_ShowGizmoSettings;
      if (this.m_GizmoSettingsFade.value)
      {
        this.serializedObject.Update();
        if (EditorGUILayout.BeginFadeGroup(this.m_GizmoSettingsFade.faded))
        {
          ++EditorGUI.indentLevel;
          EditorGUILayout.PropertyField(this.m_AlwaysShowColliders);
          EditorGUILayout.PropertyField(this.m_ShowColliderSleep);
          EditorGUILayout.PropertyField(this.m_ColliderAwakeColor);
          EditorGUILayout.PropertyField(this.m_ColliderAsleepColor);
          EditorGUILayout.PropertyField(this.m_ShowColliderContacts);
          EditorGUILayout.Slider(this.m_ContactArrowScale, 0.1f, 1f, this.m_ContactArrowScale.displayName, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_ColliderContactColor);
          EditorGUILayout.PropertyField(this.m_ShowColliderAABB);
          EditorGUILayout.PropertyField(this.m_ColliderAABBColor);
          --EditorGUI.indentLevel;
        }
        EditorGUILayout.EndFadeGroup();
        this.serializedObject.ApplyModifiedProperties();
      }
      string title = "Layer Collision Matrix";
      // ISSUE: reference to a compiler-generated field
      if (Physics2DSettingsInspector.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Physics2DSettingsInspector.\u003C\u003Ef__mg\u0024cache0 = new LayerMatrixGUI.GetValueFunc(Physics2DSettingsInspector.GetValue);
      }
      // ISSUE: reference to a compiler-generated field
      LayerMatrixGUI.GetValueFunc fMgCache0 = Physics2DSettingsInspector.\u003C\u003Ef__mg\u0024cache0;
      // ISSUE: reference to a compiler-generated field
      if (Physics2DSettingsInspector.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Physics2DSettingsInspector.\u003C\u003Ef__mg\u0024cache1 = new LayerMatrixGUI.SetValueFunc(Physics2DSettingsInspector.SetValue);
      }
      // ISSUE: reference to a compiler-generated field
      LayerMatrixGUI.SetValueFunc fMgCache1 = Physics2DSettingsInspector.\u003C\u003Ef__mg\u0024cache1;
      LayerMatrixGUI.DoGUI(title, ref this.m_ShowLayerCollisionMatrix, ref this.m_LayerCollisionMatrixScrollPos, fMgCache0, fMgCache1);
    }

    private static bool GetValue(int layerA, int layerB)
    {
      return !Physics2D.GetIgnoreLayerCollision(layerA, layerB);
    }

    private static void SetValue(int layerA, int layerB, bool val)
    {
      Physics2D.IgnoreLayerCollision(layerA, layerB, !val);
    }
  }
}
