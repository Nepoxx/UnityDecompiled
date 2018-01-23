// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIStyleState
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Specialized values for the given states used by GUIStyle objects.</para>
  /// </summary>
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class GUIStyleState
  {
    [NonSerialized]
    internal IntPtr m_Ptr;
    private readonly GUIStyle m_SourceStyle;
    [NonSerialized]
    private Texture2D m_Background;
    [NonSerialized]
    private Texture2D[] m_ScaledBackgrounds;

    public GUIStyleState()
    {
      this.Init();
    }

    private GUIStyleState(GUIStyle sourceStyle, IntPtr source)
    {
      this.m_SourceStyle = sourceStyle;
      this.m_Ptr = source;
    }

    internal static GUIStyleState ProduceGUIStyleStateFromDeserialization(GUIStyle sourceStyle, IntPtr source)
    {
      GUIStyleState guiStyleState = new GUIStyleState(sourceStyle, source);
      guiStyleState.m_Background = guiStyleState.GetBackgroundInternalFromDeserialization();
      guiStyleState.m_ScaledBackgrounds = guiStyleState.GetScaledBackgroundsInternalFromDeserialization();
      return guiStyleState;
    }

    internal static GUIStyleState GetGUIStyleState(GUIStyle sourceStyle, IntPtr source)
    {
      GUIStyleState guiStyleState = new GUIStyleState(sourceStyle, source);
      guiStyleState.m_Background = guiStyleState.GetBackgroundInternal();
      guiStyleState.m_ScaledBackgrounds = guiStyleState.GetScaledBackgroundsInternalFromDeserialization();
      return guiStyleState;
    }

    ~GUIStyleState()
    {
      if (this.m_SourceStyle != null)
        return;
      this.Cleanup();
    }

    /// <summary>
    ///   <para>The background image used by GUI elements in this given state.</para>
    /// </summary>
    public Texture2D background
    {
      get
      {
        return this.GetBackgroundInternal();
      }
      set
      {
        this.SetBackgroundInternal(value);
        this.m_Background = value;
      }
    }

    /// <summary>
    ///   <para>Background images used by this state when on a high-resolution screen. It should either be left empty, or contain a single image that is exactly twice the resolution of background. This is only used by the editor. The field is not copied to player data, and is not accessible from player code.</para>
    /// </summary>
    public Texture2D[] scaledBackgrounds
    {
      get
      {
        return this.GetScaledBackgroundsInternal();
      }
      set
      {
        this.SetScaledBackgroundsInternal(value);
        this.m_ScaledBackgrounds = value;
      }
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Init();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Cleanup();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetBackgroundInternal(Texture2D value);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Texture2D GetBackgroundInternalFromDeserialization();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Texture2D GetBackgroundInternal();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Texture2D[] GetScaledBackgroundsInternalFromDeserialization();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Texture2D[] GetScaledBackgroundsInternal();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetScaledBackgroundsInternal(Texture2D[] newValue);

    /// <summary>
    ///   <para>The text color used by GUI elements in this state.</para>
    /// </summary>
    public Color textColor
    {
      get
      {
        Color color;
        this.INTERNAL_get_textColor(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_textColor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_textColor(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_textColor(ref Color value);
  }
}
