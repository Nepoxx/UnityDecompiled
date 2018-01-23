// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorBuildSettingsScene
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace UnityEditor
{
  /// <summary>
  ///         <para>This class is used for entries in the Scenes list, as displayed in the window. This class contains the scene path of a scene and an enabled flag that indicates wether the scene is enabled in the BuildSettings window or not.
  /// 
  /// You can use this class in combination with EditorBuildSettings.scenes to populate the list of Scenes included in the build via script. This is useful when creating custom editor scripts to automate your build pipeline.
  /// 
  /// See EditorBuildSettings.scenes for an example script.</para>
  ///       </summary>
  [StructLayout(LayoutKind.Sequential)]
  public sealed class EditorBuildSettingsScene : IComparable
  {
    private int m_Enabled;
    private string m_Path;
    private GUID m_GUID;

    public EditorBuildSettingsScene()
    {
    }

    public EditorBuildSettingsScene(string path, bool enable)
    {
      this.m_Path = path.Replace("\\", "/");
      this.enabled = enable;
      GUID.TryParse(AssetDatabase.AssetPathToGUID(path), out this.m_GUID);
    }

    public EditorBuildSettingsScene(GUID guid, bool enable)
    {
      this.m_GUID = guid;
      this.enabled = enable;
      this.m_Path = AssetDatabase.GUIDToAssetPath(guid.ToString());
    }

    public int CompareTo(object obj)
    {
      if (obj is EditorBuildSettingsScene)
        return ((EditorBuildSettingsScene) obj).m_Path.CompareTo(this.m_Path);
      throw new ArgumentException("object is not a EditorBuildSettingsScene");
    }

    /// <summary>
    ///         <para>Whether this scene is enabled in the for an example of how to use this class.
    /// 
    /// See Also: EditorBuildSettingsScene, EditorBuildSettings.scenes.</para>
    ///       </summary>
    public bool enabled
    {
      get
      {
        return this.m_Enabled != 0;
      }
      set
      {
        this.m_Enabled = !value ? 0 : 1;
      }
    }

    /// <summary>
    ///         <para>The file path of the scene as listed in the Editor for an example of how to use this class.
    /// 
    /// See Also: EditorBuildSettingsScene, EditorBuildSettings.scenes.</para>
    ///       </summary>
    public string path
    {
      get
      {
        return this.m_Path;
      }
      set
      {
        this.m_Path = value.Replace("\\", "/");
      }
    }

    public GUID guid
    {
      get
      {
        return this.m_GUID;
      }
      set
      {
        this.m_GUID = value;
      }
    }

    public static string[] GetActiveSceneList(EditorBuildSettingsScene[] scenes)
    {
      return ((IEnumerable<EditorBuildSettingsScene>) scenes).Where<EditorBuildSettingsScene>((Func<EditorBuildSettingsScene, bool>) (scene => scene.enabled)).Select<EditorBuildSettingsScene, string>((Func<EditorBuildSettingsScene, string>) (scene => scene.path)).ToArray<string>();
    }
  }
}
