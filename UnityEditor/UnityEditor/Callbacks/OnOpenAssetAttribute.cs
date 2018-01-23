// Decompiled with JetBrains decompiler
// Type: UnityEditor.Callbacks.OnOpenAssetAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Callbacks
{
  /// <summary>
  ///   <para>Callback attribute for opening an asset in Unity (e.g the callback is fired when double clicking an asset in the Project Browser).</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class OnOpenAssetAttribute : CallbackOrderAttribute
  {
    public OnOpenAssetAttribute()
    {
      this.m_CallbackOrder = 1;
    }

    public OnOpenAssetAttribute(int callbackOrder)
    {
      this.m_CallbackOrder = callbackOrder;
    }
  }
}
