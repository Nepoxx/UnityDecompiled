// Decompiled with JetBrains decompiler
// Type: UnityEditor.FlattenHeightmap
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class FlattenHeightmap : TerrainWizard
  {
    public float height = 0.0f;

    internal override void OnWizardUpdate()
    {
      if (!(bool) ((Object) this.terrainData))
        return;
      this.helpString = ((double) this.height).ToString() + " meters (" + (object) (float) ((double) this.height / (double) this.terrainData.size.y * 100.0) + "%)";
    }

    private void OnWizardCreate()
    {
      Undo.RegisterCompleteObjectUndo((Object) this.terrainData, "Flatten Heightmap");
      HeightmapFilters.Flatten(this.terrainData, this.height / this.terrainData.size.y);
    }
  }
}
