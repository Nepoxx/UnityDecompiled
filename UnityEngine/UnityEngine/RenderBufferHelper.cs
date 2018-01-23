// Decompiled with JetBrains decompiler
// Type: UnityEngine.RenderBufferHelper
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal struct RenderBufferHelper
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetLoadAction(out RenderBuffer b);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetLoadAction(out RenderBuffer b, int a);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetStoreAction(out RenderBuffer b);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetStoreAction(out RenderBuffer b, int a);

    internal static IntPtr GetNativeRenderBufferPtr(IntPtr rb)
    {
      IntPtr num;
      RenderBufferHelper.INTERNAL_CALL_GetNativeRenderBufferPtr(rb, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetNativeRenderBufferPtr(IntPtr rb, out IntPtr value);
  }
}
