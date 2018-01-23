// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.ScrollView
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
  public class ScrollView : VisualElement
  {
    public static readonly Vector2 kDefaultScrollerValues = new Vector2(0.0f, 100f);
    private VisualElement m_ContentContainer;

    public ScrollView()
      : this(ScrollView.kDefaultScrollerValues, ScrollView.kDefaultScrollerValues)
    {
    }

    public ScrollView(Vector2 horizontalScrollerValues, Vector2 verticalScrollerValues)
    {
      this.horizontalScrollerValues = horizontalScrollerValues;
      this.verticalScrollerValues = verticalScrollerValues;
      this.contentViewport = new VisualElement()
      {
        name = "ContentViewport"
      };
      this.contentViewport.clippingOptions = VisualElement.ClippingOptions.ClipContents;
      this.shadow.Add(this.contentViewport);
      this.m_ContentContainer = new VisualElement()
      {
        name = "ContentView"
      };
      this.contentViewport.Add(this.m_ContentContainer);
      Scroller scroller1 = new Scroller(horizontalScrollerValues.x, horizontalScrollerValues.y, (Action<float>) (value => this.scrollOffset = new Vector2(value, this.scrollOffset.y)), Slider.Direction.Horizontal);
      scroller1.name = "HorizontalScroller";
      scroller1.persistenceKey = "HorizontalScroller";
      this.horizontalScroller = scroller1;
      this.shadow.Add((VisualElement) this.horizontalScroller);
      Scroller scroller2 = new Scroller(verticalScrollerValues.x, verticalScrollerValues.y, (Action<float>) (value => this.scrollOffset = new Vector2(this.scrollOffset.x, value)), Slider.Direction.Vertical);
      scroller2.name = "VerticalScroller";
      scroller2.persistenceKey = "VerticalScroller";
      this.verticalScroller = scroller2;
      this.shadow.Add((VisualElement) this.verticalScroller);
      this.RegisterCallback<WheelEvent>(new EventCallback<WheelEvent>(this.OnScrollWheel), Capture.NoCapture);
    }

    public Vector2 horizontalScrollerValues { get; set; }

    public Vector2 verticalScrollerValues { get; set; }

    public bool showHorizontal { get; set; }

    public bool showVertical { get; set; }

    public bool needsHorizontal
    {
      get
      {
        return this.showHorizontal || (double) this.contentContainer.layout.width - (double) this.layout.width > 0.0;
      }
    }

    public bool needsVertical
    {
      get
      {
        return this.showVertical || (double) this.contentContainer.layout.height - (double) this.layout.height > 0.0;
      }
    }

    public Vector2 scrollOffset
    {
      get
      {
        return new Vector2(this.horizontalScroller.value, this.verticalScroller.value);
      }
      set
      {
        if (!(value != this.scrollOffset))
          return;
        this.horizontalScroller.value = value.x;
        this.verticalScroller.value = value.y;
        this.UpdateContentViewTransform();
      }
    }

    private float scrollableWidth
    {
      get
      {
        return this.contentContainer.layout.width - this.contentViewport.layout.width;
      }
    }

    private float scrollableHeight
    {
      get
      {
        return this.contentContainer.layout.height - this.contentViewport.layout.height;
      }
    }

    private void UpdateContentViewTransform()
    {
      Vector3 position = this.contentContainer.transform.position;
      Vector2 scrollOffset = this.scrollOffset;
      position.x = -scrollOffset.x;
      position.y = -scrollOffset.y;
      this.contentContainer.transform.position = position;
      this.Dirty(ChangeType.Repaint);
    }

    public VisualElement contentViewport { get; private set; }

    [Obsolete("Please use contentContainer instead", false)]
    public VisualElement contentView
    {
      get
      {
        return this.contentContainer;
      }
    }

    public Scroller horizontalScroller { get; private set; }

    public Scroller verticalScroller { get; private set; }

    public override VisualElement contentContainer
    {
      get
      {
        return this.m_ContentContainer;
      }
    }

    protected internal override void ExecuteDefaultAction(EventBase evt)
    {
      base.ExecuteDefaultAction(evt);
      if (evt.GetEventTypeId() != EventBase<PostLayoutEvent>.TypeId())
        return;
      this.OnPostLayout(((PostLayoutEvent) evt).hasNewLayout);
    }

    private void OnPostLayout(bool hasNewLayout)
    {
      if (!hasNewLayout)
        return;
      if ((double) this.contentContainer.layout.width > (double) Mathf.Epsilon)
        this.horizontalScroller.Adjust(this.contentViewport.layout.width / this.contentContainer.layout.width);
      if ((double) this.contentContainer.layout.height > (double) Mathf.Epsilon)
        this.verticalScroller.Adjust(this.contentViewport.layout.height / this.contentContainer.layout.height);
      this.horizontalScroller.SetEnabled((double) this.contentContainer.layout.width - (double) this.layout.width > 0.0);
      this.verticalScroller.SetEnabled((double) this.contentContainer.layout.height - (double) this.layout.height > 0.0);
      this.contentViewport.style.positionRight = (StyleValue<float>) (!this.needsVertical ? 0.0f : this.verticalScroller.layout.width);
      this.horizontalScroller.style.positionRight = (StyleValue<float>) (!this.needsVertical ? 0.0f : this.verticalScroller.layout.width);
      this.contentViewport.style.positionBottom = (StyleValue<float>) (!this.needsHorizontal ? 0.0f : this.horizontalScroller.layout.height);
      this.verticalScroller.style.positionBottom = (StyleValue<float>) (!this.needsHorizontal ? 0.0f : this.horizontalScroller.layout.height);
      if (this.needsHorizontal)
      {
        this.horizontalScroller.lowValue = 0.0f;
        this.horizontalScroller.highValue = this.scrollableWidth;
      }
      else
        this.horizontalScroller.value = 0.0f;
      if (this.needsVertical)
      {
        this.verticalScroller.lowValue = 0.0f;
        this.verticalScroller.highValue = this.scrollableHeight;
      }
      else
        this.verticalScroller.value = 0.0f;
      if (this.horizontalScroller.visible != this.needsHorizontal)
      {
        this.horizontalScroller.visible = this.needsHorizontal;
        if (this.needsHorizontal)
          this.contentViewport.AddToClassList("HorizontalScroll");
        else
          this.contentViewport.RemoveFromClassList("HorizontalScroll");
      }
      if (this.verticalScroller.visible != this.needsVertical)
      {
        this.verticalScroller.visible = this.needsVertical;
        if (this.needsVertical)
          this.contentViewport.AddToClassList("VerticalScroll");
        else
          this.contentViewport.RemoveFromClassList("VerticalScroll");
      }
      this.UpdateContentViewTransform();
    }

    private void OnScrollWheel(WheelEvent evt)
    {
      if ((double) this.contentContainer.layout.height - (double) this.layout.height > 0.0)
      {
        if ((double) evt.delta.y < 0.0)
          this.verticalScroller.ScrollPageUp();
        else if ((double) evt.delta.y > 0.0)
          this.verticalScroller.ScrollPageDown();
      }
      evt.StopPropagation();
    }
  }
}
