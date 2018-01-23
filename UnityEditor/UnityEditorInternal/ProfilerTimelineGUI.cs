// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerTimelineGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class ProfilerTimelineGUI
  {
    private float animationTime = 1f;
    private double lastScrollUpdate = 0.0;
    private ProfilerTimelineGUI.SelectedEntryInfo m_SelectedEntry = new ProfilerTimelineGUI.SelectedEntryInfo();
    private float m_SelectedThreadY = 0.0f;
    private const float kSmallWidth = 7f;
    private const float kTextFadeStartWidth = 50f;
    private const float kTextFadeOutWidth = 20f;
    private const float kTextLongWidth = 200f;
    private const float kLineHeight = 16f;
    private const float kGroupHeight = 20f;
    private List<ProfilerTimelineGUI.GroupInfo> groups;
    private static ProfilerTimelineGUI.Styles ms_Styles;
    [NonSerialized]
    private ZoomableArea m_TimeArea;
    private IProfilerWindowController m_Window;
    private string m_LocalizedString_Total;
    private string m_LocalizedString_Instances;

    public ProfilerTimelineGUI(IProfilerWindowController window)
    {
      this.m_Window = window;
      this.groups = new List<ProfilerTimelineGUI.GroupInfo>((IEnumerable<ProfilerTimelineGUI.GroupInfo>) new ProfilerTimelineGUI.GroupInfo[3]
      {
        new ProfilerTimelineGUI.GroupInfo()
        {
          name = "",
          height = 20f,
          expanded = true,
          threads = new List<ProfilerTimelineGUI.ThreadInfo>()
        },
        new ProfilerTimelineGUI.GroupInfo()
        {
          name = "Unity Job System",
          height = 20f,
          expanded = true,
          threads = new List<ProfilerTimelineGUI.ThreadInfo>()
        },
        new ProfilerTimelineGUI.GroupInfo()
        {
          name = "Loading",
          height = 20f,
          expanded = false,
          threads = new List<ProfilerTimelineGUI.ThreadInfo>()
        }
      });
      this.m_LocalizedString_Total = LocalizationDatabase.GetLocalizedString("Total");
      this.m_LocalizedString_Instances = LocalizationDatabase.GetLocalizedString("Instances");
    }

    private static ProfilerTimelineGUI.Styles styles
    {
      get
      {
        return ProfilerTimelineGUI.ms_Styles ?? (ProfilerTimelineGUI.ms_Styles = new ProfilerTimelineGUI.Styles());
      }
    }

    private void CalculateBars(Rect r, int frameIndex, float time)
    {
      ProfilerFrameDataIterator frameDataIterator = new ProfilerFrameDataIterator();
      float num1 = 0.0f;
      frameDataIterator.SetRoot(frameIndex, 0);
      int threadCount = frameDataIterator.GetThreadCount(frameIndex);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ProfilerTimelineGUI.\u003CCalculateBars\u003Ec__AnonStorey1 barsCAnonStorey1 = new ProfilerTimelineGUI.\u003CCalculateBars\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (barsCAnonStorey1.i = 0; barsCAnonStorey1.i < threadCount; ++barsCAnonStorey1.i)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ProfilerTimelineGUI.\u003CCalculateBars\u003Ec__AnonStorey0 barsCAnonStorey0 = new ProfilerTimelineGUI.\u003CCalculateBars\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        barsCAnonStorey0.\u003C\u003Ef__ref\u00241 = barsCAnonStorey1;
        // ISSUE: reference to a compiler-generated field
        frameDataIterator.SetRoot(frameIndex, barsCAnonStorey1.i);
        // ISSUE: reference to a compiler-generated field
        barsCAnonStorey0.groupname = frameDataIterator.GetGroupName();
        // ISSUE: reference to a compiler-generated method
        ProfilerTimelineGUI.GroupInfo groupInfo = this.groups.Find(new Predicate<ProfilerTimelineGUI.GroupInfo>(barsCAnonStorey0.\u003C\u003Em__0));
        if (groupInfo == null)
        {
          groupInfo = new ProfilerTimelineGUI.GroupInfo();
          // ISSUE: reference to a compiler-generated field
          groupInfo.name = barsCAnonStorey0.groupname;
          groupInfo.height = 20f;
          groupInfo.expanded = false;
          groupInfo.threads = new List<ProfilerTimelineGUI.ThreadInfo>();
          this.groups.Add(groupInfo);
        }
        // ISSUE: reference to a compiler-generated method
        ProfilerTimelineGUI.ThreadInfo threadInfo1 = groupInfo.threads.Find(new Predicate<ProfilerTimelineGUI.ThreadInfo>(barsCAnonStorey0.\u003C\u003Em__1));
        if (threadInfo1 == null)
        {
          threadInfo1 = new ProfilerTimelineGUI.ThreadInfo();
          threadInfo1.name = frameDataIterator.GetThreadName();
          threadInfo1.height = 0.0f;
          ProfilerTimelineGUI.ThreadInfo threadInfo2 = threadInfo1;
          ProfilerTimelineGUI.ThreadInfo threadInfo3 = threadInfo1;
          int num2 = !groupInfo.expanded ? 0 : 1;
          double num3;
          float num4 = (float) (num3 = (double) num2);
          threadInfo3.desiredWeight = (float) num3;
          double num5 = (double) num4;
          threadInfo2.weight = (float) num5;
          // ISSUE: reference to a compiler-generated field
          threadInfo1.threadIndex = barsCAnonStorey1.i;
          groupInfo.threads.Add(threadInfo1);
        }
        if ((double) threadInfo1.weight != (double) threadInfo1.desiredWeight)
          threadInfo1.weight = (float) ((double) threadInfo1.desiredWeight * (double) time + (1.0 - (double) threadInfo1.desiredWeight) * (1.0 - (double) time));
        num1 += threadInfo1.weight;
      }
      float num6 = 20f * (float) this.groups.Count<ProfilerTimelineGUI.GroupInfo>((Func<ProfilerTimelineGUI.GroupInfo, bool>) (group => group.threads.Count > 1));
      float num7 = (r.height - num6) / (num1 + 1f);
      foreach (ProfilerTimelineGUI.GroupInfo group in this.groups)
      {
        foreach (ProfilerTimelineGUI.ThreadInfo thread in group.threads)
          thread.height = num7 * thread.weight;
      }
      this.groups[0].expanded = true;
      this.groups[0].height = 0.0f;
      this.groups[0].threads[0].height = 2f * num7;
    }

    private void UpdateAnimatedFoldout()
    {
      this.animationTime = Math.Min(1f, this.animationTime + (float) (EditorApplication.timeSinceStartup - this.lastScrollUpdate));
      this.m_Window.Repaint();
      if ((double) this.animationTime != 1.0)
        return;
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.UpdateAnimatedFoldout);
    }

    private bool DrawBar(Rect r, float y, float height, string name, bool group, bool expanded, bool indent)
    {
      Rect position1 = new Rect(r.x - 180f, y, 180f, height);
      Rect position2 = new Rect(r.x, y, r.width, height);
      if (Event.current.type == EventType.Repaint)
      {
        ProfilerTimelineGUI.styles.rightPane.Draw(position2, false, false, false, false);
        bool flag = (double) height < 25.0;
        GUIContent content = GUIContent.Temp(name);
        if (flag)
          ProfilerTimelineGUI.styles.leftPane.padding.top -= (int) (25.0 - (double) height) / 2;
        if (indent)
          ProfilerTimelineGUI.styles.leftPane.padding.left += 10;
        ProfilerTimelineGUI.styles.leftPane.Draw(position1, content, false, false, false, false);
        if (indent)
          ProfilerTimelineGUI.styles.leftPane.padding.left -= 10;
        if (flag)
          ProfilerTimelineGUI.styles.leftPane.padding.top += (int) (25.0 - (double) height) / 2;
      }
      if (!group)
        return false;
      --position1.width;
      ++position1.xMin;
      return GUI.Toggle(position1, expanded, GUIContent.none, ProfilerTimelineGUI.styles.foldout);
    }

    private void DrawBars(Rect r, int frameIndex)
    {
      float y = r.y;
      foreach (ProfilerTimelineGUI.GroupInfo group in this.groups)
      {
        bool flag = group.name == "";
        if (!flag)
        {
          float height = group.height;
          bool expanded = group.expanded;
          group.expanded = this.DrawBar(r, y, height, group.name, true, expanded, false);
          if (group.expanded != expanded)
          {
            this.animationTime = 0.0f;
            this.lastScrollUpdate = EditorApplication.timeSinceStartup;
            EditorApplication.update += new EditorApplication.CallbackFunction(this.UpdateAnimatedFoldout);
            foreach (ProfilerTimelineGUI.ThreadInfo thread in group.threads)
              thread.desiredWeight = !group.expanded ? 0.0f : 1f;
          }
          y += height;
        }
        foreach (ProfilerTimelineGUI.ThreadInfo thread in group.threads)
        {
          float height = thread.height;
          if ((double) height != 0.0)
            this.DrawBar(r, y, height, thread.name, false, true, !flag);
          y += height;
        }
      }
    }

    private void DrawGrid(Rect r, int threadCount, float frameTime)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      float num = 16.66667f;
      if ((double) frameTime > 1000.0)
        num = 100f;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(1);
      GL.Color(new Color(1f, 1f, 1f, 0.2f));
      float time1 = num;
      while ((double) time1 <= (double) frameTime)
      {
        float pixel = this.m_TimeArea.TimeToPixel(time1, r);
        GL.Vertex3(pixel, r.y, 0.0f);
        GL.Vertex3(pixel, r.y + r.height, 0.0f);
        time1 += num;
      }
      GL.Color(new Color(1f, 1f, 1f, 0.8f));
      float pixel1 = this.m_TimeArea.TimeToPixel(0.0f, r);
      GL.Vertex3(pixel1, r.y, 0.0f);
      GL.Vertex3(pixel1, r.y + r.height, 0.0f);
      float pixel2 = this.m_TimeArea.TimeToPixel(frameTime, r);
      GL.Vertex3(pixel2, r.y, 0.0f);
      GL.Vertex3(pixel2, r.y + r.height, 0.0f);
      GL.End();
      GUI.color = new Color(1f, 1f, 1f, 0.4f);
      float time2 = 0.0f;
      while ((double) time2 <= (double) frameTime)
      {
        Chart.DoLabel(this.m_TimeArea.TimeToPixel(time2, r) + 2f, r.yMax - 12f, string.Format("{0:f1}ms", (object) time2), 0.0f);
        time2 += num;
      }
      GUI.color = new Color(1f, 1f, 1f, 1f);
      float time3 = frameTime;
      Chart.DoLabel(this.m_TimeArea.TimeToPixel(time3, r) + 2f, r.yMax - 12f, string.Format("{0:f1}ms ({1:f0}FPS)", (object) time3, (object) (float) (1000.0 / (double) time3)), 0.0f);
    }

    private void DoNativeProfilerTimeline(Rect r, int frameIndex, int threadIndex, float timeOffset, bool ghost)
    {
      Rect position = r;
      float topMargin = Math.Min(position.height * 0.25f, 1f);
      float num = topMargin + 1f;
      position.y += topMargin;
      position.height -= num;
      GUI.BeginGroup(position);
      Rect threadRect = position;
      threadRect.x = 0.0f;
      threadRect.y = 0.0f;
      if (Event.current.type == EventType.Repaint)
        this.DrawNativeProfilerTimeline(threadRect, frameIndex, threadIndex, timeOffset, ghost);
      else if (Event.current.type == EventType.MouseDown && !ghost)
        this.HandleNativeProfilerTimelineInput(threadRect, frameIndex, threadIndex, timeOffset, topMargin);
      GUI.EndGroup();
    }

    private void DrawNativeProfilerTimeline(Rect threadRect, int frameIndex, int threadIndex, float timeOffset, bool ghost)
    {
      bool flag = this.m_SelectedEntry.threadId == threadIndex && this.m_SelectedEntry.frameId == frameIndex;
      NativeProfilerTimeline_DrawArgs args = new NativeProfilerTimeline_DrawArgs();
      args.Reset();
      args.frameIndex = frameIndex;
      args.threadIndex = threadIndex;
      args.timeOffset = timeOffset;
      args.threadRect = threadRect;
      args.shownAreaRect = this.m_TimeArea.shownArea;
      args.selectedEntryIndex = !flag ? -1 : this.m_SelectedEntry.nativeIndex;
      args.mousedOverEntryIndex = -1;
      NativeProfilerTimeline.Draw(ref args);
    }

    private void HandleNativeProfilerTimelineInput(Rect threadRect, int frameIndex, int threadIndex, float timeOffset, float topMargin)
    {
      if (!threadRect.Contains(Event.current.mousePosition))
        return;
      bool singleClick = Event.current.clickCount == 1 && Event.current.type == EventType.MouseDown;
      bool doubleClick = Event.current.clickCount == 2 && Event.current.type == EventType.MouseDown;
      bool flag = (singleClick || doubleClick) && Event.current.button == 0;
      if (!flag)
        return;
      NativeProfilerTimeline_GetEntryAtPositionArgs args1 = new NativeProfilerTimeline_GetEntryAtPositionArgs();
      args1.Reset();
      args1.frameIndex = frameIndex;
      args1.threadIndex = threadIndex;
      args1.timeOffset = timeOffset;
      args1.threadRect = threadRect;
      args1.shownAreaRect = this.m_TimeArea.shownArea;
      args1.position = Event.current.mousePosition;
      NativeProfilerTimeline.GetEntryAtPosition(ref args1);
      int outEntryIndex = args1.out_EntryIndex;
      if (outEntryIndex != -1)
      {
        if (!this.m_SelectedEntry.Equals(frameIndex, threadIndex, outEntryIndex))
        {
          NativeProfilerTimeline_GetEntryTimingInfoArgs args2 = new NativeProfilerTimeline_GetEntryTimingInfoArgs();
          args2.Reset();
          args2.frameIndex = frameIndex;
          args2.threadIndex = threadIndex;
          args2.entryIndex = outEntryIndex;
          args2.calculateFrameData = true;
          NativeProfilerTimeline.GetEntryTimingInfo(ref args2);
          NativeProfilerTimeline_GetEntryInstanceInfoArgs args3 = new NativeProfilerTimeline_GetEntryInstanceInfoArgs();
          args3.Reset();
          args3.frameIndex = frameIndex;
          args3.threadIndex = threadIndex;
          args3.entryIndex = outEntryIndex;
          NativeProfilerTimeline.GetEntryInstanceInfo(ref args3);
          this.m_Window.SetSelectedPropertyPath(args3.out_Path);
          this.m_SelectedEntry.Reset();
          this.m_SelectedEntry.frameId = frameIndex;
          this.m_SelectedEntry.threadId = threadIndex;
          this.m_SelectedEntry.nativeIndex = outEntryIndex;
          this.m_SelectedEntry.instanceId = args3.out_Id;
          this.m_SelectedEntry.time = args2.out_LocalStartTime;
          this.m_SelectedEntry.duration = args2.out_Duration;
          this.m_SelectedEntry.totalDuration = args2.out_TotalDurationForFrame;
          this.m_SelectedEntry.instanceCount = args2.out_InstanceCountForFrame;
          this.m_SelectedEntry.relativeYPos = args1.out_EntryYMaxPos + topMargin;
          this.m_SelectedEntry.name = args1.out_EntryName;
          this.m_SelectedEntry.callstackInfo = args3.out_CallstackInfo;
          this.m_SelectedEntry.metaData = args3.out_MetaData;
        }
        Event.current.Use();
        this.UpdateSelectedObject(singleClick, doubleClick);
      }
      else if (flag)
      {
        this.ClearSelection();
        Event.current.Use();
      }
    }

    private void UpdateSelectedObject(bool singleClick, bool doubleClick)
    {
      UnityEngine.Object gameObject = EditorUtility.InstanceIDToObject(this.m_SelectedEntry.instanceId);
      if (gameObject is Component)
        gameObject = (UnityEngine.Object) ((Component) gameObject).gameObject;
      if (!(gameObject != (UnityEngine.Object) null))
        return;
      if (singleClick)
        EditorGUIUtility.PingObject(gameObject.GetInstanceID());
      else if (doubleClick)
        Selection.objects = new List<UnityEngine.Object>()
        {
          gameObject
        }.ToArray();
    }

    private void ClearSelection()
    {
      this.m_Window.ClearSelectedPropertyPath();
      this.m_SelectedEntry.Reset();
    }

    private void PerformFrameSelected(float frameMS)
    {
      float num1 = this.m_SelectedEntry.time;
      float num2 = this.m_SelectedEntry.duration;
      if (this.m_SelectedEntry.instanceId < 0 || (double) num2 <= 0.0)
      {
        num1 = 0.0f;
        num2 = frameMS;
      }
      this.m_TimeArea.SetShownHRangeInsideMargins(num1 - num2 * 0.2f, num1 + num2 * 1.2f);
    }

    private void HandleFrameSelected(float frameMS)
    {
      Event current = Event.current;
      if (current.type != EventType.ValidateCommand && current.type != EventType.ExecuteCommand || !(current.commandName == "FrameSelected"))
        return;
      if (current.type == EventType.ExecuteCommand)
        this.PerformFrameSelected(frameMS);
      current.Use();
    }

    private void DoProfilerFrame(int frameIndex, Rect fullRect, bool ghost, int threadCount, float offset)
    {
      ProfilerFrameDataIterator frameDataIterator = new ProfilerFrameDataIterator();
      int threadCount1 = frameDataIterator.GetThreadCount(frameIndex);
      if (ghost && threadCount1 != threadCount)
        return;
      frameDataIterator.SetRoot(frameIndex, 0);
      if (!ghost)
      {
        this.DrawGrid(fullRect, threadCount, frameDataIterator.frameTimeMS);
        this.HandleFrameSelected(frameDataIterator.frameTimeMS);
      }
      float num1 = fullRect.y;
      foreach (ProfilerTimelineGUI.GroupInfo group in this.groups)
      {
        Rect r = fullRect;
        bool expanded = group.expanded;
        if (expanded)
          num1 += group.height;
        float num2 = num1;
        int count = group.threads.Count;
        foreach (ProfilerTimelineGUI.ThreadInfo thread in group.threads)
        {
          frameDataIterator.SetRoot(frameIndex, thread.threadIndex);
          r.y = num1;
          r.height = !expanded ? Math.Max((float) ((double) group.height / (double) count - 1.0), 2f) : thread.height;
          this.DoNativeProfilerTimeline(r, frameIndex, thread.threadIndex, offset, ghost);
          if (this.m_SelectedEntry.IsValid() && this.m_SelectedEntry.frameId == frameIndex && this.m_SelectedEntry.threadId == thread.threadIndex)
            this.m_SelectedThreadY = num1;
          num1 += r.height;
        }
        if (!expanded)
          num1 = num2 + group.height;
      }
    }

    private void DoSelectionTooltip(int frameIndex, Rect fullRect)
    {
      if (!this.m_SelectedEntry.IsValid() || this.m_SelectedEntry.frameId != frameIndex)
        return;
      string str1 = string.Format((double) this.m_SelectedEntry.duration < 1.0 ? "{0:f3}ms" : "{0:f2}ms", (object) this.m_SelectedEntry.duration);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Format("{0}\n{1}", (object) this.m_SelectedEntry.name, (object) str1));
      if (this.m_SelectedEntry.instanceCount > 1)
      {
        string str2 = string.Format((double) this.m_SelectedEntry.totalDuration < 1.0 ? "{0:f3}ms" : "{0:f2}ms", (object) this.m_SelectedEntry.totalDuration);
        stringBuilder.Append(string.Format("\n{0}: {1} ({2} {3})", (object) this.m_LocalizedString_Total, (object) str2, (object) this.m_SelectedEntry.instanceCount, (object) this.m_LocalizedString_Instances));
      }
      if (this.m_SelectedEntry.metaData.Length > 0)
        stringBuilder.Append(string.Format("\n{0}", (object) this.m_SelectedEntry.metaData));
      if (this.m_SelectedEntry.callstackInfo.Length > 0)
        stringBuilder.Append(string.Format("\n{0}", (object) this.m_SelectedEntry.callstackInfo));
      float y = fullRect.y + this.m_SelectedThreadY + this.m_SelectedEntry.relativeYPos;
      GUIContent content = new GUIContent(stringBuilder.ToString());
      GUIStyle tooltip = ProfilerTimelineGUI.styles.tooltip;
      Vector2 vector2 = tooltip.CalcSize(content);
      float pixel = this.m_TimeArea.TimeToPixel(this.m_SelectedEntry.time + this.m_SelectedEntry.duration * 0.5f, fullRect);
      Rect position1 = new Rect(pixel - 32f, y, 64f, 6f);
      Rect position2 = new Rect(pixel, y + 6f, vector2.x, vector2.y);
      if ((double) position2.xMax > (double) fullRect.xMax + 16.0)
        position2.x = (float) ((double) fullRect.xMax - (double) position2.width + 16.0);
      if ((double) position1.xMax > (double) fullRect.xMax + 20.0)
        position1.x = (float) ((double) fullRect.xMax - (double) position1.width + 20.0);
      if ((double) position2.xMin < (double) fullRect.xMin + 30.0)
        position2.x = fullRect.xMin + 30f;
      if ((double) position1.xMin < (double) fullRect.xMin - 20.0)
        position1.x = fullRect.xMin - 20f;
      float num = (float) (16.0 + (double) position2.height + 2.0 * (double) position1.height);
      bool flag = (double) y + (double) vector2.y + 6.0 > (double) fullRect.yMax && (double) position2.y - (double) num > 0.0;
      if (flag)
      {
        position2.y -= num;
        position1.y -= (float) (16.0 + 2.0 * (double) position1.height);
      }
      GUI.BeginClip(position1);
      Matrix4x4 matrix = GUI.matrix;
      if (flag)
        GUIUtility.ScaleAroundPivot(new Vector2(1f, -1f), new Vector2(position1.width * 0.5f, position1.height));
      GUI.Label(new Rect(0.0f, 0.0f, position1.width, position1.height), GUIContent.none, ProfilerTimelineGUI.styles.tooltipArrow);
      GUI.matrix = matrix;
      GUI.EndClip();
      GUI.Label(position2, content, tooltip);
    }

    public void DoGUI(int frameIndex, float width, float ypos, float height)
    {
      Rect rect = new Rect(0.0f, ypos - 1f, width, height + 1f);
      float width1 = 179f;
      if (Event.current.type == EventType.Repaint)
      {
        ProfilerTimelineGUI.styles.profilerGraphBackground.Draw(rect, false, false, false, false);
        EditorStyles.toolbar.Draw(new Rect(0.0f, (float) ((double) ypos + (double) height - 15.0), width1, 15f), false, false, false, false);
      }
      bool flag = false;
      if (this.m_TimeArea == null)
      {
        flag = true;
        this.m_TimeArea = new ZoomableArea();
        this.m_TimeArea.hRangeLocked = false;
        this.m_TimeArea.vRangeLocked = true;
        this.m_TimeArea.hSlider = true;
        this.m_TimeArea.vSlider = false;
        this.m_TimeArea.scaleWithWindow = true;
        this.m_TimeArea.rect = new Rect((float) ((double) rect.x + (double) width1 - 1.0), rect.y, rect.width - width1, rect.height);
        this.m_TimeArea.margin = 10f;
      }
      if (flag)
      {
        NativeProfilerTimeline_InitializeArgs args = new NativeProfilerTimeline_InitializeArgs();
        args.Reset();
        args.profilerColors = ProfilerColors.currentColors;
        args.allocationSampleColor = ProfilerColors.allocationSample;
        args.internalSampleColor = ProfilerColors.internalSample;
        args.ghostAlpha = 0.3f;
        args.nonSelectedAlpha = 0.75f;
        args.guiStyle = ProfilerTimelineGUI.styles.bar.m_Ptr;
        args.lineHeight = 16f;
        args.textFadeOutWidth = 20f;
        args.textFadeStartWidth = 50f;
        NativeProfilerTimeline.Initialize(ref args);
      }
      ProfilerFrameDataIterator frameDataIterator1 = new ProfilerFrameDataIterator();
      frameDataIterator1.SetRoot(frameIndex, 0);
      this.m_TimeArea.hBaseRangeMin = 0.0f;
      this.m_TimeArea.hBaseRangeMax = frameDataIterator1.frameTimeMS;
      if (flag)
        this.PerformFrameSelected(frameDataIterator1.frameTimeMS);
      this.m_TimeArea.rect = new Rect(rect.x + width1, rect.y, rect.width - width1, rect.height);
      this.m_TimeArea.BeginViewGUI();
      this.m_TimeArea.EndViewGUI();
      rect = this.m_TimeArea.drawRect;
      this.CalculateBars(rect, frameIndex, this.animationTime);
      this.DrawBars(rect, frameIndex);
      GUI.BeginClip(this.m_TimeArea.drawRect);
      rect.x = 0.0f;
      rect.y = 0.0f;
      bool enabled = GUI.enabled;
      GUI.enabled = false;
      ProfilerFrameDataIterator frameDataIterator2 = new ProfilerFrameDataIterator();
      int threadCount1 = frameDataIterator2.GetThreadCount(frameIndex);
      int previousFrameIndex = ProfilerDriver.GetPreviousFrameIndex(frameIndex);
      if (previousFrameIndex != -1)
      {
        frameDataIterator2.SetRoot(previousFrameIndex, 0);
        this.DoProfilerFrame(previousFrameIndex, rect, true, threadCount1, -frameDataIterator2.frameTimeMS);
      }
      int nextFrameIndex = ProfilerDriver.GetNextFrameIndex(frameIndex);
      if (nextFrameIndex != -1)
      {
        frameDataIterator2.SetRoot(frameIndex, 0);
        this.DoProfilerFrame(nextFrameIndex, rect, true, threadCount1, frameDataIterator2.frameTimeMS);
      }
      GUI.enabled = enabled;
      int threadCount2 = 0;
      this.DoProfilerFrame(frameIndex, rect, false, threadCount2, 0.0f);
      GUI.EndClip();
      this.DoSelectionTooltip(frameIndex, this.m_TimeArea.drawRect);
    }

    internal class ThreadInfo
    {
      public float height;
      public float desiredWeight;
      public float weight;
      public int threadIndex;
      public string name;
    }

    internal class GroupInfo
    {
      public bool expanded;
      public string name;
      public float height;
      public List<ProfilerTimelineGUI.ThreadInfo> threads;
    }

    internal class Styles
    {
      public GUIStyle background = (GUIStyle) "OL Box";
      public GUIStyle tooltip = (GUIStyle) "AnimationEventTooltip";
      public GUIStyle tooltipArrow = (GUIStyle) "AnimationEventTooltipArrow";
      public GUIStyle bar = (GUIStyle) "ProfilerTimelineBar";
      public GUIStyle leftPane = (GUIStyle) "ProfilerTimelineLeftPane";
      public GUIStyle rightPane = (GUIStyle) "ProfilerRightPane";
      public GUIStyle foldout = (GUIStyle) "ProfilerTimelineFoldout";
      public GUIStyle profilerGraphBackground = new GUIStyle((GUIStyle) "ProfilerScrollviewBackground");

      internal Styles()
      {
        GUIStyleState normal1 = this.bar.normal;
        Texture2D whiteTexture = EditorGUIUtility.whiteTexture;
        this.bar.active.background = whiteTexture;
        Texture2D texture2D1 = whiteTexture;
        this.bar.hover.background = texture2D1;
        Texture2D texture2D2 = texture2D1;
        normal1.background = texture2D2;
        GUIStyleState normal2 = this.bar.normal;
        Color black = Color.black;
        this.bar.active.textColor = black;
        Color color1 = black;
        this.bar.hover.textColor = color1;
        Color color2 = color1;
        normal2.textColor = color2;
        this.profilerGraphBackground.overflow.left = -179;
        this.leftPane.padding.left = 15;
      }
    }

    private class EntryInfo
    {
      public int frameId = -1;
      public int threadId = -1;
      public int nativeIndex = -1;
      public float relativeYPos = 0.0f;
      public float time = 0.0f;
      public float duration = 0.0f;
      public string name = string.Empty;

      public bool IsValid()
      {
        return this.name.Length > 0;
      }

      public bool Equals(int frameId, int threadId, int nativeIndex)
      {
        return frameId == this.frameId && threadId == this.threadId && nativeIndex == this.nativeIndex;
      }

      public virtual void Reset()
      {
        this.frameId = -1;
        this.threadId = -1;
        this.nativeIndex = -1;
        this.relativeYPos = 0.0f;
        this.time = 0.0f;
        this.duration = 0.0f;
        this.name = string.Empty;
      }
    }

    private class SelectedEntryInfo : ProfilerTimelineGUI.EntryInfo
    {
      public int instanceId = -1;
      public string metaData = string.Empty;
      public float totalDuration = -1f;
      public int instanceCount = -1;
      public string callstackInfo = string.Empty;

      public override void Reset()
      {
        base.Reset();
        this.instanceId = -1;
        this.metaData = string.Empty;
        this.totalDuration = -1f;
        this.instanceCount = -1;
        this.callstackInfo = string.Empty;
      }
    }
  }
}
