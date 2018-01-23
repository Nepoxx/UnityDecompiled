// Decompiled with JetBrains decompiler
// Type: UnityEditor.CapsuleColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (CapsuleCollider))]
  [CanEditMultipleObjects]
  internal class CapsuleColliderEditor : PrimitiveCollider3DEditor
  {
    private readonly CapsuleBoundsHandle m_BoundsHandle = new CapsuleBoundsHandle();
    private SerializedProperty m_Center;
    private SerializedProperty m_Radius;
    private SerializedProperty m_Height;
    private SerializedProperty m_Direction;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Center = this.serializedObject.FindProperty("m_Center");
      this.m_Radius = this.serializedObject.FindProperty("m_Radius");
      this.m_Height = this.serializedObject.FindProperty("m_Height");
      this.m_Direction = this.serializedObject.FindProperty("m_Direction");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.InspectorEditButtonGUI();
      EditorGUILayout.PropertyField(this.m_IsTrigger);
      EditorGUILayout.PropertyField(this.m_Material);
      EditorGUILayout.PropertyField(this.m_Center);
      EditorGUILayout.PropertyField(this.m_Radius);
      EditorGUILayout.PropertyField(this.m_Height);
      EditorGUILayout.PropertyField(this.m_Direction);
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
      CapsuleCollider target = (CapsuleCollider) this.target;
      this.m_BoundsHandle.center = this.TransformColliderCenterToHandleSpace(target.transform, target.center);
      float radiusScaleFactor;
      Vector3 colliderHandleScale = this.GetCapsuleColliderHandleScale(target.transform.lossyScale, target.direction, out radiusScaleFactor);
      CapsuleBoundsHandle boundsHandle = this.m_BoundsHandle;
      float num1 = 0.0f;
      this.m_BoundsHandle.radius = num1;
      double num2 = (double) num1;
      boundsHandle.height = (float) num2;
      this.m_BoundsHandle.height = target.height * Mathf.Abs(colliderHandleScale[target.direction]);
      this.m_BoundsHandle.radius = target.radius * radiusScaleFactor;
      switch (target.direction)
      {
        case 0:
          this.m_BoundsHandle.heightAxis = CapsuleBoundsHandle.HeightAxis.X;
          break;
        case 1:
          this.m_BoundsHandle.heightAxis = CapsuleBoundsHandle.HeightAxis.Y;
          break;
        case 2:
          this.m_BoundsHandle.heightAxis = CapsuleBoundsHandle.HeightAxis.Z;
          break;
      }
    }

    protected override void CopyHandlePropertiesToCollider()
    {
      CapsuleCollider target = (CapsuleCollider) this.target;
      target.center = this.TransformHandleCenterToColliderSpace(target.transform, this.m_BoundsHandle.center);
      float radiusScaleFactor;
      Vector3 vector3 = this.InvertScaleVector(this.GetCapsuleColliderHandleScale(target.transform.lossyScale, target.direction, out radiusScaleFactor));
      if ((double) radiusScaleFactor != 0.0)
        target.radius = this.m_BoundsHandle.radius / radiusScaleFactor;
      if ((double) vector3[target.direction] == 0.0)
        return;
      target.height = this.m_BoundsHandle.height * Mathf.Abs(vector3[target.direction]);
    }

    protected override void OnSceneGUI()
    {
      CapsuleCollider target = (CapsuleCollider) this.target;
      float radiusScaleFactor;
      this.GetCapsuleColliderHandleScale(target.transform.lossyScale, target.direction, out radiusScaleFactor);
      this.boundsHandle.axes = PrimitiveBoundsHandle.Axes.All;
      if ((double) radiusScaleFactor == 0.0)
      {
        switch (target.direction)
        {
          case 0:
            this.boundsHandle.axes = PrimitiveBoundsHandle.Axes.X;
            break;
          case 1:
            this.boundsHandle.axes = PrimitiveBoundsHandle.Axes.Y;
            break;
          case 2:
            this.boundsHandle.axes = PrimitiveBoundsHandle.Axes.Z;
            break;
        }
      }
      base.OnSceneGUI();
    }

    private Vector3 GetCapsuleColliderHandleScale(Vector3 lossyScale, int capsuleDirection, out float radiusScaleFactor)
    {
      radiusScaleFactor = 0.0f;
      for (int index = 0; index < 3; ++index)
      {
        if (index != capsuleDirection)
          radiusScaleFactor = Mathf.Max(radiusScaleFactor, Mathf.Abs(lossyScale[index]));
      }
      for (int index = 0; index < 3; ++index)
      {
        if (index != capsuleDirection)
          lossyScale[index] = Mathf.Sign(lossyScale[index]) * radiusScaleFactor;
      }
      return lossyScale;
    }
  }
}
