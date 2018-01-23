// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IMask
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;

namespace UnityEngine.UI
{
  [Obsolete("Not supported anymore.", true)]
  public interface IMask
  {
    /// <summary>
    ///   <para>Is the mask enabled.</para>
    /// </summary>
    bool Enabled();

    /// <summary>
    ///   <para>Return the RectTransform associated with this mask.</para>
    /// </summary>
    RectTransform rectTransform { get; }
  }
}
