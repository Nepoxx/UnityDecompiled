// Decompiled with JetBrains decompiler
// Type: UnityEditor.BoxCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (BoxCollider2D))]
  [CanEditMultipleObjects]
  internal class BoxCollider2DEditor : Collider2DEditorBase
  {
    private readonly AnimBool m_ShowCompositeRedundants = new AnimBool();
    private readonly BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();
    private SerializedProperty m_Size;
    private SerializedProperty m_EdgeRadius;
    private SerializedProperty m_UsedByComposite;

    protected override GUIContent editModeButton
    {
      get
      {
        return PrimitiveBoundsHandle.editModeButton;
      }
    }

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Size = this.serializedObject.FindProperty("m_Size");
      this.m_EdgeRadius = this.serializedObject.FindProperty("m_EdgeRadius");
      this.m_BoundsHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;
      this.m_UsedByComposite = this.serializedObject.FindProperty("m_UsedByComposite");
      this.m_AutoTiling = this.serializedObject.FindProperty("m_AutoTiling");
      this.m_ShowCompositeRedundants.value = !this.m_UsedByComposite.boolValue;
      this.m_ShowCompositeRedundants.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnDisable()
    {
      base.OnDisable();
      this.m_ShowCompositeRedundants.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      if (!this.CanEditCollider())
      {
        EditorGUILayout.HelpBox(Collider2DEditorBase.Styles.s_ColliderEditDisableHelp.text, MessageType.Info);
        if (this.editingCollider)
          UnityEditorInternal.EditMode.QuitEditMode();
      }
      else
        this.InspectorEditButtonGUI();
      base.OnInspectorGUI();
      EditorGUILayout.PropertyField(this.m_Size);
      this.m_ShowCompositeRedundants.target = !this.m_UsedByComposite.boolValue;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowCompositeRedundants.faded))
        EditorGUILayout.PropertyField(this.m_EdgeRadius);
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
      this.FinalizeInspectorGUI();
    }

    protected virtual void OnSceneGUI()
    {
      if (!this.editingCollider)
        return;
      BoxCollider2D target = (BoxCollider2D) this.target;
      if (Mathf.Approximately(target.transform.lossyScale.sqrMagnitude, 0.0f))
        return;
      Matrix4x4 matrix = target.transform.localToWorldMatrix;
      matrix.SetRow(0, Vector4.Scale(matrix.GetRow(0), new Vector4(1f, 1f, 0.0f, 1f)));
      matrix.SetRow(1, Vector4.Scale(matrix.GetRow(1), new Vector4(1f, 1f, 0.0f, 1f)));
      matrix.SetRow(2, new Vector4(0.0f, 0.0f, 1f, target.transform.position.z));
      if (target.usedByComposite && (Object) target.composite != (Object) null)
      {
        Vector3 pos = target.composite.transform.rotation * (Vector3) target.composite.offset;
        pos.z = 0.0f;
        matrix = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one) * matrix;
      }
      using (new Handles.DrawingScope(matrix))
      {
        this.m_BoundsHandle.center = (Vector3) target.offset;
        this.m_BoundsHandle.size = (Vector3) target.size;
        this.m_BoundsHandle.SetColor(!target.enabled ? Handles.s_ColliderHandleColorDisabled : Handles.s_ColliderHandleColor);
        EditorGUI.BeginChangeCheck();
        this.m_BoundsHandle.DrawHandle();
        if (!EditorGUI.EndChangeCheck())
          return;
        Undo.RecordObject((Object) target, string.Format("Modify {0}", (object) ObjectNames.NicifyVariableName(this.target.GetType().Name)));
        Vector2 size = target.size;
        target.size = (Vector2) this.m_BoundsHandle.size;
        if (target.size != size)
          target.offset = (Vector2) this.m_BoundsHandle.center;
      }
    }
  }
}
