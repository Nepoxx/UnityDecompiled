// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorPreviewTilemap
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  [RequiredByNativeCode]
  internal class EditorPreviewTilemap : ITilemap
  {
    private EditorPreviewTilemap()
    {
    }

    public override Sprite GetSprite(Vector3Int position)
    {
      return !(bool) ((Object) this.m_Tilemap.GetEditorPreviewTile(position)) ? this.m_Tilemap.GetSprite(position) : this.m_Tilemap.GetEditorPreviewSprite(position);
    }

    public override Color GetColor(Vector3Int position)
    {
      return !(bool) ((Object) this.m_Tilemap.GetEditorPreviewTile(position)) ? this.m_Tilemap.GetColor(position) : this.m_Tilemap.GetEditorPreviewColor(position);
    }

    public override Matrix4x4 GetTransformMatrix(Vector3Int position)
    {
      return !(bool) ((Object) this.m_Tilemap.GetEditorPreviewTile(position)) ? this.m_Tilemap.GetTransformMatrix(position) : this.m_Tilemap.GetEditorPreviewTransformMatrix(position);
    }

    public override TileFlags GetTileFlags(Vector3Int position)
    {
      return !(bool) ((Object) this.m_Tilemap.GetEditorPreviewTile(position)) ? this.m_Tilemap.GetTileFlags(position) : this.m_Tilemap.GetEditorPreviewTileFlags(position);
    }

    public override TileBase GetTile(Vector3Int position)
    {
      return this.m_Tilemap.GetEditorPreviewTile(position) ?? this.m_Tilemap.GetTile(position);
    }

    public override T GetTile<T>(Vector3Int position)
    {
      T obj = this.m_Tilemap.GetEditorPreviewTile<T>(position);
      if ((object) obj == null)
        obj = this.m_Tilemap.GetTile<T>(position);
      return obj;
    }

    private TileBase CreateInvalidTile()
    {
      Texture2D whiteTexture = Texture2D.whiteTexture;
      Sprite sprite = Sprite.Create(whiteTexture, new Rect(0.0f, 0.0f, (float) whiteTexture.width, (float) whiteTexture.height), new Vector2(0.5f, 0.5f), (float) whiteTexture.width);
      Tile instance = ScriptableObject.CreateInstance<Tile>();
      instance.sprite = sprite;
      instance.color = Random.ColorHSV(0.9444444f, 1f, 0.3f, 0.6f, 0.7f, 1f);
      instance.transform = Matrix4x4.identity;
      instance.flags = TileFlags.LockAll;
      return (TileBase) instance;
    }

    private static ITilemap CreateInstance()
    {
      ITilemap.s_Instance = (ITilemap) new EditorPreviewTilemap();
      return ITilemap.s_Instance;
    }
  }
}
