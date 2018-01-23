// Decompiled with JetBrains decompiler
// Type: UnityEditor.TileDragAndDrop
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  internal static class TileDragAndDrop
  {
    private static readonly string k_TileExtension = "asset";

    public static List<Sprite> GetSpritesFromTexture(Texture2D texture)
    {
      UnityEngine.Object[] objectArray = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) texture));
      List<Sprite> spriteList = new List<Sprite>();
      foreach (UnityEngine.Object @object in objectArray)
      {
        if (@object is Sprite)
          spriteList.Add(@object as Sprite);
      }
      return spriteList;
    }

    public static bool AllSpritesAreSameSize(List<Sprite> sprites)
    {
      if (!sprites.Any<Sprite>())
        return false;
      for (int index = 1; index < sprites.Count - 1; ++index)
      {
        if ((int) sprites[index].rect.width != (int) sprites[index + 1].rect.width || (int) sprites[index].rect.height != (int) sprites[index + 1].rect.height)
          return false;
      }
      return true;
    }

    public static Dictionary<Vector2Int, UnityEngine.Object> CreateHoverData(List<Texture2D> sheetTextures, List<Sprite> singleSprites, List<TileBase> tiles)
    {
      Dictionary<Vector2Int, UnityEngine.Object> dictionary = new Dictionary<Vector2Int, UnityEngine.Object>();
      Vector2Int key = new Vector2Int(0, 0);
      int val2 = 0;
      if (sheetTextures != null)
      {
        foreach (Texture2D sheetTexture in sheetTextures)
        {
          Dictionary<Vector2Int, UnityEngine.Object> hoverData = TileDragAndDrop.CreateHoverData(sheetTexture);
          foreach (KeyValuePair<Vector2Int, UnityEngine.Object> keyValuePair in hoverData)
            dictionary.Add(keyValuePair.Key + key, keyValuePair.Value);
          Vector2Int min = TileDragAndDrop.GetMinMaxRect(hoverData.Keys.ToList<Vector2Int>()).min;
          key += new Vector2Int(0, min.y - 1);
        }
      }
      if (key.x > 0)
        key = new Vector2Int(0, key.y - 1);
      if (singleSprites != null)
      {
        val2 = Mathf.FloorToInt(Mathf.Sqrt((float) singleSprites.Count));
        foreach (Sprite singleSprite in singleSprites)
        {
          dictionary.Add(key, (UnityEngine.Object) singleSprite);
          key += new Vector2Int(1, 0);
          if (key.x > val2)
            key = new Vector2Int(0, key.y - 1);
        }
      }
      if (key.x > 0)
        key = new Vector2Int(0, key.y - 1);
      if (tiles != null)
      {
        int num = Math.Max(Mathf.FloorToInt(Mathf.Sqrt((float) tiles.Count)), val2);
        foreach (TileBase tile in tiles)
        {
          dictionary.Add(key, (UnityEngine.Object) tile);
          key += new Vector2Int(1, 0);
          if (key.x > num)
            key = new Vector2Int(0, key.y - 1);
        }
      }
      return dictionary;
    }

    public static List<Texture2D> GetValidSpritesheets(UnityEngine.Object[] objects)
    {
      List<Texture2D> texture2DList = new List<Texture2D>();
      foreach (UnityEngine.Object @object in objects)
      {
        if (@object is Texture2D)
        {
          Texture2D texture = @object as Texture2D;
          List<Sprite> spritesFromTexture = TileDragAndDrop.GetSpritesFromTexture(texture);
          if (spritesFromTexture.Count<Sprite>() > 1 && TileDragAndDrop.AllSpritesAreSameSize(spritesFromTexture))
            texture2DList.Add(texture);
        }
      }
      return texture2DList;
    }

    public static List<Sprite> GetValidSingleSprites(UnityEngine.Object[] objects)
    {
      List<Sprite> spriteList = new List<Sprite>();
      foreach (UnityEngine.Object @object in objects)
      {
        if (@object is Sprite)
          spriteList.Add(@object as Sprite);
        else if (@object is Texture2D)
        {
          List<Sprite> spritesFromTexture = TileDragAndDrop.GetSpritesFromTexture(@object as Texture2D);
          if (spritesFromTexture.Count == 1 || !TileDragAndDrop.AllSpritesAreSameSize(spritesFromTexture))
            spriteList.AddRange((IEnumerable<Sprite>) spritesFromTexture);
        }
      }
      return spriteList;
    }

    public static List<TileBase> GetValidTiles(UnityEngine.Object[] objects)
    {
      List<TileBase> tileBaseList = new List<TileBase>();
      foreach (UnityEngine.Object @object in objects)
      {
        if (@object is TileBase)
          tileBaseList.Add(@object as TileBase);
      }
      return tileBaseList;
    }

    public static Dictionary<Vector2Int, UnityEngine.Object> CreateHoverData(Texture2D sheet)
    {
      Dictionary<Vector2Int, UnityEngine.Object> dictionary = new Dictionary<Vector2Int, UnityEngine.Object>();
      List<Sprite> spritesFromTexture = TileDragAndDrop.GetSpritesFromTexture(sheet);
      Vector2Int cellPixelSize = TileDragAndDrop.EstimateGridPixelSize(spritesFromTexture);
      foreach (Sprite sprite in spritesFromTexture)
      {
        Vector2Int gridPosition = TileDragAndDrop.GetGridPosition(sprite, cellPixelSize);
        dictionary[gridPosition] = (UnityEngine.Object) sprite;
      }
      return dictionary;
    }

    public static Dictionary<Vector2Int, TileBase> ConvertToTileSheet(Dictionary<Vector2Int, UnityEngine.Object> sheet)
    {
      Dictionary<Vector2Int, TileBase> dictionary = new Dictionary<Vector2Int, TileBase>();
      string str1 = !(bool) ((UnityEngine.Object) ProjectBrowser.s_LastInteractedProjectBrowser) ? "Assets" : ProjectBrowser.s_LastInteractedProjectBrowser.GetActiveFolderPath();
      if (sheet.Values.ToList<UnityEngine.Object>().FindAll((Predicate<UnityEngine.Object>) (obj => obj is TileBase)).Count == sheet.Values.Count)
      {
        foreach (KeyValuePair<Vector2Int, UnityEngine.Object> keyValuePair in sheet)
          dictionary.Add(keyValuePair.Key, keyValuePair.Value as TileBase);
        return dictionary;
      }
      TileDragAndDrop.UserTileCreationMode tileCreationMode = TileDragAndDrop.UserTileCreationMode.Overwrite;
      bool flag1 = sheet.Count > 1;
      string str2;
      if (flag1)
      {
        bool flag2 = false;
        str2 = FileUtil.GetProjectRelativePath(EditorUtility.SaveFolderPanel("Generate tiles into folder ", str1, ""));
        foreach (UnityEngine.Object @object in sheet.Values)
        {
          if (@object is Sprite)
          {
            if (File.Exists(FileUtil.CombinePaths(str2, string.Format("{0}.{1}", (object) @object.name, (object) TileDragAndDrop.k_TileExtension))))
            {
              flag2 = true;
              break;
            }
          }
        }
        if (flag2)
        {
          switch (EditorUtility.DisplayDialogComplex("Overwrite?", string.Format("Assets exist at {0}. Do you wish to overwrite existing assets?", (object) str2), "Overwrite", "Create New Copy", "Reuse"))
          {
            case 0:
              tileCreationMode = TileDragAndDrop.UserTileCreationMode.Overwrite;
              break;
            case 1:
              tileCreationMode = TileDragAndDrop.UserTileCreationMode.CreateUnique;
              break;
            case 2:
              tileCreationMode = TileDragAndDrop.UserTileCreationMode.Reuse;
              break;
          }
        }
      }
      else
        str2 = EditorUtility.SaveFilePanelInProject("Generate new tile", sheet.Values.First<UnityEngine.Object>().name, TileDragAndDrop.k_TileExtension, "Generate new tile", str1);
      if (string.IsNullOrEmpty(str2))
        return dictionary;
      int num = 0;
      EditorUtility.DisplayProgressBar("Generating Tile Assets (" + (object) num + "/" + (object) sheet.Count + ")", "Generating tiles", 0.0f);
      foreach (KeyValuePair<Vector2Int, UnityEngine.Object> keyValuePair in sheet)
      {
        string str3 = "";
        TileBase tileBase;
        if (keyValuePair.Value is Sprite)
        {
          tileBase = (TileBase) TileDragAndDrop.CreateTile(keyValuePair.Value as Sprite);
          string str4;
          if (flag1)
            str4 = FileUtil.CombinePaths(str2, string.Format("{0}.{1}", (object) tileBase.name, (object) TileDragAndDrop.k_TileExtension));
          else
            str4 = str2;
          str3 = str4;
          switch (tileCreationMode)
          {
            case TileDragAndDrop.UserTileCreationMode.Overwrite:
              AssetDatabase.CreateAsset((UnityEngine.Object) tileBase, str3);
              break;
            case TileDragAndDrop.UserTileCreationMode.CreateUnique:
              if (File.Exists(str3))
                str3 = AssetDatabase.GenerateUniqueAssetPath(str3);
              AssetDatabase.CreateAsset((UnityEngine.Object) tileBase, str3);
              break;
            case TileDragAndDrop.UserTileCreationMode.Reuse:
              if (File.Exists(str3))
              {
                tileBase = AssetDatabase.LoadAssetAtPath<TileBase>(str3);
                break;
              }
              AssetDatabase.CreateAsset((UnityEngine.Object) tileBase, str3);
              break;
          }
        }
        else
          tileBase = keyValuePair.Value as TileBase;
        EditorUtility.DisplayProgressBar("Generating Tile Assets (" + (object) num + "/" + (object) sheet.Count + ")", "Generating " + str3, (float) num++ / (float) sheet.Count);
        dictionary.Add(keyValuePair.Key, tileBase);
      }
      EditorUtility.ClearProgressBar();
      AssetDatabase.Refresh();
      return dictionary;
    }

    public static Vector2Int EstimateGridPixelSize(List<Sprite> sprites)
    {
      if (!sprites.Any<Sprite>())
        return new Vector2Int(0, 0);
      if (sprites.Count == 1)
        return Vector2Int.FloorToInt(sprites[0].rect.size);
      Vector2 min = TileDragAndDrop.GetMin(sprites, new Vector2(float.MinValue, float.MinValue));
      Vector2Int vector2Int = Vector2Int.FloorToInt(TileDragAndDrop.GetMin(sprites, min) - min);
      vector2Int.x = Math.Max(Mathf.FloorToInt(sprites[0].rect.width), vector2Int.x);
      vector2Int.y = Math.Max(Mathf.FloorToInt(sprites[0].rect.height), vector2Int.y);
      return vector2Int;
    }

    private static Vector2 GetMin(List<Sprite> sprites, Vector2 biggerThan)
    {
      List<Sprite> all1 = sprites.FindAll((Predicate<Sprite>) (sprite => (double) sprite.rect.xMin > (double) biggerThan.x));
      List<Sprite> all2 = sprites.FindAll((Predicate<Sprite>) (sprite => (double) sprite.texture.height - (double) sprite.rect.yMax > (double) biggerThan.y));
      return new Vector2(all1.Count <= 0 ? 0.0f : all1.Min<Sprite>((Func<Sprite, float>) (s => s.rect.xMin)), all2.Count <= 0 ? 0.0f : all2.Min<Sprite>((Func<Sprite, float>) (s => (float) s.texture.height - s.rect.yMax)));
    }

    public static Vector2Int GetGridPosition(Sprite sprite, Vector2Int cellPixelSize)
    {
      return new Vector2Int(Mathf.FloorToInt(sprite.rect.center.x / (float) cellPixelSize.x), Mathf.FloorToInt((float) -((double) sprite.texture.height - (double) sprite.rect.center.y) / (float) cellPixelSize.y) + 1);
    }

    public static RectInt GetMinMaxRect(List<Vector2Int> positions)
    {
      if (positions == null || positions.Count == 0)
        return new RectInt();
      return GridEditorUtility.GetMarqueeRect(new Vector2Int(positions.Min<Vector2Int>((Func<Vector2Int, int>) (p1 => p1.x)), positions.Min<Vector2Int>((Func<Vector2Int, int>) (p1 => p1.y))), new Vector2Int(positions.Max<Vector2Int>((Func<Vector2Int, int>) (p1 => p1.x)), positions.Max<Vector2Int>((Func<Vector2Int, int>) (p1 => p1.y))));
    }

    public static Tile CreateTile(Sprite sprite)
    {
      Tile instance = ScriptableObject.CreateInstance<Tile>();
      instance.name = sprite.name;
      instance.sprite = sprite;
      instance.color = Color.white;
      return instance;
    }

    private enum UserTileCreationMode
    {
      Overwrite,
      CreateUnique,
      Reuse,
    }
  }
}
