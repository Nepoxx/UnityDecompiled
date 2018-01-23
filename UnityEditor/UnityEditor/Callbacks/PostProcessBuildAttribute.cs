// Decompiled with JetBrains decompiler
// Type: UnityEditor.Callbacks.PostProcessBuildAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Callbacks
{
  /// <summary>
  ///   <para>Add this attribute to a method to get a notification just after building the player.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class PostProcessBuildAttribute : CallbackOrderAttribute
  {
    public PostProcessBuildAttribute()
    {
      this.m_CallbackOrder = 1;
    }

    public PostProcessBuildAttribute(int callbackOrder)
    {
      this.m_CallbackOrder = callbackOrder;
    }
  }
}
