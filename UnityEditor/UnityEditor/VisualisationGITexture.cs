// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualisationGITexture
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngineInternal;

namespace UnityEditor
{
  internal struct VisualisationGITexture
  {
    public GITextureType type;
    public GITextureAvailability textureAvailability;
    public Texture2D texture;
    public Hash128 contentHash;
  }
}
