// Decompiled with JetBrains decompiler
// Type: UnityEditor.Callbacks.PostProcessSceneAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Callbacks
{
  /// <summary>
  ///   <para>Add this attribute to a method to get a notification just after building the scene.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class PostProcessSceneAttribute : CallbackOrderAttribute
  {
    private int m_version;

    public PostProcessSceneAttribute()
    {
      this.m_CallbackOrder = 1;
      this.m_version = 0;
    }

    public PostProcessSceneAttribute(int callbackOrder)
    {
      this.m_CallbackOrder = callbackOrder;
      this.m_version = 0;
    }

    public PostProcessSceneAttribute(int callbackOrder, int version)
    {
      this.m_CallbackOrder = callbackOrder;
      this.m_version = version;
    }

    internal int version
    {
      get
      {
        return this.m_version;
      }
    }
  }
}
