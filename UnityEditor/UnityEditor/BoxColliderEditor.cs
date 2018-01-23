// Decompiled with JetBrains decompiler
// Type: UnityEditor.BoxColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (BoxCollider))]
  internal class BoxColliderEditor : PrimitiveCollider3DEditor
  {
    private readonly BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();
    protected GUIContent centerContent = EditorGUIUtility.TextContent("Center|The position of the Collider in the object’s local space.");
    protected GUIContent sizeContent = EditorGUIUtility.TextContent("Size|The size of the Collider in the X, Y, Z directions.");
    private SerializedProperty m_Center;
    private SerializedProperty m_Size;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Center = this.serializedObject.FindProperty("m_Center");
      this.m_Size = this.serializedObject.FindProperty("m_Size");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.InspectorEditButtonGUI();
      EditorGUILayout.PropertyField(this.m_IsTrigger, this.triggerContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Material, this.materialContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Center, this.centerContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Size, this.sizeContent, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
    }

    protected override PrimitiveBoundsHandle boundsHandle
    {
      get
      {
        return (PrimitiveBoundsHandle) this.m_BoundsHandle;
      }
    }

    protected override void CopyColliderPropertiesToHandle()
    {
      BoxCollider target = (BoxCollider) this.target;
      this.m_BoundsHandle.center = this.TransformColliderCenterToHandleSpace(target.transform, target.center);
      this.m_BoundsHandle.size = Vector3.Scale(target.size, target.transform.lossyScale);
    }

    protected override void CopyHandlePropertiesToCollider()
    {
      BoxCollider target = (BoxCollider) this.target;
      target.center = this.TransformHandleCenterToColliderSpace(target.transform, this.m_BoundsHandle.center);
      Vector3 vector3 = Vector3.Scale(this.m_BoundsHandle.size, this.InvertScaleVector(target.transform.lossyScale));
      vector3 = new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
      target.size = vector3;
    }
  }
}
