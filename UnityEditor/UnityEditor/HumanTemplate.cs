// Decompiled with JetBrains decompiler
// Type: UnityEditor.HumanTemplate
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [NativeType(Header = "Editor/Src/Animation/HumanTemplate.h")]
  [UsedByNativeCode]
  public sealed class HumanTemplate : Object
  {
    public HumanTemplate()
    {
      HumanTemplate.Internal_Create(this);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] HumanTemplate self);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Insert(string name, string templateName);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string Find(string name);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearTemplate();
  }
}
