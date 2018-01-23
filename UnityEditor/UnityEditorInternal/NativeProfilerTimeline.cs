// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.NativeProfilerTimeline
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditorInternal
{
  public sealed class NativeProfilerTimeline
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Initialize(ref NativeProfilerTimeline_InitializeArgs args);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Draw(ref NativeProfilerTimeline_DrawArgs args);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetEntryAtPosition(ref NativeProfilerTimeline_GetEntryAtPositionArgs args);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetEntryInstanceInfo(ref NativeProfilerTimeline_GetEntryInstanceInfoArgs args);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetEntryTimingInfo(ref NativeProfilerTimeline_GetEntryTimingInfoArgs args);
  }
}
