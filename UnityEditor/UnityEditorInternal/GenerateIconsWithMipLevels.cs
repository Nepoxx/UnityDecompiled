// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.GenerateIconsWithMipLevels
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  public class GenerateIconsWithMipLevels
  {
    private static string k_IconSourceFolder = "Assets/MipLevels For Icons/";
    private static string k_IconTargetFolder = "Assets/Editor Default Resources/Icons/Processed";
    private static string k_IconMipIdentifier = "@";

    private static GenerateIconsWithMipLevels.InputData GetInputData()
    {
      return new GenerateIconsWithMipLevels.InputData() { sourceFolder = GenerateIconsWithMipLevels.k_IconSourceFolder, targetFolder = GenerateIconsWithMipLevels.k_IconTargetFolder, mipIdentifier = GenerateIconsWithMipLevels.k_IconMipIdentifier, mipFileExtension = "png" };
    }

    public static void GenerateAllIconsWithMipLevels()
    {
      GenerateIconsWithMipLevels.InputData inputData = GenerateIconsWithMipLevels.GetInputData();
      GenerateIconsWithMipLevels.EnsureFolderIsCreated(inputData.targetFolder);
      float realtimeSinceStartup = Time.realtimeSinceStartup;
      GenerateIconsWithMipLevels.GenerateIconsWithMips(inputData);
      Debug.Log((object) string.Format("Generated {0} icons with mip levels in {1} seconds", (object) inputData.generatedFileNames.Count, (object) (float) ((double) Time.realtimeSinceStartup - (double) realtimeSinceStartup)));
      GenerateIconsWithMipLevels.RemoveUnusedFiles(inputData.generatedFileNames);
      AssetDatabase.Refresh();
      InternalEditorUtility.RepaintAllViews();
    }

    public static bool VerifyIconPath(string assetPath, bool logError)
    {
      if (string.IsNullOrEmpty(assetPath))
        return false;
      if (assetPath.IndexOf(GenerateIconsWithMipLevels.k_IconSourceFolder) < 0)
      {
        if (logError)
          Debug.Log((object) ("Selection is not a valid mip texture, it should be located in: " + GenerateIconsWithMipLevels.k_IconSourceFolder));
        return false;
      }
      if (assetPath.IndexOf(GenerateIconsWithMipLevels.k_IconMipIdentifier) >= 0)
        return true;
      if (logError)
        Debug.Log((object) ("Selection does not have a valid mip identifier " + assetPath + "  " + GenerateIconsWithMipLevels.k_IconMipIdentifier));
      return false;
    }

    public static void GenerateSelectedIconsWithMips()
    {
      if (Selection.activeInstanceID == 0)
      {
        Debug.Log((object) ("Ensure to select a mip texture..." + (object) Selection.activeInstanceID));
      }
      else
      {
        GenerateIconsWithMipLevels.InputData inputData = GenerateIconsWithMipLevels.GetInputData();
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        if (!GenerateIconsWithMipLevels.VerifyIconPath(assetPath, true))
          return;
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        string str = assetPath.Replace(inputData.sourceFolder, "");
        string baseName = str.Substring(0, str.LastIndexOf(inputData.mipIdentifier));
        List<string> iconAssetPaths = GenerateIconsWithMipLevels.GetIconAssetPaths(inputData.sourceFolder, inputData.mipIdentifier, inputData.mipFileExtension);
        GenerateIconsWithMipLevels.EnsureFolderIsCreated(inputData.targetFolder);
        GenerateIconsWithMipLevels.GenerateIcon(inputData, baseName, iconAssetPaths, (Dictionary<int, Texture2D>) null, (FileInfo) null);
        Debug.Log((object) string.Format("Generated {0} icon with mip levels in {1} seconds", (object) baseName, (object) (float) ((double) Time.realtimeSinceStartup - (double) realtimeSinceStartup)));
        InternalEditorUtility.RepaintAllViews();
      }
    }

    public static void GenerateIconWithMipLevels(string assetPath, Dictionary<int, Texture2D> mipTextures, FileInfo fileInfo)
    {
      if (!GenerateIconsWithMipLevels.VerifyIconPath(assetPath, true))
        return;
      GenerateIconsWithMipLevels.InputData inputData = GenerateIconsWithMipLevels.GetInputData();
      float realtimeSinceStartup = Time.realtimeSinceStartup;
      string str = assetPath.Replace(inputData.sourceFolder, "");
      string baseName = str.Substring(0, str.LastIndexOf(inputData.mipIdentifier));
      List<string> iconAssetPaths = GenerateIconsWithMipLevels.GetIconAssetPaths(inputData.sourceFolder, inputData.mipIdentifier, inputData.mipFileExtension);
      GenerateIconsWithMipLevels.EnsureFolderIsCreated(inputData.targetFolder);
      if (GenerateIconsWithMipLevels.GenerateIcon(inputData, baseName, iconAssetPaths, mipTextures, fileInfo))
        Debug.Log((object) string.Format("Generated {0} icon with mip levels in {1} seconds", (object) baseName, (object) (float) ((double) Time.realtimeSinceStartup - (double) realtimeSinceStartup)));
      InternalEditorUtility.RepaintAllViews();
    }

    public static int MipLevelForAssetPath(string assetPath, string separator)
    {
      if (string.IsNullOrEmpty(assetPath) || string.IsNullOrEmpty(separator))
        return -1;
      int num1 = assetPath.IndexOf(separator);
      if (num1 == -1)
      {
        Debug.LogError((object) ("\"" + separator + "\" could not be found in asset path: " + assetPath));
        return -1;
      }
      int startIndex = num1 + separator.Length;
      int num2 = assetPath.IndexOf(".", startIndex);
      if (num2 != -1)
        return int.Parse(assetPath.Substring(startIndex, num2 - startIndex));
      Debug.LogError((object) ("Could not find path extension in asset path: " + assetPath));
      return -1;
    }

    private static void GenerateIconsWithMips(GenerateIconsWithMipLevels.InputData inputData)
    {
      List<string> iconAssetPaths = GenerateIconsWithMipLevels.GetIconAssetPaths(inputData.sourceFolder, inputData.mipIdentifier, inputData.mipFileExtension);
      if (iconAssetPaths.Count == 0)
        Debug.LogWarning((object) ("No mip files found for generating icons! Searching in: " + inputData.sourceFolder + ", for files with extension: " + inputData.mipFileExtension));
      foreach (string baseName in GenerateIconsWithMipLevels.GetBaseNames(inputData, iconAssetPaths))
        GenerateIconsWithMipLevels.GenerateIcon(inputData, baseName, iconAssetPaths, (Dictionary<int, Texture2D>) null, (FileInfo) null);
    }

    private static bool GenerateIcon(GenerateIconsWithMipLevels.InputData inputData, string baseName, List<string> assetPathsOfAllIcons, Dictionary<int, Texture2D> mipTextures, FileInfo sourceFileInfo)
    {
      string str = inputData.targetFolder + "/" + baseName + " Icon.asset";
      if (sourceFileInfo != null && File.Exists(str) && new FileInfo(str).LastWriteTime > sourceFileInfo.LastWriteTime)
        return false;
      Debug.Log((object) ("Generating MIP levels for " + str));
      GenerateIconsWithMipLevels.EnsureFolderIsCreatedRecursively(Path.GetDirectoryName(str));
      Texture2D iconWithMipLevels = GenerateIconsWithMipLevels.CreateIconWithMipLevels(inputData, baseName, assetPathsOfAllIcons, mipTextures);
      if ((UnityEngine.Object) iconWithMipLevels == (UnityEngine.Object) null)
      {
        Debug.Log((object) "CreateIconWithMipLevels failed");
        return false;
      }
      iconWithMipLevels.name = baseName + " Icon.png";
      AssetDatabase.CreateAsset((UnityEngine.Object) iconWithMipLevels, str);
      inputData.generatedFileNames.Add(str);
      return true;
    }

    private static Texture2D CreateIconWithMipLevels(GenerateIconsWithMipLevels.InputData inputData, string baseName, List<string> assetPathsOfAllIcons, Dictionary<int, Texture2D> mipTextures)
    {
      List<string> all = assetPathsOfAllIcons.FindAll((Predicate<string>) (o => o.IndexOf(47.ToString() + baseName + inputData.mipIdentifier) >= 0));
      List<Texture2D> sortedTextures = new List<Texture2D>();
      foreach (string str in all)
      {
        int key = GenerateIconsWithMipLevels.MipLevelForAssetPath(str, inputData.mipIdentifier);
        Texture2D texture2D = mipTextures == null || !mipTextures.ContainsKey(key) ? GenerateIconsWithMipLevels.GetTexture2D(str) : mipTextures[key];
        if ((UnityEngine.Object) texture2D != (UnityEngine.Object) null)
          sortedTextures.Add(texture2D);
        else
          Debug.LogError((object) ("Mip not found " + str));
      }
      sortedTextures.Sort((Comparison<Texture2D>) ((first, second) =>
      {
        if (first.width == second.width)
          return 0;
        return first.width < second.width ? 1 : -1;
      }));
      int num1 = 99999;
      int num2 = 0;
      foreach (Texture texture in sortedTextures)
      {
        int width = texture.width;
        if (width > num2)
          num2 = width;
        if (width < num1)
          num1 = width;
      }
      if (num2 == 0)
        return (Texture2D) null;
      Texture2D iconWithMips = new Texture2D(num2, num2, TextureFormat.RGBA32, true, true);
      if (!GenerateIconsWithMipLevels.BlitMip(iconWithMips, sortedTextures, 0))
        return iconWithMips;
      iconWithMips.Apply(true);
      int num3 = num2;
      for (int mipLevel = 1; mipLevel < iconWithMips.mipmapCount; ++mipLevel)
      {
        num3 /= 2;
        if (num3 >= num1)
          GenerateIconsWithMipLevels.BlitMip(iconWithMips, sortedTextures, mipLevel);
        else
          break;
      }
      iconWithMips.Apply(false, true);
      return iconWithMips;
    }

    private static bool BlitMip(Texture2D iconWithMips, List<Texture2D> sortedTextures, int mipLevel)
    {
      if (mipLevel < 0 || mipLevel >= sortedTextures.Count)
      {
        Debug.LogError((object) ("Invalid mip level: " + (object) mipLevel));
        return false;
      }
      Texture2D sortedTexture = sortedTextures[mipLevel];
      if ((bool) ((UnityEngine.Object) sortedTexture))
      {
        GenerateIconsWithMipLevels.Blit(sortedTexture, iconWithMips, mipLevel);
        return true;
      }
      Debug.LogError((object) ("No texture at mip level: " + (object) mipLevel));
      return false;
    }

    private static Texture2D GetTexture2D(string path)
    {
      return AssetDatabase.LoadAssetAtPath(path, typeof (Texture2D)) as Texture2D;
    }

    private static List<string> GetIconAssetPaths(string folderPath, string mustHaveIdentifier, string extension)
    {
      string str = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
      Uri uri1 = new Uri(str);
      List<string> stringList = new List<string>((IEnumerable<string>) Directory.GetFiles(str, "*." + extension, SearchOption.AllDirectories));
      stringList.RemoveAll((Predicate<string>) (o => o.IndexOf(mustHaveIdentifier) < 0));
      for (int index = 0; index < stringList.Count; ++index)
      {
        Uri uri2 = new Uri(stringList[index]);
        Uri uri3 = uri1.MakeRelativeUri(uri2);
        stringList[index] = folderPath + uri3.ToString();
      }
      return stringList;
    }

    private static void Blit(Texture2D source, Texture2D dest, int mipLevel)
    {
      Color32[] pixels32 = source.GetPixels32();
      for (int index = 0; index < pixels32.Length; ++index)
      {
        Color32 color32 = pixels32[index];
        if ((int) color32.a >= 3)
          color32.a -= (byte) 3;
        pixels32[index] = color32;
      }
      dest.SetPixels32(pixels32, mipLevel);
    }

    private static void EnsureFolderIsCreatedRecursively(string targetFolder)
    {
      if (AssetDatabase.GetMainAssetInstanceID(targetFolder) != 0)
        return;
      GenerateIconsWithMipLevels.EnsureFolderIsCreatedRecursively(Path.GetDirectoryName(targetFolder));
      Debug.Log((object) ("Created target folder " + targetFolder));
      AssetDatabase.CreateFolder(Path.GetDirectoryName(targetFolder), Path.GetFileName(targetFolder));
    }

    private static void EnsureFolderIsCreated(string targetFolder)
    {
      if (AssetDatabase.GetMainAssetInstanceID(targetFolder) != 0)
        return;
      Debug.Log((object) ("Created target folder " + targetFolder));
      AssetDatabase.CreateFolder(Path.GetDirectoryName(targetFolder), Path.GetFileName(targetFolder));
    }

    private static void DeleteFile(string file)
    {
      if (AssetDatabase.GetMainAssetInstanceID(file) == 0)
        return;
      Debug.Log((object) ("Deleted unused file: " + file));
      AssetDatabase.DeleteAsset(file);
    }

    private static void RemoveUnusedFiles(List<string> generatedFiles)
    {
      for (int index = 0; index < generatedFiles.Count; ++index)
      {
        string str = generatedFiles[index].Replace("Icons/Processed", "Icons").Replace(".asset", ".png");
        GenerateIconsWithMipLevels.DeleteFile(str);
        string withoutExtension = Path.GetFileNameWithoutExtension(str);
        if (!withoutExtension.StartsWith("d_"))
          GenerateIconsWithMipLevels.DeleteFile(str.Replace(withoutExtension, "d_" + withoutExtension));
      }
      AssetDatabase.Refresh();
    }

    private static string[] GetBaseNames(GenerateIconsWithMipLevels.InputData inputData, List<string> files)
    {
      string[] strArray = new string[files.Count];
      int length = inputData.sourceFolder.Length;
      for (int index = 0; index < files.Count; ++index)
        strArray[index] = files[index].Substring(length, files[index].IndexOf(inputData.mipIdentifier) - length);
      HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) strArray);
      string[] array = new string[stringSet.Count];
      stringSet.CopyTo(array);
      return array;
    }

    private class InputData
    {
      public List<string> generatedFileNames = new List<string>();
      public string sourceFolder;
      public string targetFolder;
      public string mipIdentifier;
      public string mipFileExtension;

      public string GetMipFileName(string baseName, int mipResolution)
      {
        return this.sourceFolder + baseName + this.mipIdentifier + (object) mipResolution + "." + this.mipFileExtension;
      }
    }
  }
}
