// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.AssetImporters.ImportedObject
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Experimental.AssetImporters
{
  internal class ImportedObject
  {
    public bool mainAssetObject { get; set; }

    public Object obj { get; set; }

    public string localIdentifier { get; set; }

    public Texture2D thumbnail { get; set; }
  }
}
