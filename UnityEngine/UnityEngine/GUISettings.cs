// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUISettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>General settings for how the GUI behaves.</para>
  /// </summary>
  [Serializable]
  public sealed class GUISettings
  {
    [SerializeField]
    private bool m_DoubleClickSelectsWord = true;
    [SerializeField]
    private bool m_TripleClickSelectsLine = true;
    [SerializeField]
    private Color m_CursorColor = Color.white;
    [SerializeField]
    private float m_CursorFlashSpeed = -1f;
    [SerializeField]
    private Color m_SelectionColor = new Color(0.5f, 0.5f, 1f);

    /// <summary>
    ///   <para>Should double-clicking select words in text fields.</para>
    /// </summary>
    public bool doubleClickSelectsWord
    {
      get
      {
        return this.m_DoubleClickSelectsWord;
      }
      set
      {
        this.m_DoubleClickSelectsWord = value;
      }
    }

    /// <summary>
    ///   <para>Should triple-clicking select whole text in text fields.</para>
    /// </summary>
    public bool tripleClickSelectsLine
    {
      get
      {
        return this.m_TripleClickSelectsLine;
      }
      set
      {
        this.m_TripleClickSelectsLine = value;
      }
    }

    /// <summary>
    ///   <para>The color of the cursor in text fields.</para>
    /// </summary>
    public Color cursorColor
    {
      get
      {
        return this.m_CursorColor;
      }
      set
      {
        this.m_CursorColor = value;
      }
    }

    /// <summary>
    ///   <para>The speed of text field cursor flashes.</para>
    /// </summary>
    public float cursorFlashSpeed
    {
      get
      {
        if ((double) this.m_CursorFlashSpeed >= 0.0)
          return this.m_CursorFlashSpeed;
        return GUISettings.Internal_GetCursorFlashSpeed();
      }
      set
      {
        this.m_CursorFlashSpeed = value;
      }
    }

    /// <summary>
    ///   <para>The color of the selection rect in text fields.</para>
    /// </summary>
    public Color selectionColor
    {
      get
      {
        return this.m_SelectionColor;
      }
      set
      {
        this.m_SelectionColor = value;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float Internal_GetCursorFlashSpeed();
  }
}
