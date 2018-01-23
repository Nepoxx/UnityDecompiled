// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.CoreFactories
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  internal static class CoreFactories
  {
    internal static void RegisterAll()
    {
      // ISSUE: reference to a compiler-generated field
      if (CoreFactories.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CoreFactories.\u003C\u003Ef__mg\u0024cache0 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateButton);
      }
      // ISSUE: reference to a compiler-generated field
      Factories.RegisterFactory<Button>(CoreFactories.\u003C\u003Ef__mg\u0024cache0);
      // ISSUE: reference to a compiler-generated field
      if (CoreFactories.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CoreFactories.\u003C\u003Ef__mg\u0024cache1 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateIMGUIContainer);
      }
      // ISSUE: reference to a compiler-generated field
      Factories.RegisterFactory<IMGUIContainer>(CoreFactories.\u003C\u003Ef__mg\u0024cache1);
      Factories.RegisterFactory<Image>((Func<IUxmlAttributes, CreationContext, VisualElement>) ((_, __) => (VisualElement) new Image()));
      Factories.RegisterFactory<Label>((Func<IUxmlAttributes, CreationContext, VisualElement>) ((_, __) => (VisualElement) new Label()));
      // ISSUE: reference to a compiler-generated field
      if (CoreFactories.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CoreFactories.\u003C\u003Ef__mg\u0024cache2 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateRepeatButton);
      }
      // ISSUE: reference to a compiler-generated field
      Factories.RegisterFactory<RepeatButton>(CoreFactories.\u003C\u003Ef__mg\u0024cache2);
      // ISSUE: reference to a compiler-generated field
      if (CoreFactories.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CoreFactories.\u003C\u003Ef__mg\u0024cache3 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateScrollerButton);
      }
      // ISSUE: reference to a compiler-generated field
      Factories.RegisterFactory<ScrollerButton>(CoreFactories.\u003C\u003Ef__mg\u0024cache3);
      Factories.RegisterFactory<ScrollView>((Func<IUxmlAttributes, CreationContext, VisualElement>) ((_, __) => (VisualElement) new ScrollView()));
      // ISSUE: reference to a compiler-generated field
      if (CoreFactories.\u003C\u003Ef__mg\u0024cache4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CoreFactories.\u003C\u003Ef__mg\u0024cache4 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateScroller);
      }
      // ISSUE: reference to a compiler-generated field
      Factories.RegisterFactory<Scroller>(CoreFactories.\u003C\u003Ef__mg\u0024cache4);
      // ISSUE: reference to a compiler-generated field
      if (CoreFactories.\u003C\u003Ef__mg\u0024cache5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CoreFactories.\u003C\u003Ef__mg\u0024cache5 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateSlider);
      }
      // ISSUE: reference to a compiler-generated field
      Factories.RegisterFactory<Slider>(CoreFactories.\u003C\u003Ef__mg\u0024cache5);
      Factories.RegisterFactory<TextField>((Func<IUxmlAttributes, CreationContext, VisualElement>) ((_, __) => (VisualElement) new TextField()));
      // ISSUE: reference to a compiler-generated field
      if (CoreFactories.\u003C\u003Ef__mg\u0024cache6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CoreFactories.\u003C\u003Ef__mg\u0024cache6 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateToggle);
      }
      // ISSUE: reference to a compiler-generated field
      Factories.RegisterFactory<Toggle>(CoreFactories.\u003C\u003Ef__mg\u0024cache6);
      Factories.RegisterFactory<VisualContainer>((Func<IUxmlAttributes, CreationContext, VisualElement>) ((_, __) => (VisualElement) new VisualContainer()));
      Factories.RegisterFactory<VisualElement>((Func<IUxmlAttributes, CreationContext, VisualElement>) ((_, __) => new VisualElement()));
      // ISSUE: reference to a compiler-generated field
      if (CoreFactories.\u003C\u003Ef__mg\u0024cache7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CoreFactories.\u003C\u003Ef__mg\u0024cache7 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateTemplate);
      }
      // ISSUE: reference to a compiler-generated field
      Factories.RegisterFactory<TemplateContainer>(CoreFactories.\u003C\u003Ef__mg\u0024cache7);
    }

    private static VisualElement CreateButton(IUxmlAttributes bag, CreationContext ctx)
    {
      return (VisualElement) new Button((Action) null);
    }

    private static VisualElement CreateTemplate(IUxmlAttributes bag, CreationContext ctx)
    {
      string templateAlias = ((TemplateAsset) bag).templateAlias;
      VisualTreeAsset visualTreeAsset = ctx.visualTreeAsset.ResolveUsing(templateAlias);
      TemplateContainer templateContainer = new TemplateContainer(templateAlias);
      if ((UnityEngine.Object) visualTreeAsset == (UnityEngine.Object) null)
        templateContainer.Add((VisualElement) new Label(string.Format("Unknown Element: '{0}'", (object) templateAlias)));
      else
        visualTreeAsset.CloneTree((VisualElement) templateContainer, ctx.slotInsertionPoints);
      if ((UnityEngine.Object) visualTreeAsset == (UnityEngine.Object) null)
        Debug.LogErrorFormat("Could not resolve template with alias '{0}'", (object) templateAlias);
      return (VisualElement) templateContainer;
    }

    private static VisualElement CreateIMGUIContainer(IUxmlAttributes bag, CreationContext ctx)
    {
      return (VisualElement) new IMGUIContainer((Action) null);
    }

    private static VisualElement CreateRepeatButton(IUxmlAttributes bag, CreationContext ctx)
    {
      return (VisualElement) new RepeatButton((Action) null, bag.GetPropertyLong("delay", 0L), bag.GetPropertyLong("interval", 0L));
    }

    private static VisualElement CreateScrollerButton(IUxmlAttributes bag, CreationContext ctx)
    {
      return (VisualElement) new ScrollerButton((Action) null, bag.GetPropertyLong("delay", 0L), bag.GetPropertyLong("interval", 0L));
    }

    private static VisualElement CreateScroller(IUxmlAttributes bag, CreationContext ctx)
    {
      return (VisualElement) new Scroller(bag.GetPropertyFloat("lowValue", 0.0f), bag.GetPropertyFloat("highValue", 0.0f), (Action<float>) null, bag.GetPropertyEnum<Slider.Direction>("direction", Slider.Direction.Horizontal));
    }

    private static VisualElement CreateSlider(IUxmlAttributes bag, CreationContext ctx)
    {
      return (VisualElement) new Slider(bag.GetPropertyFloat("lowValue", 0.0f), bag.GetPropertyFloat("highValue", 0.0f), (Action<float>) null, bag.GetPropertyEnum<Slider.Direction>("direction", Slider.Direction.Horizontal), 10f);
    }

    private static VisualElement CreateToggle(IUxmlAttributes bag, CreationContext ctx)
    {
      return (VisualElement) new Toggle((Action) null);
    }
  }
}
