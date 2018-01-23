// Decompiled with JetBrains decompiler
// Type: UnityEditor.VideoClipImporterSourcePreview
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [CustomPreview(typeof (VideoClipImporter))]
  internal class VideoClipImporterSourcePreview : ObjectPreview
  {
    private VideoClipImporterSourcePreview.Styles m_Styles = new VideoClipImporterSourcePreview.Styles();
    private GUIContent m_Title;
    private const float kLabelWidth = 120f;
    private const float kIndentWidth = 30f;
    private const float kValueWidth = 200f;

    public override GUIContent GetPreviewTitle()
    {
      if (this.m_Title == null)
        this.m_Title = new GUIContent("Source Info");
      return this.m_Title;
    }

    public override bool HasPreviewGUI()
    {
      VideoClipImporter target = this.target as VideoClipImporter;
      return (UnityEngine.Object) target != (UnityEngine.Object) null && !target.useLegacyImporter;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      VideoClipImporter target = (VideoClipImporter) this.target;
      r = new RectOffset(-5, -5, -5, -5).Add(r);
      r.height = EditorGUIUtility.singleLineHeight;
      Rect labelRect = r;
      Rect valueRect = r;
      labelRect.width = 120f;
      valueRect.xMin += 120f;
      valueRect.width = 200f;
      this.ShowProperty(ref labelRect, ref valueRect, "Original Size", EditorUtility.FormatBytes((long) target.sourceFileSize));
      this.ShowProperty(ref labelRect, ref valueRect, "Imported Size", EditorUtility.FormatBytes((long) target.outputFileSize));
      int frameCount = target.frameCount;
      double frameRate = target.frameRate;
      string str1 = frameRate <= 0.0 ? new TimeSpan(0L).ToString() : TimeSpan.FromSeconds((double) frameCount / frameRate).ToString();
      if (str1.IndexOf('.') != -1)
        str1 = str1.Substring(0, str1.Length - 4);
      this.ShowProperty(ref labelRect, ref valueRect, "Duration", str1);
      this.ShowProperty(ref labelRect, ref valueRect, "Frames", frameCount.ToString());
      this.ShowProperty(ref labelRect, ref valueRect, "FPS", frameRate.ToString("F2"));
      int resizeWidth = target.GetResizeWidth(VideoResizeMode.OriginalSize);
      int resizeHeight = target.GetResizeHeight(VideoResizeMode.OriginalSize);
      this.ShowProperty(ref labelRect, ref valueRect, "Pixels", resizeWidth.ToString() + "x" + (object) resizeHeight);
      this.ShowProperty(ref labelRect, ref valueRect, "PAR", target.pixelAspectRatioNumerator.ToString() + ":" + (object) target.pixelAspectRatioDenominator);
      this.ShowProperty(ref labelRect, ref valueRect, "Alpha", !target.sourceHasAlpha ? "No" : "Yes");
      ushort sourceAudioTrackCount = target.sourceAudioTrackCount;
      string label = "Audio";
      string str2;
      switch (sourceAudioTrackCount)
      {
        case 0:
          str2 = "none";
          break;
        case 1:
          str2 = this.GetAudioTrackDescription(target, (ushort) 0);
          break;
        default:
          str2 = "";
          break;
      }
      this.ShowProperty(ref labelRect, ref valueRect, label, str2);
      if ((int) sourceAudioTrackCount <= 1)
        return;
      labelRect.xMin += 30f;
      labelRect.width -= 30f;
      for (ushort audioTrackIdx = 0; (int) audioTrackIdx < (int) sourceAudioTrackCount; ++audioTrackIdx)
        this.ShowProperty(ref labelRect, ref valueRect, "Track #" + (object) ((int) audioTrackIdx + 1), this.GetAudioTrackDescription(target, audioTrackIdx));
    }

    private string GetAudioTrackDescription(VideoClipImporter importer, ushort audioTrackIdx)
    {
      ushort audioChannelCount = importer.GetSourceAudioChannelCount(audioTrackIdx);
      string str1;
      switch (audioChannelCount)
      {
        case 0:
          str1 = "No channels";
          break;
        case 1:
          str1 = "Mono";
          break;
        case 2:
          str1 = "Stereo";
          break;
        case 4:
          str1 = audioChannelCount.ToString();
          break;
        default:
          str1 = ((int) audioChannelCount - 1).ToString() + ".1";
          break;
      }
      string str2 = str1;
      return ((int) importer.GetSourceAudioSampleRate(audioTrackIdx)).ToString() + " Hz, " + str2;
    }

    private void ShowProperty(ref Rect labelRect, ref Rect valueRect, string label, string value)
    {
      GUI.Label(labelRect, label, this.m_Styles.labelStyle);
      GUI.Label(valueRect, value, this.m_Styles.labelStyle);
      labelRect.y += EditorGUIUtility.singleLineHeight;
      valueRect.y += EditorGUIUtility.singleLineHeight;
    }

    private class Styles
    {
      public GUIStyle labelStyle = new GUIStyle(EditorStyles.label);

      public Styles()
      {
        Color color = new Color(0.7f, 0.7f, 0.7f);
        this.labelStyle.padding.right += 4;
        this.labelStyle.normal.textColor = color;
      }
    }
  }
}
