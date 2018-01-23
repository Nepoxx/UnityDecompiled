// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.VisualContainer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public class VisualContainer : VisualElement
  {
    [Obsolete("VisualContainer.AddChild will be removed. Use VisualElement.Add or VisualElement.shadow.Add instead", false)]
    public void AddChild(VisualElement child)
    {
      this.shadow.Add(child);
    }

    [Obsolete("VisualContainer.InsertChild will be removed. Use VisualElement.Insert or VisualElement.shadow.Insert instead", false)]
    public void InsertChild(int index, VisualElement child)
    {
      this.shadow.Insert(index, child);
    }

    [Obsolete("VisualContainer.RemoveChild will be removed. Use VisualElement.Remove or VisualElement.shadow.Remove instead", false)]
    public void RemoveChild(VisualElement child)
    {
      this.shadow.Remove(child);
    }

    [Obsolete("VisualContainer.RemoveChildAt will be removed. Use VisualElement.RemoveAt or VisualElement.shadow.RemoveAt instead", false)]
    public void RemoveChildAt(int index)
    {
      this.shadow.RemoveAt(index);
    }

    [Obsolete("VisualContainer.ClearChildren will be removed. Use VisualElement.Clear or VisualElement.shadow.Clear instead", false)]
    public void ClearChildren()
    {
      this.shadow.Clear();
    }

    [Obsolete("VisualContainer.GetChildAt will be removed. Use VisualElement.ElementAt or VisualElement.shadow.ElementAt instead", false)]
    public VisualElement GetChildAt(int index)
    {
      return this.shadow[index];
    }
  }
}
