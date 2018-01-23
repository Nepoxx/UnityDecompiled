// Decompiled with JetBrains decompiler
// Type: UnityEditor.Utils.Paths
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEditor.Utils
{
  internal static class Paths
  {
    public static string Combine(params string[] components)
    {
      if (components.Length < 1)
        throw new ArgumentException("At least one component must be provided!");
      string path1 = components[0];
      for (int index = 1; index < components.Length; ++index)
        path1 = Path.Combine(path1, components[index]);
      return path1;
    }

    public static string[] Split(string path)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) path.Split(Path.DirectorySeparatorChar));
      int index = 0;
      while (index < stringList.Count)
      {
        stringList[index] = stringList[index].Trim();
        if (stringList[index].Equals(""))
          stringList.RemoveAt(index);
        else
          ++index;
      }
      return stringList.ToArray();
    }

    public static string GetFileOrFolderName(string path)
    {
      string fileName;
      if (File.Exists(path))
      {
        fileName = Path.GetFileName(path);
      }
      else
      {
        if (!Directory.Exists(path))
          throw new ArgumentException("Target '" + path + "' does not exist.");
        string[] strArray = Paths.Split(path);
        fileName = strArray[strArray.Length - 1];
      }
      return fileName;
    }

    public static string CreateTempDirectory()
    {
      for (int index = 0; index < 32; ++index)
      {
        string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        if (!File.Exists(path) && !Directory.Exists(path))
        {
          Directory.CreateDirectory(path);
          return path;
        }
      }
      throw new IOException("CreateTempDirectory failed after 32 tries");
    }

    public static string NormalizePath(this string path)
    {
      if ((int) Path.DirectorySeparatorChar == 92)
        return path.Replace('/', Path.DirectorySeparatorChar);
      return path.Replace('\\', Path.DirectorySeparatorChar);
    }

    public static string ConvertSeparatorsToUnity(this string path)
    {
      return path.Replace('\\', '/');
    }

    public static string UnifyDirectorySeparator(string path)
    {
      return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
    }

    public static bool AreEqual(string pathA, string pathB, bool ignoreCase)
    {
      if (pathA == "" && pathB == "")
        return true;
      if (string.IsNullOrEmpty(pathA) || string.IsNullOrEmpty(pathB))
        return false;
      return string.Compare(Path.GetFullPath(pathA), Path.GetFullPath(pathB), !ignoreCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool IsValidAssetPathWithErrorLogging(string assetPath, string requiredExtensionWithDot)
    {
      string errorMsg;
      if (Paths.IsValidAssetPath(assetPath, requiredExtensionWithDot, out errorMsg))
        return true;
      Debug.LogError((object) errorMsg);
      return false;
    }

    public static bool IsValidAssetPath(string assetPath)
    {
      return Paths.IsValidAssetPath(assetPath, (string) null);
    }

    public static bool IsValidAssetPath(string assetPath, string requiredExtensionWithDot)
    {
      string errorMsg = (string) null;
      return Paths.CheckIfAssetPathIsValid(assetPath, requiredExtensionWithDot, ref errorMsg);
    }

    public static bool IsValidAssetPath(string assetPath, string requiredExtensionWithDot, out string errorMsg)
    {
      errorMsg = string.Empty;
      return Paths.CheckIfAssetPathIsValid(assetPath, requiredExtensionWithDot, ref errorMsg);
    }

    private static bool CheckIfAssetPathIsValid(string assetPath, string requiredExtensionWithDot, ref string errorMsg)
    {
      try
      {
        if (string.IsNullOrEmpty(assetPath))
        {
          if (errorMsg != null)
            Paths.SetFullErrorMessage("Asset path is empty", assetPath, ref errorMsg);
          return false;
        }
        string fileName = Path.GetFileName(assetPath);
        if (fileName.StartsWith("."))
        {
          if (errorMsg != null)
            Paths.SetFullErrorMessage("Do not prefix asset name with '.'", assetPath, ref errorMsg);
          return false;
        }
        if (fileName.StartsWith(" "))
        {
          if (errorMsg != null)
            Paths.SetFullErrorMessage("Do not prefix asset name with white space", assetPath, ref errorMsg);
          return false;
        }
        if (!string.IsNullOrEmpty(requiredExtensionWithDot))
        {
          if (!string.Equals(Path.GetExtension(assetPath), requiredExtensionWithDot, StringComparison.OrdinalIgnoreCase))
          {
            if (errorMsg != null)
              Paths.SetFullErrorMessage(string.Format("Incorrect extension. Required extension is: '{0}'", (object) requiredExtensionWithDot), assetPath, ref errorMsg);
            return false;
          }
        }
      }
      catch (Exception ex)
      {
        if (errorMsg != null)
          Paths.SetFullErrorMessage(ex.Message, assetPath, ref errorMsg);
        return false;
      }
      return true;
    }

    private static void SetFullErrorMessage(string error, string assetPath, ref string errorMsg)
    {
      errorMsg = string.Format("Asset path error: '{0}' is not valid: {1}", (object) Paths.ToLiteral(assetPath), (object) error);
    }

    private static string ToLiteral(string input)
    {
      if (string.IsNullOrEmpty(input))
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder(input.Length + 2);
      foreach (char ch in input)
      {
        switch (ch)
        {
          case char.MinValue:
            stringBuilder.Append("\\0");
            break;
          case '\a':
            stringBuilder.Append("\\a");
            break;
          case '\b':
            stringBuilder.Append("\\b");
            break;
          case '\t':
            stringBuilder.Append("\\t");
            break;
          case '\n':
            stringBuilder.Append("\\n");
            break;
          case '\v':
            stringBuilder.Append("\\v");
            break;
          case '\f':
            stringBuilder.Append("\\f");
            break;
          case '\r':
            stringBuilder.Append("\\r");
            break;
          case '"':
            stringBuilder.Append("\\\"");
            break;
          case '\'':
            stringBuilder.Append("\\'");
            break;
          case '\\':
            stringBuilder.Append("\\\\");
            break;
          default:
            if ((int) ch >= 32 && (int) ch <= 126)
            {
              stringBuilder.Append(ch);
              break;
            }
            stringBuilder.Append("\\u");
            stringBuilder.Append(((int) ch).ToString("x4"));
            break;
        }
      }
      return stringBuilder.ToString();
    }
  }
}
