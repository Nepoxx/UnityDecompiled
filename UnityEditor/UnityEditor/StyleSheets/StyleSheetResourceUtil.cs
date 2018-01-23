// Decompiled with JetBrains decompiler
// Type: UnityEditor.StyleSheets.StyleSheetResourceUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.IO;
using UnityEngine;
using UnityEngine.StyleSheets;

namespace UnityEditor.StyleSheets
{
  internal class StyleSheetResourceUtil
  {
    internal static UnityEngine.Object LoadResource(string pathName, System.Type type)
    {
      return StyleSheetResourceUtil.LoadResource(pathName, type, (double) GUIUtility.pixelsPerPoint > 1.0);
    }

    internal static UnityEngine.Object LoadResource(string pathName, System.Type type, bool lookForRetinaAssets)
    {
      UnityEngine.Object assetObject = (UnityEngine.Object) null;
      string str = string.Empty;
      lookForRetinaAssets &= type == typeof (Texture2D);
      if (lookForRetinaAssets)
      {
        string extension = Path.GetExtension(pathName);
        string path2 = Path.GetFileNameWithoutExtension(pathName) + "@2x" + extension;
        str = Path.Combine(Path.GetDirectoryName(pathName), path2);
        lookForRetinaAssets = !string.IsNullOrEmpty(str);
      }
      if (lookForRetinaAssets)
        assetObject = EditorGUIUtility.Load(str);
      if (assetObject == (UnityEngine.Object) null)
        assetObject = EditorGUIUtility.Load(pathName);
      if (assetObject == (UnityEngine.Object) null && lookForRetinaAssets)
        assetObject = UnityEngine.Resources.Load(str, type);
      if (assetObject == (UnityEngine.Object) null)
        assetObject = UnityEngine.Resources.Load(pathName, type);
      if (assetObject == (UnityEngine.Object) null && lookForRetinaAssets)
        assetObject = AssetDatabase.LoadMainAssetAtPath(str);
      if (assetObject == (UnityEngine.Object) null)
        assetObject = AssetDatabase.LoadMainAssetAtPath(pathName);
      if (assetObject != (UnityEngine.Object) null)
      {
        string assetPath = AssetDatabase.GetAssetPath(assetObject);
        if (type != typeof (StyleSheet) && !assetPath.StartsWith("Library"))
          StyleSheetAssetPostprocessor.AddReferencedAssetPath(assetPath);
      }
      return assetObject;
    }
  }
}
