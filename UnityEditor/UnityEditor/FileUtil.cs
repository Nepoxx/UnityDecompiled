// Decompiled with JetBrains decompiler
// Type: UnityEditor.FileUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Lets you do move, copy, delete operations over files or directories.</para>
  /// </summary>
  public sealed class FileUtil
  {
    /// <summary>
    ///   <para>Deletes a file or a directory given a path.</para>
    /// </summary>
    /// <param name="path"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool DeleteFileOrDirectory(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool PathExists(string path);

    /// <summary>
    ///   <para>Copies a file or a directory.</para>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    public static void CopyFileOrDirectory(string source, string dest)
    {
      FileUtil.CheckForValidSourceAndDestinationArgumentsAndRaiseAnExceptionWhenNullOrEmpty(source, dest);
      if (FileUtil.PathExists(dest))
        throw new IOException(string.Format("Failed to Copy File / Directory from '{0}' to '{1}': destination path already exists.", (object) source, (object) dest));
      if (!FileUtil.CopyFileOrDirectoryInternal(source, dest))
        throw new IOException(string.Format("Failed to Copy File / Directory from '{0}' to '{1}'.", (object) source, (object) dest));
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool CopyFileOrDirectoryInternal(string source, string dest);

    /// <summary>
    ///   <para>Copies the file or directory.</para>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    public static void CopyFileOrDirectoryFollowSymlinks(string source, string dest)
    {
      FileUtil.CheckForValidSourceAndDestinationArgumentsAndRaiseAnExceptionWhenNullOrEmpty(source, dest);
      if (FileUtil.PathExists(dest))
        throw new IOException(string.Format("Failed to Copy File / Directory from '{0}' to '{1}': destination path already exists.", (object) source, (object) dest));
      if (!FileUtil.CopyFileOrDirectoryFollowSymlinksInternal(source, dest))
        throw new IOException(string.Format("Failed to Copy File / Directory from '{0}' to '{1}'.", (object) source, (object) dest));
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool CopyFileOrDirectoryFollowSymlinksInternal(string source, string dest);

    /// <summary>
    ///   <para>Moves a file or a directory from a given path to another path.</para>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    public static void MoveFileOrDirectory(string source, string dest)
    {
      FileUtil.CheckForValidSourceAndDestinationArgumentsAndRaiseAnExceptionWhenNullOrEmpty(source, dest);
      if (FileUtil.PathExists(dest))
        throw new IOException(string.Format("Failed to Copy File / Directory from '{0}' to '{1}': destination path already exists.", (object) source, (object) dest));
      if (!FileUtil.MoveFileOrDirectoryInternal(source, dest))
        throw new IOException(string.Format("Failed to Move File / Directory from '{0}' to '{1}'.", (object) source, (object) dest));
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool MoveFileOrDirectoryInternal(string source, string dest);

    private static void CheckForValidSourceAndDestinationArgumentsAndRaiseAnExceptionWhenNullOrEmpty(string source, string dest)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dest == null)
        throw new ArgumentNullException(nameof (dest));
      if (source == string.Empty)
        throw new ArgumentException(nameof (source), "The source path cannot be empty.");
      if (dest == string.Empty)
        throw new ArgumentException(nameof (dest), "The destination path cannot be empty.");
    }

    /// <summary>
    ///   <para>Returns a unique path in the Temp folder within your current project.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetUniqueTempPathInProject();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetActualPathName(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetProjectRelativePath(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetLastPathNameComponent(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string DeleteLastPathNameComponent(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetPathExtension(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetPathWithoutExtension(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string ResolveSymlinks(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsSymlink(string path);

    /// <summary>
    ///   <para>Replaces a file.</para>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    public static void ReplaceFile(string src, string dst)
    {
      if (File.Exists(dst))
        FileUtil.DeleteFileOrDirectory(dst);
      FileUtil.CopyFileOrDirectory(src, dst);
    }

    /// <summary>
    ///   <para>Replaces a directory.</para>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    public static void ReplaceDirectory(string src, string dst)
    {
      if (Directory.Exists(dst))
        FileUtil.DeleteFileOrDirectory(dst);
      FileUtil.CopyFileOrDirectory(src, dst);
    }

    internal static void ReplaceText(string path, params string[] input)
    {
      path = FileUtil.NiceWinPath(path);
      string[] contents = File.ReadAllLines(path);
      int index1 = 0;
      while (index1 < input.Length)
      {
        for (int index2 = 0; index2 < contents.Length; ++index2)
          contents[index2] = contents[index2].Replace(input[index1], input[index1 + 1]);
        index1 += 2;
      }
      File.WriteAllLines(path, contents);
    }

    internal static bool ReplaceTextRegex(string path, params string[] input)
    {
      bool flag = false;
      path = FileUtil.NiceWinPath(path);
      string[] contents = File.ReadAllLines(path);
      int index1 = 0;
      while (index1 < input.Length)
      {
        for (int index2 = 0; index2 < contents.Length; ++index2)
        {
          string input1 = contents[index2];
          contents[index2] = Regex.Replace(input1, input[index1], input[index1 + 1]);
          if (input1 != contents[index2])
            flag = true;
        }
        index1 += 2;
      }
      File.WriteAllLines(path, contents);
      return flag;
    }

    internal static bool AppendTextAfter(string path, string find, string append)
    {
      bool flag = false;
      path = FileUtil.NiceWinPath(path);
      List<string> stringList = new List<string>((IEnumerable<string>) File.ReadAllLines(path));
      for (int index = 0; index < stringList.Count; ++index)
      {
        if (stringList[index].Contains(find))
        {
          stringList.Insert(index + 1, append);
          flag = true;
          break;
        }
      }
      File.WriteAllLines(path, stringList.ToArray());
      return flag;
    }

    internal static void CopyDirectoryRecursive(string source, string target)
    {
      FileUtil.CopyDirectoryRecursive(source, target, false, false);
    }

    internal static void CopyDirectoryRecursiveIgnoreMeta(string source, string target)
    {
      FileUtil.CopyDirectoryRecursive(source, target, false, true);
    }

    internal static void CopyDirectoryRecursive(string source, string target, bool overwrite)
    {
      FileUtil.CopyDirectoryRecursive(source, target, overwrite, false);
    }

    internal static void CopyDirectory(string source, string target, bool overwrite)
    {
      FileUtil.CopyDirectoryFiltered(source, target, overwrite, (Func<string, bool>) (f => true), false);
    }

    internal static void CopyDirectoryRecursive(string source, string target, bool overwrite, bool ignoreMeta)
    {
      FileUtil.CopyDirectoryRecursiveFiltered(source, target, overwrite, !ignoreMeta ? (string) null : "\\.meta$");
    }

    internal static void CopyDirectoryRecursiveForPostprocess(string source, string target, bool overwrite)
    {
      FileUtil.CopyDirectoryRecursiveFiltered(source, target, overwrite, ".*/\\.+|\\.meta$");
    }

    internal static void CopyDirectoryRecursiveFiltered(string source, string target, bool overwrite, string regExExcludeFilter)
    {
      FileUtil.CopyDirectoryFiltered(source, target, overwrite, regExExcludeFilter, true);
    }

    internal static void CopyDirectoryFiltered(string source, string target, bool overwrite, string regExExcludeFilter, bool recursive)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FileUtil.\u003CCopyDirectoryFiltered\u003Ec__AnonStorey0 filteredCAnonStorey0 = new FileUtil.\u003CCopyDirectoryFiltered\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      filteredCAnonStorey0.exclude = (Regex) null;
      try
      {
        if (regExExcludeFilter != null)
        {
          // ISSUE: reference to a compiler-generated field
          filteredCAnonStorey0.exclude = new Regex(regExExcludeFilter);
        }
      }
      catch (ArgumentException ex)
      {
        Debug.Log((object) ("CopyDirectoryRecursive: Pattern '" + regExExcludeFilter + "' is not a correct Regular Expression. Not excluding any files."));
        return;
      }
      // ISSUE: reference to a compiler-generated method
      Func<string, bool> includeCallback = new Func<string, bool>(filteredCAnonStorey0.\u003C\u003Em__0);
      FileUtil.CopyDirectoryFiltered(source, target, overwrite, includeCallback, recursive);
    }

    internal static void CopyDirectoryFiltered(string source, string target, bool overwrite, Func<string, bool> includeCallback, bool recursive)
    {
      if (!Directory.Exists(target))
      {
        Directory.CreateDirectory(target);
        overwrite = false;
      }
      foreach (string file in Directory.GetFiles(source))
      {
        if (includeCallback(file))
        {
          string fileName = Path.GetFileName(file);
          string to = Path.Combine(target, fileName);
          FileUtil.UnityFileCopy(file, to, overwrite);
        }
      }
      if (!recursive)
        return;
      foreach (string directory in Directory.GetDirectories(source))
      {
        if (includeCallback(directory))
        {
          string fileName = Path.GetFileName(directory);
          FileUtil.CopyDirectoryFiltered(Path.Combine(source, fileName), Path.Combine(target, fileName), overwrite, includeCallback, recursive);
        }
      }
    }

    internal static void UnityDirectoryDelete(string path)
    {
      FileUtil.UnityDirectoryDelete(path, false);
    }

    internal static void UnityDirectoryDelete(string path, bool recursive)
    {
      Directory.Delete(FileUtil.NiceWinPath(path), recursive);
    }

    internal static void UnityDirectoryRemoveReadonlyAttribute(string target_dir)
    {
      string[] files = Directory.GetFiles(target_dir);
      string[] directories = Directory.GetDirectories(target_dir);
      foreach (string path in files)
        File.SetAttributes(path, FileAttributes.Normal);
      foreach (string target_dir1 in directories)
        FileUtil.UnityDirectoryRemoveReadonlyAttribute(target_dir1);
    }

    internal static void MoveFileIfExists(string src, string dst)
    {
      if (!File.Exists(src))
        return;
      FileUtil.DeleteFileOrDirectory(dst);
      FileUtil.MoveFileOrDirectory(src, dst);
      File.SetLastWriteTime(dst, DateTime.Now);
    }

    internal static void CopyFileIfExists(string src, string dst, bool overwrite)
    {
      if (!File.Exists(src))
        return;
      FileUtil.UnityFileCopy(src, dst, overwrite);
    }

    internal static void UnityFileCopy(string from, string to, bool overwrite)
    {
      File.Copy(FileUtil.NiceWinPath(from), FileUtil.NiceWinPath(to), overwrite);
    }

    internal static string NiceWinPath(string unityPath)
    {
      return Application.platform != RuntimePlatform.WindowsEditor ? unityPath : unityPath.Replace("/", "\\");
    }

    internal static string UnityGetFileNameWithoutExtension(string path)
    {
      return Path.GetFileNameWithoutExtension(path.Replace("//", "\\\\")).Replace("\\\\", "//");
    }

    internal static string UnityGetFileName(string path)
    {
      return Path.GetFileName(path.Replace("//", "\\\\")).Replace("\\\\", "//");
    }

    internal static string UnityGetDirectoryName(string path)
    {
      return Path.GetDirectoryName(path.Replace("//", "\\\\")).Replace("\\\\", "//");
    }

    internal static void UnityFileCopy(string from, string to)
    {
      FileUtil.UnityFileCopy(from, to, false);
    }

    internal static void CreateOrCleanDirectory(string dir)
    {
      if (Directory.Exists(dir))
        Directory.Delete(dir, true);
      Directory.CreateDirectory(dir);
    }

    internal static string RemovePathPrefix(string fullPath, string prefix)
    {
      string[] strArray1 = fullPath.Split(Path.DirectorySeparatorChar);
      string[] strArray2 = prefix.Split(Path.DirectorySeparatorChar);
      int startIndex = 0;
      if (strArray1[0] == string.Empty)
        startIndex = 1;
      while (startIndex < strArray1.Length && startIndex < strArray2.Length && strArray1[startIndex] == strArray2[startIndex])
        ++startIndex;
      if (startIndex == strArray1.Length)
        return "";
      return string.Join(Path.DirectorySeparatorChar.ToString(), strArray1, startIndex, strArray1.Length - startIndex);
    }

    internal static string CombinePaths(params string[] paths)
    {
      if (paths == null)
        return string.Empty;
      return string.Join(Path.DirectorySeparatorChar.ToString(), paths);
    }

    internal static List<string> GetAllFilesRecursive(string path)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FileUtil.\u003CGetAllFilesRecursive\u003Ec__AnonStorey1 recursiveCAnonStorey1 = new FileUtil.\u003CGetAllFilesRecursive\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      recursiveCAnonStorey1.files = new List<string>();
      // ISSUE: reference to a compiler-generated method
      FileUtil.WalkFilesystemRecursively(path, new Action<string>(recursiveCAnonStorey1.\u003C\u003Em__0), (Func<string, bool>) (p => true));
      // ISSUE: reference to a compiler-generated field
      return recursiveCAnonStorey1.files;
    }

    internal static void WalkFilesystemRecursively(string path, Action<string> fileCallback, Func<string, bool> directoryCallback)
    {
      foreach (string file in Directory.GetFiles(path))
        fileCallback(file);
      foreach (string directory in Directory.GetDirectories(path))
      {
        if (directoryCallback(directory))
          FileUtil.WalkFilesystemRecursively(directory, fileCallback, directoryCallback);
      }
    }

    internal static long GetDirectorySize(string path)
    {
      string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
      long num = 0;
      foreach (string fileName in files)
      {
        FileInfo fileInfo = new FileInfo(fileName);
        num += fileInfo.Length;
      }
      return num;
    }
  }
}
