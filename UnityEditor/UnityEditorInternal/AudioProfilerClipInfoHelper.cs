// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerClipInfoHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditorInternal
{
  internal class AudioProfilerClipInfoHelper
  {
    private static string[] m_LoadStateNames = new string[5]{ "Unloaded", "Loading Base", "Loading Sub", "Loaded", "Failed" };
    private static string[] m_InternalLoadStateNames = new string[3]{ "Pending", "Loaded", "Failed" };

    public static string GetColumnString(AudioProfilerClipInfoWrapper info, AudioProfilerClipInfoHelper.ColumnIndices index)
    {
      switch (index)
      {
        case AudioProfilerClipInfoHelper.ColumnIndices.AssetName:
          return info.assetName;
        case AudioProfilerClipInfoHelper.ColumnIndices.LoadState:
          return AudioProfilerClipInfoHelper.m_LoadStateNames[info.info.loadState];
        case AudioProfilerClipInfoHelper.ColumnIndices.InternalLoadState:
          return AudioProfilerClipInfoHelper.m_InternalLoadStateNames[info.info.internalLoadState];
        case AudioProfilerClipInfoHelper.ColumnIndices.Age:
          return info.info.age.ToString();
        case AudioProfilerClipInfoHelper.ColumnIndices.Disposed:
          return info.info.disposed == 0 ? "NO" : "YES";
        case AudioProfilerClipInfoHelper.ColumnIndices.NumChannelInstances:
          return info.info.numChannelInstances.ToString();
        default:
          return "Unknown";
      }
    }

    public static int GetLastColumnIndex()
    {
      return 5;
    }

    public enum ColumnIndices
    {
      AssetName,
      LoadState,
      InternalLoadState,
      Age,
      Disposed,
      NumChannelInstances,
      _LastColumn,
    }

    public class AudioProfilerClipInfoComparer : IComparer<AudioProfilerClipInfoWrapper>
    {
      public AudioProfilerClipInfoHelper.ColumnIndices primarySortKey;
      public AudioProfilerClipInfoHelper.ColumnIndices secondarySortKey;
      public bool sortByDescendingOrder;

      public AudioProfilerClipInfoComparer(AudioProfilerClipInfoHelper.ColumnIndices primarySortKey, AudioProfilerClipInfoHelper.ColumnIndices secondarySortKey, bool sortByDescendingOrder)
      {
        this.primarySortKey = primarySortKey;
        this.secondarySortKey = secondarySortKey;
        this.sortByDescendingOrder = sortByDescendingOrder;
      }

      private int CompareInternal(AudioProfilerClipInfoWrapper a, AudioProfilerClipInfoWrapper b, AudioProfilerClipInfoHelper.ColumnIndices key)
      {
        int num = 0;
        switch (key)
        {
          case AudioProfilerClipInfoHelper.ColumnIndices.AssetName:
            num = a.assetName.CompareTo(b.assetName);
            break;
          case AudioProfilerClipInfoHelper.ColumnIndices.LoadState:
            num = a.info.loadState.CompareTo(b.info.loadState);
            break;
          case AudioProfilerClipInfoHelper.ColumnIndices.InternalLoadState:
            num = a.info.internalLoadState.CompareTo(b.info.internalLoadState);
            break;
          case AudioProfilerClipInfoHelper.ColumnIndices.Age:
            num = a.info.age.CompareTo(b.info.age);
            break;
          case AudioProfilerClipInfoHelper.ColumnIndices.Disposed:
            num = a.info.disposed.CompareTo(b.info.disposed);
            break;
          case AudioProfilerClipInfoHelper.ColumnIndices.NumChannelInstances:
            num = a.info.numChannelInstances.CompareTo(b.info.numChannelInstances);
            break;
        }
        return !this.sortByDescendingOrder ? num : -num;
      }

      public int Compare(AudioProfilerClipInfoWrapper a, AudioProfilerClipInfoWrapper b)
      {
        int num = this.CompareInternal(a, b, this.primarySortKey);
        return num != 0 ? num : this.CompareInternal(a, b, this.secondarySortKey);
      }
    }
  }
}
