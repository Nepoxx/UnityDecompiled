// Decompiled with JetBrains decompiler
// Type: UnityEditor.TimeArea
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TimeArea : ZoomableArea
  {
    [SerializeField]
    private TickHandler m_HTicks;
    [SerializeField]
    private TickHandler m_VTicks;
    internal const int kTickRulerDistMin = 3;
    internal const int kTickRulerDistFull = 80;
    internal const int kTickRulerDistLabel = 40;
    internal const float kTickRulerHeightMax = 0.7f;
    internal const float kTickRulerFatThreshold = 0.5f;
    private static TimeArea.Styles2 styles;
    private static float s_OriginalTime;
    private static float s_PickOffset;

    public TimeArea(bool minimalGUI)
      : base(minimalGUI)
    {
      float[] tickModulos = new float[29]{ 1E-07f, 5E-07f, 1E-06f, 5E-06f, 1E-05f, 5E-05f, 0.0001f, 0.0005f, 1f / 1000f, 0.005f, 0.01f, 0.05f, 0.1f, 0.5f, 1f, 5f, 10f, 50f, 100f, 500f, 1000f, 5000f, 10000f, 50000f, 100000f, 500000f, 1000000f, 5000000f, 1E+07f };
      this.hTicks = new TickHandler();
      this.hTicks.SetTickModulos(tickModulos);
      this.vTicks = new TickHandler();
      this.vTicks.SetTickModulos(tickModulos);
    }

    public TickHandler hTicks
    {
      get
      {
        return this.m_HTicks;
      }
      set
      {
        this.m_HTicks = value;
      }
    }

    public TickHandler vTicks
    {
      get
      {
        return this.m_VTicks;
      }
      set
      {
        this.m_VTicks = value;
      }
    }

    private static void InitStyles()
    {
      if (TimeArea.styles != null)
        return;
      TimeArea.styles = new TimeArea.Styles2();
    }

    public void SetTickMarkerRanges()
    {
      this.hTicks.SetRanges(this.shownArea.xMin, this.shownArea.xMax, this.drawRect.xMin, this.drawRect.xMax);
      this.vTicks.SetRanges(this.shownArea.yMin, this.shownArea.yMax, this.drawRect.yMin, this.drawRect.yMax);
    }

    public void DrawMajorTicks(Rect position, float frameRate)
    {
      GUI.BeginGroup(position);
      if (Event.current.type != EventType.Repaint)
      {
        GUI.EndGroup();
      }
      else
      {
        TimeArea.InitStyles();
        HandleUtility.ApplyWireMaterial();
        this.SetTickMarkerRanges();
        this.hTicks.SetTickStrengths(3f, 80f, true);
        Color textColor = TimeArea.styles.timelineTick.normal.textColor;
        textColor.a = 0.1f;
        if (Application.platform == RuntimePlatform.WindowsEditor)
          GL.Begin(7);
        else
          GL.Begin(1);
        Rect shownArea = this.shownArea;
        for (int level = 0; level < this.hTicks.tickLevels; ++level)
        {
          if ((double) (this.hTicks.GetStrengthOfLevel(level) * 0.9f) > 0.5)
          {
            float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(level, true);
            for (int index = 0; index < ticksAtLevel.Length; ++index)
            {
              if ((double) ticksAtLevel[index] >= 0.0)
                TimeArea.DrawVerticalLineFast(this.FrameToPixel((float) Mathf.RoundToInt(ticksAtLevel[index] * frameRate), frameRate, position, shownArea), 0.0f, position.height, textColor);
            }
          }
        }
        GL.End();
        GUI.EndGroup();
      }
    }

    public void TimeRuler(Rect position, float frameRate)
    {
      this.TimeRuler(position, frameRate, true, false, 1f, TimeArea.TimeFormat.TimeFrame);
    }

    public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha)
    {
      this.TimeRuler(position, frameRate, labels, useEntireHeight, alpha, TimeArea.TimeFormat.TimeFrame);
    }

    public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha, TimeArea.TimeFormat timeFormat)
    {
      Color color = GUI.color;
      GUI.BeginGroup(position);
      TimeArea.InitStyles();
      HandleUtility.ApplyWireMaterial();
      Color backgroundColor = GUI.backgroundColor;
      this.SetTickMarkerRanges();
      this.hTicks.SetTickStrengths(3f, 80f, true);
      Color textColor = TimeArea.styles.timelineTick.normal.textColor;
      textColor.a = 0.75f * alpha;
      if (Event.current.type == EventType.Repaint)
      {
        if (Application.platform == RuntimePlatform.WindowsEditor)
          GL.Begin(7);
        else
          GL.Begin(1);
        Rect shownArea = this.shownArea;
        for (int level = 0; level < this.hTicks.tickLevels; ++level)
        {
          float b = this.hTicks.GetStrengthOfLevel(level) * 0.9f;
          float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(level, true);
          for (int index = 0; index < ticksAtLevel.Length; ++index)
          {
            if ((double) ticksAtLevel[index] >= (double) this.hRangeMin && (double) ticksAtLevel[index] <= (double) this.hRangeMax)
            {
              int num1 = Mathf.RoundToInt(ticksAtLevel[index] * frameRate);
              float num2 = !useEntireHeight ? (float) ((double) position.height * (double) Mathf.Min(1f, b) * 0.699999988079071) : position.height;
              TimeArea.DrawVerticalLineFast(this.FrameToPixel((float) num1, frameRate, position, shownArea), (float) ((double) position.height - (double) num2 + 0.5), position.height - 0.5f, new Color(1f, 1f, 1f, b / 0.5f) * textColor);
            }
          }
        }
        GL.End();
      }
      if (labels)
      {
        float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(this.hTicks.GetLevelWithMinSeparation(40f), false);
        for (int index = 0; index < ticksAtLevel.Length; ++index)
        {
          if ((double) ticksAtLevel[index] >= (double) this.hRangeMin && (double) ticksAtLevel[index] <= (double) this.hRangeMax)
            GUI.Label(new Rect(Mathf.Floor(this.FrameToPixel((float) Mathf.RoundToInt(ticksAtLevel[index] * frameRate), frameRate, position)) + 3f, -3f, 40f, 20f), this.FormatTime(ticksAtLevel[index], frameRate, timeFormat), TimeArea.styles.timelineTick);
        }
      }
      GUI.EndGroup();
      GUI.backgroundColor = backgroundColor;
      GUI.color = color;
    }

    public static void DrawPlayhead(float x, float yMin, float yMax, float thickness, float alpha)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      TimeArea.InitStyles();
      float num = thickness * 0.5f;
      Color color = TimeArea.styles.playhead.normal.textColor.AlphaMultiplied(alpha);
      if ((double) thickness > 1.0)
        EditorGUI.DrawRect(Rect.MinMaxRect(x - num, yMin, x + num, yMax), color);
      else
        TimeArea.DrawVerticalLine(x, yMin, yMax, color);
    }

    public static void DrawVerticalLine(float x, float minY, float maxY, Color color)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color1 = Handles.color;
      HandleUtility.ApplyWireMaterial();
      if (Application.platform == RuntimePlatform.WindowsEditor)
        GL.Begin(7);
      else
        GL.Begin(1);
      TimeArea.DrawVerticalLineFast(x, minY, maxY, color);
      GL.End();
      Handles.color = color1;
    }

    public static void DrawVerticalLineFast(float x, float minY, float maxY, Color color)
    {
      if (Application.platform == RuntimePlatform.WindowsEditor)
      {
        GL.Color(color);
        GL.Vertex(new Vector3(x - 0.5f, minY, 0.0f));
        GL.Vertex(new Vector3(x + 0.5f, minY, 0.0f));
        GL.Vertex(new Vector3(x + 0.5f, maxY, 0.0f));
        GL.Vertex(new Vector3(x - 0.5f, maxY, 0.0f));
      }
      else
      {
        GL.Color(color);
        GL.Vertex(new Vector3(x, minY, 0.0f));
        GL.Vertex(new Vector3(x, maxY, 0.0f));
      }
    }

    public TimeArea.TimeRulerDragMode BrowseRuler(Rect position, ref float time, float frameRate, bool pickAnywhere, GUIStyle thumbStyle)
    {
      int controlId = GUIUtility.GetControlID(3126789, FocusType.Passive);
      return this.BrowseRuler(position, controlId, ref time, frameRate, pickAnywhere, thumbStyle);
    }

    public TimeArea.TimeRulerDragMode BrowseRuler(Rect position, int id, ref float time, float frameRate, bool pickAnywhere, GUIStyle thumbStyle)
    {
      Event current = Event.current;
      Rect position1 = position;
      if ((double) time != -1.0)
      {
        position1.x = Mathf.Round(this.TimeToPixel(time, position)) - (float) thumbStyle.overflow.left;
        position1.width = thumbStyle.fixedWidth + (float) thumbStyle.overflow.horizontal;
      }
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (position1.Contains(current.mousePosition))
          {
            GUIUtility.hotControl = id;
            TimeArea.s_PickOffset = current.mousePosition.x - this.TimeToPixel(time, position);
            current.Use();
            return TimeArea.TimeRulerDragMode.Start;
          }
          if (pickAnywhere && position.Contains(current.mousePosition))
          {
            GUIUtility.hotControl = id;
            float wholeFps = this.SnapTimeToWholeFPS(this.PixelToTime(current.mousePosition.x, position), frameRate);
            TimeArea.s_OriginalTime = time;
            if ((double) wholeFps != (double) time)
              GUI.changed = true;
            time = wholeFps;
            TimeArea.s_PickOffset = 0.0f;
            current.Use();
            return TimeArea.TimeRulerDragMode.Start;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id)
          {
            GUIUtility.hotControl = 0;
            current.Use();
            return TimeArea.TimeRulerDragMode.End;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            float wholeFps = this.SnapTimeToWholeFPS(this.PixelToTime(current.mousePosition.x - TimeArea.s_PickOffset, position), frameRate);
            if ((double) wholeFps != (double) time)
              GUI.changed = true;
            time = wholeFps;
            current.Use();
            return TimeArea.TimeRulerDragMode.Dragging;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == id && current.keyCode == KeyCode.Escape)
          {
            if ((double) time != (double) TimeArea.s_OriginalTime)
              GUI.changed = true;
            time = TimeArea.s_OriginalTime;
            GUIUtility.hotControl = 0;
            current.Use();
            return TimeArea.TimeRulerDragMode.Cancel;
          }
          break;
        case EventType.Repaint:
          if ((double) time != -1.0)
          {
            bool flag = position.Contains(current.mousePosition);
            position1.x += (float) thumbStyle.overflow.left;
            thumbStyle.Draw(position1, id == GUIUtility.hotControl, flag || id == GUIUtility.hotControl, false, false);
            break;
          }
          break;
      }
      return TimeArea.TimeRulerDragMode.None;
    }

    private float FrameToPixel(float i, float frameRate, Rect rect, Rect theShownArea)
    {
      return (float) (((double) i - (double) theShownArea.xMin * (double) frameRate) * (double) rect.width / ((double) theShownArea.width * (double) frameRate));
    }

    public float FrameToPixel(float i, float frameRate, Rect rect)
    {
      return this.FrameToPixel(i, frameRate, rect, this.shownArea);
    }

    public float TimeField(Rect rect, int id, float time, float frameRate, TimeArea.TimeFormat timeFormat)
    {
      if (timeFormat == TimeArea.TimeFormat.None)
        return this.SnapTimeToWholeFPS(EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, rect, new Rect(0.0f, 0.0f, 0.0f, 0.0f), id, time, EditorGUI.kFloatFieldFormatString, EditorStyles.numberField, false), frameRate);
      if (timeFormat == TimeArea.TimeFormat.Frame)
      {
        int num = Mathf.RoundToInt(time * frameRate);
        return (float) EditorGUI.DoIntField(EditorGUI.s_RecycledEditor, rect, new Rect(0.0f, 0.0f, 0.0f, 0.0f), id, num, EditorGUI.kIntFieldFormatString, EditorStyles.numberField, false, 0.0f) / frameRate;
      }
      string text = this.FormatTime(time, frameRate, TimeArea.TimeFormat.TimeFrame);
      string allowedletters = "0123456789.,:";
      bool changed;
      string str = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, id, rect, text, EditorStyles.numberField, allowedletters, out changed, false, false, false);
      if (changed && GUIUtility.keyboardControl == id)
      {
        GUI.changed = true;
        string s1 = str.Replace(',', '.');
        int length = s1.IndexOf(':');
        if (length >= 0)
        {
          string s2 = s1.Substring(0, length);
          string s3 = s1.Substring(length + 1);
          int result1;
          int result2;
          if (int.TryParse(s2, out result1) && int.TryParse(s3, out result2))
            return (float) result1 + (float) result2 / frameRate;
        }
        else
        {
          float result;
          if (float.TryParse(s1, out result))
            return this.SnapTimeToWholeFPS(result, frameRate);
        }
      }
      return time;
    }

    public float ValueField(Rect rect, int id, float value)
    {
      return EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, rect, new Rect(0.0f, 0.0f, 0.0f, 0.0f), id, value, EditorGUI.kFloatFieldFormatString, EditorStyles.numberField, false);
    }

    public string FormatTime(float time, float frameRate, TimeArea.TimeFormat timeFormat)
    {
      if (timeFormat == TimeArea.TimeFormat.None)
      {
        int num = (double) frameRate == 0.0 ? MathUtils.GetNumberOfDecimalsForMinimumDifference(this.shownArea.width / this.drawRect.width) : MathUtils.GetNumberOfDecimalsForMinimumDifference(1f / frameRate);
        return time.ToString("N" + (object) num);
      }
      int num1 = Mathf.RoundToInt(time * frameRate);
      if (timeFormat != TimeArea.TimeFormat.TimeFrame)
        return num1.ToString();
      int length = ((int) frameRate).ToString().Length;
      string str = string.Empty;
      if (num1 < 0)
      {
        str = "-";
        num1 = -num1;
      }
      return str + (num1 / (int) frameRate).ToString() + ":" + ((float) num1 % frameRate).ToString().PadLeft(length, '0');
    }

    public string FormatValue(float value)
    {
      int minimumDifference = MathUtils.GetNumberOfDecimalsForMinimumDifference(this.shownArea.height / this.drawRect.height);
      return value.ToString("N" + (object) minimumDifference);
    }

    public float SnapTimeToWholeFPS(float time, float frameRate)
    {
      if ((double) frameRate == 0.0)
        return time;
      return Mathf.Round(time * frameRate) / frameRate;
    }

    public enum TimeFormat
    {
      None,
      TimeFrame,
      Frame,
    }

    private class Styles2
    {
      public GUIStyle timelineTick = (GUIStyle) "AnimationTimelineTick";
      public GUIStyle labelTickMarks = (GUIStyle) "CurveEditorLabelTickMarks";
      public GUIStyle playhead = (GUIStyle) "AnimationPlayHead";
    }

    public enum TimeRulerDragMode
    {
      None,
      Start,
      End,
      Dragging,
      Cancel,
    }
  }
}
