// Decompiled with JetBrains decompiler
// Type: UnityEditor.CircleCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (CircleCollider2D))]
  [CanEditMultipleObjects]
  internal class CircleCollider2DEditor : PrimitiveCollider2DEditor
  {
    private readonly SphereBoundsHandle m_BoundsHandle = new SphereBoundsHandle();
    private SerializedProperty m_Radius;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Radius = this.serializedObject.FindProperty("m_Radius");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.InspectorEditButtonGUI();
      base.OnInspectorGUI();
      EditorGUILayout.PropertyField(this.m_Radius);
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
      this.m_BoundsHandle.radius = ((CircleCollider2D) this.target).radius * this.GetRadiusScaleFactor();
    }

    protected override bool CopyHandleSizeToCollider()
    {
      CircleCollider2D target = (CircleCollider2D) this.target;
      float radius = target.radius;
      float radiusScaleFactor = this.GetRadiusScaleFactor();
      target.radius = !Mathf.Approximately(radiusScaleFactor, 0.0f) ? this.m_BoundsHandle.radius / this.GetRadiusScaleFactor() : 0.0f;
      return (double) target.radius != (double) radius;
    }

    private float GetRadiusScaleFactor()
    {
      Vector3 lossyScale = ((Component) this.target).transform.lossyScale;
      return Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y));
    }
  }
}
