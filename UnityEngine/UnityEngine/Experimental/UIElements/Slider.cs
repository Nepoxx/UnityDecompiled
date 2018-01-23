// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Slider
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
  public class Slider : VisualElement
  {
    private float m_LowValue;
    private float m_HighValue;
    private Rect m_DragElementStartPos;
    private Slider.SliderValue m_SliderValue;
    private Slider.Direction m_Direction;

    public Slider(float start, float end, Action<float> valueChanged, Slider.Direction direction = Slider.Direction.Horizontal, float pageSize = 10f)
    {
      // ISSUE: reference to a compiler-generated field
      this.valueChanged = valueChanged;
      this.direction = direction;
      this.pageSize = pageSize;
      this.lowValue = start;
      this.highValue = end;
      this.Add(new VisualElement() { name = "TrackElement" });
      this.dragElement = new VisualElement()
      {
        name = "DragElement"
      };
      this.Add(this.dragElement);
      this.clampedDragger = new ClampedDragger(this, new Action(this.SetSliderValueFromClick), new Action(this.SetSliderValueFromDrag));
      this.AddManipulator((IManipulator) this.clampedDragger);
    }

    public VisualElement dragElement { get; private set; }

    public float lowValue
    {
      get
      {
        return this.m_LowValue;
      }
      set
      {
        if (Mathf.Approximately(this.m_LowValue, value))
          return;
        this.m_LowValue = value;
        this.ClampValue();
      }
    }

    public float highValue
    {
      get
      {
        return this.m_HighValue;
      }
      set
      {
        if (Mathf.Approximately(this.m_HighValue, value))
          return;
        this.m_HighValue = value;
        this.ClampValue();
      }
    }

    public float range
    {
      get
      {
        return this.highValue - this.lowValue;
      }
    }

    public float pageSize { get; set; }

    public event Action<float> valueChanged;

    internal ClampedDragger clampedDragger { get; private set; }

    public float value
    {
      get
      {
        return this.m_SliderValue != null ? this.m_SliderValue.m_Value : 0.0f;
      }
      set
      {
        if (this.m_SliderValue == null)
          this.m_SliderValue = new Slider.SliderValue()
          {
            m_Value = this.lowValue
          };
        float b = Mathf.Clamp(value, this.lowValue, this.highValue);
        if (Mathf.Approximately(this.m_SliderValue.m_Value, b))
          return;
        this.m_SliderValue.m_Value = b;
        this.UpdateDragElementPosition();
        if (this.valueChanged != null)
          this.valueChanged(this.m_SliderValue.m_Value);
        this.Dirty(ChangeType.Repaint);
        this.SavePersistentData();
      }
    }

    public Slider.Direction direction
    {
      get
      {
        return this.m_Direction;
      }
      set
      {
        this.m_Direction = value;
        if (this.m_Direction == Slider.Direction.Horizontal)
        {
          this.RemoveFromClassList("vertical");
          this.AddToClassList("horizontal");
        }
        else
        {
          this.RemoveFromClassList("horizontal");
          this.AddToClassList("vertical");
        }
      }
    }

    private void ClampValue()
    {
      this.value = this.value;
    }

    /// <summary>
    ///   <para>Called when the persistent data is accessible and/or when the data or persistence key have changed (VisualElement is properly parented).</para>
    /// </summary>
    public override void OnPersistentDataReady()
    {
      base.OnPersistentDataReady();
      this.m_SliderValue = this.GetOrCreatePersistentData<Slider.SliderValue>((object) this.m_SliderValue, this.GetFullHierarchicalPersistenceKey());
    }

    private void SetSliderValueFromDrag()
    {
      if (this.clampedDragger.dragDirection != ClampedDragger.DragDirection.Free)
        return;
      Vector2 delta = this.clampedDragger.delta;
      if (this.direction == Slider.Direction.Horizontal)
        this.ComputeValueAndDirectionFromDrag(this.layout.width, (float) this.dragElement.style.width, this.m_DragElementStartPos.x + delta.x);
      else
        this.ComputeValueAndDirectionFromDrag(this.layout.height, (float) this.dragElement.style.height, this.m_DragElementStartPos.y + delta.y);
    }

    private void ComputeValueAndDirectionFromDrag(float sliderLength, float dragElementLength, float dragElementPos)
    {
      float num = sliderLength - dragElementLength;
      if ((double) Mathf.Abs(num) < (double) Mathf.Epsilon)
        return;
      this.value = Mathf.Max(0.0f, Mathf.Min(dragElementPos, num)) / num * this.range + this.lowValue;
    }

    private void SetSliderValueFromClick()
    {
      if (this.clampedDragger.dragDirection == ClampedDragger.DragDirection.Free)
        return;
      if (this.clampedDragger.dragDirection == ClampedDragger.DragDirection.None)
      {
        if ((double) this.pageSize == 0.0)
        {
          float x = this.direction != Slider.Direction.Horizontal ? this.dragElement.style.positionLeft.value : this.clampedDragger.startMousePosition.x - (float) this.dragElement.style.width / 2f;
          float y = this.direction != Slider.Direction.Horizontal ? this.clampedDragger.startMousePosition.y - (float) this.dragElement.style.height / 2f : this.dragElement.style.positionTop.value;
          this.dragElement.style.positionLeft = (StyleValue<float>) x;
          this.dragElement.style.positionTop = (StyleValue<float>) y;
          this.m_DragElementStartPos = new Rect(x, y, (float) this.dragElement.style.width, (float) this.dragElement.style.height);
          this.clampedDragger.dragDirection = ClampedDragger.DragDirection.Free;
          if (this.direction == Slider.Direction.Horizontal)
          {
            this.ComputeValueAndDirectionFromDrag(this.layout.width, (float) this.dragElement.style.width, this.m_DragElementStartPos.x);
            return;
          }
          this.ComputeValueAndDirectionFromDrag(this.layout.height, (float) this.dragElement.style.height, this.m_DragElementStartPos.y);
          return;
        }
        this.m_DragElementStartPos = new Rect((float) this.dragElement.style.positionLeft, (float) this.dragElement.style.positionTop, (float) this.dragElement.style.width, (float) this.dragElement.style.height);
      }
      if (this.direction == Slider.Direction.Horizontal)
        this.ComputeValueAndDirectionFromClick(this.layout.width, (float) this.dragElement.style.width, (float) this.dragElement.style.positionLeft, this.clampedDragger.lastMousePosition.x);
      else
        this.ComputeValueAndDirectionFromClick(this.layout.height, (float) this.dragElement.style.height, (float) this.dragElement.style.positionTop, this.clampedDragger.lastMousePosition.y);
    }

    private void ComputeValueAndDirectionFromClick(float sliderLength, float dragElementLength, float dragElementPos, float dragElementLastPos)
    {
      float num = sliderLength - dragElementLength;
      if ((double) Mathf.Abs(num) < (double) Mathf.Epsilon)
        return;
      if ((double) dragElementLastPos < (double) dragElementPos && this.clampedDragger.dragDirection != ClampedDragger.DragDirection.LowToHigh)
      {
        this.clampedDragger.dragDirection = ClampedDragger.DragDirection.HighToLow;
        this.value = Mathf.Max(0.0f, Mathf.Min(dragElementPos - this.pageSize, num)) / num * this.range + this.lowValue;
      }
      else
      {
        if ((double) dragElementLastPos <= (double) dragElementPos + (double) dragElementLength || this.clampedDragger.dragDirection == ClampedDragger.DragDirection.HighToLow)
          return;
        this.clampedDragger.dragDirection = ClampedDragger.DragDirection.LowToHigh;
        this.value = Mathf.Max(0.0f, Mathf.Min(dragElementPos + this.pageSize, num)) / num * this.range + this.lowValue;
      }
    }

    public void AdjustDragElement(float factor)
    {
      bool flag = (double) factor < 1.0;
      this.dragElement.visible = flag;
      if (!flag)
        return;
      IStyle style = this.dragElement.style;
      this.dragElement.visible = true;
      if (this.direction == Slider.Direction.Horizontal)
      {
        float specifiedValueOrDefault = style.minWidth.GetSpecifiedValueOrDefault(0.0f);
        style.width = (StyleValue<float>) Mathf.Max(this.layout.width * factor, specifiedValueOrDefault);
      }
      else
      {
        float specifiedValueOrDefault = style.minHeight.GetSpecifiedValueOrDefault(0.0f);
        style.height = (StyleValue<float>) Mathf.Max(this.layout.height * factor, specifiedValueOrDefault);
      }
    }

    private void UpdateDragElementPosition()
    {
      if (this.panel == null)
        return;
      float num1 = this.value - this.lowValue;
      float width = (float) this.dragElement.style.width;
      float height = (float) this.dragElement.style.height;
      if (this.direction == Slider.Direction.Horizontal)
      {
        float num2 = this.layout.width - width;
        this.dragElement.style.positionLeft = (StyleValue<float>) (num1 / this.range * num2);
      }
      else
      {
        float num2 = this.layout.height - height;
        this.dragElement.style.positionTop = (StyleValue<float>) (num1 / this.range * num2);
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
      this.UpdateDragElementPosition();
    }

    public enum Direction
    {
      Horizontal,
      Vertical,
    }

    [Serializable]
    private class SliderValue
    {
      public float m_Value = 0.0f;
    }
  }
}
