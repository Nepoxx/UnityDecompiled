// Decompiled with JetBrains decompiler
// Type: UnityEditor.StreamedAudioClipPreview
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class StreamedAudioClipPreview : WaveformPreview
  {
    private Dictionary<WaveformStreamer, StreamedAudioClipPreview.StreamingContext> m_Contexts = new Dictionary<WaveformStreamer, StreamedAudioClipPreview.StreamingContext>();
    private StreamedAudioClipPreview.Segment[] m_StreamedSegments;
    private AudioClip m_Clip;

    public StreamedAudioClipPreview(AudioClip clip, int initialSize)
      : base((UnityEngine.Object) clip, initialSize, clip.channels)
    {
      this.m_ClearTexture = false;
      this.m_Clip = clip;
      this.m_Start = 0.0;
      this.m_Length = (double) clip.length;
    }

    protected override void InternalDispose()
    {
      base.InternalDispose();
      this.KillAndClearStreamers();
      this.m_StreamedSegments = (StreamedAudioClipPreview.Segment[]) null;
    }

    protected override void OnModifications(WaveformPreview.MessageFlags cFlags)
    {
      bool flag = false;
      if (WaveformPreview.HasFlag(cFlags, WaveformPreview.MessageFlags.TextureChanged) || WaveformPreview.HasFlag(cFlags, WaveformPreview.MessageFlags.Size) || (WaveformPreview.HasFlag(cFlags, WaveformPreview.MessageFlags.Length) || WaveformPreview.HasFlag(cFlags, WaveformPreview.MessageFlags.Looping)))
      {
        this.KillAndClearStreamers();
        if (this.length <= 0.0)
          return;
        StreamedAudioClipPreview.ClipPreviewDetails details = new StreamedAudioClipPreview.ClipPreviewDetails(this.m_Clip, this.looping, (int) this.Size.x, this.start, this.length);
        this.UploadPreview(details);
        if (details.IsCandidateForStreaming())
          flag = true;
      }
      if (!this.optimized)
      {
        this.KillAndClearStreamers();
        flag = false;
      }
      else if (WaveformPreview.HasFlag(cFlags, WaveformPreview.MessageFlags.Optimization) && !flag && new StreamedAudioClipPreview.ClipPreviewDetails(this.m_Clip, this.looping, (int) this.Size.x, this.start, this.length).IsCandidateForStreaming())
        flag = true;
      if (flag)
      {
        this.m_StreamedSegments = this.CalculateAndStartStreamers(this.start, this.length);
        if (this.m_StreamedSegments != null && this.m_StreamedSegments.Length > 0)
        {
          foreach (StreamedAudioClipPreview.Segment streamedSegment in this.m_StreamedSegments)
          {
            if (!this.m_Contexts.ContainsKey(streamedSegment.streamer))
              this.m_Contexts.Add(streamedSegment.streamer, new StreamedAudioClipPreview.StreamingContext());
          }
        }
      }
      base.OnModifications(cFlags);
    }

    private void KillAndClearStreamers()
    {
      foreach (KeyValuePair<WaveformStreamer, StreamedAudioClipPreview.StreamingContext> context in this.m_Contexts)
        context.Key.Stop();
      this.m_Contexts.Clear();
    }

    private StreamedAudioClipPreview.Segment[] CalculateAndStartStreamers(double localStart, double localLength)
    {
      double start = localStart;
      localStart %= (double) this.m_Clip.length;
      double num1 = localLength / (double) this.Size.x;
      if (!this.looping)
      {
        if (start > (double) this.m_Clip.length)
          return (StreamedAudioClipPreview.Segment[]) null;
        double duration = Math.Min((double) this.m_Clip.length - start, localLength);
        int numOutputSamples = (int) Math.Min((double) this.Size.x, (double) this.Size.x * Math.Max(0.0, duration / localLength));
        if (numOutputSamples < 1)
          return (StreamedAudioClipPreview.Segment[]) null;
        StreamedAudioClipPreview.Segment[] segmentArray = new StreamedAudioClipPreview.Segment[1];
        segmentArray[0].streamer = new WaveformStreamer(this.m_Clip, start, duration, numOutputSamples, new Func<WaveformStreamer, float[], int, bool>(this.OnNewWaveformData));
        segmentArray[0].segmentLength = (int) this.Size.x;
        segmentArray[0].textureOffset = 0;
        segmentArray[0].streamingIndexOffset = 0;
        return segmentArray;
      }
      StreamedAudioClipPreview.Segment[] segmentArray1;
      if (localStart + localLength - num1 > (double) this.m_Clip.length)
      {
        double num2 = (double) this.Size.x / localLength;
        if (localLength >= (double) this.m_Clip.length)
        {
          WaveformStreamer waveformStreamer = new WaveformStreamer(this.m_Clip, 0.0, (double) this.m_Clip.length, (int) ((double) this.Size.x / (localLength / (double) this.m_Clip.length)), new Func<WaveformStreamer, float[], int, bool>(this.OnNewWaveformData));
          double num3 = (double) this.m_Clip.length - localStart;
          double num4 = 0.0;
          segmentArray1 = new StreamedAudioClipPreview.Segment[Mathf.CeilToInt((float) (localStart + localLength) / this.m_Clip.length)];
          for (int index = 0; index < segmentArray1.Length; ++index)
          {
            double num5 = Math.Min(num3 + num4, localLength) - num4;
            segmentArray1[index].streamer = waveformStreamer;
            segmentArray1[index].segmentLength = (int) (num5 * num2);
            segmentArray1[index].textureOffset = (int) (num4 * num2);
            segmentArray1[index].streamingIndexOffset = (int) (((double) this.m_Clip.length - num3) * num2);
            num4 += num3;
            num3 = (double) this.m_Clip.length;
          }
        }
        else
        {
          double duration1 = (double) this.m_Clip.length - localStart;
          double duration2 = localLength - duration1;
          segmentArray1 = new StreamedAudioClipPreview.Segment[2];
          segmentArray1[0].streamer = new WaveformStreamer(this.m_Clip, localStart, duration1, (int) (duration1 * num2), new Func<WaveformStreamer, float[], int, bool>(this.OnNewWaveformData));
          segmentArray1[0].segmentLength = (int) (duration1 * num2);
          segmentArray1[0].textureOffset = 0;
          segmentArray1[0].streamingIndexOffset = 0;
          segmentArray1[1].streamer = new WaveformStreamer(this.m_Clip, 0.0, duration2, (int) (duration2 * num2), new Func<WaveformStreamer, float[], int, bool>(this.OnNewWaveformData));
          segmentArray1[1].segmentLength = (int) (duration2 * num2);
          segmentArray1[1].textureOffset = (int) (duration1 * num2);
          segmentArray1[1].streamingIndexOffset = 0;
        }
      }
      else
      {
        segmentArray1 = new StreamedAudioClipPreview.Segment[1];
        segmentArray1[0].streamer = new WaveformStreamer(this.m_Clip, localStart, localLength, (int) this.Size.x, new Func<WaveformStreamer, float[], int, bool>(this.OnNewWaveformData));
        segmentArray1[0].segmentLength = (int) this.Size.x;
        segmentArray1[0].textureOffset = 0;
        segmentArray1[0].streamingIndexOffset = 0;
      }
      return segmentArray1;
    }

    private void UploadPreview(StreamedAudioClipPreview.ClipPreviewDetails details)
    {
      float[] numArray = new float[(int) ((double) details.clip.channels * (double) this.Size.x * 2.0)];
      if (details.localStart + details.localLength > (double) details.clip.length)
        this.ResamplePreviewLooped(details, numArray);
      else
        this.ResamplePreviewConfined(details, numArray);
      this.SetMMWaveData(0, numArray);
    }

    private void ResamplePreviewConfined(StreamedAudioClipPreview.ClipPreviewDetails details, float[] resampledPreview)
    {
      int channels = this.m_Clip.channels;
      int previewSamples = details.previewSamples;
      double deltaStep = details.deltaStep;
      double num1 = details.normalizedStart * (double) previewSamples;
      float[] preview = details.preview;
      if (deltaStep > 0.5)
      {
        int num2 = (int) num1;
        int num3 = num2;
        for (int index1 = 0; index1 < details.previewPixelsToRender; ++index1)
        {
          for (int index2 = 0; index2 < channels; ++index2)
          {
            int num4 = num2;
            num3 = (int) num1;
            float a1 = preview[2 * num4 * channels + index2 * 2];
            float a2 = preview[2 * num4 * channels + index2 * 2 + 1];
            while (++num4 < num3)
            {
              a1 = Mathf.Max(a1, preview[2 * num4 * channels + index2 * 2]);
              a2 = Mathf.Min(a2, preview[2 * num4 * channels + index2 * 2 + 1]);
            }
            resampledPreview[2 * index1 * channels + index2 * 2] = a2;
            resampledPreview[2 * index1 * channels + index2 * 2 + 1] = a1;
          }
          num1 += deltaStep;
          num2 = num3;
        }
      }
      else
      {
        for (int index1 = 0; index1 < details.previewPixelsToRender; ++index1)
        {
          int b = (int) (num1 - 1.0);
          int a = b + 1;
          float num2 = (float) (num1 - 1.0) - (float) b;
          int num3 = Mathf.Max(0, b);
          int num4 = Mathf.Min(a, previewSamples - 1);
          for (int index2 = 0; index2 < channels; ++index2)
          {
            float num5 = preview[2 * num3 * channels + index2 * 2];
            float num6 = preview[2 * num3 * channels + index2 * 2 + 1];
            float num7 = preview[2 * num4 * channels + index2 * 2];
            float num8 = preview[2 * num4 * channels + index2 * 2 + 1];
            resampledPreview[2 * index1 * channels + index2 * 2] = (float) ((double) num2 * (double) num8 + (1.0 - (double) num2) * (double) num6);
            resampledPreview[2 * index1 * channels + index2 * 2 + 1] = (float) ((double) num2 * (double) num7 + (1.0 - (double) num2) * (double) num5);
          }
          num1 += deltaStep;
        }
      }
    }

    private void ResamplePreviewLooped(StreamedAudioClipPreview.ClipPreviewDetails details, float[] resampledPreview)
    {
      int length = details.preview.Length;
      int channels = this.m_Clip.channels;
      int previewSamples = details.previewSamples;
      double deltaStep = details.deltaStep;
      double num1 = details.normalizedStart * (double) previewSamples;
      float[] preview = details.preview;
      if (deltaStep > 0.5)
      {
        int num2 = (int) num1;
        int num3 = num2;
        for (int index1 = 0; index1 < details.previewPixelsToRender; ++index1)
        {
          for (int index2 = 0; index2 < channels; ++index2)
          {
            int num4 = num2;
            num3 = (int) num1;
            int index3 = (2 * num4 * channels + index2 * 2) % length;
            float a1 = preview[index3];
            float a2 = preview[index3 + 1];
            while (++num4 < num3)
            {
              int index4 = (2 * num4 * channels + index2 * 2) % length;
              a1 = Mathf.Max(a1, preview[index4]);
              a2 = Mathf.Min(a2, preview[index4 + 1]);
            }
            resampledPreview[2 * index1 * channels + index2 * 2] = a2;
            resampledPreview[2 * index1 * channels + index2 * 2 + 1] = a1;
          }
          num1 += deltaStep;
          num2 = num3;
        }
      }
      else
      {
        for (int index1 = 0; index1 < details.previewPixelsToRender; ++index1)
        {
          int num2 = (int) (num1 - 1.0);
          int num3 = num2 + 1;
          float num4 = (float) (num1 - 1.0) - (float) num2;
          for (int index2 = 0; index2 < channels; ++index2)
          {
            int index3 = (2 * num2 * channels + index2 * 2) % length;
            float num5 = preview[index3];
            float num6 = preview[index3 + 1];
            int index4 = (2 * num3 * channels + index2 * 2) % length;
            float num7 = preview[index4];
            float num8 = preview[index4 + 1];
            resampledPreview[2 * index1 * channels + index2 * 2] = (float) ((double) num4 * (double) num8 + (1.0 - (double) num4) * (double) num6);
            resampledPreview[2 * index1 * channels + index2 * 2 + 1] = (float) ((double) num4 * (double) num7 + (1.0 - (double) num4) * (double) num5);
          }
          num1 += deltaStep;
        }
      }
    }

    private bool OnNewWaveformData(WaveformStreamer streamer, float[] data, int remaining)
    {
      StreamedAudioClipPreview.StreamingContext context = this.m_Contexts[streamer];
      int num = context.index / this.m_Clip.channels;
      for (int index = 0; index < this.m_StreamedSegments.Length; ++index)
      {
        if (this.m_StreamedSegments[index].streamer == streamer && num >= this.m_StreamedSegments[index].streamingIndexOffset && this.m_StreamedSegments[index].segmentLength > num - this.m_StreamedSegments[index].streamingIndexOffset)
          this.SetMMWaveData((this.m_StreamedSegments[index].textureOffset - this.m_StreamedSegments[index].streamingIndexOffset) * this.m_Clip.channels + context.index, data);
      }
      context.index += data.Length / 2;
      return remaining != 0;
    }

    private static class AudioClipMinMaxOverview
    {
      private static Dictionary<AudioClip, float[]> s_Data = new Dictionary<AudioClip, float[]>();

      public static float[] GetOverviewFor(AudioClip clip)
      {
        if (!StreamedAudioClipPreview.AudioClipMinMaxOverview.s_Data.ContainsKey(clip))
        {
          string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object) clip);
          if (assetPath == null)
            return (float[]) null;
          AssetImporter atPath = AssetImporter.GetAtPath(assetPath);
          if ((UnityEngine.Object) atPath == (UnityEngine.Object) null)
            return (float[]) null;
          StreamedAudioClipPreview.AudioClipMinMaxOverview.s_Data[clip] = AudioUtil.GetMinMaxData(atPath as AudioImporter);
        }
        return StreamedAudioClipPreview.AudioClipMinMaxOverview.s_Data[clip];
      }
    }

    private struct ClipPreviewDetails
    {
      public float[] preview;
      public int previewSamples;
      public double normalizedDuration;
      public double normalizedStart;
      public double deltaStep;
      public AudioClip clip;
      public int previewPixelsToRender;
      public double localStart;
      public double localLength;
      public bool looping;

      public ClipPreviewDetails(AudioClip clip, bool isLooping, int size, double localStart, double localLength)
      {
        if (size < 2)
          throw new ArgumentException("Size has to be larger than 1");
        if (localLength <= 0.0)
          throw new ArgumentException("length has to be longer than zero", nameof (localLength));
        if (localStart < 0.0)
          throw new ArgumentException("localStart has to be positive", nameof (localStart));
        if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
          throw new ArgumentNullException(nameof (clip));
        this.clip = clip;
        this.preview = StreamedAudioClipPreview.AudioClipMinMaxOverview.GetOverviewFor(clip);
        if (this.preview == null)
          throw new ArgumentException("Clip " + (object) clip + "'s overview preview is null");
        this.looping = isLooping;
        this.localStart = localStart;
        this.localLength = localLength;
        if (this.looping)
        {
          this.previewPixelsToRender = size;
        }
        else
        {
          double num = Math.Min((double) clip.length - localStart, localLength);
          this.previewPixelsToRender = (int) Math.Min((double) size, (double) size * Math.Max(0.0, num / localLength));
        }
        this.previewSamples = this.preview.Length / (clip.channels * 2);
        this.normalizedDuration = localLength / (double) clip.length;
        this.normalizedStart = localStart / (double) clip.length;
        this.deltaStep = (double) this.previewSamples * this.normalizedDuration / (double) (size - 1);
      }

      public bool IsCandidateForStreaming()
      {
        if (!this.looping && this.localStart >= (double) this.clip.length)
          return false;
        return this.deltaStep < 0.5;
      }
    }

    private struct Segment
    {
      public WaveformStreamer streamer;
      public int streamingIndexOffset;
      public int textureOffset;
      public int segmentLength;
    }

    private class StreamingContext
    {
      public int index;
    }
  }
}
