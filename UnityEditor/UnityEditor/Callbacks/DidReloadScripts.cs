// Decompiled with JetBrains decompiler
// Type: UnityEditor.Callbacks.DidReloadScripts
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Callbacks
{
  /// <summary>
  ///   <para>Add this attribute to a method to get a notification after scripts have been reloaded.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class DidReloadScripts : CallbackOrderAttribute
  {
    /// <summary>
    ///   <para>DidReloadScripts attribute.</para>
    /// </summary>
    /// <param name="callbackOrder">Order in which separate attributes will be processed.</param>
    public DidReloadScripts()
    {
      this.m_CallbackOrder = 1;
    }

    /// <summary>
    ///   <para>DidReloadScripts attribute.</para>
    /// </summary>
    /// <param name="callbackOrder">Order in which separate attributes will be processed.</param>
    public DidReloadScripts(int callbackOrder)
    {
      this.m_CallbackOrder = callbackOrder;
    }
  }
}
