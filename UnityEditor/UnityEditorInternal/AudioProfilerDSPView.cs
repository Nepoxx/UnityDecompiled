// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerDSPView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AudioProfilerDSPView
  {
    private const int AUDIOPROFILER_DSPFLAGS_ACTIVE = 1;
    private const int AUDIOPROFILER_DSPFLAGS_BYPASS = 2;

    private void DrawRectClipped(Rect r, Color col, string name, Rect c, float zoomFactor)
    {
      Rect rect = new Rect(r.x * zoomFactor, r.y * zoomFactor, r.width * zoomFactor, r.height * zoomFactor);
      float x1 = rect.x;
      float a1 = rect.x + rect.width;
      float y1 = rect.y;
      float a2 = rect.y + rect.height;
      float x2 = c.x;
      float b1 = c.x + c.width;
      float y2 = c.y;
      float b2 = c.y + c.height;
      float num1 = Mathf.Max(x1, x2);
      float num2 = Mathf.Max(y1, y2);
      float num3 = Mathf.Min(a1, b1);
      float num4 = Mathf.Min(a2, b2);
      if ((double) num1 >= (double) num3 || (double) num2 >= (double) num4)
        return;
      if (name == null)
      {
        EditorGUI.DrawRect(rect, col);
      }
      else
      {
        GUI.color = col;
        GUI.Button(rect, name);
      }
    }

    private static int GetOutCode(Vector3 p, Rect c)
    {
      int num = 0;
      if ((double) p.x < (double) c.x)
        num |= 1;
      if ((double) p.x > (double) c.x + (double) c.width)
        num |= 2;
      if ((double) p.y < (double) c.y)
        num |= 4;
      if ((double) p.y > (double) c.y + (double) c.height)
        num |= 8;
      return num;
    }

    public void OnGUI(Rect clippingRect, ProfilerProperty property, bool showInactiveDSPChains, bool highlightAudibleDSPChains, ref float zoomFactor, ref Vector2 scrollPos)
    {
      if (Event.current.type == EventType.ScrollWheel && clippingRect.Contains(Event.current.mousePosition) && Event.current.shift)
      {
        float num1 = 1.05f;
        float num2 = zoomFactor;
        zoomFactor *= (double) Event.current.delta.y <= 0.0 ? 1f / num1 : num1;
        scrollPos += (Event.current.mousePosition - scrollPos) * (zoomFactor - num2);
        Event.current.Use();
      }
      else
      {
        if (Event.current.type != EventType.Repaint)
          return;
        int num1 = 64;
        int num2 = 16;
        int num3 = 140;
        int num4 = 30;
        int num5 = num1 + 10;
        int num6 = 5;
        Dictionary<int, AudioProfilerDSPView.AudioProfilerDSPNode> dictionary = new Dictionary<int, AudioProfilerDSPView.AudioProfilerDSPNode>();
        List<AudioProfilerDSPView.AudioProfilerDSPWire> audioProfilerDspWireList = new List<AudioProfilerDSPView.AudioProfilerDSPWire>();
        AudioProfilerDSPInfo[] audioProfilerDspInfo = property.GetAudioProfilerDSPInfo();
        if (audioProfilerDspInfo == null)
          return;
        bool flag1 = true;
        foreach (AudioProfilerDSPInfo info in audioProfilerDspInfo)
        {
          if (showInactiveDSPChains || (info.flags & 1) != 0)
          {
            if (!dictionary.ContainsKey(info.id))
            {
              AudioProfilerDSPView.AudioProfilerDSPNode audioProfilerDspNode1 = !dictionary.ContainsKey(info.target) ? (AudioProfilerDSPView.AudioProfilerDSPNode) null : dictionary[info.target];
              if (audioProfilerDspNode1 != null)
              {
                dictionary[info.id] = new AudioProfilerDSPView.AudioProfilerDSPNode(audioProfilerDspNode1, info, audioProfilerDspNode1.x + num3 + num5, audioProfilerDspNode1.maxY, audioProfilerDspNode1.level + 1);
                audioProfilerDspNode1.maxY += num4 + num6;
                for (AudioProfilerDSPView.AudioProfilerDSPNode audioProfilerDspNode2 = audioProfilerDspNode1; audioProfilerDspNode2 != null; audioProfilerDspNode2 = audioProfilerDspNode2.firstTarget)
                  audioProfilerDspNode2.maxY = Mathf.Max(audioProfilerDspNode2.maxY, audioProfilerDspNode1.maxY);
              }
              else if (flag1)
              {
                flag1 = false;
                dictionary[info.id] = new AudioProfilerDSPView.AudioProfilerDSPNode(audioProfilerDspNode1, info, 10 + num3 / 2, 10 + num4 / 2, 1);
              }
              if (audioProfilerDspNode1 != null)
                audioProfilerDspWireList.Add(new AudioProfilerDSPView.AudioProfilerDSPWire(dictionary[info.id], audioProfilerDspNode1, info));
            }
            else
              audioProfilerDspWireList.Add(new AudioProfilerDSPView.AudioProfilerDSPWire(dictionary[info.id], dictionary[info.target], info));
          }
        }
        foreach (KeyValuePair<int, AudioProfilerDSPView.AudioProfilerDSPNode> keyValuePair in dictionary)
        {
          AudioProfilerDSPView.AudioProfilerDSPNode audioProfilerDspNode = keyValuePair.Value;
          audioProfilerDspNode.y += (audioProfilerDspNode.maxY != audioProfilerDspNode.y ? audioProfilerDspNode.maxY - audioProfilerDspNode.y : num4 + num6) / 2;
        }
        foreach (AudioProfilerDSPView.AudioProfilerDSPWire audioProfilerDspWire in audioProfilerDspWireList)
        {
          float num7 = 4f;
          AudioProfilerDSPView.AudioProfilerDSPNode source = audioProfilerDspWire.source;
          AudioProfilerDSPView.AudioProfilerDSPNode target = audioProfilerDspWire.target;
          AudioProfilerDSPInfo info = audioProfilerDspWire.info;
          Vector3 p1 = new Vector3(((float) source.x - (float) num3 * 0.5f) * zoomFactor, (float) source.y * zoomFactor, 0.0f);
          Vector3 p2 = new Vector3(((float) target.x + (float) num3 * 0.5f) * zoomFactor, ((float) target.y + (float) audioProfilerDspWire.targetPort * num7) * zoomFactor, 0.0f);
          if ((AudioProfilerDSPView.GetOutCode(p1, clippingRect) & AudioProfilerDSPView.GetOutCode(p2, clippingRect)) == 0)
          {
            float width = 3f;
            Handles.color = new Color(info.weight, 0.0f, 0.0f, !highlightAudibleDSPChains || source.audible ? 1f : 0.4f);
            Handles.DrawAAPolyLine(width, 2, p1, p2);
          }
        }
        foreach (AudioProfilerDSPView.AudioProfilerDSPWire audioProfilerDspWire in audioProfilerDspWireList)
        {
          AudioProfilerDSPView.AudioProfilerDSPNode source = audioProfilerDspWire.source;
          AudioProfilerDSPView.AudioProfilerDSPNode target = audioProfilerDspWire.target;
          AudioProfilerDSPInfo info = audioProfilerDspWire.info;
          if ((double) info.weight != 1.0)
          {
            int num7 = source.x - (num5 + num3) / 2;
            int num8 = target == null ? target.y : (int) ((double) target.y + ((double) (num7 - target.x) - (double) num3 * 0.5) * (double) (source.y - target.y) / (double) (source.x - target.x - num3));
            this.DrawRectClipped(new Rect((float) (num7 - num1 / 2), (float) (num8 - num2 / 2), (float) num1, (float) num2), new Color(1f, 0.3f, 0.2f, !highlightAudibleDSPChains || source.audible ? 1f : 0.4f), string.Format("{0:0.00}%", (object) (float) (100.0 * (double) info.weight)), clippingRect, zoomFactor);
          }
        }
        foreach (KeyValuePair<int, AudioProfilerDSPView.AudioProfilerDSPNode> keyValuePair in dictionary)
        {
          AudioProfilerDSPView.AudioProfilerDSPNode audioProfilerDspNode = keyValuePair.Value;
          AudioProfilerDSPInfo info = audioProfilerDspNode.info;
          if (dictionary.ContainsKey(info.target) && audioProfilerDspNode.firstTarget == dictionary[info.target])
          {
            string profilerNameByOffset = property.GetAudioProfilerNameByOffset(info.nameOffset);
            float num7 = 0.01f * info.cpuLoad;
            float num8 = 0.1f;
            bool flag2 = (info.flags & 1) != 0;
            bool flag3 = (info.flags & 2) != 0;
            Color col = new Color(!flag2 || flag3 ? 0.5f : Mathf.Clamp(2f * num8 * num7, 0.0f, 1f), !flag2 || flag3 ? 0.5f : Mathf.Clamp((float) (2.0 - 2.0 * (double) num8 * (double) num7), 0.0f, 1f), !flag3 ? (!flag2 ? 0.5f : 0.0f) : 1f, !highlightAudibleDSPChains || audioProfilerDspNode.audible ? 1f : 0.4f);
            string name = profilerNameByOffset.Replace("ChannelGroup", "Group").Replace("FMOD Channel", "Channel").Replace("FMOD WaveTable Unit", "Wavetable").Replace("FMOD Resampler Unit", "Resampler").Replace("FMOD Channel DSPHead Unit", "Channel DSP").Replace("FMOD Channel DSPHead Unit", "Channel DSP") + string.Format(" ({0:0.00}%)", (object) num7);
            this.DrawRectClipped(new Rect((float) audioProfilerDspNode.x - (float) num3 * 0.5f, (float) audioProfilerDspNode.y - (float) num4 * 0.5f, (float) num3, (float) num4), col, name, clippingRect, zoomFactor);
            if (audioProfilerDspNode.audible)
            {
              if (info.numLevels >= 1)
              {
                float height = (float) (num4 - 6) * Mathf.Clamp(info.level1, 0.0f, 1f);
                this.DrawRectClipped(new Rect((float) ((double) audioProfilerDspNode.x - (double) num3 * 0.5 + 3.0), (float) ((double) audioProfilerDspNode.y - (double) num4 * 0.5 + 3.0), 4f, (float) (num4 - 6)), Color.black, (string) null, clippingRect, zoomFactor);
                this.DrawRectClipped(new Rect((float) ((double) audioProfilerDspNode.x - (double) num3 * 0.5 + 3.0), (float) ((double) audioProfilerDspNode.y - (double) num4 * 0.5 - 3.0) + (float) num4 - height, 4f, height), Color.red, (string) null, clippingRect, zoomFactor);
              }
              if (info.numLevels >= 2)
              {
                float height = (float) (num4 - 6) * Mathf.Clamp(info.level2, 0.0f, 1f);
                this.DrawRectClipped(new Rect((float) ((double) audioProfilerDspNode.x - (double) num3 * 0.5 + 8.0), (float) ((double) audioProfilerDspNode.y - (double) num4 * 0.5 + 3.0), 4f, (float) (num4 - 6)), Color.black, (string) null, clippingRect, zoomFactor);
                this.DrawRectClipped(new Rect((float) ((double) audioProfilerDspNode.x - (double) num3 * 0.5 + 8.0), (float) ((double) audioProfilerDspNode.y - (double) num4 * 0.5 - 3.0) + (float) num4 - height, 4f, height), Color.red, (string) null, clippingRect, zoomFactor);
              }
            }
          }
        }
      }
    }

    private class AudioProfilerDSPNode
    {
      public AudioProfilerDSPView.AudioProfilerDSPNode firstTarget;
      public AudioProfilerDSPInfo info;
      public int x;
      public int y;
      public int level;
      public int maxY;
      public int targetPort;
      public bool audible;

      public AudioProfilerDSPNode(AudioProfilerDSPView.AudioProfilerDSPNode firstTarget, AudioProfilerDSPInfo info, int x, int y, int level)
      {
        this.firstTarget = firstTarget;
        this.info = info;
        this.x = x;
        this.y = y;
        this.level = level;
        this.maxY = y;
        this.audible = (info.flags & 1) != 0 && (info.flags & 2) == 0;
        if (firstTarget == null)
          return;
        this.audible &= firstTarget.audible;
      }
    }

    private class AudioProfilerDSPWire
    {
      public AudioProfilerDSPView.AudioProfilerDSPNode source;
      public AudioProfilerDSPView.AudioProfilerDSPNode target;
      public AudioProfilerDSPInfo info;
      public int targetPort;

      public AudioProfilerDSPWire(AudioProfilerDSPView.AudioProfilerDSPNode source, AudioProfilerDSPView.AudioProfilerDSPNode target, AudioProfilerDSPInfo info)
      {
        this.source = source;
        this.target = target;
        this.info = info;
        this.targetPort = target.targetPort;
      }
    }
  }
}
