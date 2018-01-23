// Decompiled with JetBrains decompiler
// Type: UnityEditor.StyleSheets.StyleSheetAssetPostprocessor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.StyleSheets
{
  internal class StyleSheetAssetPostprocessor : AssetPostprocessor
  {
    private static HashSet<string> s_StyleSheetReferencedAssetPaths;

    public static void ClearReferencedAssets()
    {
      StyleSheetAssetPostprocessor.s_StyleSheetReferencedAssetPaths = (HashSet<string>) null;
    }

    public static void AddReferencedAssetPath(string assetPath)
    {
      if (StyleSheetAssetPostprocessor.s_StyleSheetReferencedAssetPaths == null)
        StyleSheetAssetPostprocessor.s_StyleSheetReferencedAssetPaths = new HashSet<string>();
      StyleSheetAssetPostprocessor.s_StyleSheetReferencedAssetPaths.Add(assetPath);
    }

    private static void ProcessAssetPath(string assetPath)
    {
      if (StyleSheetAssetPostprocessor.s_StyleSheetReferencedAssetPaths == null || !StyleSheetAssetPostprocessor.s_StyleSheetReferencedAssetPaths.Contains(assetPath))
        return;
      StyleContext.ClearStyleCache();
      Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
      while (panelsIterator.MoveNext())
        panelsIterator.Current.Value.visualTree.Dirty(ChangeType.Styles | ChangeType.Repaint);
    }

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
      if (StyleSheetAssetPostprocessor.s_StyleSheetReferencedAssetPaths == null)
        return;
      foreach (string deletedAsset in deletedAssets)
        StyleSheetAssetPostprocessor.ProcessAssetPath(deletedAsset);
      for (int index = 0; index < movedAssets.Length; ++index)
      {
        StyleSheetAssetPostprocessor.ProcessAssetPath(movedAssets[index]);
        StyleSheetAssetPostprocessor.ProcessAssetPath(movedFromAssetPaths[index]);
      }
    }
  }
}
