// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.CheckoutMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>What to checkout when starting the Checkout task through the version control Provider.</para>
  /// </summary>
  [System.Flags]
  public enum CheckoutMode
  {
    Asset = 1,
    Meta = 2,
    Both = Meta | Asset, // 0x00000003
    Exact = 4,
  }
}
