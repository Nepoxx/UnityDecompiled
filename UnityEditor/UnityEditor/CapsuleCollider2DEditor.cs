// Decompiled with JetBrains decompiler
// Type: UnityEditor.CapsuleCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (CapsuleCollider2D))]
  internal class CapsuleCollider2DEditor : PrimitiveCollider2DEditor
  {
    private readonly CapsuleBoundsHandle m_BoundsHandle = new CapsuleBoundsHandle();
    private SerializedProperty m_Size;
    private SerializedProperty m_Direction;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Size = this.serializedObject.FindProperty("m_Size");
      this.m_Direction = this.serializedObject.FindProperty("m_Direction");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.InspectorEditButtonGUI();
      base.OnInspectorGUI();
      EditorGUILayout.PropertyField(this.m_Size);
      EditorGUILayout.PropertyField(this.m_Direction);
      this.serializedObject.ApplyModifiedProperties();
      this.FinalizeInspectorGUI();
    }

    protected override PrimitiveBoundsHandle boundsHandle
    {
      get
      {
        return (PrimitiveBoundsHandle) this.m_BoundsHandle;
      }
    }

    protected override void CopyColliderSizeToHandle()
    {
      Vector3 handleHeightVector;
      Vector3 handleDiameterVector;
      this.GetHandleVectorsInWorldSpace((CapsuleCollider2D) this.target, out handleHeightVector, out handleDiameterVector);
      CapsuleBoundsHandle boundsHandle = this.m_BoundsHandle;
      float num1 = 0.0f;
      this.m_BoundsHandle.radius = num1;
      double num2 = (double) num1;
      boundsHandle.height = (float) num2;
      this.m_BoundsHandle.height = handleHeightVector.magnitude;
      this.m_BoundsHandle.radius = handleDiameterVector.magnitude * 0.5f;
    }

    protected override bool CopyHandleSizeToCollider()
    {
      CapsuleCollider2D target = (CapsuleCollider2D) this.target;
      Vector3 vector3_1;
      Vector3 vector3_2;
      if (target.direction == CapsuleDirection2D.Horizontal)
      {
        vector3_1 = Vector3.up;
        vector3_2 = Vector3.right;
      }
      else
      {
        vector3_1 = Vector3.right;
        vector3_2 = Vector3.up;
      }
      Vector3 vector3_3 = (Vector3) (Handles.matrix * (Vector4) (vector3_2 * this.m_BoundsHandle.height));
      Vector3 vector3_4 = (Vector3) (Handles.matrix * (Vector4) (vector3_1 * this.m_BoundsHandle.radius * 2f));
      Matrix4x4 localToWorldMatrix = target.transform.localToWorldMatrix;
      Vector3 worldVector1 = this.ProjectOntoWorldPlane((Vector3) (localToWorldMatrix * (Vector4) vector3_1)).normalized * vector3_4.magnitude;
      Vector3 worldVector2 = this.ProjectOntoWorldPlane((Vector3) (localToWorldMatrix * (Vector4) vector3_2)).normalized * vector3_3.magnitude;
      Vector3 vector3_5 = this.ProjectOntoColliderPlane(worldVector1, localToWorldMatrix);
      Vector3 vector3_6 = this.ProjectOntoColliderPlane(worldVector2, localToWorldMatrix);
      Vector2 size = target.size;
      target.size = (Vector2) (localToWorldMatrix.inverse * (Vector4) (vector3_5 + vector3_6));
      return target.size != size;
    }

    protected override Quaternion GetHandleRotation()
    {
      Vector3 handleHeightVector;
      Vector3 handleDiameterVector;
      this.GetHandleVectorsInWorldSpace(this.target as CapsuleCollider2D, out handleHeightVector, out handleDiameterVector);
      return Quaternion.LookRotation(Vector3.forward, handleHeightVector);
    }

    private void GetHandleVectorsInWorldSpace(CapsuleCollider2D collider, out Vector3 handleHeightVector, out Vector3 handleDiameterVector)
    {
      Matrix4x4 localToWorldMatrix = collider.transform.localToWorldMatrix;
      Vector3 vector3_1 = this.ProjectOntoWorldPlane((Vector3) (localToWorldMatrix * (Vector4) (Vector3.right * collider.size.x)));
      Vector3 vector3_2 = this.ProjectOntoWorldPlane((Vector3) (localToWorldMatrix * (Vector4) (Vector3.up * collider.size.y)));
      if (collider.direction == CapsuleDirection2D.Horizontal)
      {
        handleDiameterVector = vector3_2;
        handleHeightVector = vector3_1;
      }
      else
      {
        handleDiameterVector = vector3_1;
        handleHeightVector = vector3_2;
      }
    }
  }
}
