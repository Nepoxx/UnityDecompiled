// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.ChangeType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Enum which describes the various types of changes that can occur on a VisualElement.</para>
  /// </summary>
  [Flags]
  public enum ChangeType
  {
    PersistentData = 64, // 0x00000040
    PersistentDataPath = 32, // 0x00000020
    Layout = 16, // 0x00000010
    Styles = 8,
    Transform = 4,
    StylesPath = 2,
    Repaint = 1,
    All = Repaint | StylesPath | Transform | Styles | Layout | PersistentDataPath | PersistentData, // 0x0000007F
  }
}
