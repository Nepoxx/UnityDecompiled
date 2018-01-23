// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrimitiveCollider3DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class PrimitiveCollider3DEditor : Collider3DEditorBase
  {
    protected abstract PrimitiveBoundsHandle boundsHandle { get; }

    protected abstract void CopyColliderPropertiesToHandle();

    protected abstract void CopyHandlePropertiesToCollider();

    protected override GUIContent editModeButton
    {
      get
      {
        return PrimitiveBoundsHandle.editModeButton;
      }
    }

    protected Vector3 InvertScaleVector(Vector3 scaleVector)
    {
      for (int index = 0; index < 3; ++index)
        scaleVector[index] = (double) scaleVector[index] != 0.0 ? 1f / scaleVector[index] : 0.0f;
      return scaleVector;
    }

    protected virtual void OnSceneGUI()
    {
      if (!this.editingCollider)
        return;
      Collider target = (Collider) this.target;
      if (Mathf.Approximately(target.transform.lossyScale.sqrMagnitude, 0.0f))
        return;
      using (new Handles.DrawingScope(Matrix4x4.TRS(target.transform.position, target.transform.rotation, Vector3.one)))
      {
        this.CopyColliderPropertiesToHandle();
        this.boundsHandle.SetColor(!target.enabled ? Handles.s_ColliderHandleColorDisabled : Handles.s_ColliderHandleColor);
        EditorGUI.BeginChangeCheck();
        this.boundsHandle.DrawHandle();
        if (!EditorGUI.EndChangeCheck())
          return;
        Undo.RecordObject((Object) target, string.Format("Modify {0}", (object) ObjectNames.NicifyVariableName(this.target.GetType().Name)));
        this.CopyHandlePropertiesToCollider();
      }
    }

    protected Vector3 TransformColliderCenterToHandleSpace(Transform colliderTransform, Vector3 colliderCenter)
    {
      return (Vector3) (Handles.inverseMatrix * (colliderTransform.localToWorldMatrix * (Vector4) colliderCenter));
    }

    protected Vector3 TransformHandleCenterToColliderSpace(Transform colliderTransform, Vector3 handleCenter)
    {
      return (Vector3) (colliderTransform.localToWorldMatrix.inverse * (Handles.matrix * (Vector4) handleCenter));
    }
  }
}
