// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.LocalNotification
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
  public sealed class LocalNotification
  {
    private static long m_NSReferenceDateTicks = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
    private IntPtr m_Ptr;

    public LocalNotification()
    {
      this.m_Ptr = NotificationHelper.CreateLocal();
    }

    ~LocalNotification()
    {
      NotificationHelper.DestroyLocal(this.m_Ptr);
    }

    public extern string timeZone { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern CalendarIdentifier repeatCalendar { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern CalendarUnit repeatInterval { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    private extern double fireDateImpl { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public DateTime fireDate
    {
      get
      {
        return new DateTime((long) (this.fireDateImpl * 10000000.0) + LocalNotification.m_NSReferenceDateTicks);
      }
      set
      {
        this.fireDateImpl = (double) (value.ToUniversalTime().Ticks - LocalNotification.m_NSReferenceDateTicks) / 10000000.0;
      }
    }

    public extern string alertBody { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern string alertAction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern string alertLaunchImage { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern string soundName { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern int applicationIconBadgeNumber { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string defaultSoundName { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern IDictionary userInfo { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool hasAction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void Schedule();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void PresentNow();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void Cancel();
  }
}
