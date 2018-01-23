// Decompiled with JetBrains decompiler
// Type: UnityEditor.SphereColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (SphereCollider))]
  internal class SphereColliderEditor : PrimitiveCollider3DEditor
  {
    private readonly SphereBoundsHandle m_BoundsHandle = new SphereBoundsHandle();
    private SerializedProperty m_Center;
    private SerializedProperty m_Radius;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Center = this.serializedObject.FindProperty("m_Center");
      this.m_Radius = this.serializedObject.FindProperty("m_Radius");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.InspectorEditButtonGUI();
      EditorGUILayout.PropertyField(this.m_IsTrigger);
      EditorGUILayout.PropertyField(this.m_Material);
      EditorGUILayout.PropertyField(this.m_Center);
      EditorGUILayout.PropertyField(this.m_Radius);
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
      SphereCollider target = (SphereCollider) this.target;
      this.m_BoundsHandle.center = this.TransformColliderCenterToHandleSpace(target.transform, target.center);
      this.m_BoundsHandle.radius = target.radius * this.GetRadiusScaleFactor();
    }

    protected override void CopyHandlePropertiesToCollider()
    {
      SphereCollider target = (SphereCollider) this.target;
      target.center = this.TransformHandleCenterToColliderSpace(target.transform, this.m_BoundsHandle.center);
      float radiusScaleFactor = this.GetRadiusScaleFactor();
      target.radius = !Mathf.Approximately(radiusScaleFactor, 0.0f) ? this.m_BoundsHandle.radius / this.GetRadiusScaleFactor() : 0.0f;
    }

    private float GetRadiusScaleFactor()
    {
      float a = 0.0f;
      Vector3 lossyScale = ((Component) this.target).transform.lossyScale;
      for (int index = 0; index < 3; ++index)
        a = Mathf.Max(a, Mathf.Abs(lossyScale[index]));
      return a;
    }
  }
}
