// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorBuildSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///         <para>This class allows you to modify the Editor for an example of how to use this class.
  /// 
  /// See Also: EditorBuildSettingsScene, EditorBuildSettings.scenes.</para>
  ///       </summary>
  public sealed class EditorBuildSettings
  {
    public static event Action sceneListChanged;

    [RequiredByNativeCode]
    private static void SceneListChanged()
    {
      // ISSUE: reference to a compiler-generated field
      if (EditorBuildSettings.sceneListChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      EditorBuildSettings.sceneListChanged();
    }

    /// <summary>
    ///         <para>The list of Scenes that should be included in the build.
    /// This is the same list of Scenes that is shown in the window. You can modify this list to set up which Scenes should be included in the build.</para>
    ///       </summary>
    public static extern EditorBuildSettingsScene[] scenes { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
