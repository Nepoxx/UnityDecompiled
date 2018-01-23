// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterTangentSpaceMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Tangent space generation options for ModelImporter.</para>
  /// </summary>
  public enum ModelImporterTangentSpaceMode
  {
    [Obsolete("Use ModelImporterNormals.Import instead")] Import,
    [Obsolete("Use ModelImporterNormals.Calculate instead")] Calculate,
    [Obsolete("Use ModelImporterNormals.None instead")] None,
  }
}
