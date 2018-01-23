// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.TileUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditorInternal
{
  internal class TileUtility
  {
    [MenuItem("Assets/Create/Tile", priority = 357)]
    public static void CreateNewTile()
    {
      string path = EditorUtility.SaveFilePanelInProject("Save tile", "New Tile", "asset", string.Format("Save tile'{0}':", (object) "tile"), ProjectWindowUtil.GetActiveFolderPath());
      if (string.IsNullOrEmpty(path))
        return;
      AssetDatabase.CreateAsset((Object) ScriptableObject.CreateInstance<Tile>(), path);
    }
  }
}
