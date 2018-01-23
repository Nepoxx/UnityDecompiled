// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.PerceptionRemotingPlugin
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditorInternal.VR
{
  internal sealed class PerceptionRemotingPlugin
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Connect(string clientName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Disconnect();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern HolographicStreamerConnectionFailureReason CheckForDisconnect_Internal();

    internal static HolographicStreamerConnectionFailureReason CheckForDisconnect()
    {
      return PerceptionRemotingPlugin.CheckForDisconnect_Internal();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern HolographicStreamerConnectionState GetConnectionState_Internal();

    internal static HolographicStreamerConnectionState GetConnectionState()
    {
      return PerceptionRemotingPlugin.GetConnectionState_Internal();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetEnableAudio(bool enable);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetEnableVideo(bool enable);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetVideoEncodingParameters(int maxBitRate);
  }
}
