// Decompiled with JetBrains decompiler
// Type: UnityEditor.AndroidETC2Fallback
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>This enumeration has values for different qualities to decompress ETC2 textures on Android devices that don't support the ETC2 texture format.</para>
  /// </summary>
  public enum AndroidETC2Fallback
  {
    Quality32Bit,
    Quality16Bit,
    Quality32BitDownscaled,
  }
}
