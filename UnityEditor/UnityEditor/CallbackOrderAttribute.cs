// Decompiled with JetBrains decompiler
// Type: UnityEditor.CallbackOrderAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Base class for Attributes that require a callback index.</para>
  /// </summary>
  [RequiredByNativeCode]
  public abstract class CallbackOrderAttribute : Attribute
  {
    protected int m_CallbackOrder;

    internal int callbackOrder
    {
      get
      {
        return this.m_CallbackOrder;
      }
    }
  }
}
