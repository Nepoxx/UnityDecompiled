// Decompiled with JetBrains decompiler
// Type: UnityEditor.PostProcessAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [RequiredByNativeCode]
  [Obsolete("PostProcessAttribute has been renamed to CallbackOrderAttribute.")]
  public abstract class PostProcessAttribute : CallbackOrderAttribute
  {
    [Obsolete("PostProcessAttribute has been renamed. Use m_CallbackOrder of CallbackOrderAttribute.")]
    protected int m_PostprocessOrder;

    [Obsolete("PostProcessAttribute has been renamed. Use callbackOrder of CallbackOrderAttribute.")]
    internal int GetPostprocessOrder
    {
      get
      {
        return this.m_PostprocessOrder;
      }
    }
  }
}
