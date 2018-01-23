// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.RemoteNotification
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.iOS
{
  [RequiredByNativeCode]
  public sealed class RemoteNotification
  {
    private IntPtr m_Ptr;

    private RemoteNotification()
    {
    }

    ~RemoteNotification()
    {
      NotificationHelper.DestroyRemote(this.m_Ptr);
    }

    public extern string alertBody { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern string soundName { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern int applicationIconBadgeNumber { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern IDictionary userInfo { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool hasAction { [MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
