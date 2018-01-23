// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainDetailContextMenus
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TerrainDetailContextMenus
  {
    [MenuItem("CONTEXT/TerrainEngineDetails/Add Grass Texture")]
    internal static void AddDetailTexture(MenuCommand item)
    {
      DetailTextureWizard detailTextureWizard = TerrainWizard.DisplayTerrainWizard<DetailTextureWizard>("Add Grass Texture", "Add");
      detailTextureWizard.m_DetailTexture = (Texture2D) null;
      detailTextureWizard.InitializeDefaults((Terrain) item.context, -1);
    }

    [MenuItem("CONTEXT/TerrainEngineDetails/Add Detail Mesh")]
    internal static void AddDetailMesh(MenuCommand item)
    {
      DetailMeshWizard detailMeshWizard = TerrainWizard.DisplayTerrainWizard<DetailMeshWizard>("Add Detail Mesh", "Add");
      detailMeshWizard.m_Detail = (GameObject) null;
      detailMeshWizard.InitializeDefaults((Terrain) item.context, -1);
    }

    [MenuItem("CONTEXT/TerrainEngineDetails/Edit")]
    internal static void EditDetail(MenuCommand item)
    {
      if (((Terrain) item.context).terrainData.detailPrototypes[item.userData].usePrototypeMesh)
        TerrainWizard.DisplayTerrainWizard<DetailMeshWizard>("Edit Detail Mesh", "Apply").InitializeDefaults((Terrain) item.context, item.userData);
      else
        TerrainWizard.DisplayTerrainWizard<DetailTextureWizard>("Edit Grass Texture", "Apply").InitializeDefaults((Terrain) item.context, item.userData);
    }

    [MenuItem("CONTEXT/TerrainEngineDetails/Edit", true)]
    internal static bool EditDetailCheck(MenuCommand item)
    {
      Terrain context = (Terrain) item.context;
      return item.userData >= 0 && item.userData < context.terrainData.detailPrototypes.Length;
    }

    [MenuItem("CONTEXT/TerrainEngineDetails/Remove")]
    internal static void RemoveDetail(MenuCommand item)
    {
      TerrainEditorUtility.RemoveDetail((Terrain) item.context, item.userData);
    }

    [MenuItem("CONTEXT/TerrainEngineDetails/Remove", true)]
    internal static bool RemoveDetailCheck(MenuCommand item)
    {
      Terrain context = (Terrain) item.context;
      return item.userData >= 0 && item.userData < context.terrainData.detailPrototypes.Length;
    }
  }
}
