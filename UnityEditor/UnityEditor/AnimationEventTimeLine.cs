// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationEventTimeLine
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class AnimationEventTimeLine
  {
    private bool m_DirtyTooltip = false;
    private int m_HoverEvent = -1;
    private string m_InstantTooltipText = (string) null;
    private Vector2 m_InstantTooltipPoint = Vector2.zero;
    [NonSerialized]
    private AnimationEvent[] m_EventsAtMouseDown;
    [NonSerialized]
    private float[] m_EventTimes;

    public AnimationEventTimeLine(EditorWindow owner)
    {
    }

    public void AddEvent(float time, GameObject gameObject, AnimationClip animationClip)
    {
      Selection.activeObject = (UnityEngine.Object) AnimationWindowEvent.CreateAndEdit(gameObject, animationClip, time);
    }

    public void EditEvents(GameObject gameObject, AnimationClip clip, bool[] selectedIndices)
    {
      List<AnimationWindowEvent> animationWindowEventList = new List<AnimationWindowEvent>();
      for (int eventIndex = 0; eventIndex < selectedIndices.Length; ++eventIndex)
      {
        if (selectedIndices[eventIndex])
          animationWindowEventList.Add(AnimationWindowEvent.Edit(gameObject, clip, eventIndex));
      }
      if (animationWindowEventList.Count > 0)
        Selection.objects = (UnityEngine.Object[]) animationWindowEventList.ToArray();
      else
        this.ClearSelection();
    }

    public void EditEvent(GameObject gameObject, AnimationClip clip, int index)
    {
      Selection.activeObject = (UnityEngine.Object) AnimationWindowEvent.Edit(gameObject, clip, index);
    }

    public void ClearSelection()
    {
      if (!(Selection.activeObject is AnimationWindowEvent))
        return;
      Selection.activeObject = (UnityEngine.Object) null;
    }

    public void DeleteEvents(AnimationClip clip, bool[] deleteIndices)
    {
      bool flag = false;
      List<AnimationEvent> animationEventList = new List<AnimationEvent>((IEnumerable<AnimationEvent>) AnimationUtility.GetAnimationEvents(clip));
      for (int index = animationEventList.Count - 1; index >= 0; --index)
      {
        if (deleteIndices[index])
        {
          animationEventList.RemoveAt(index);
          flag = true;
        }
      }
      if (!flag)
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) clip, "Delete Event");
      AnimationUtility.SetAnimationEvents(clip, animationEventList.ToArray());
      Selection.objects = (UnityEngine.Object[]) new AnimationWindowEvent[0];
      this.m_DirtyTooltip = true;
    }

    public void EventLineGUI(Rect rect, AnimationWindowState state)
    {
      if ((UnityEngine.Object) state.selectedItem == (UnityEngine.Object) null)
        return;
      AnimationClip animationClip = state.selectedItem.animationClip;
      GameObject rootGameObject = state.selectedItem.rootGameObject;
      GUI.BeginGroup(rect);
      Color color = GUI.color;
      Rect rect1 = new Rect(0.0f, 0.0f, rect.width, rect.height);
      float time = Mathf.Max((float) Mathf.RoundToInt(state.PixelToTime(Event.current.mousePosition.x, rect) * state.frameRate) / state.frameRate, 0.0f);
      if ((UnityEngine.Object) animationClip != (UnityEngine.Object) null)
      {
        AnimationEvent[] animationEvents = AnimationUtility.GetAnimationEvents(animationClip);
        Texture image = EditorGUIUtility.IconContent("Animation.EventMarker").image;
        Rect[] rectArray = new Rect[animationEvents.Length];
        Rect[] positions = new Rect[animationEvents.Length];
        int num1 = 1;
        int num2 = 0;
        for (int index = 0; index < animationEvents.Length; ++index)
        {
          AnimationEvent animationEvent = animationEvents[index];
          if (num2 == 0)
          {
            num1 = 1;
            while (index + num1 < animationEvents.Length && (double) animationEvents[index + num1].time == (double) animationEvent.time)
              ++num1;
            num2 = num1;
          }
          --num2;
          float num3 = Mathf.Floor(state.FrameToPixel(animationEvent.time * animationClip.frameRate, rect));
          int num4 = 0;
          if (num1 > 1)
            num4 = Mathf.FloorToInt(Mathf.Max(0.0f, (float) Mathf.Min((num1 - 1) * (image.width - 1), (int) ((double) state.FrameDeltaToPixel(rect) - (double) (image.width * 2))) - (float) ((image.width - 1) * num2)));
          Rect rect2 = new Rect(num3 + (float) num4 - (float) (image.width / 2), (rect.height - 10f) * (float) (num2 - num1 + 1) / (float) Mathf.Max(1, num1 - 1), (float) image.width, (float) image.height);
          rectArray[index] = rect2;
          positions[index] = rect2;
        }
        if (this.m_DirtyTooltip)
        {
          if (this.m_HoverEvent >= 0 && this.m_HoverEvent < rectArray.Length)
          {
            this.m_InstantTooltipText = AnimationWindowEventInspector.FormatEvent(rootGameObject, animationEvents[this.m_HoverEvent]);
            this.m_InstantTooltipPoint = new Vector2((float) ((double) rectArray[this.m_HoverEvent].xMin + (double) (int) ((double) rectArray[this.m_HoverEvent].width / 2.0) + (double) rect.x - 30.0), rect.yMax);
          }
          this.m_DirtyTooltip = false;
        }
        bool[] selections = new bool[animationEvents.Length];
        foreach (UnityEngine.Object @object in Selection.objects)
        {
          AnimationWindowEvent animationWindowEvent = @object as AnimationWindowEvent;
          if ((UnityEngine.Object) animationWindowEvent != (UnityEngine.Object) null && animationWindowEvent.eventIndex >= 0 && animationWindowEvent.eventIndex < selections.Length)
            selections[animationWindowEvent.eventIndex] = true;
        }
        Vector2 offset = Vector2.zero;
        int clickedIndex;
        float startSelect;
        float endSelect;
        switch (EditorGUIExt.MultiSelection(rect, positions, new GUIContent(image), rectArray, ref selections, (bool[]) null, out clickedIndex, out offset, out startSelect, out endSelect, GUIStyle.none))
        {
          case HighLevelEvent.None:
            this.CheckRectsOnMouseMove(rect, animationEvents, rectArray);
            if (Event.current.type == EventType.ContextClick && rect1.Contains(Event.current.mousePosition))
            {
              Event.current.Use();
              GenericMenu genericMenu = new GenericMenu();
              AnimationEventTimeLine.EventLineContextMenuObject contextMenuObject = new AnimationEventTimeLine.EventLineContextMenuObject(rootGameObject, animationClip, time, -1, selections);
              int num3 = ((IEnumerable<bool>) selections).Count<bool>((Func<bool, bool>) (selected => selected));
              genericMenu.AddItem(new GUIContent("Add Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuAdd), (object) contextMenuObject);
              if (num3 > 0)
                genericMenu.AddItem(new GUIContent(num3 <= 1 ? "Delete Animation Event" : "Delete Animation Events"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuDelete), (object) contextMenuObject);
              genericMenu.ShowAsContext();
            }
            break;
          case HighLevelEvent.DoubleClick:
            if (clickedIndex != -1)
            {
              this.EditEvents(rootGameObject, animationClip, selections);
              goto default;
            }
            else
            {
              this.EventLineContextMenuAdd((object) new AnimationEventTimeLine.EventLineContextMenuObject(rootGameObject, animationClip, time, -1, selections));
              goto default;
            }
          case HighLevelEvent.ContextClick:
            GenericMenu genericMenu1 = new GenericMenu();
            AnimationEventTimeLine.EventLineContextMenuObject contextMenuObject1 = new AnimationEventTimeLine.EventLineContextMenuObject(rootGameObject, animationClip, animationEvents[clickedIndex].time, clickedIndex, selections);
            int num5 = ((IEnumerable<bool>) selections).Count<bool>((Func<bool, bool>) (selected => selected));
            genericMenu1.AddItem(new GUIContent("Add Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuAdd), (object) contextMenuObject1);
            genericMenu1.AddItem(new GUIContent(num5 <= 1 ? "Delete Animation Event" : "Delete Animation Events"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuDelete), (object) contextMenuObject1);
            genericMenu1.ShowAsContext();
            this.m_InstantTooltipText = (string) null;
            this.m_DirtyTooltip = true;
            state.Repaint();
            goto default;
          case HighLevelEvent.BeginDrag:
            this.m_EventsAtMouseDown = animationEvents;
            this.m_EventTimes = new float[animationEvents.Length];
            for (int index = 0; index < animationEvents.Length; ++index)
              this.m_EventTimes[index] = animationEvents[index].time;
            goto default;
          case HighLevelEvent.Drag:
            for (int index = animationEvents.Length - 1; index >= 0; --index)
            {
              if (selections[index])
              {
                AnimationEvent animationEvent = this.m_EventsAtMouseDown[index];
                animationEvent.time = this.m_EventTimes[index] + offset.x * state.PixelDeltaToTime(rect);
                animationEvent.time = Mathf.Max(0.0f, animationEvent.time);
                animationEvent.time = (float) Mathf.RoundToInt(animationEvent.time * animationClip.frameRate) / animationClip.frameRate;
              }
            }
            int[] numArray1 = new int[selections.Length];
            for (int index = 0; index < numArray1.Length; ++index)
              numArray1[index] = index;
            Array.Sort((Array) this.m_EventsAtMouseDown, (Array) numArray1, (IComparer) new AnimationEventTimeLine.EventComparer());
            bool[] flagArray = (bool[]) selections.Clone();
            float[] numArray2 = (float[]) this.m_EventTimes.Clone();
            for (int index = 0; index < numArray1.Length; ++index)
            {
              selections[index] = flagArray[numArray1[index]];
              this.m_EventTimes[index] = numArray2[numArray1[index]];
            }
            this.EditEvents(rootGameObject, animationClip, selections);
            Undo.RegisterCompleteObjectUndo((UnityEngine.Object) animationClip, "Move Event");
            AnimationUtility.SetAnimationEvents(animationClip, this.m_EventsAtMouseDown);
            this.m_DirtyTooltip = true;
            goto default;
          case HighLevelEvent.Delete:
            this.DeleteEvents(animationClip, selections);
            goto default;
          case HighLevelEvent.SelectionChanged:
            state.ClearKeySelections();
            this.EditEvents(rootGameObject, animationClip, selections);
            goto default;
          default:
            goto case HighLevelEvent.None;
        }
      }
      GUI.color = color;
      GUI.EndGroup();
    }

    public void DrawInstantTooltip(Rect position)
    {
      if (this.m_InstantTooltipText == null || !(this.m_InstantTooltipText != ""))
        return;
      GUIStyle style = (GUIStyle) "AnimationEventTooltip";
      style.contentOffset = new Vector2(0.0f, 0.0f);
      style.overflow = new RectOffset(10, 10, 0, 0);
      Vector2 vector2 = style.CalcSize(new GUIContent(this.m_InstantTooltipText));
      Rect position1 = new Rect(this.m_InstantTooltipPoint.x - vector2.x * 0.5f, this.m_InstantTooltipPoint.y + 24f, vector2.x, vector2.y);
      if ((double) position1.xMax > (double) position.width)
        position1.x = position.width - position1.width;
      GUI.Label(position1, this.m_InstantTooltipText, style);
      position1 = new Rect(this.m_InstantTooltipPoint.x - 33f, this.m_InstantTooltipPoint.y, 7f, 25f);
      GUI.Label(position1, "", (GUIStyle) "AnimationEventTooltipArrow");
    }

    public void EventLineContextMenuAdd(object obj)
    {
      AnimationEventTimeLine.EventLineContextMenuObject contextMenuObject = (AnimationEventTimeLine.EventLineContextMenuObject) obj;
      this.AddEvent(contextMenuObject.m_Time, contextMenuObject.m_Animated, contextMenuObject.m_Clip);
    }

    public void EventLineContextMenuEdit(object obj)
    {
      AnimationEventTimeLine.EventLineContextMenuObject contextMenuObject = (AnimationEventTimeLine.EventLineContextMenuObject) obj;
      if (Array.Exists<bool>(contextMenuObject.m_Selected, (Predicate<bool>) (selected => selected)))
      {
        this.EditEvents(contextMenuObject.m_Animated, contextMenuObject.m_Clip, contextMenuObject.m_Selected);
      }
      else
      {
        if (contextMenuObject.m_Index < 0)
          return;
        this.EditEvent(contextMenuObject.m_Animated, contextMenuObject.m_Clip, contextMenuObject.m_Index);
      }
    }

    public void EventLineContextMenuDelete(object obj)
    {
      AnimationEventTimeLine.EventLineContextMenuObject contextMenuObject = (AnimationEventTimeLine.EventLineContextMenuObject) obj;
      AnimationClip clip = contextMenuObject.m_Clip;
      if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
        return;
      int index = contextMenuObject.m_Index;
      if (Array.Exists<bool>(contextMenuObject.m_Selected, (Predicate<bool>) (selected => selected)))
      {
        this.DeleteEvents(clip, contextMenuObject.m_Selected);
      }
      else
      {
        if (index < 0)
          return;
        bool[] deleteIndices = new bool[contextMenuObject.m_Selected.Length];
        deleteIndices[index] = true;
        this.DeleteEvents(clip, deleteIndices);
      }
    }

    private void CheckRectsOnMouseMove(Rect eventLineRect, AnimationEvent[] events, Rect[] hitRects)
    {
      Vector2 mousePosition = Event.current.mousePosition;
      bool flag = false;
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
              this.m_InstantTooltipPoint = new Vector2(hitRects[this.m_HoverEvent].xMin + (float) (int) ((double) hitRects[this.m_HoverEvent].width / 2.0) + eventLineRect.x, eventLineRect.yMax);
              this.m_DirtyTooltip = true;
            }
          }
        }
      }
      if (flag)
        return;
      this.m_HoverEvent = -1;
      this.m_InstantTooltipText = "";
    }

    public class EventComparer : IComparer
    {
      int IComparer.Compare(object objX, object objY)
      {
        AnimationEvent animationEvent1 = (AnimationEvent) objX;
        AnimationEvent animationEvent2 = (AnimationEvent) objY;
        float time1 = animationEvent1.time;
        float time2 = animationEvent2.time;
        if ((double) time1 != (double) time2)
          return (int) Mathf.Sign(time1 - time2);
        return animationEvent1.GetHashCode() - animationEvent2.GetHashCode();
      }
    }

    private class EventLineContextMenuObject
    {
      public GameObject m_Animated;
      public AnimationClip m_Clip;
      public float m_Time;
      public int m_Index;
      public bool[] m_Selected;

      public EventLineContextMenuObject(GameObject animated, AnimationClip clip, float time, int index, bool[] selected)
      {
        this.m_Animated = animated;
        this.m_Clip = clip;
        this.m_Time = time;
        this.m_Index = index;
        this.m_Selected = selected;
      }
    }
  }
}
