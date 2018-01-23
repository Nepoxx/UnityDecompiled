// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.NotificationServices
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.iOS
{
  public sealed class NotificationServices
  {
    public static extern int localNotificationCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int remoteNotificationCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearLocalNotifications();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearRemoteNotifications();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_RegisterImpl(NotificationType notificationTypes, bool registerForRemote);

    public static void RegisterForNotifications(NotificationType notificationTypes)
    {
      NotificationServices.Internal_RegisterImpl(notificationTypes, true);
    }

    public static void RegisterForNotifications(NotificationType notificationTypes, bool registerForRemote)
    {
      NotificationServices.Internal_RegisterImpl(notificationTypes, registerForRemote);
    }

    public static extern NotificationType enabledNotificationTypes { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static void ScheduleLocalNotification(LocalNotification notification)
    {
      notification.Schedule();
    }

    public static void PresentLocalNotificationNow(LocalNotification notification)
    {
      notification.PresentNow();
    }

    public static void CancelLocalNotification(LocalNotification notification)
    {
      notification.Cancel();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CancelAllLocalNotifications();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void UnregisterForRemoteNotifications();

    public static extern string registrationError { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern byte[] deviceToken { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern LocalNotification GetLocalNotificationImpl(int index);

    public static LocalNotification GetLocalNotification(int index)
    {
      if (index < 0 || index >= NotificationServices.localNotificationCount)
        throw new ArgumentOutOfRangeException(nameof (index), "Index out of bounds.");
      return NotificationServices.GetLocalNotificationImpl(index);
    }

    public static LocalNotification[] localNotifications
    {
      get
      {
        int notificationCount = NotificationServices.localNotificationCount;
        LocalNotification[] localNotificationArray = new LocalNotification[notificationCount];
        for (int index = 0; index < notificationCount; ++index)
          localNotificationArray[index] = NotificationServices.GetLocalNotificationImpl(index);
        return localNotificationArray;
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern RemoteNotification GetRemoteNotificationImpl(int index);

    public static RemoteNotification GetRemoteNotification(int index)
    {
      if (index < 0 || index >= NotificationServices.remoteNotificationCount)
        throw new ArgumentOutOfRangeException(nameof (index), "Index out of bounds.");
      return NotificationServices.GetRemoteNotificationImpl(index);
    }

    public static RemoteNotification[] remoteNotifications
    {
      get
      {
        int notificationCount = NotificationServices.remoteNotificationCount;
        RemoteNotification[] remoteNotificationArray = new RemoteNotification[notificationCount];
        for (int index = 0; index < notificationCount; ++index)
          remoteNotificationArray[index] = NotificationServices.GetRemoteNotificationImpl(index);
        return remoteNotificationArray;
      }
    }

    public static extern LocalNotification[] scheduledLocalNotifications { [MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
