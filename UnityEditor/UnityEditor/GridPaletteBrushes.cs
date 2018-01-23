// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridPaletteBrushes
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class GridPaletteBrushes : ScriptableSingleton<GridPaletteBrushes>
  {
    private static readonly string s_LibraryPath = "Library/GridBrush";
    private static readonly string s_GridBrushExtension = ".asset";
    private static bool s_RefreshCache;
    [SerializeField]
    private List<GridBrushBase> m_Brushes;
    private string[] m_BrushNames;

    public static List<GridBrushBase> brushes
    {
      get
      {
        if (ScriptableSingleton<GridPaletteBrushes>.instance.m_Brushes == null || ScriptableSingleton<GridPaletteBrushes>.instance.m_Brushes.Count == 0 || GridPaletteBrushes.s_RefreshCache)
        {
          ScriptableSingleton<GridPaletteBrushes>.instance.RefreshBrushesCache();
          GridPaletteBrushes.s_RefreshCache = false;
        }
        return ScriptableSingleton<GridPaletteBrushes>.instance.m_Brushes;
      }
    }

    public static string[] brushNames
    {
      get
      {
        return ScriptableSingleton<GridPaletteBrushes>.instance.m_BrushNames;
      }
    }

    public static System.Type GetDefaultBrushType()
    {
      System.Type type1 = typeof (GridBrush);
      Assembly[] loadedAssemblies = EditorAssemblies.loadedAssemblies;
      int num = 0;
      for (int index = loadedAssemblies.Length - 1; index >= 0; --index)
      {
        foreach (System.Type type2 in AssemblyHelper.GetTypesFromAssembly(loadedAssemblies[index]))
        {
          CustomGridBrushAttribute[] customAttributes = type2.GetCustomAttributes(typeof (CustomGridBrushAttribute), false) as CustomGridBrushAttribute[];
          if (customAttributes != null && customAttributes.Length > 0 && customAttributes[0].defaultBrush)
          {
            type1 = type2;
            ++num;
          }
        }
      }
      if (num > 1)
        Debug.LogWarning((object) "Multiple occurrences of defaultBrush == true found. It should only be declared once.");
      return type1;
    }

    public static void ActiveGridBrushAssetChanged()
    {
      if ((UnityEngine.Object) GridPaintingState.gridBrush == (UnityEngine.Object) null || !GridPaletteBrushes.IsLibraryBrush(GridPaintingState.gridBrush))
        return;
      ScriptableSingleton<GridPaletteBrushes>.instance.SaveLibraryGridBrushAsset(GridPaintingState.gridBrush);
    }

    private void RefreshBrushesCache()
    {
      if (this.m_Brushes == null)
        this.m_Brushes = new List<GridBrushBase>();
      if (this.m_Brushes.Count == 0 || !(this.m_Brushes[0] is GridBrush))
      {
        this.m_Brushes.Insert(0, this.LoadOrCreateLibraryGridBrushAsset(GridPaletteBrushes.GetDefaultBrushType()));
        this.m_Brushes[0].name = this.GetBrushDropdownName(this.m_Brushes[0]);
      }
      foreach (Assembly loadedAssembly in EditorAssemblies.loadedAssemblies)
      {
        try
        {
          foreach (System.Type brushType in ((IEnumerable<System.Type>) loadedAssembly.GetTypes()).Where<System.Type>((Func<System.Type, bool>) (t => t != typeof (GridBrushBase) && t != typeof (GridBrush) && typeof (GridBrushBase).IsAssignableFrom(t))))
          {
            if (this.IsDefaultInstanceVisibleGridBrushType(brushType))
            {
              GridBrushBase libraryGridBrushAsset = this.LoadOrCreateLibraryGridBrushAsset(brushType);
              if ((UnityEngine.Object) libraryGridBrushAsset != (UnityEngine.Object) null)
                this.m_Brushes.Add(libraryGridBrushAsset);
            }
          }
        }
        catch (Exception ex)
        {
          Debug.Log((object) string.Format("TilePalette failed to get types from {0}. Error: {1}", (object) loadedAssembly.FullName, (object) ex.Message));
        }
      }
      foreach (string asset in AssetDatabase.FindAssets("t:GridBrushBase"))
      {
        GridBrushBase gridBrushBase = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(asset), typeof (GridBrushBase)) as GridBrushBase;
        if ((UnityEngine.Object) gridBrushBase != (UnityEngine.Object) null && this.IsAssetVisibleGridBrushType(gridBrushBase.GetType()))
          this.m_Brushes.Add(gridBrushBase);
      }
      this.m_BrushNames = new string[this.m_Brushes.Count];
      for (int index = 0; index < this.m_Brushes.Count; ++index)
        this.m_BrushNames[index] = this.m_Brushes[index].name;
    }

    private bool IsDefaultInstanceVisibleGridBrushType(System.Type brushType)
    {
      CustomGridBrushAttribute[] customAttributes = brushType.GetCustomAttributes(typeof (CustomGridBrushAttribute), false) as CustomGridBrushAttribute[];
      if (customAttributes != null && customAttributes.Length > 0)
        return !customAttributes[0].hideDefaultInstance;
      return false;
    }

    private bool IsAssetVisibleGridBrushType(System.Type brushType)
    {
      CustomGridBrushAttribute[] customAttributes = brushType.GetCustomAttributes(typeof (CustomGridBrushAttribute), false) as CustomGridBrushAttribute[];
      if (customAttributes != null && customAttributes.Length > 0)
        return !customAttributes[0].hideAssetInstances;
      return false;
    }

    private void SaveLibraryGridBrushAsset(GridBrushBase brush)
    {
      string instanceLibraryPath = this.GenerateGridBrushInstanceLibraryPath(brush.GetType());
      string directoryName = Path.GetDirectoryName(instanceLibraryPath);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      InternalEditorUtility.SaveToSerializedFileAndForget((UnityEngine.Object[]) new GridBrushBase[1]
      {
        brush
      }, instanceLibraryPath, true);
    }

    private GridBrushBase LoadOrCreateLibraryGridBrushAsset(System.Type brushType)
    {
      UnityEngine.Object[] objectArray = InternalEditorUtility.LoadSerializedFileAndForget(this.GenerateGridBrushInstanceLibraryPath(brushType));
      if (objectArray != null && objectArray.Length > 0)
      {
        GridBrushBase gridBrushBase = objectArray[0] as GridBrushBase;
        if ((UnityEngine.Object) gridBrushBase != (UnityEngine.Object) null && gridBrushBase.GetType() == brushType)
          return gridBrushBase;
      }
      return this.CreateLibraryGridBrushAsset(brushType);
    }

    private GridBrushBase CreateLibraryGridBrushAsset(System.Type brushType)
    {
      GridBrushBase instance = ScriptableObject.CreateInstance(brushType) as GridBrushBase;
      instance.hideFlags = HideFlags.DontSave;
      instance.name = this.GetBrushDropdownName(instance);
      this.SaveLibraryGridBrushAsset(instance);
      return instance;
    }

    private string GenerateGridBrushInstanceLibraryPath(System.Type brushType)
    {
      return FileUtil.NiceWinPath(FileUtil.CombinePaths(GridPaletteBrushes.s_LibraryPath, brushType.ToString() + GridPaletteBrushes.s_GridBrushExtension));
    }

    private string GetBrushDropdownName(GridBrushBase brush)
    {
      if (!GridPaletteBrushes.IsLibraryBrush(brush))
        return brush.name;
      CustomGridBrushAttribute[] customAttributes = brush.GetType().GetCustomAttributes(typeof (CustomGridBrushAttribute), false) as CustomGridBrushAttribute[];
      if (customAttributes != null && customAttributes.Length > 0 && customAttributes[0].defaultName.Length > 0)
        return customAttributes[0].defaultName;
      if (brush.GetType() == typeof (GridBrush))
        return "Default Brush";
      return brush.GetType().Name;
    }

    private static bool IsLibraryBrush(GridBrushBase brush)
    {
      return !AssetDatabase.Contains((UnityEngine.Object) brush);
    }

    internal static void FlushCache()
    {
      GridPaletteBrushes.s_RefreshCache = true;
      if (ScriptableSingleton<GridPaletteBrushes>.instance.m_Brushes == null)
        return;
      ScriptableSingleton<GridPaletteBrushes>.instance.m_Brushes.Clear();
      GridPaintingState.FlushCache();
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
        GridPaletteBrushes.FlushCache();
      }
    }
  }
}
