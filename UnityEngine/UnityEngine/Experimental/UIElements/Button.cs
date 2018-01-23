// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Button
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public class Button : VisualElement
  {
    public Clickable clickable;

    public Button(Action clickEvent)
    {
      this.clickable = new Clickable(clickEvent);
      this.AddManipulator((IManipulator) this.clickable);
    }
  }
}
