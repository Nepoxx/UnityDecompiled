// Decompiled with JetBrains decompiler
// Type: UnityEditor.AndroidBuildSystem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Type of Android build system.</para>
  /// </summary>
  public enum AndroidBuildSystem
  {
    Internal,
    Gradle,
    [Obsolete("ADT/eclipse project export for Android is no longer supported - please use Gradle export instead", true)] ADT,
    VisualStudio,
  }
}
