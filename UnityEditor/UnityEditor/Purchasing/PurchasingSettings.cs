// Decompiled with JetBrains decompiler
// Type: UnityEditor.Purchasing.PurchasingSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Purchasing
{
  /// <summary>
  ///   <para>Editor API for the Unity Services editor feature. Normally Purchasing is enabled from the Services window, but if writing your own editor extension, this API can be used.</para>
  /// </summary>
  public static class PurchasingSettings
  {
    /// <summary>
    ///   <para>This Boolean field will cause the Purchasing feature in Unity to be enabled if true, or disabled if false.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public static extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool enabledForPlatform { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ApplyEnableSettings(BuildTarget target);
  }
}
