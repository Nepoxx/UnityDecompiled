// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterGenerateMaterials
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Material generation options for ModelImporter.</para>
  /// </summary>
  [Obsolete("Use ModelImporterMaterialName, ModelImporter.materialName and ModelImporter.importMaterials instead")]
  public enum ModelImporterGenerateMaterials
  {
    [Obsolete("Use ModelImporter.importMaterials=false instead")] None,
    [Obsolete("Use ModelImporter.importMaterials=true and ModelImporter.materialName=ModelImporterMaterialName.BasedOnTextureName instead")] PerTexture,
    [Obsolete("Use ModelImporter.importMaterials=true and ModelImporter.materialName=ModelImporterMaterialName.BasedOnModelNameAndMaterialName instead")] PerSourceMaterial,
  }
}
