// Decompiled with JetBrains decompiler
// Type: UnityEditor.OcclusionPortalEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (OcclusionPortal))]
  internal class OcclusionPortalEditor : Editor
  {
    private readonly BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();
    private const string k_CenterPath = "m_Center";
    private const string k_SizePath = "m_Size";

    protected virtual void OnEnable()
    {
      this.m_BoundsHandle.SetColor(Handles.s_ColliderHandleColor);
    }

    public override void OnInspectorGUI()
    {
      UnityEditorInternal.EditMode.DoEditModeInspectorModeButton(UnityEditorInternal.EditMode.SceneViewEditMode.Collider, "Edit Bounds", PrimitiveBoundsHandle.editModeButton, (IToolModeOwner) this);
      base.OnInspectorGUI();
    }

    protected virtual void OnSceneGUI()
    {
      if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.Collider || !UnityEditorInternal.EditMode.IsOwner((Editor) this))
        return;
      OcclusionPortal target = this.target as OcclusionPortal;
      SerializedObject serializedObject = new SerializedObject((Object) target);
      serializedObject.Update();
      using (new Handles.DrawingScope(target.transform.localToWorldMatrix))
      {
        SerializedProperty property1 = serializedObject.FindProperty("m_Center");
        SerializedProperty property2 = serializedObject.FindProperty("m_Size");
        this.m_BoundsHandle.center = property1.vector3Value;
        this.m_BoundsHandle.size = property2.vector3Value;
        EditorGUI.BeginChangeCheck();
        this.m_BoundsHandle.DrawHandle();
        if (!EditorGUI.EndChangeCheck())
          return;
        property1.vector3Value = this.m_BoundsHandle.center;
        property2.vector3Value = this.m_BoundsHandle.size;
        serializedObject.ApplyModifiedProperties();
      }
    }

    internal override Bounds GetWorldBoundsOfTarget(Object targetObject)
    {
      SerializedObject serializedObject = new SerializedObject(targetObject);
      Bounds bounds1 = new Bounds(serializedObject.FindProperty("m_Center").vector3Value, serializedObject.FindProperty("m_Size").vector3Value);
      Vector3 max = bounds1.max;
      Vector3 min = bounds1.min;
      Matrix4x4 localToWorldMatrix = ((Component) targetObject).transform.localToWorldMatrix;
      Bounds bounds2 = new Bounds(localToWorldMatrix.MultiplyPoint3x4(new Vector3(max.x, max.y, max.z)), Vector3.zero);
      bounds2.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(max.x, max.y, max.z)));
      bounds2.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(max.x, max.y, min.z)));
      bounds2.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(max.x, min.y, max.z)));
      bounds2.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(min.x, max.y, max.z)));
      bounds2.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(max.x, min.y, min.z)));
      bounds2.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(min.x, max.y, min.z)));
      bounds2.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(min.x, min.y, max.z)));
      bounds2.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(min.x, min.y, min.z)));
      return bounds2;
    }
  }
}
