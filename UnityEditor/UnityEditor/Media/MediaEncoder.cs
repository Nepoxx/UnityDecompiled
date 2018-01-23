// Decompiled with JetBrains decompiler
// Type: UnityEditor.Media.MediaEncoder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Collections;

namespace UnityEditor.Media
{
  /// <summary>
  ///   <para>Encodes images and audio samples into an audio or movie file.</para>
  /// </summary>
  public class MediaEncoder : IDisposable
  {
    public IntPtr m_Ptr;

    /// <summary>
    ///   <para>Create a new encoder with various track arrangements.</para>
    /// </summary>
    /// <param name="filePath">Path fo the media file to be written.</param>
    /// <param name="videoAttrs">Attributes for the file's video track, if any.</param>
    /// <param name="audioAttrs">Attributes for the file's audio tracks, if any.</param>
    public MediaEncoder(string filePath, VideoTrackAttributes videoAttrs, AudioTrackAttributes[] audioAttrs)
    {
      this.m_Ptr = this.Create(filePath, new VideoTrackAttributes[1]
      {
        videoAttrs
      }, audioAttrs);
    }

    /// <summary>
    ///   <para>Create a new encoder with various track arrangements.</para>
    /// </summary>
    /// <param name="filePath">Path fo the media file to be written.</param>
    /// <param name="videoAttrs">Attributes for the file's video track, if any.</param>
    /// <param name="audioAttrs">Attributes for the file's audio tracks, if any.</param>
    public MediaEncoder(string filePath, VideoTrackAttributes videoAttrs, AudioTrackAttributes audioAttrs)
      : this(filePath, videoAttrs, new AudioTrackAttributes[1]{ audioAttrs })
    {
    }

    /// <summary>
    ///   <para>Create a new encoder with various track arrangements.</para>
    /// </summary>
    /// <param name="filePath">Path fo the media file to be written.</param>
    /// <param name="videoAttrs">Attributes for the file's video track, if any.</param>
    /// <param name="audioAttrs">Attributes for the file's audio tracks, if any.</param>
    public MediaEncoder(string filePath, VideoTrackAttributes videoAttrs)
      : this(filePath, videoAttrs, new AudioTrackAttributes[0])
    {
    }

    /// <summary>
    ///   <para>Create a new encoder with various track arrangements.</para>
    /// </summary>
    /// <param name="filePath">Path fo the media file to be written.</param>
    /// <param name="videoAttrs">Attributes for the file's video track, if any.</param>
    /// <param name="audioAttrs">Attributes for the file's audio tracks, if any.</param>
    public MediaEncoder(string filePath, AudioTrackAttributes[] audioAttrs)
    {
      this.m_Ptr = this.Create(filePath, new VideoTrackAttributes[0], audioAttrs);
    }

    /// <summary>
    ///   <para>Create a new encoder with various track arrangements.</para>
    /// </summary>
    /// <param name="filePath">Path fo the media file to be written.</param>
    /// <param name="videoAttrs">Attributes for the file's video track, if any.</param>
    /// <param name="audioAttrs">Attributes for the file's audio tracks, if any.</param>
    public MediaEncoder(string filePath, AudioTrackAttributes audioAttrs)
      : this(filePath, new AudioTrackAttributes[1]{ audioAttrs })
    {
    }

    ~MediaEncoder()
    {
      this.Dispose();
    }

    /// <summary>
    ///   <para>Appends a frame to the file's video track.</para>
    /// </summary>
    /// <param name="texture">Texture containing the pixels to be written into the track for the current frame.</param>
    /// <returns>
    ///   <para>True if the operation succeeded. False otherwise.</para>
    /// </returns>
    public bool AddFrame(Texture2D texture)
    {
      return MediaEncoder.Internal_AddFrame(this.m_Ptr, texture);
    }

    public bool AddSamples(ushort trackIndex, NativeArray<float> interleavedSamples)
    {
      return MediaEncoder.Internal_AddSamples(this.m_Ptr, trackIndex, interleavedSamples.UnsafeReadOnlyPtr, interleavedSamples.Length);
    }

    public bool AddSamples(NativeArray<float> interleavedSamples)
    {
      return this.AddSamples((ushort) 0, interleavedSamples);
    }

    /// <summary>
    ///   <para>Finishes writing all tracks and closes the file being written.</para>
    /// </summary>
    public void Dispose()
    {
      if (this.m_Ptr != IntPtr.Zero)
      {
        MediaEncoder.Internal_Release(this.m_Ptr);
        this.m_Ptr = IntPtr.Zero;
      }
      GC.SuppressFinalize((object) this);
    }

    private IntPtr Create(string filePath, VideoTrackAttributes[] videoAttrs, AudioTrackAttributes[] audioAttrs)
    {
      IntPtr num = MediaEncoder.Internal_Create(filePath, videoAttrs, audioAttrs);
      if (num == IntPtr.Zero)
        throw new InvalidOperationException("MediaEncoder: Output file creation failed for " + filePath);
      return num;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr Internal_Create(string filePath, VideoTrackAttributes[] videoAttrs, AudioTrackAttributes[] audioAttrs);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Release(IntPtr encoder);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_AddFrame(IntPtr encoder, Texture2D texture);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_AddSamples(IntPtr encoder, ushort trackIndex, IntPtr buffer, int sampleCount);
  }
}
