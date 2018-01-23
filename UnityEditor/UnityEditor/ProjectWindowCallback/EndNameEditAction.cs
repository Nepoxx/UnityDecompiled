// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.EndNameEditAction
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.ProjectWindowCallback
{
  public abstract class EndNameEditAction : ScriptableObject
  {
    public virtual void OnEnable()
    {
      this.hideFlags = HideFlags.HideAndDontSave;
    }

    public abstract void Action(int instanceId, string pathName, string resourceFile);

    public virtual void CleanUp()
    {
      Object.DestroyImmediate((Object) this);
    }
  }
}
