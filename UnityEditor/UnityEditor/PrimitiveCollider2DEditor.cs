// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrimitiveCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class PrimitiveCollider2DEditor : Collider2DEditorBase
  {
    protected abstract PrimitiveBoundsHandle boundsHandle { get; }

    protected abstract void CopyColliderSizeToHandle();

    protected abstract bool CopyHandleSizeToCollider();

    protected override GUIContent editModeButton
    {
      get
      {
        return PrimitiveBoundsHandle.editModeButton;
      }
    }

    protected virtual Quaternion GetHandleRotation()
    {
      return Quaternion.identity;
    }

    public override void OnEnable()
    {
      base.OnEnable();
      this.boundsHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;
    }

    protected virtual void OnSceneGUI()
    {
      if (!this.editingCollider)
        return;
      Collider2D target = (Collider2D) this.target;
      if (Mathf.Approximately(target.transform.lossyScale.sqrMagnitude, 0.0f))
        return;
      using (new Handles.DrawingScope(Matrix4x4.TRS(target.transform.position, this.GetHandleRotation(), Vector3.one)))
      {
        Matrix4x4 localToWorldMatrix = target.transform.localToWorldMatrix;
        this.boundsHandle.center = this.ProjectOntoWorldPlane((Vector3) (Handles.inverseMatrix * (localToWorldMatrix * (Vector4) target.offset)));
        this.CopyColliderSizeToHandle();
        this.boundsHandle.SetColor(!target.enabled ? Handles.s_ColliderHandleColorDisabled : Handles.s_ColliderHandleColor);
        EditorGUI.BeginChangeCheck();
        this.boundsHandle.DrawHandle();
        if (!EditorGUI.EndChangeCheck())
          return;
        Undo.RecordObject((Object) target, string.Format("Modify {0}", (object) ObjectNames.NicifyVariableName(this.target.GetType().Name)));
        if (this.CopyHandleSizeToCollider())
          target.offset = (Vector2) (localToWorldMatrix.inverse * (Vector4) this.ProjectOntoColliderPlane((Vector3) (Handles.matrix * (Vector4) this.boundsHandle.center), localToWorldMatrix));
      }
    }

    protected Vector3 ProjectOntoColliderPlane(Vector3 worldVector, Matrix4x4 colliderTransformMatrix)
    {
      Plane plane = new Plane(Vector3.Cross((Vector3) (colliderTransformMatrix * (Vector4) Vector3.right), (Vector3) (colliderTransformMatrix * (Vector4) Vector3.up)), Vector3.zero);
      Ray ray = new Ray(worldVector, Vector3.forward);
      float enter;
      if (plane.Raycast(ray, out enter))
        return ray.GetPoint(enter);
      ray.direction = Vector3.back;
      plane.Raycast(ray, out enter);
      return ray.GetPoint(enter);
    }

    protected Vector3 ProjectOntoWorldPlane(Vector3 worldVector)
    {
      worldVector.z = 0.0f;
      return worldVector;
    }
  }
}
