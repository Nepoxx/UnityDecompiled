// Decompiled with JetBrains decompiler
// Type: UnityEditor.MenuCommand
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Used to extract the context for a MenuItem.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class MenuCommand
  {
    /// <summary>
    ///   <para>Context is the object that is the target of a menu command.</para>
    /// </summary>
    public Object context;
    /// <summary>
    ///   <para>An integer for passing custom information to a menu item.</para>
    /// </summary>
    public int userData;

    /// <summary>
    ///   <para>Creates a new MenuCommand object.</para>
    /// </summary>
    /// <param name="inContext"></param>
    /// <param name="inUserData"></param>
    public MenuCommand(Object inContext, int inUserData)
    {
      this.context = inContext;
      this.userData = inUserData;
    }

    /// <summary>
    ///   <para>Creates a new MenuCommand object.</para>
    /// </summary>
    /// <param name="inContext"></param>
    public MenuCommand(Object inContext)
    {
      this.context = inContext;
      this.userData = 0;
    }
  }
}
