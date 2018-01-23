// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationModeDriver
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal sealed class AnimationModeDriver : ScriptableObject
  {
    internal AnimationModeDriver.IsKeyCallback isKeyCallback;

    [UsedByNativeCode]
    internal bool InvokeIsKeyCallback_Internal(Object target, string propertyPath)
    {
      if (this.isKeyCallback == null)
        return false;
      return this.isKeyCallback(target, propertyPath);
    }

    internal delegate bool IsKeyCallback(Object target, string propertyPath);
  }
}
