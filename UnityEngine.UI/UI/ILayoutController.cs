// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ILayoutController
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  public interface ILayoutController
  {
    /// <summary>
    ///   <para>Callback invoked by the auto layout system which handles horizontal aspects of the layout.</para>
    /// </summary>
    void SetLayoutHorizontal();

    /// <summary>
    ///   <para>Callback invoked by the auto layout system which handles vertical aspects of the layout.</para>
    /// </summary>
    void SetLayoutVertical();
  }
}
