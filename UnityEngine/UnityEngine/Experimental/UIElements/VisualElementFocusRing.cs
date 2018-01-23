// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.VisualElementFocusRing
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Implementation of a linear focus ring. Elements are sorted according to their focusIndex.</para>
  /// </summary>
  public class VisualElementFocusRing : IFocusRing
  {
    private VisualElement root;
    private List<VisualElementFocusRing.FocusRingRecord> m_FocusRing;

    public VisualElementFocusRing(VisualElement root, VisualElementFocusRing.DefaultFocusOrder dfo = VisualElementFocusRing.DefaultFocusOrder.ChildOrder)
    {
      this.defaultFocusOrder = dfo;
      this.root = root;
      this.m_FocusRing = new List<VisualElementFocusRing.FocusRingRecord>();
    }

    /// <summary>
    ///   <para>The focus order for elements having 0 has a focusIndex.</para>
    /// </summary>
    public VisualElementFocusRing.DefaultFocusOrder defaultFocusOrder { get; set; }

    private int FocusRingSort(VisualElementFocusRing.FocusRingRecord a, VisualElementFocusRing.FocusRingRecord b)
    {
      if (a.m_Focusable.focusIndex == 0 && b.m_Focusable.focusIndex == 0)
      {
        switch (this.defaultFocusOrder)
        {
          case VisualElementFocusRing.DefaultFocusOrder.PositionXY:
            VisualElement focusable1 = a.m_Focusable as VisualElement;
            VisualElement focusable2 = b.m_Focusable as VisualElement;
            if (focusable1 != null && focusable2 != null)
            {
              if ((double) focusable1.layout.position.x < (double) focusable2.layout.position.x)
                return -1;
              if ((double) focusable1.layout.position.x > (double) focusable2.layout.position.x)
                return 1;
              if ((double) focusable1.layout.position.y < (double) focusable2.layout.position.y)
                return -1;
              if ((double) focusable1.layout.position.y > (double) focusable2.layout.position.y)
                return 1;
            }
            return Comparer<int>.Default.Compare(a.m_AutoIndex, b.m_AutoIndex);
          case VisualElementFocusRing.DefaultFocusOrder.PositionYX:
            VisualElement focusable3 = a.m_Focusable as VisualElement;
            VisualElement focusable4 = b.m_Focusable as VisualElement;
            if (focusable3 != null && focusable4 != null)
            {
              if ((double) focusable3.layout.position.y < (double) focusable4.layout.position.y)
                return -1;
              if ((double) focusable3.layout.position.y > (double) focusable4.layout.position.y)
                return 1;
              if ((double) focusable3.layout.position.x < (double) focusable4.layout.position.x)
                return -1;
              if ((double) focusable3.layout.position.x > (double) focusable4.layout.position.x)
                return 1;
            }
            return Comparer<int>.Default.Compare(a.m_AutoIndex, b.m_AutoIndex);
          default:
            return Comparer<int>.Default.Compare(a.m_AutoIndex, b.m_AutoIndex);
        }
      }
      else
      {
        if (a.m_Focusable.focusIndex == 0)
          return 1;
        if (b.m_Focusable.focusIndex == 0)
          return -1;
        return Comparer<int>.Default.Compare(a.m_Focusable.focusIndex, b.m_Focusable.focusIndex);
      }
    }

    private void DoUpdate()
    {
      this.m_FocusRing.Clear();
      if (this.root == null)
        return;
      int focusIndex = 0;
      this.BuildRingRecursive(this.root, ref focusIndex);
      this.m_FocusRing.Sort(new Comparison<VisualElementFocusRing.FocusRingRecord>(this.FocusRingSort));
    }

    private void BuildRingRecursive(VisualElement vc, ref int focusIndex)
    {
      for (int index = 0; index < vc.shadow.childCount; ++index)
      {
        VisualElement vc1 = vc.shadow[index];
        if (vc1.canGrabFocus)
        {
          List<VisualElementFocusRing.FocusRingRecord> focusRing = this.m_FocusRing;
          VisualElementFocusRing.FocusRingRecord focusRingRecord1 = new VisualElementFocusRing.FocusRingRecord();
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          VisualElementFocusRing.FocusRingRecord& local = @focusRingRecord1;
          int num1;
          focusIndex = (num1 = focusIndex) + 1;
          int num2 = num1;
          // ISSUE: explicit reference operation
          (^local).m_AutoIndex = num2;
          focusRingRecord1.m_Focusable = (Focusable) vc1;
          VisualElementFocusRing.FocusRingRecord focusRingRecord2 = focusRingRecord1;
          focusRing.Add(focusRingRecord2);
        }
        this.BuildRingRecursive(vc1, ref focusIndex);
      }
    }

    private int GetFocusableInternalIndex(Focusable f)
    {
      if (f != null)
      {
        for (int index = 0; index < this.m_FocusRing.Count; ++index)
        {
          if (f == this.m_FocusRing[index].m_Focusable)
            return index;
        }
      }
      return -1;
    }

    /// <summary>
    ///   <para>Get the direction of the focus change for the given event. For example, when the Tab key is pressed, focus should be given to the element to the right in the focus ring.</para>
    /// </summary>
    /// <param name="currentFocusable"></param>
    /// <param name="e"></param>
    public FocusChangeDirection GetFocusChangeDirection(Focusable currentFocusable, EventBase e)
    {
      if (currentFocusable is IMGUIContainer && e.imguiEvent != null || e.GetEventTypeId() != EventBase<KeyDownEvent>.TypeId())
        return FocusChangeDirection.none;
      KeyDownEvent keyDownEvent = e as KeyDownEvent;
      EventModifiers modifiers = keyDownEvent.modifiers;
      if (keyDownEvent.keyCode != KeyCode.Tab || currentFocusable == null)
        return FocusChangeDirection.none;
      if ((modifiers & EventModifiers.Shift) == EventModifiers.None)
        return VisualElementFocusChangeDirection.right;
      return VisualElementFocusChangeDirection.left;
    }

    /// <summary>
    ///   <para>Get the next element in the given direction.</para>
    /// </summary>
    /// <param name="currentFocusable"></param>
    /// <param name="direction"></param>
    public Focusable GetNextFocusable(Focusable currentFocusable, FocusChangeDirection direction)
    {
      if (direction == FocusChangeDirection.none || direction == FocusChangeDirection.unspecified)
        return currentFocusable;
      this.DoUpdate();
      if (this.m_FocusRing.Count == 0)
        return (Focusable) null;
      int index = 0;
      if (direction == VisualElementFocusChangeDirection.right)
      {
        index = this.GetFocusableInternalIndex(currentFocusable) + 1;
        if (index == this.m_FocusRing.Count)
          index = 0;
      }
      else if (direction == VisualElementFocusChangeDirection.left)
      {
        index = this.GetFocusableInternalIndex(currentFocusable) - 1;
        if (index == -1)
          index = this.m_FocusRing.Count - 1;
      }
      return this.m_FocusRing[index].m_Focusable;
    }

    /// <summary>
    ///   <para>Ordering of elements in the focus ring.</para>
    /// </summary>
    public enum DefaultFocusOrder
    {
      ChildOrder,
      PositionXY,
      PositionYX,
    }

    private struct FocusRingRecord
    {
      public int m_AutoIndex;
      public Focusable m_Focusable;
    }
  }
}
