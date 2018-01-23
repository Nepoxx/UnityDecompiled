// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FileMirroring
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEditorInternal
{
  internal static class FileMirroring
  {
    public static void MirrorFile(string from, string to)
    {
      string from1 = from;
      string to1 = to;
      // ISSUE: reference to a compiler-generated field
      if (FileMirroring.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        FileMirroring.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string, bool>(FileMirroring.CanSkipCopy);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, string, bool> fMgCache0 = FileMirroring.\u003C\u003Ef__mg\u0024cache0;
      FileMirroring.MirrorFile(from1, to1, fMgCache0);
    }

    public static void MirrorFile(string from, string to, Func<string, string, bool> comparer)
    {
      if (comparer(from, to))
        return;
      if (!File.Exists(from))
      {
        FileMirroring.DeleteFileOrDirectory(to);
      }
      else
      {
        string directoryName = Path.GetDirectoryName(to);
        if (!Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        File.Copy(from, to, true);
      }
    }

    public static void MirrorFolder(string from, string to)
    {
      string from1 = from;
      string to1 = to;
      // ISSUE: reference to a compiler-generated field
      if (FileMirroring.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        FileMirroring.\u003C\u003Ef__mg\u0024cache1 = new Func<string, string, bool>(FileMirroring.CanSkipCopy);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, string, bool> fMgCache1 = FileMirroring.\u003C\u003Ef__mg\u0024cache1;
      FileMirroring.MirrorFolder(from1, to1, fMgCache1);
    }

    public static void MirrorFolder(string from, string to, Func<string, string, bool> comparer)
    {
      from = Path.GetFullPath(from);
      to = Path.GetFullPath(to);
      if (!Directory.Exists(from))
      {
        if (!Directory.Exists(to))
          return;
        Directory.Delete(to, true);
      }
      else
      {
        if (!Directory.Exists(to))
          Directory.CreateDirectory(to);
        IEnumerable<string> first = ((IEnumerable<string>) Directory.GetFileSystemEntries(to)).Select<string, string>((Func<string, string>) (s => FileMirroring.StripPrefix(s, to)));
        IEnumerable<string> second = ((IEnumerable<string>) Directory.GetFileSystemEntries(from)).Select<string, string>((Func<string, string>) (s => FileMirroring.StripPrefix(s, from)));
        foreach (string path2 in first.Except<string>(second))
          FileMirroring.DeleteFileOrDirectory(Path.Combine(to, path2));
        foreach (string path2 in second)
        {
          string str1 = Path.Combine(from, path2);
          string str2 = Path.Combine(to, path2);
          FileMirroring.FileEntryType fileEntryType1 = FileMirroring.FileEntryTypeFor(str1);
          FileMirroring.FileEntryType fileEntryType2 = FileMirroring.FileEntryTypeFor(str2);
          if (fileEntryType1 == FileMirroring.FileEntryType.File && fileEntryType2 == FileMirroring.FileEntryType.Directory)
            FileMirroring.DeleteFileOrDirectory(str2);
          if (fileEntryType1 == FileMirroring.FileEntryType.Directory)
          {
            if (fileEntryType2 == FileMirroring.FileEntryType.File)
              FileMirroring.DeleteFileOrDirectory(str2);
            if (fileEntryType2 != FileMirroring.FileEntryType.Directory)
              Directory.CreateDirectory(str2);
            FileMirroring.MirrorFolder(str1, str2);
          }
          if (fileEntryType1 == FileMirroring.FileEntryType.File)
            FileMirroring.MirrorFile(str1, str2, comparer);
        }
      }
    }

    private static void DeleteFileOrDirectory(string path)
    {
      if (File.Exists(path))
      {
        File.Delete(path);
      }
      else
      {
        if (!Directory.Exists(path))
          return;
        Directory.Delete(path, true);
      }
    }

    private static string StripPrefix(string s, string prefix)
    {
      return s.Substring(prefix.Length + 1);
    }

    private static FileMirroring.FileEntryType FileEntryTypeFor(string fileEntry)
    {
      if (File.Exists(fileEntry))
        return FileMirroring.FileEntryType.File;
      return Directory.Exists(fileEntry) ? FileMirroring.FileEntryType.Directory : FileMirroring.FileEntryType.NotExisting;
    }

    public static bool CanSkipCopy(string from, string to)
    {
      bool flag1 = !File.Exists(from);
      bool flag2 = !File.Exists(to);
      if (flag1 || flag2)
        return flag1 && flag2;
      return FileMirroring.AreFilesIdentical(from, to);
    }

    private static bool AreFilesIdentical(string filePath1, string filePath2)
    {
      using (FileStream fileStream1 = File.OpenRead(filePath1))
      {
        using (FileStream fileStream2 = File.OpenRead(filePath2))
        {
          if (fileStream1.Length != fileStream2.Length)
            return false;
          byte[] buffer1 = new byte[65536];
          byte[] buffer2 = new byte[65536];
          int num;
          while ((num = fileStream1.Read(buffer1, 0, buffer1.Length)) > 0)
          {
            fileStream2.Read(buffer2, 0, buffer2.Length);
            for (int index = 0; index < num; ++index)
            {
              if ((int) buffer1[index] != (int) buffer2[index])
                return false;
            }
          }
        }
      }
      return true;
    }

    private enum FileEntryType
    {
      File,
      Directory,
      NotExisting,
    }
  }
}
