// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssemblyReloadEvents
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>This class has event dispatchers for assembly reload events.</para>
  /// </summary>
  public static class AssemblyReloadEvents
  {
    public static event AssemblyReloadEvents.AssemblyReloadCallback beforeAssemblyReload;

    public static event AssemblyReloadEvents.AssemblyReloadCallback afterAssemblyReload;

    [RequiredByNativeCode]
    private static void OnBeforeAssemblyReload()
    {
      // ISSUE: reference to a compiler-generated field
      if (AssemblyReloadEvents.beforeAssemblyReload == null)
        return;
      // ISSUE: reference to a compiler-generated field
      AssemblyReloadEvents.beforeAssemblyReload();
    }

    [RequiredByNativeCode]
    private static void OnAfterAssemblyReload()
    {
      // ISSUE: reference to a compiler-generated field
      if (AssemblyReloadEvents.afterAssemblyReload == null)
        return;
      // ISSUE: reference to a compiler-generated field
      AssemblyReloadEvents.afterAssemblyReload();
    }

    /// <summary>
    ///   <para>Delegate used for assembly reload events.</para>
    /// </summary>
    public delegate void AssemblyReloadCallback();
  }
}
