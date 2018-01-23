// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.TextureUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.IO;
using UnityEngine;

namespace UnityEditor.Collaboration
{
  internal static class TextureUtility
  {
    public static Texture2D LoadTextureFromApplicationContents(string path)
    {
      Texture2D tex = new Texture2D(2, 2);
      path = Path.Combine(Path.Combine(Path.Combine(Path.Combine(EditorApplication.applicationContentsPath, "Resources"), "Collab"), "overlays"), path);
      try
      {
        FileStream fileStream = File.OpenRead(path);
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, (int) fileStream.Length);
        if (!tex.LoadImage(numArray))
          return (Texture2D) null;
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) ("Collab Overlay Texture load fail, path: " + path));
        return (Texture2D) null;
      }
      return tex;
    }
  }
}
