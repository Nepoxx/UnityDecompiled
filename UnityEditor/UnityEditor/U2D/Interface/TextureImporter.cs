// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.Interface.TextureImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.Experimental.U2D;
using UnityEngine;

namespace UnityEditor.U2D.Interface
{
  internal class TextureImporter : ITextureImporter, ISpriteEditorDataProvider
  {
    protected AssetImporter m_AssetImporter;
    private List<SpriteDataMultipleMode> m_SpritesMultiple;
    private SpriteDataSingleMode m_SpriteSingle;
    private SerializedObject m_TextureImporterSO;

    public TextureImporter(UnityEditor.TextureImporter textureImporter)
    {
      this.m_AssetImporter = (AssetImporter) textureImporter;
    }

    public override string assetPath
    {
      get
      {
        return this.m_AssetImporter.assetPath;
      }
    }

    public override bool Equals(object other)
    {
      TextureImporter textureImporter = other as TextureImporter;
      if (object.ReferenceEquals((object) textureImporter, (object) null))
        return (Object) this.m_AssetImporter == (Object) null;
      return (Object) this.m_AssetImporter == (Object) textureImporter.m_AssetImporter;
    }

    public override int GetHashCode()
    {
      return this.m_AssetImporter.GetHashCode();
    }

    public override void GetWidthAndHeight(ref int width, ref int height)
    {
      ((UnityEditor.TextureImporter) this.m_AssetImporter).GetWidthAndHeight(ref width, ref height);
    }

    public override SpriteImportMode spriteImportMode
    {
      get
      {
        return ((UnityEditor.TextureImporter) this.m_AssetImporter).spriteImportMode;
      }
    }

    public override Vector4 spriteBorder
    {
      get
      {
        return ((UnityEditor.TextureImporter) this.m_AssetImporter).spriteBorder;
      }
    }

    public override Vector2 spritePivot
    {
      get
      {
        return ((UnityEditor.TextureImporter) this.m_AssetImporter).spritePivot;
      }
    }

    public void InitSpriteEditorDataProvider(SerializedObject so)
    {
      this.m_TextureImporterSO = so;
      SerializedProperty property = this.m_TextureImporterSO.FindProperty("m_SpriteSheet.m_Sprites");
      this.m_SpritesMultiple = new List<SpriteDataMultipleMode>();
      this.m_SpriteSingle = new SpriteDataSingleMode();
      this.m_SpriteSingle.Load(this.m_TextureImporterSO);
      for (int index = 0; index < property.arraySize; ++index)
      {
        SpriteDataMultipleMode dataMultipleMode = new SpriteDataMultipleMode();
        SerializedProperty arrayElementAtIndex = property.GetArrayElementAtIndex(index);
        dataMultipleMode.Load(arrayElementAtIndex);
        this.m_SpritesMultiple.Add(dataMultipleMode);
      }
    }

    public int spriteDataCount
    {
      get
      {
        switch (this.spriteImportMode)
        {
          case SpriteImportMode.Single:
          case SpriteImportMode.Polygon:
            return 1;
          case SpriteImportMode.Multiple:
            return this.m_SpritesMultiple.Count;
          default:
            return 0;
        }
      }
      set
      {
        if (this.spriteImportMode != SpriteImportMode.Multiple)
        {
          Debug.LogError((object) "SetSpriteDataSize can only be called when in SpriteImportMode.Multiple");
        }
        else
        {
          while (this.m_SpritesMultiple.Count < value)
            this.m_SpritesMultiple.Add(new SpriteDataMultipleMode());
          if (this.m_SpritesMultiple.Count <= value)
            return;
          int count = this.m_SpritesMultiple.Count - value;
          this.m_SpritesMultiple.RemoveRange(this.m_SpritesMultiple.Count - count, count);
        }
      }
    }

    public Object targetObject
    {
      get
      {
        return (Object) this.m_AssetImporter;
      }
    }

    public SpriteDataBase GetSpriteData(int i)
    {
      switch (this.spriteImportMode)
      {
        case SpriteImportMode.Single:
        case SpriteImportMode.Polygon:
          return (SpriteDataBase) this.m_SpriteSingle;
        case SpriteImportMode.Multiple:
          if (this.m_SpritesMultiple.Count > i)
            return (SpriteDataBase) this.m_SpritesMultiple[i];
          break;
      }
      return (SpriteDataBase) null;
    }

    public void Apply(SerializedObject so)
    {
      this.m_SpriteSingle.Apply(so);
      SerializedProperty property = so.FindProperty("m_SpriteSheet.m_Sprites");
      for (int index = 0; index < this.m_SpritesMultiple.Count; ++index)
      {
        if (property.arraySize < this.m_SpritesMultiple.Count)
          property.InsertArrayElementAtIndex(property.arraySize);
        SerializedProperty arrayElementAtIndex = property.GetArrayElementAtIndex(index);
        this.m_SpritesMultiple[index].Apply(arrayElementAtIndex);
      }
      while (this.m_SpritesMultiple.Count < property.arraySize)
        property.DeleteArrayElementAtIndex(this.m_SpritesMultiple.Count);
    }

    public void GetTextureActualWidthAndHeight(out int width, out int height)
    {
      width = height = 0;
      ((UnityEditor.TextureImporter) this.m_AssetImporter).GetWidthAndHeight(ref width, ref height);
    }
  }
}
