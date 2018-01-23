// Decompiled with JetBrains decompiler
// Type: UnityEngine.DrivenPropertyManager
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal sealed class DrivenPropertyManager
  {
    [Conditional("UNITY_EDITOR")]
    public static void RegisterProperty(Object driver, Object target, string propertyPath)
    {
      if (driver == (Object) null)
        throw new ArgumentNullException(nameof (driver));
      if (target == (Object) null)
        throw new ArgumentNullException(nameof (target));
      if (propertyPath == null)
        throw new ArgumentNullException(nameof (propertyPath));
      DrivenPropertyManager.RegisterPropertyInternal(driver, target, propertyPath);
    }

    [Conditional("UNITY_EDITOR")]
    public static void UnregisterProperty(Object driver, Object target, string propertyPath)
    {
      if (driver == (Object) null)
        throw new ArgumentNullException(nameof (driver));
      if (target == (Object) null)
        throw new ArgumentNullException(nameof (target));
      if (propertyPath == null)
        throw new ArgumentNullException(nameof (propertyPath));
      DrivenPropertyManager.UnregisterPropertyInternal(driver, target, propertyPath);
    }

    [Conditional("UNITY_EDITOR")]
    public static void UnregisterProperties(Object driver)
    {
      if (driver == (Object) null)
        throw new ArgumentNullException(nameof (driver));
      DrivenPropertyManager.UnregisterPropertiesInternal(driver);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void RegisterPropertyInternal(Object driver, Object target, string propertyPath);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void UnregisterPropertyInternal(Object driver, Object target, string propertyPath);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void UnregisterPropertiesInternal(Object driver);
  }
}
