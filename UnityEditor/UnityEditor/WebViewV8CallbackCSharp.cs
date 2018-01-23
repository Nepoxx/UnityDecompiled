// Decompiled with JetBrains decompiler
// Type: UnityEditor.WebViewV8CallbackCSharp
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [Serializable]
  internal sealed class WebViewV8CallbackCSharp
  {
    [SerializeField]
    private IntPtr m_thisDummy;

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Callback(string result);

    public void OnDestroy()
    {
      this.DestroyCallBack();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void DestroyCallBack();
  }
}
