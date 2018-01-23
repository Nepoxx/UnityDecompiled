// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridPaletteUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  internal static class GridPaletteUtility
  {
    public static RectInt GetBounds(GameObject palette)
    {
      if ((Object) palette == (Object) null)
        return new RectInt();
      Vector2Int p1 = new Vector2Int(int.MaxValue, int.MaxValue);
      Vector2Int p2 = new Vector2Int(int.MinValue, int.MinValue);
      foreach (Tilemap componentsInChild in palette.GetComponentsInChildren<Tilemap>())
      {
        Vector3Int editorPreviewOrigin = componentsInChild.editorPreviewOrigin;
        Vector3Int vector3Int = editorPreviewOrigin + componentsInChild.editorPreviewSize;
        Vector2Int vector2Int1 = new Vector2Int(Mathf.Min(editorPreviewOrigin.x, vector3Int.x), Mathf.Min(editorPreviewOrigin.y, vector3Int.y));
        Vector2Int vector2Int2 = new Vector2Int(Mathf.Max(editorPreviewOrigin.x, vector3Int.x), Mathf.Max(editorPreviewOrigin.y, vector3Int.y));
        p1 = new Vector2Int(Mathf.Min(p1.x, vector2Int1.x), Mathf.Min(p1.y, vector2Int1.y));
        p2 = new Vector2Int(Mathf.Max(p2.x, vector2Int2.x), Mathf.Max(p2.y, vector2Int2.y));
      }
      return GridEditorUtility.GetMarqueeRect(p1, p2);
    }

    public static GameObject CreateNewPaletteNamed(string name, GridLayout.CellLayout layout, GridPalette.CellSizing cellSizing, Vector3 cellSize)
    {
      string projectRelativePath = FileUtil.GetProjectRelativePath(EditorUtility.SaveFolderPanel("Create palette into folder ", !(bool) ((Object) ProjectBrowser.s_LastInteractedProjectBrowser) ? "Assets" : ProjectBrowser.s_LastInteractedProjectBrowser.GetActiveFolderPath(), ""));
      if (string.IsNullOrEmpty(projectRelativePath))
        return (GameObject) null;
      return GridPaletteUtility.CreateNewPalette(projectRelativePath, name, layout, cellSizing, cellSize);
    }

    public static GameObject CreateNewPalette(string folderPath, string name, GridLayout.CellLayout layout, GridPalette.CellSizing cellSizing, Vector3 cellSize)
    {
      GameObject gameObject = new GameObject(name);
      Grid grid = gameObject.AddComponent<Grid>();
      grid.cellSize = cellSize;
      grid.cellLayout = layout;
      GridPaletteUtility.CreateNewLayer(gameObject, "Layer1", layout);
      string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/" + name + ".prefab");
      Object emptyPrefab = PrefabUtility.CreateEmptyPrefab(uniqueAssetPath);
      GridPalette instance = ScriptableObject.CreateInstance<GridPalette>();
      instance.name = "Palette Settings";
      instance.cellSizing = cellSizing;
      AssetDatabase.AddObjectToAsset((Object) instance, emptyPrefab);
      PrefabUtility.ReplacePrefab(gameObject, emptyPrefab, ReplacePrefabOptions.Default);
      AssetDatabase.Refresh();
      Object.DestroyImmediate((Object) gameObject);
      return AssetDatabase.LoadAssetAtPath<GameObject>(uniqueAssetPath);
    }

    public static GameObject CreateNewLayer(GameObject paletteGO, string name, GridLayout.CellLayout layout)
    {
      GameObject gameObject = new GameObject(name);
      gameObject.AddComponent<Tilemap>();
      gameObject.AddComponent<TilemapRenderer>();
      gameObject.transform.parent = paletteGO.transform;
      gameObject.layer = paletteGO.layer;
      if (layout == GridLayout.CellLayout.Rectangle)
        paletteGO.GetComponent<Grid>().cellSize = new Vector3(1f, 1f, 0.0f);
      return gameObject;
    }

    public static Vector3 CalculateAutoCellSize(Grid grid, Vector3 defaultValue)
    {
      foreach (Tilemap componentsInChild in grid.GetComponentsInChildren<Tilemap>())
      {
        foreach (Vector3Int position in componentsInChild.cellBounds.allPositionsWithin)
        {
          Sprite sprite = componentsInChild.GetSprite(position);
          if ((Object) sprite != (Object) null)
            return new Vector3(sprite.rect.width, sprite.rect.height, 0.0f) / sprite.pixelsPerUnit;
        }
      }
      return defaultValue;
    }
  }
}
