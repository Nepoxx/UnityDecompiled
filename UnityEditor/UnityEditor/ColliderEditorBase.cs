// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColliderEditorBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class ColliderEditorBase : Editor
  {
    protected virtual void OnEditStart()
    {
    }

    protected virtual void OnEditEnd()
    {
    }

    public bool editingCollider
    {
      get
      {
        return UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.Collider && UnityEditorInternal.EditMode.IsOwner((Editor) this);
      }
    }

    public virtual void OnEnable()
    {
      UnityEditorInternal.EditMode.editModeStarted += new Action<IToolModeOwner, UnityEditorInternal.EditMode.SceneViewEditMode>(this.OnEditModeStart);
      UnityEditorInternal.EditMode.editModeEnded += new Action<IToolModeOwner>(this.OnEditModeEnd);
    }

    public virtual void OnDisable()
    {
      UnityEditorInternal.EditMode.editModeStarted -= new Action<IToolModeOwner, UnityEditorInternal.EditMode.SceneViewEditMode>(this.OnEditModeStart);
      UnityEditorInternal.EditMode.editModeEnded -= new Action<IToolModeOwner>(this.OnEditModeEnd);
    }

    protected virtual GUIContent editModeButton
    {
      get
      {
        return EditorGUIUtility.IconContent("EditCollider");
      }
    }

    protected void InspectorEditButtonGUI()
    {
      UnityEditorInternal.EditMode.DoEditModeInspectorModeButton(UnityEditorInternal.EditMode.SceneViewEditMode.Collider, "Edit Collider", this.editModeButton, (IToolModeOwner) this);
    }

    internal override Bounds GetWorldBoundsOfTarget(UnityEngine.Object targetObject)
    {
      if (targetObject is Collider2D)
        return ((Collider2D) targetObject).bounds;
      if (targetObject is Collider)
        return ((Collider) targetObject).bounds;
      return base.GetWorldBoundsOfTarget(targetObject);
    }

    protected void OnEditModeStart(IToolModeOwner owner, UnityEditorInternal.EditMode.SceneViewEditMode mode)
    {
      if (mode != UnityEditorInternal.EditMode.SceneViewEditMode.Collider || owner != this)
        return;
      this.OnEditStart();
    }

    protected void OnEditModeEnd(IToolModeOwner owner)
    {
      if (owner != this)
        return;
      this.OnEditEnd();
    }
  }
}
