// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.AssetPath
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.IO;
using UnityEditor.Utils;

namespace UnityEditor.Scripting.ScriptCompilation
{
  internal static class AssetPath
  {
    public static readonly char Separator = '/';

    public static string GetFullPath(string path)
    {
      return AssetPath.ReplaceSeparators(Path.GetFullPath(path.NormalizePath()));
    }

    public static string Combine(string path1, string path2)
    {
      return AssetPath.ReplaceSeparators(Path.Combine(path1, path2));
    }

    public static bool IsPathRooted(string path)
    {
      return Path.IsPathRooted(path.NormalizePath());
    }

    public static string GetFileName(string path)
    {
      return Path.GetFileName(path.NormalizePath());
    }

    public static string GetExtension(string path)
    {
      return Path.GetExtension(path.NormalizePath());
    }

    public static string GetDirectoryName(string path)
    {
      return AssetPath.ReplaceSeparators(Path.GetDirectoryName(path.NormalizePath()));
    }

    public static string ReplaceSeparators(string path)
    {
      return path.Replace('\\', AssetPath.Separator);
    }

    public static string GetAssemblyNameWithoutExtension(string assemblyName)
    {
      if (AssetPath.GetExtension(assemblyName) == ".dll")
        return Path.GetFileNameWithoutExtension(assemblyName.NormalizePath());
      return Path.GetFileName(assemblyName.NormalizePath());
    }
  }
}
