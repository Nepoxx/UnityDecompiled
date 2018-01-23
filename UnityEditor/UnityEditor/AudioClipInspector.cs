// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioClipInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AudioClip))]
  [CanEditMultipleObjects]
  internal class AudioClipInspector : Editor
  {
    private static GUIContent[] s_PlayIcons = new GUIContent[2];
    private static GUIContent[] s_AutoPlayIcons = new GUIContent[2];
    private static GUIContent[] s_LoopIcons = new GUIContent[2];
    private Vector2 m_Position = Vector2.zero;
    private PreviewRenderUtility m_PreviewUtility;
    private static AudioClipInspector m_PlayingInspector;
    private static AudioClip m_PlayingClip;
    private static bool m_bAutoPlay;
    private static bool m_bLoop;
    private Rect m_wantedRect;
    private static GUIStyle s_PreButton;
    private static Texture2D s_DefaultIcon;

    private static bool playing
    {
      get
      {
        return (UnityEngine.Object) AudioClipInspector.m_PlayingClip != (UnityEngine.Object) null && AudioUtil.IsClipPlaying(AudioClipInspector.m_PlayingClip);
      }
    }

    public override void OnInspectorGUI()
    {
    }

    private static void Init()
    {
      if (AudioClipInspector.s_PreButton != null)
        return;
      AudioClipInspector.s_PreButton = (GUIStyle) "preButton";
      AudioClipInspector.m_bAutoPlay = EditorPrefs.GetBool("AutoPlayAudio", false);
      AudioClipInspector.s_AutoPlayIcons[0] = EditorGUIUtility.IconContent("preAudioAutoPlayOff", "|Turn Auto Play on");
      AudioClipInspector.s_AutoPlayIcons[1] = EditorGUIUtility.IconContent("preAudioAutoPlayOn", "|Turn Auto Play off");
      AudioClipInspector.s_PlayIcons[0] = EditorGUIUtility.IconContent("preAudioPlayOff", "|Play");
      AudioClipInspector.s_PlayIcons[1] = EditorGUIUtility.IconContent("preAudioPlayOn", "|Stop");
      AudioClipInspector.s_LoopIcons[0] = EditorGUIUtility.IconContent("preAudioLoopOff", "|Loop on");
      AudioClipInspector.s_LoopIcons[1] = EditorGUIUtility.IconContent("preAudioLoopOn", "|Loop off");
      AudioClipInspector.s_DefaultIcon = EditorGUIUtility.LoadIcon("Profiler.Audio");
    }

    public void OnDisable()
    {
      if ((UnityEngine.Object) AudioClipInspector.m_PlayingInspector == (UnityEngine.Object) this)
      {
        AudioUtil.StopAllClips();
        AudioClipInspector.m_PlayingClip = (AudioClip) null;
      }
      EditorPrefs.SetBool("AutoPlayAudio", AudioClipInspector.m_bAutoPlay);
    }

    public void OnEnable()
    {
      AudioUtil.StopAllClips();
      AudioClipInspector.m_PlayingClip = (AudioClip) null;
      AudioClipInspector.m_PlayingInspector = this;
      AudioClipInspector.m_bAutoPlay = EditorPrefs.GetBool("AutoPlayAudio", false);
    }

    public void OnDestroy()
    {
      if (this.m_PreviewUtility == null)
        return;
      this.m_PreviewUtility.Cleanup();
      this.m_PreviewUtility = (PreviewRenderUtility) null;
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      AudioClip target = this.target as AudioClip;
      AudioImporter atPath = AssetImporter.GetAtPath(assetPath) as AudioImporter;
      if ((UnityEngine.Object) atPath == (UnityEngine.Object) null || !ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      if (this.m_PreviewUtility == null)
        this.m_PreviewUtility = new PreviewRenderUtility();
      this.m_PreviewUtility.BeginStaticPreview(new Rect(0.0f, 0.0f, (float) width, (float) height));
      this.DoRenderPreview(target, atPath, new Rect(0.05f * (float) width * EditorGUIUtility.pixelsPerPoint, 0.05f * (float) width * EditorGUIUtility.pixelsPerPoint, 1.9f * (float) width * EditorGUIUtility.pixelsPerPoint, 1.9f * (float) height * EditorGUIUtility.pixelsPerPoint), 1f);
      return this.m_PreviewUtility.EndStaticPreview();
    }

    public override bool HasPreviewGUI()
    {
      return this.targets != null;
    }

    public override void OnPreviewSettings()
    {
      if ((UnityEngine.Object) AudioClipInspector.s_DefaultIcon == (UnityEngine.Object) null)
        AudioClipInspector.Init();
      AudioClip target = this.target as AudioClip;
      using (new EditorGUI.DisabledScope(AudioUtil.IsMovieAudio(target)))
      {
        bool disabled = this.targets.Length > 1;
        using (new EditorGUI.DisabledScope(disabled))
        {
          bool flag1 = !disabled && AudioClipInspector.m_bAutoPlay;
          bool flag2 = PreviewGUI.CycleButton(!flag1 ? 0 : 1, AudioClipInspector.s_AutoPlayIcons) != 0;
          if (flag1 != flag2)
          {
            AudioClipInspector.m_bAutoPlay = flag2;
            InspectorWindow.RepaintAllInspectors();
          }
          bool flag3 = !disabled && AudioClipInspector.m_bLoop;
          bool on = PreviewGUI.CycleButton(!flag3 ? 0 : 1, AudioClipInspector.s_LoopIcons) != 0;
          if (flag3 != on)
          {
            AudioClipInspector.m_bLoop = on;
            if (AudioClipInspector.playing)
              AudioUtil.LoopClip(target, on);
            InspectorWindow.RepaintAllInspectors();
          }
        }
        using (new EditorGUI.DisabledScope(disabled && !AudioClipInspector.playing && (UnityEngine.Object) AudioClipInspector.m_PlayingInspector != (UnityEngine.Object) this))
        {
          bool flag1 = (UnityEngine.Object) AudioClipInspector.m_PlayingInspector == (UnityEngine.Object) this && AudioClipInspector.playing;
          bool flag2 = PreviewGUI.CycleButton(!flag1 ? 0 : 1, AudioClipInspector.s_PlayIcons) != 0;
          if (flag2 == flag1)
            return;
          AudioUtil.StopAllClips();
          if (flag2)
          {
            AudioUtil.PlayClip(target, 0, AudioClipInspector.m_bLoop);
            AudioClipInspector.m_PlayingClip = target;
            AudioClipInspector.m_PlayingInspector = this;
          }
        }
      }
    }

    private void DoRenderPreview(AudioClip clip, AudioImporter audioImporter, Rect wantedRect, float scaleFactor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioClipInspector.\u003CDoRenderPreview\u003Ec__AnonStorey1 previewCAnonStorey1 = new AudioClipInspector.\u003CDoRenderPreview\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey1.scaleFactor = scaleFactor;
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey1.scaleFactor *= 0.95f;
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey1.minMaxData = !((UnityEngine.Object) audioImporter == (UnityEngine.Object) null) ? AudioUtil.GetMinMaxData(audioImporter) : (float[]) null;
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey1.numChannels = clip.channels;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey1.numSamples = previewCAnonStorey1.minMaxData != null ? previewCAnonStorey1.minMaxData.Length / (2 * previewCAnonStorey1.numChannels) : 0;
      // ISSUE: reference to a compiler-generated field
      float height = wantedRect.height / (float) previewCAnonStorey1.numChannels;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioClipInspector.\u003CDoRenderPreview\u003Ec__AnonStorey2 previewCAnonStorey2 = new AudioClipInspector.\u003CDoRenderPreview\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (previewCAnonStorey2.channel = 0; previewCAnonStorey2.channel < previewCAnonStorey1.numChannels; ++previewCAnonStorey2.channel)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AudioClipInspector.\u003CDoRenderPreview\u003Ec__AnonStorey0 previewCAnonStorey0 = new AudioClipInspector.\u003CDoRenderPreview\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        previewCAnonStorey0.\u003C\u003Ef__ref\u00241 = previewCAnonStorey1;
        // ISSUE: reference to a compiler-generated field
        previewCAnonStorey0.\u003C\u003Ef__ref\u00242 = previewCAnonStorey2;
        // ISSUE: reference to a compiler-generated field
        Rect r = new Rect(wantedRect.x, wantedRect.y + height * (float) previewCAnonStorey2.channel, wantedRect.width, height);
        // ISSUE: reference to a compiler-generated field
        previewCAnonStorey0.curveColor = new Color(1f, 0.5490196f, 0.0f, 1f);
        // ISSUE: reference to a compiler-generated method
        AudioCurveRendering.DrawMinMaxFilledCurve(r, new AudioCurveRendering.AudioMinMaxCurveAndColorEvaluator(previewCAnonStorey0.\u003C\u003Em__0));
      }
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if ((UnityEngine.Object) AudioClipInspector.s_DefaultIcon == (UnityEngine.Object) null)
        AudioClipInspector.Init();
      AudioClip target = this.target as AudioClip;
      Event current = Event.current;
      if (current.type != EventType.Repaint && current.type != EventType.Layout && current.type != EventType.Used)
      {
        int num = AudioUtil.GetSampleCount(target) / (int) r.width;
        switch (current.type)
        {
          case EventType.MouseDown:
          case EventType.MouseDrag:
            if (!r.Contains(current.mousePosition) || AudioUtil.IsMovieAudio(target))
              break;
            if ((UnityEngine.Object) AudioClipInspector.m_PlayingClip != (UnityEngine.Object) target || !AudioUtil.IsClipPlaying(target))
            {
              AudioUtil.StopAllClips();
              AudioUtil.PlayClip(target, 0, AudioClipInspector.m_bLoop);
              AudioClipInspector.m_PlayingClip = target;
              AudioClipInspector.m_PlayingInspector = this;
            }
            AudioUtil.SetClipSamplePosition(target, num * (int) current.mousePosition.x);
            current.Use();
            break;
        }
      }
      else
      {
        if (Event.current.type == EventType.Repaint)
          background.Draw(r, false, false, false, false);
        int channelCount = AudioUtil.GetChannelCount(target);
        this.m_wantedRect = new Rect(r.x, r.y, r.width, r.height);
        float num = this.m_wantedRect.width / target.length;
        if (!AudioUtil.HasPreview(target) && (AudioUtil.IsTrackerFile(target) ? 1 : (AudioUtil.IsMovieAudio(target) ? 1 : 0)) != 0)
        {
          float y = (double) r.height <= 150.0 ? (float) ((double) r.y + (double) r.height / 2.0 - 25.0) : (float) ((double) r.y + (double) r.height / 2.0 - 10.0);
          if ((double) r.width > 64.0)
          {
            if (AudioUtil.IsTrackerFile(target))
              EditorGUI.DropShadowLabel(new Rect(r.x, y, r.width, 20f), string.Format("Module file with " + (object) AudioUtil.GetMusicChannelCount(target) + " channels."));
            else if (AudioUtil.IsMovieAudio(target))
            {
              if ((double) r.width > 450.0)
              {
                EditorGUI.DropShadowLabel(new Rect(r.x, y, r.width, 20f), "Audio is attached to a movie. To audition the sound, play the movie.");
              }
              else
              {
                EditorGUI.DropShadowLabel(new Rect(r.x, y, r.width, 20f), "Audio is attached to a movie.");
                EditorGUI.DropShadowLabel(new Rect(r.x, y + 10f, r.width, 20f), "To audition the sound, play the movie.");
              }
            }
            else
              EditorGUI.DropShadowLabel(new Rect(r.x, y, r.width, 20f), "Can not show PCM data for this file");
          }
          if ((UnityEngine.Object) AudioClipInspector.m_PlayingInspector == (UnityEngine.Object) this && (UnityEngine.Object) AudioClipInspector.m_PlayingClip == (UnityEngine.Object) target)
          {
            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, (int) ((double) AudioUtil.GetClipPosition(target) * 1000.0));
            EditorGUI.DropShadowLabel(new Rect(this.m_wantedRect.x, this.m_wantedRect.y, this.m_wantedRect.width, 20f), string.Format("Playing - {0:00}:{1:00}.{2:000}", (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds));
          }
        }
        else
        {
          PreviewGUI.BeginScrollView(this.m_wantedRect, this.m_Position, this.m_wantedRect, (GUIStyle) "PreHorizontalScrollbar", (GUIStyle) "PreHorizontalScrollbarThumb");
          if (Event.current.type == EventType.Repaint)
            this.DoRenderPreview(target, AudioUtil.GetImporterFromClip(target), this.m_wantedRect, 1f);
          for (int index = 0; index < channelCount; ++index)
          {
            if (channelCount > 1 && (double) r.width > 64.0)
              EditorGUI.DropShadowLabel(new Rect(this.m_wantedRect.x + 5f, this.m_wantedRect.y + this.m_wantedRect.height / (float) channelCount * (float) index, 30f, 20f), "ch " + (index + 1).ToString());
          }
          if ((UnityEngine.Object) AudioClipInspector.m_PlayingInspector == (UnityEngine.Object) this && (UnityEngine.Object) AudioClipInspector.m_PlayingClip == (UnityEngine.Object) target)
          {
            float clipPosition = AudioUtil.GetClipPosition(target);
            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, (int) ((double) clipPosition * 1000.0));
            GUI.DrawTexture(new Rect(this.m_wantedRect.x + (float) (int) ((double) num * (double) clipPosition), this.m_wantedRect.y, 2f, this.m_wantedRect.height), (Texture) EditorGUIUtility.whiteTexture);
            if ((double) r.width > 64.0)
              EditorGUI.DropShadowLabel(new Rect(this.m_wantedRect.x, this.m_wantedRect.y, this.m_wantedRect.width, 20f), string.Format("{0:00}:{1:00}.{2:000}", (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds));
            else
              EditorGUI.DropShadowLabel(new Rect(this.m_wantedRect.x, this.m_wantedRect.y, this.m_wantedRect.width, 20f), string.Format("{0:00}:{1:00}", (object) timeSpan.Minutes, (object) timeSpan.Seconds));
          }
          PreviewGUI.EndScrollView();
        }
        if (AudioClipInspector.m_bAutoPlay && (UnityEngine.Object) AudioClipInspector.m_PlayingClip != (UnityEngine.Object) target && (UnityEngine.Object) AudioClipInspector.m_PlayingInspector == (UnityEngine.Object) this)
        {
          AudioUtil.StopAllClips();
          AudioUtil.PlayClip(target, 0, AudioClipInspector.m_bLoop);
          AudioClipInspector.m_PlayingClip = target;
          AudioClipInspector.m_PlayingInspector = this;
        }
        if (!AudioClipInspector.playing)
          return;
        GUIView.current.Repaint();
      }
    }

    public override string GetInfoString()
    {
      AudioClip target = this.target as AudioClip;
      int channelCount = AudioUtil.GetChannelCount(target);
      string str1;
      switch (channelCount)
      {
        case 1:
          str1 = "Mono";
          break;
        case 2:
          str1 = "Stereo";
          break;
        default:
          str1 = (channelCount - 1).ToString() + ".1";
          break;
      }
      string str2 = str1;
      AudioCompressionFormat compressionFormat1 = AudioUtil.GetTargetPlatformSoundCompressionFormat(target);
      AudioCompressionFormat compressionFormat2 = AudioUtil.GetSoundCompressionFormat(target);
      string str3 = compressionFormat1.ToString();
      if (compressionFormat1 != compressionFormat2)
        str3 = str3 + " (" + compressionFormat2.ToString() + " in editor)";
      string str4 = str3 + ", " + (object) AudioUtil.GetFrequency(target) + " Hz, " + str2 + ", ";
      TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, (int) AudioUtil.GetDuration(target));
      return (int) (uint) AudioUtil.GetDuration(target) != -1 ? str4 + string.Format("{0:00}:{1:00}.{2:000}", (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds) : str4 + "Unlimited";
    }
  }
}
