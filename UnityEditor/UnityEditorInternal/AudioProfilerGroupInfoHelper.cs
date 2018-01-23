// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerGroupInfoHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AudioProfilerGroupInfoHelper
  {
    public const int AUDIOPROFILER_FLAGS_3D = 1;
    public const int AUDIOPROFILER_FLAGS_ISSPATIAL = 2;
    public const int AUDIOPROFILER_FLAGS_PAUSED = 4;
    public const int AUDIOPROFILER_FLAGS_MUTED = 8;
    public const int AUDIOPROFILER_FLAGS_VIRTUAL = 16;
    public const int AUDIOPROFILER_FLAGS_ONESHOT = 32;
    public const int AUDIOPROFILER_FLAGS_GROUP = 64;
    public const int AUDIOPROFILER_FLAGS_STREAM = 128;
    public const int AUDIOPROFILER_FLAGS_COMPRESSED = 256;
    public const int AUDIOPROFILER_FLAGS_LOOPED = 512;
    public const int AUDIOPROFILER_FLAGS_OPENMEMORY = 1024;
    public const int AUDIOPROFILER_FLAGS_OPENMEMORYPOINT = 2048;
    public const int AUDIOPROFILER_FLAGS_OPENUSER = 4096;
    public const int AUDIOPROFILER_FLAGS_NONBLOCKING = 8192;

    private static string FormatDb(float vol)
    {
      if ((double) vol == 0.0)
        return "-∞ dB";
      return string.Format("{0:0.00} dB", (object) (float) (20.0 * (double) Mathf.Log10(vol)));
    }

    public static string GetColumnString(AudioProfilerGroupInfoWrapper info, AudioProfilerGroupInfoHelper.ColumnIndices index)
    {
      bool flag1 = (info.info.flags & 1) != 0;
      bool flag2 = (info.info.flags & 64) != 0;
      switch (index)
      {
        case AudioProfilerGroupInfoHelper.ColumnIndices.ObjectName:
          return info.objectName;
        case AudioProfilerGroupInfoHelper.ColumnIndices.AssetName:
          return info.assetName;
        case AudioProfilerGroupInfoHelper.ColumnIndices.Volume:
          return AudioProfilerGroupInfoHelper.FormatDb(info.info.volume);
        case AudioProfilerGroupInfoHelper.ColumnIndices.Audibility:
          return !flag2 ? AudioProfilerGroupInfoHelper.FormatDb(info.info.audibility) : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.PlayCount:
          return !flag2 ? info.info.playCount.ToString() : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.Is3D:
          return !flag2 ? (!flag1 ? "NO" : ((info.info.flags & 2) == 0 ? "YES" : "Spatial")) : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsPaused:
          return !flag2 ? ((info.info.flags & 4) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsMuted:
          return !flag2 ? ((info.info.flags & 8) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsVirtual:
          return !flag2 ? ((info.info.flags & 16) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsOneShot:
          return !flag2 ? ((info.info.flags & 32) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsLooped:
          return !flag2 ? ((info.info.flags & 512) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.DistanceToListener:
          return !flag2 ? (flag1 ? ((double) info.info.distanceToListener < 1000.0 ? string.Format("{0:0.00} m", (object) info.info.distanceToListener) : string.Format("{0:0.00} km", (object) (float) ((double) info.info.distanceToListener * (1.0 / 1000.0)))) : "N/A") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.MinDist:
          return !flag2 ? (flag1 ? ((double) info.info.minDist < 1000.0 ? string.Format("{0:0.00} m", (object) info.info.minDist) : string.Format("{0:0.00} km", (object) (float) ((double) info.info.minDist * (1.0 / 1000.0)))) : "N/A") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.MaxDist:
          return !flag2 ? (flag1 ? ((double) info.info.maxDist < 1000.0 ? string.Format("{0:0.00} m", (object) info.info.maxDist) : string.Format("{0:0.00} km", (object) (float) ((double) info.info.maxDist * (1.0 / 1000.0)))) : "N/A") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.Time:
          return !flag2 ? string.Format("{0:0.00} s", (object) info.info.time) : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.Duration:
          return !flag2 ? string.Format("{0:0.00} s", (object) info.info.duration) : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.Frequency:
          return !flag2 ? ((double) info.info.frequency < 1000.0 ? string.Format("{0:0.00} Hz", (object) info.info.frequency) : string.Format("{0:0.00} kHz", (object) (float) ((double) info.info.frequency * (1.0 / 1000.0)))) : string.Format("{0:0.00} x", (object) info.info.frequency);
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsStream:
          return !flag2 ? ((info.info.flags & 128) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsCompressed:
          return !flag2 ? ((info.info.flags & 256) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsNonBlocking:
          return !flag2 ? ((info.info.flags & 8192) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsOpenUser:
          return !flag2 ? ((info.info.flags & 4096) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsOpenMemory:
          return !flag2 ? ((info.info.flags & 1024) == 0 ? "NO" : "YES") : "";
        case AudioProfilerGroupInfoHelper.ColumnIndices.IsOpenMemoryPoint:
          return !flag2 ? ((info.info.flags & 2048) == 0 ? "NO" : "YES") : "";
        default:
          return "Unknown";
      }
    }

    public static int GetLastColumnIndex()
    {
      return !Unsupported.IsDeveloperBuild() ? 15 : 22;
    }

    public enum ColumnIndices
    {
      ObjectName,
      AssetName,
      Volume,
      Audibility,
      PlayCount,
      Is3D,
      IsPaused,
      IsMuted,
      IsVirtual,
      IsOneShot,
      IsLooped,
      DistanceToListener,
      MinDist,
      MaxDist,
      Time,
      Duration,
      Frequency,
      IsStream,
      IsCompressed,
      IsNonBlocking,
      IsOpenUser,
      IsOpenMemory,
      IsOpenMemoryPoint,
      _LastColumn,
    }

    public class AudioProfilerGroupInfoComparer : IComparer<AudioProfilerGroupInfoWrapper>
    {
      public AudioProfilerGroupInfoHelper.ColumnIndices primarySortKey;
      public AudioProfilerGroupInfoHelper.ColumnIndices secondarySortKey;
      public bool sortByDescendingOrder;

      public AudioProfilerGroupInfoComparer(AudioProfilerGroupInfoHelper.ColumnIndices primarySortKey, AudioProfilerGroupInfoHelper.ColumnIndices secondarySortKey, bool sortByDescendingOrder)
      {
        this.primarySortKey = primarySortKey;
        this.secondarySortKey = secondarySortKey;
        this.sortByDescendingOrder = sortByDescendingOrder;
      }

      private int CompareInternal(AudioProfilerGroupInfoWrapper a, AudioProfilerGroupInfoWrapper b, AudioProfilerGroupInfoHelper.ColumnIndices key)
      {
        int num = 0;
        switch (key)
        {
          case AudioProfilerGroupInfoHelper.ColumnIndices.ObjectName:
            num = a.objectName.CompareTo(b.objectName);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.AssetName:
            num = a.assetName.CompareTo(b.assetName);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.Volume:
            num = a.info.volume.CompareTo(b.info.volume);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.Audibility:
            num = a.info.audibility.CompareTo(b.info.audibility);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.PlayCount:
            num = a.info.playCount.CompareTo(b.info.playCount);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.Is3D:
            num = (a.info.flags & 1).CompareTo(b.info.flags & 1) + (a.info.flags & 2).CompareTo(b.info.flags & 2) * 2;
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsPaused:
            num = (a.info.flags & 4).CompareTo(b.info.flags & 4);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsMuted:
            num = (a.info.flags & 8).CompareTo(b.info.flags & 8);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsVirtual:
            num = (a.info.flags & 16).CompareTo(b.info.flags & 16);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsOneShot:
            num = (a.info.flags & 32).CompareTo(b.info.flags & 32);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsLooped:
            num = (a.info.flags & 512).CompareTo(b.info.flags & 512);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.DistanceToListener:
            num = a.info.distanceToListener.CompareTo(b.info.distanceToListener);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.MinDist:
            num = a.info.minDist.CompareTo(b.info.minDist);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.MaxDist:
            num = a.info.maxDist.CompareTo(b.info.maxDist);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.Time:
            num = a.info.time.CompareTo(b.info.time);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.Duration:
            num = a.info.duration.CompareTo(b.info.duration);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.Frequency:
            num = a.info.frequency.CompareTo(b.info.frequency);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsStream:
            num = (a.info.flags & 128).CompareTo(b.info.flags & 128);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsCompressed:
            num = (a.info.flags & 256).CompareTo(b.info.flags & 256);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsNonBlocking:
            num = (a.info.flags & 8192).CompareTo(b.info.flags & 8192);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsOpenUser:
            num = (a.info.flags & 4096).CompareTo(b.info.flags & 4096);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsOpenMemory:
            num = (a.info.flags & 1024).CompareTo(b.info.flags & 1024);
            break;
          case AudioProfilerGroupInfoHelper.ColumnIndices.IsOpenMemoryPoint:
            num = (a.info.flags & 2048).CompareTo(b.info.flags & 2048);
            break;
        }
        return !this.sortByDescendingOrder ? num : -num;
      }

      public int Compare(AudioProfilerGroupInfoWrapper a, AudioProfilerGroupInfoWrapper b)
      {
        int num = this.CompareInternal(a, b, this.primarySortKey);
        return num != 0 ? num : this.CompareInternal(a, b, this.secondarySortKey);
      }
    }
  }
}
