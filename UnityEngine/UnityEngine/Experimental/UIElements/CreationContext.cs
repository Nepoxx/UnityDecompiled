// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.CreationContext
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>This class is used during UXML template instantiation.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct CreationContext
  {
    public static readonly CreationContext Default = new CreationContext();

    internal CreationContext(Dictionary<string, VisualElement> slotInsertionPoints, VisualTreeAsset vta, VisualElement target)
    {
      this.target = target;
      this.slotInsertionPoints = slotInsertionPoints;
      this.visualTreeAsset = vta;
    }

    public VisualElement target { get; }

    public VisualTreeAsset visualTreeAsset { get; }

    public Dictionary<string, VisualElement> slotInsertionPoints { get; }
  }
}
