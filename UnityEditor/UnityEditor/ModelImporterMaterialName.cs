// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterMaterialName
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Material naming options for ModelImporter.</para>
  /// </summary>
  [NativeType(Header = "Editor/Src/AssetPipeline/ModelImporting/ModelImporter.h")]
  public enum ModelImporterMaterialName
  {
    BasedOnTextureName,
    BasedOnMaterialName,
    BasedOnModelNameAndMaterialName,
    [Obsolete("You should use ModelImporterMaterialName.BasedOnTextureName instead, because it it less complicated and behaves in more consistent way.")] BasedOnTextureName_Or_ModelNameAndMaterialName,
  }
}
