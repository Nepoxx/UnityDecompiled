// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.TemplateContainer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public class TemplateContainer : VisualElement
  {
    public readonly string templateId;
    private VisualElement m_ContentContainer;

    public TemplateContainer(string templateId)
    {
      this.templateId = templateId;
      this.m_ContentContainer = (VisualElement) this;
    }

    public override VisualElement contentContainer
    {
      get
      {
        return this.m_ContentContainer;
      }
    }

    internal void SetContentContainer(VisualElement content)
    {
      this.m_ContentContainer = content;
    }
  }
}
