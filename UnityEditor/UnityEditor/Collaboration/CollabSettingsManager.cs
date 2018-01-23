// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.CollabSettingsManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor.Collaboration
{
  internal sealed class CollabSettingsManager
  {
    public static Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier = new Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged>();

    static CollabSettingsManager()
    {
      IEnumerator enumerator = Enum.GetValues(typeof (CollabSettingType)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          CollabSettingType current = (CollabSettingType) enumerator.Current;
          CollabSettingsManager.statusNotifier[current] = (CollabSettingsManager.SettingStatusChanged) null;
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    private static void NotifyStatusListeners(CollabSettingType type, CollabSettingStatus status)
    {
      if (CollabSettingsManager.statusNotifier[type] == null)
        return;
      CollabSettingsManager.statusNotifier[type](type, status);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsAvailable(CollabSettingType type);

    public static extern bool inProgressEnabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public delegate void SettingStatusChanged(CollabSettingType type, CollabSettingStatus status);
  }
}
