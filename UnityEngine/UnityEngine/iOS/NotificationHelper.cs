// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.NotificationHelper
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.iOS
{
  internal sealed class NotificationHelper
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern IntPtr CreateLocal();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DestroyLocal(IntPtr target);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DestroyRemote(IntPtr target);
  }
}
