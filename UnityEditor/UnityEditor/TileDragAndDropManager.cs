// Decompiled with JetBrains decompiler
// Type: UnityEditor.TileDragAndDropManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  internal class TileDragAndDropManager : ScriptableSingleton<TileDragAndDropManager>
  {
    private bool m_RegisteredEventHandlers;
    private Dictionary<Vector2Int, Object> m_HoverData;

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
      ScriptableSingleton<TileDragAndDropManager>.instance.RegisterEventHandlers();
    }

    private void OnEnable()
    {
      this.RegisterEventHandlers();
    }

    private void RegisterEventHandlers()
    {
      if (this.m_RegisteredEventHandlers)
        return;
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneGUIDelegate);
      this.m_RegisteredEventHandlers = true;
    }

    private void OnDisable()
    {
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneGUIDelegate);
      this.m_RegisteredEventHandlers = false;
    }

    private void OnSceneGUIDelegate(SceneView sceneView)
    {
      Event current1 = Event.current;
      if (current1.type != EventType.DragUpdated && current1.type != EventType.DragPerform && (current1.type != EventType.DragExited && current1.type != EventType.Repaint))
        return;
      Grid activeGrid = TileDragAndDropManager.GetActiveGrid();
      if ((Object) activeGrid == (Object) null || DragAndDrop.objectReferences.Length == 0)
        return;
      Vector3 local = GridEditorUtility.ScreenToLocal(activeGrid.transform, current1.mousePosition);
      Vector3Int cell = activeGrid.LocalToCell(local);
      switch (current1.type)
      {
        case EventType.Repaint:
          if (ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData != null)
          {
            Tilemap componentInParent = Selection.activeGameObject.GetComponentInParent<Tilemap>();
            if ((Object) componentInParent != (Object) null)
              componentInParent.ClearAllEditorPreviewTiles();
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            using (Dictionary<Vector2Int, Object>.Enumerator enumerator = ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                KeyValuePair<Vector2Int, Object> current2 = enumerator.Current;
                Vector3Int position = cell + new Vector3Int(current2.Key.x, current2.Key.y, 0);
                if (current2.Value is TileBase)
                {
                  TileBase tile = current2.Value as TileBase;
                  if ((Object) componentInParent != (Object) null)
                    componentInParent.SetEditorPreviewTile(position, tile);
                }
              }
              break;
            }
          }
          else
            break;
        case EventType.DragUpdated:
          DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
          ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData = TileDragAndDrop.CreateHoverData((List<Texture2D>) null, (List<Sprite>) null, TileDragAndDrop.GetValidTiles(DragAndDrop.objectReferences));
          if (ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData.Count > 0)
          {
            Event.current.Use();
            GUI.changed = true;
            break;
          }
          break;
        case EventType.DragPerform:
          if (ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData.Count > 0)
          {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            Dictionary<Vector2Int, TileBase> tileSheet = TileDragAndDrop.ConvertToTileSheet(ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData);
            Tilemap activeTilemap = TileDragAndDropManager.GetOrCreateActiveTilemap();
            activeTilemap.ClearAllEditorPreviewTiles();
            foreach (KeyValuePair<Vector2Int, TileBase> keyValuePair in tileSheet)
            {
              Vector3Int position = new Vector3Int(cell.x + keyValuePair.Key.x, cell.y + keyValuePair.Key.y, 0);
              activeTilemap.SetTile(position, keyValuePair.Value);
            }
            ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData = (Dictionary<Vector2Int, Object>) null;
            GUI.changed = true;
            Event.current.Use();
            break;
          }
          break;
      }
      if (ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData == null || Event.current.type != EventType.DragExited && (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape))
        return;
      if (ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData.Count > 0)
      {
        Tilemap componentInParent = Selection.activeGameObject.GetComponentInParent<Tilemap>();
        if ((Object) componentInParent != (Object) null)
          componentInParent.ClearAllEditorPreviewTiles();
        Event.current.Use();
      }
      ScriptableSingleton<TileDragAndDropManager>.instance.m_HoverData = (Dictionary<Vector2Int, Object>) null;
    }

    private static Tilemap GetOrCreateActiveTilemap()
    {
      if (!((Object) Selection.activeGameObject != (Object) null))
        return (Tilemap) null;
      Tilemap tilemap = Selection.activeGameObject.GetComponentInParent<Tilemap>();
      if ((Object) tilemap == (Object) null)
        tilemap = TileDragAndDropManager.CreateNewTilemap(Selection.activeGameObject.GetComponentInParent<Grid>());
      return tilemap;
    }

    private static Tilemap CreateNewTilemap(Grid grid)
    {
      GameObject gameObject = new GameObject("Tilemap");
      gameObject.transform.SetParent(grid.gameObject.transform);
      Tilemap tilemap = gameObject.AddComponent<Tilemap>();
      gameObject.AddComponent<TilemapRenderer>();
      return tilemap;
    }

    public static Grid GetActiveGrid()
    {
      if ((Object) Selection.activeGameObject != (Object) null)
        return Selection.activeGameObject.GetComponentInParent<Grid>();
      return (Grid) null;
    }
  }
}
