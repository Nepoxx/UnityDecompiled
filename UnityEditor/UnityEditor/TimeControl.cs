// Decompiled with JetBrains decompiler
// Type: UnityEditor.TimeControl
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TimeControl
  {
    private static readonly int kScrubberIDHash = "ScrubberIDHash".GetHashCode();
    public float currentTime = float.NegativeInfinity;
    private bool m_NextCurrentTimeSet = false;
    public float startTime = 0.0f;
    public float stopTime = 1f;
    public bool playSelection = false;
    public bool loop = true;
    public float playbackSpeed = 1f;
    private float m_DeltaTime = 0.0f;
    private bool m_DeltaTimeSet = false;
    private double m_LastFrameEditorTime = 0.0;
    private bool m_Playing = false;
    private bool m_ResetOnPlay = false;
    private float m_MouseDrag = 0.0f;
    private bool m_WrapForwardDrag = false;
    private const float kStepTime = 0.01f;
    private const float kScrubberHeight = 21f;
    private const float kPlayButtonWidth = 33f;
    private static TimeControl.Styles s_Styles;

    public float nextCurrentTime
    {
      set
      {
        this.deltaTime = value - this.currentTime;
        this.m_NextCurrentTimeSet = true;
      }
    }

    public float deltaTime
    {
      get
      {
        return this.m_DeltaTime;
      }
      set
      {
        this.m_DeltaTime = value;
        this.m_DeltaTimeSet = true;
      }
    }

    public float normalizedTime
    {
      get
      {
        return (double) this.stopTime != (double) this.startTime ? (float) (((double) this.currentTime - (double) this.startTime) / ((double) this.stopTime - (double) this.startTime)) : 0.0f;
      }
      set
      {
        this.currentTime = (float) ((double) this.startTime * (1.0 - (double) value) + (double) this.stopTime * (double) value);
      }
    }

    public bool playing
    {
      get
      {
        return this.m_Playing;
      }
      set
      {
        if (this.m_Playing != value)
        {
          if (value)
          {
            EditorApplication.CallbackFunction update = EditorApplication.update;
            if (TimeControl.\u003C\u003Ef__mg\u0024cache0 == null)
              TimeControl.\u003C\u003Ef__mg\u0024cache0 = new EditorApplication.CallbackFunction(InspectorWindow.RepaintAllInspectors);
            EditorApplication.CallbackFunction fMgCache0 = TimeControl.\u003C\u003Ef__mg\u0024cache0;
            EditorApplication.update = update + fMgCache0;
            this.m_LastFrameEditorTime = EditorApplication.timeSinceStartup;
            if (this.m_ResetOnPlay)
            {
              this.nextCurrentTime = this.startTime;
              this.m_ResetOnPlay = false;
            }
          }
          else
          {
            EditorApplication.CallbackFunction update = EditorApplication.update;
            if (TimeControl.\u003C\u003Ef__mg\u0024cache1 == null)
              TimeControl.\u003C\u003Ef__mg\u0024cache1 = new EditorApplication.CallbackFunction(InspectorWindow.RepaintAllInspectors);
            EditorApplication.CallbackFunction fMgCache1 = TimeControl.\u003C\u003Ef__mg\u0024cache1;
            EditorApplication.update = update - fMgCache1;
          }
        }
        this.m_Playing = value;
      }
    }

    public void DoTimeControl(Rect rect)
    {
      if (TimeControl.s_Styles == null)
        TimeControl.s_Styles = new TimeControl.Styles();
      Event current = Event.current;
      int controlId = GUIUtility.GetControlID(TimeControl.kScrubberIDHash, FocusType.Keyboard);
      Rect position = rect;
      position.height = 21f;
      Rect rect1 = position;
      rect1.xMin += 33f;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (rect.Contains(current.mousePosition))
            GUIUtility.keyboardControl = controlId;
          if (rect1.Contains(current.mousePosition))
          {
            EditorGUIUtility.SetWantsMouseJumping(1);
            GUIUtility.hotControl = controlId;
            this.m_MouseDrag = current.mousePosition.x - rect1.xMin;
            this.nextCurrentTime = this.m_MouseDrag * (this.stopTime - this.startTime) / rect1.width + this.startTime;
            this.m_WrapForwardDrag = false;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            EditorGUIUtility.SetWantsMouseJumping(0);
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            this.m_MouseDrag += current.delta.x * this.playbackSpeed;
            if (this.loop && ((double) this.m_MouseDrag < 0.0 && this.m_WrapForwardDrag || (double) this.m_MouseDrag > (double) rect1.width))
            {
              if ((double) this.m_MouseDrag > (double) rect1.width)
                this.currentTime -= this.stopTime - this.startTime;
              else if ((double) this.m_MouseDrag < 0.0)
                this.currentTime += this.stopTime - this.startTime;
              this.m_WrapForwardDrag = true;
              this.m_MouseDrag = Mathf.Repeat(this.m_MouseDrag, rect1.width);
            }
            this.nextCurrentTime = Mathf.Clamp(this.m_MouseDrag, 0.0f, rect1.width) * (this.stopTime - this.startTime) / rect1.width + this.startTime;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == controlId)
          {
            if (current.keyCode == KeyCode.LeftArrow)
            {
              if ((double) this.currentTime - (double) this.startTime > 0.00999999977648258)
                this.deltaTime = -0.01f;
              current.Use();
            }
            if (current.keyCode == KeyCode.RightArrow)
            {
              if ((double) this.stopTime - (double) this.currentTime > 0.00999999977648258)
                this.deltaTime = 0.01f;
              current.Use();
            }
            break;
          }
          break;
      }
      GUI.Box(position, GUIContent.none, TimeControl.s_Styles.timeScrubber);
      this.playing = GUI.Toggle(position, this.playing, !this.playing ? TimeControl.s_Styles.playIcon : TimeControl.s_Styles.pauseIcon, TimeControl.s_Styles.playButton);
      TimeArea.DrawPlayhead(Mathf.Lerp(rect1.x, rect1.xMax, this.normalizedTime), rect1.yMin, rect1.yMax, 2f, GUIUtility.keyboardControl != controlId ? 0.5f : 1f);
    }

    public void OnDisable()
    {
      this.playing = false;
    }

    public void Update()
    {
      if (!this.m_DeltaTimeSet)
      {
        if (this.playing)
        {
          double timeSinceStartup = EditorApplication.timeSinceStartup;
          this.deltaTime = (float) (timeSinceStartup - this.m_LastFrameEditorTime) * this.playbackSpeed;
          this.m_LastFrameEditorTime = timeSinceStartup;
        }
        else
          this.deltaTime = 0.0f;
      }
      this.currentTime += this.deltaTime;
      if (this.loop && this.playing && !this.m_NextCurrentTimeSet)
      {
        this.normalizedTime = Mathf.Repeat(this.normalizedTime, 1f);
      }
      else
      {
        if ((double) this.normalizedTime > 1.0)
        {
          this.playing = false;
          this.m_ResetOnPlay = true;
        }
        this.normalizedTime = Mathf.Clamp01(this.normalizedTime);
      }
      this.m_DeltaTimeSet = false;
      this.m_NextCurrentTimeSet = false;
    }

    private class Styles
    {
      public GUIContent playIcon = EditorGUIUtility.IconContent("PlayButton");
      public GUIContent pauseIcon = EditorGUIUtility.IconContent("PauseButton");
      public GUIStyle playButton = (GUIStyle) "TimeScrubberButton";
      public GUIStyle timeScrubber = (GUIStyle) "TimeScrubber";
    }
  }
}
