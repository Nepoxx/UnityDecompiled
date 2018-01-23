// Decompiled with JetBrains decompiler
// Type: UnityEditor.EventManipulationHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class EventManipulationHandler
  {
    private Rect[] m_EventRects = new Rect[0];
    private int m_HoverEvent = -1;
    private string m_InstantTooltipText = (string) null;
    private Vector2 m_InstantTooltipPoint = Vector2.zero;
    private readonly GUIContent m_DeleteAnimationEventText = new GUIContent("Delete Animation Event");
    private readonly GUIContent m_DeleteAnimationEventsText = new GUIContent("Delete Animation Events");
    private static AnimationEvent[] m_EventsAtMouseDown;
    private static float[] m_EventTimes;
    private bool[] m_EventsSelected;
    private AnimationWindowEvent[] m_Events;
    private TimeArea m_Timeline;

    public EventManipulationHandler(TimeArea timeArea)
    {
      this.m_Timeline = timeArea;
    }

    public void SelectEvent(AnimationEvent[] events, int index, AnimationClipInfoProperties clipInfo)
    {
      this.m_EventsSelected = new bool[events.Length];
      this.m_EventsSelected[index] = true;
      this.EditEvents(clipInfo, this.m_EventsSelected);
    }

    public bool HandleEventManipulation(Rect rect, ref AnimationEvent[] events, AnimationClipInfoProperties clipInfo)
    {
      Texture image = EditorGUIUtility.IconContent("Animation.EventMarker").image;
      bool flag = false;
      Rect[] rectArray = new Rect[events.Length];
      Rect[] positions = new Rect[events.Length];
      int num1 = 1;
      int num2 = 0;
      for (int index = 0; index < events.Length; ++index)
      {
        AnimationEvent animationEvent = events[index];
        if (num2 == 0)
        {
          num1 = 1;
          while (index + num1 < events.Length && (double) events[index + num1].time == (double) animationEvent.time)
            ++num1;
          num2 = num1;
        }
        --num2;
        float num3 = Mathf.Floor(this.m_Timeline.TimeToPixel(animationEvent.time, rect));
        int num4 = 0;
        if (num1 > 1)
          num4 = Mathf.FloorToInt(Mathf.Max(0.0f, (float) Mathf.Min((num1 - 1) * (image.width - 1), (int) (1.0 / (double) this.m_Timeline.PixelDeltaToTime(rect) - (double) (image.width * 2))) - (float) ((image.width - 1) * num2)));
        Rect rect1 = new Rect(num3 + (float) num4 - (float) (image.width / 2), (rect.height - 10f) * (float) (num2 - num1 + 1) / (float) Mathf.Max(1, num1 - 1), (float) image.width, (float) image.height);
        rectArray[index] = rect1;
        positions[index] = rect1;
      }
      this.m_EventRects = new Rect[rectArray.Length];
      for (int index = 0; index < rectArray.Length; ++index)
        this.m_EventRects[index] = new Rect(rectArray[index].x + rect.x, rectArray[index].y + rect.y, rectArray[index].width, rectArray[index].height);
      if (this.m_EventsSelected == null || this.m_EventsSelected.Length != events.Length || this.m_EventsSelected.Length == 0)
      {
        this.m_EventsSelected = new bool[events.Length];
        this.m_Events = (AnimationWindowEvent[]) null;
      }
      Vector2 offset = Vector2.zero;
      int clickedIndex;
      float startSelect;
      float endSelect;
      switch (EditorGUIExt.MultiSelection(rect, positions, new GUIContent(image), rectArray, ref this.m_EventsSelected, (bool[]) null, out clickedIndex, out offset, out startSelect, out endSelect, GUIStyle.none))
      {
        case HighLevelEvent.None:
          if (Event.current.type == EventType.ContextClick && rect.Contains(Event.current.mousePosition))
          {
            Event.current.Use();
            int num3 = ((IEnumerable<bool>) this.m_EventsSelected).Count<bool>((Func<bool, bool>) (selected => selected));
            float time = Mathf.Max(this.m_Timeline.PixelToTime(Event.current.mousePosition.x, rect), 0.0f);
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuAdd), (object) new EventManipulationHandler.EventModificationContextMenuObject(clipInfo, time, -1, this.m_EventsSelected));
            if (num3 > 0)
              genericMenu.AddItem(num3 <= 1 ? this.m_DeleteAnimationEventText : this.m_DeleteAnimationEventsText, false, new GenericMenu.MenuFunction2(this.EventLineContextMenuDelete), (object) new EventManipulationHandler.EventModificationContextMenuObject(clipInfo, time, -1, this.m_EventsSelected));
            genericMenu.ShowAsContext();
            this.m_InstantTooltipText = (string) null;
          }
          this.CheckRectsOnMouseMove(rect, events, rectArray);
          return flag;
        case HighLevelEvent.ContextClick:
          int num5 = ((IEnumerable<bool>) this.m_EventsSelected).Count<bool>((Func<bool, bool>) (selected => selected));
          GenericMenu genericMenu1 = new GenericMenu();
          genericMenu1.AddItem(new GUIContent("Add Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuAdd), (object) new EventManipulationHandler.EventModificationContextMenuObject(clipInfo, events[clickedIndex].time, clickedIndex, this.m_EventsSelected));
          genericMenu1.AddItem(num5 <= 1 ? this.m_DeleteAnimationEventText : this.m_DeleteAnimationEventsText, false, new GenericMenu.MenuFunction2(this.EventLineContextMenuDelete), (object) new EventManipulationHandler.EventModificationContextMenuObject(clipInfo, events[clickedIndex].time, clickedIndex, this.m_EventsSelected));
          genericMenu1.ShowAsContext();
          this.m_InstantTooltipText = (string) null;
          goto default;
        case HighLevelEvent.BeginDrag:
          EventManipulationHandler.m_EventsAtMouseDown = events;
          EventManipulationHandler.m_EventTimes = new float[events.Length];
          for (int index = 0; index < events.Length; ++index)
            EventManipulationHandler.m_EventTimes[index] = events[index].time;
          goto default;
        case HighLevelEvent.Drag:
          for (int index = events.Length - 1; index >= 0; --index)
          {
            if (this.m_EventsSelected[index])
              EventManipulationHandler.m_EventsAtMouseDown[index].time = Mathf.Clamp01(EventManipulationHandler.m_EventTimes[index] + offset.x / rect.width);
          }
          int[] numArray1 = new int[this.m_EventsSelected.Length];
          for (int index = 0; index < numArray1.Length; ++index)
            numArray1[index] = index;
          Array.Sort((Array) EventManipulationHandler.m_EventsAtMouseDown, (Array) numArray1, (IComparer) new AnimationEventTimeLine.EventComparer());
          bool[] flagArray = (bool[]) this.m_EventsSelected.Clone();
          float[] numArray2 = (float[]) EventManipulationHandler.m_EventTimes.Clone();
          for (int index = 0; index < numArray1.Length; ++index)
          {
            this.m_EventsSelected[index] = flagArray[numArray1[index]];
            EventManipulationHandler.m_EventTimes[index] = numArray2[numArray1[index]];
          }
          events = EventManipulationHandler.m_EventsAtMouseDown;
          flag = true;
          goto default;
        case HighLevelEvent.Delete:
          flag = this.DeleteEvents(ref events, this.m_EventsSelected);
          goto default;
        case HighLevelEvent.SelectionChanged:
          this.EditEvents(clipInfo, this.m_EventsSelected);
          goto default;
        default:
          goto case HighLevelEvent.None;
      }
    }

    public void EventLineContextMenuAdd(object obj)
    {
      EventManipulationHandler.EventModificationContextMenuObject contextMenuObject = (EventManipulationHandler.EventModificationContextMenuObject) obj;
      contextMenuObject.m_Info.AddEvent(contextMenuObject.m_Time);
      this.SelectEvent(contextMenuObject.m_Info.GetEvents(), contextMenuObject.m_Info.GetEventCount() - 1, contextMenuObject.m_Info);
    }

    public void EventLineContextMenuDelete(object obj)
    {
      EventManipulationHandler.EventModificationContextMenuObject contextMenuObject = (EventManipulationHandler.EventModificationContextMenuObject) obj;
      if (Array.Exists<bool>(contextMenuObject.m_Selected, (Predicate<bool>) (selected => selected)))
      {
        for (int index = contextMenuObject.m_Selected.Length - 1; index >= 0; --index)
        {
          if (contextMenuObject.m_Selected[index])
            contextMenuObject.m_Info.RemoveEvent(index);
        }
      }
      else
      {
        if (contextMenuObject.m_Index < 0)
          return;
        contextMenuObject.m_Info.RemoveEvent(contextMenuObject.m_Index);
      }
    }

    private void CheckRectsOnMouseMove(Rect eventLineRect, AnimationEvent[] events, Rect[] hitRects)
    {
      Vector2 mousePosition = Event.current.mousePosition;
      bool flag = false;
      this.m_InstantTooltipText = "";
      if (events.Length == hitRects.Length)
      {
        for (int index = hitRects.Length - 1; index >= 0; --index)
        {
          if (hitRects[index].Contains(mousePosition))
          {
            flag = true;
            if (this.m_HoverEvent != index)
            {
              this.m_HoverEvent = index;
              this.m_InstantTooltipText = events[this.m_HoverEvent].functionName;
              this.m_InstantTooltipPoint = new Vector2(mousePosition.x, mousePosition.y);
            }
          }
        }
      }
      if (flag)
        return;
      this.m_HoverEvent = -1;
    }

    public void Draw(Rect window)
    {
      ++EditorGUI.indentLevel;
      if (this.m_Events != null && this.m_Events.Length > 0)
        AnimationWindowEventInspector.OnEditAnimationEvents(this.m_Events);
      else
        AnimationWindowEventInspector.OnDisabledAnimationEvent();
      --EditorGUI.indentLevel;
      if (this.m_InstantTooltipText == null || !(this.m_InstantTooltipText != ""))
        return;
      GUIStyle style = (GUIStyle) "AnimationEventTooltip";
      Vector2 vector2 = style.CalcSize(new GUIContent(this.m_InstantTooltipText));
      Rect position = new Rect(window.x + this.m_InstantTooltipPoint.x, window.y + this.m_InstantTooltipPoint.y, vector2.x, vector2.y);
      if ((double) position.xMax > (double) window.width)
        position.x = window.width - position.width;
      GUI.Label(position, this.m_InstantTooltipText, style);
    }

    public bool DeleteEvents(ref AnimationEvent[] eventList, bool[] deleteIndices)
    {
      bool flag = false;
      for (int index = eventList.Length - 1; index >= 0; --index)
      {
        if (deleteIndices[index])
        {
          ArrayUtility.RemoveAt<AnimationEvent>(ref eventList, index);
          flag = true;
        }
      }
      if (flag)
      {
        this.m_EventsSelected = new bool[eventList.Length];
        this.m_Events = (AnimationWindowEvent[]) null;
      }
      return flag;
    }

    public void EditEvents(AnimationClipInfoProperties clipInfo, bool[] selectedIndices)
    {
      List<AnimationWindowEvent> animationWindowEventList = new List<AnimationWindowEvent>();
      for (int eventIndex = 0; eventIndex < selectedIndices.Length; ++eventIndex)
      {
        if (selectedIndices[eventIndex])
          animationWindowEventList.Add(AnimationWindowEvent.Edit(clipInfo, eventIndex));
      }
      this.m_Events = animationWindowEventList.ToArray();
    }

    public void UpdateEvents(AnimationClipInfoProperties clipInfo)
    {
      if (this.m_Events == null)
        return;
      foreach (AnimationWindowEvent animationWindowEvent in this.m_Events)
        animationWindowEvent.clipInfo = clipInfo;
    }

    private class EventModificationContextMenuObject
    {
      public AnimationClipInfoProperties m_Info;
      public float m_Time;
      public int m_Index;
      public bool[] m_Selected;

      public EventModificationContextMenuObject(AnimationClipInfoProperties info, float time, int index, bool[] selected)
      {
        this.m_Info = info;
        this.m_Time = time;
        this.m_Index = index;
        this.m_Selected = selected;
      }
    }
  }
}
