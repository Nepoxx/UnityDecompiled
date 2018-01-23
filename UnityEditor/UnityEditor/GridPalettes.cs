// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridPalettes
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class GridPalettes : ScriptableSingleton<GridPalettes>
  {
    private static bool s_RefreshCache;
    [SerializeField]
    private List<GameObject> m_PalettesCache;

    public static List<GameObject> palettes
    {
      get
      {
        if (ScriptableSingleton<GridPalettes>.instance.m_PalettesCache == null || GridPalettes.s_RefreshCache)
        {
          ScriptableSingleton<GridPalettes>.instance.RefreshPalettesCache();
          GridPalettes.s_RefreshCache = false;
        }
        return ScriptableSingleton<GridPalettes>.instance.m_PalettesCache;
      }
    }

    private void RefreshPalettesCache()
    {
      if (ScriptableSingleton<GridPalettes>.instance.m_PalettesCache == null)
        ScriptableSingleton<GridPalettes>.instance.m_PalettesCache = new List<GameObject>();
      foreach (string asset in AssetDatabase.FindAssets("t:GridPalette"))
      {
        GridPalette gridPalette = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(asset), typeof (GridPalette)) as GridPalette;
        if ((UnityEngine.Object) gridPalette != (UnityEngine.Object) null)
        {
          GameObject gameObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) gridPalette)) as GameObject;
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
            this.m_PalettesCache.Add(gameObject);
        }
      }
      this.m_PalettesCache.Sort((Comparison<GameObject>) ((x, y) => string.Compare(x.name, y.name, StringComparison.OrdinalIgnoreCase)));
    }

    internal static void CleanCache()
    {
      ScriptableSingleton<GridPalettes>.instance.m_PalettesCache = (List<GameObject>) null;
    }

    public class AssetProcessor : AssetPostprocessor
    {
      public override int GetPostprocessOrder()
      {
        return 1;
      }

      private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
      {
        if (GridPaintingState.savingPalette)
          return;
        GridPalettes.CleanCache();
      }
    }
  }
}
