// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.Interface.IAssetDatabase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.U2D.Interface
{
  internal interface IAssetDatabase
  {
    string GetAssetPath(Object o);

    ITextureImporter GetAssetImporterFromPath(string path);
  }
}
