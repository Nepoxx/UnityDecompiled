// Decompiled with JetBrains decompiler
// Type: UnityEditor.Menu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Menu class to manipulate the menu item.</para>
  /// </summary>
  public sealed class Menu
  {
    /// <summary>
    ///   <para>Set the check status of the given menu.</para>
    /// </summary>
    /// <param name="menuPath"></param>
    /// <param name="isChecked"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetChecked(string menuPath, bool isChecked);

    /// <summary>
    ///   <para>Get the check status of the given menu.</para>
    /// </summary>
    /// <param name="menuPath"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetChecked(string menuPath);
  }
}
