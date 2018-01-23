// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IClipper
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  public interface IClipper
  {
    /// <summary>
    ///   <para>Called after layout and before Graphic update of the Canvas update loop.</para>
    /// </summary>
    void PerformClipping();
  }
}
