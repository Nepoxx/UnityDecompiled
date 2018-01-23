// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Scroller
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
  public class Scroller : VisualElement
  {
    public Scroller(float lowValue, float highValue, Action<float> valueChanged, Slider.Direction direction = Slider.Direction.Vertical)
    {
      this.direction = direction;
      // ISSUE: reference to a compiler-generated field
      this.valueChanged = valueChanged;
      Slider slider = new Slider(lowValue, highValue, new Action<float>(this.OnSliderValueChange), direction, 10f);
      slider.name = "Slider";
      slider.persistenceKey = "Slider";
      this.slider = slider;
      this.Add((VisualElement) this.slider);
      ScrollerButton scrollerButton1 = new ScrollerButton(new Action(this.ScrollPageUp), 250L, 30L);
      scrollerButton1.name = "LowButton";
      this.lowButton = scrollerButton1;
      this.Add((VisualElement) this.lowButton);
      ScrollerButton scrollerButton2 = new ScrollerButton(new Action(this.ScrollPageDown), 250L, 30L);
      scrollerButton2.name = "HighButton";
      this.highButton = scrollerButton2;
      this.Add((VisualElement) this.highButton);
    }

    public event Action<float> valueChanged;

    public Slider slider { get; private set; }

    public ScrollerButton lowButton { get; private set; }

    public ScrollerButton highButton { get; private set; }

    public float value
    {
      get
      {
        return this.slider.value;
      }
      set
      {
        this.slider.value = value;
      }
    }

    public float lowValue
    {
      get
      {
        return this.slider.lowValue;
      }
      set
      {
        this.slider.lowValue = value;
      }
    }

    public float highValue
    {
      get
      {
        return this.slider.highValue;
      }
      set
      {
        this.slider.highValue = value;
      }
    }

    public Slider.Direction direction
    {
      get
      {
        return (FlexDirection) this.style.flexDirection != FlexDirection.Row ? Slider.Direction.Vertical : Slider.Direction.Horizontal;
      }
      set
      {
        if (value == Slider.Direction.Horizontal)
        {
          this.style.flexDirection = (StyleValue<FlexDirection>) FlexDirection.Row;
          this.AddToClassList("horizontal");
        }
        else
        {
          this.style.flexDirection = (StyleValue<FlexDirection>) FlexDirection.Column;
          this.AddToClassList("vertical");
        }
      }
    }

    public void Adjust(float factor)
    {
      this.SetEnabled((double) factor < 1.0);
      this.slider.AdjustDragElement(factor);
    }

    private void OnSliderValueChange(float newValue)
    {
      this.value = newValue;
      // ISSUE: reference to a compiler-generated field
      if (this.valueChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.valueChanged(this.slider.value);
      }
      this.Dirty(ChangeType.Repaint);
    }

    public void ScrollPageUp()
    {
      this.value -= this.slider.pageSize * ((double) this.slider.lowValue >= (double) this.slider.highValue ? -1f : 1f);
    }

    public void ScrollPageDown()
    {
      this.value += this.slider.pageSize * ((double) this.slider.lowValue >= (double) this.slider.highValue ? -1f : 1f);
    }
  }
}
