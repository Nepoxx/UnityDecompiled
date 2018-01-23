// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Navigation
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Structure storing details related to navigation.</para>
  /// </summary>
  [Serializable]
  public struct Navigation : IEquatable<Navigation>
  {
    [FormerlySerializedAs("mode")]
    [SerializeField]
    private Navigation.Mode m_Mode;
    [FormerlySerializedAs("selectOnUp")]
    [SerializeField]
    private Selectable m_SelectOnUp;
    [FormerlySerializedAs("selectOnDown")]
    [SerializeField]
    private Selectable m_SelectOnDown;
    [FormerlySerializedAs("selectOnLeft")]
    [SerializeField]
    private Selectable m_SelectOnLeft;
    [FormerlySerializedAs("selectOnRight")]
    [SerializeField]
    private Selectable m_SelectOnRight;

    /// <summary>
    ///   <para>Navigation mode.</para>
    /// </summary>
    public Navigation.Mode mode
    {
      get
      {
        return this.m_Mode;
      }
      set
      {
        this.m_Mode = value;
      }
    }

    /// <summary>
    ///   <para>Specify a Selectable UI GameObject to highlight when the Up arrow key is pressed.</para>
    /// </summary>
    public Selectable selectOnUp
    {
      get
      {
        return this.m_SelectOnUp;
      }
      set
      {
        this.m_SelectOnUp = value;
      }
    }

    /// <summary>
    ///   <para>Specify a Selectable UI GameObject to highlight when the down arrow key is pressed.</para>
    /// </summary>
    public Selectable selectOnDown
    {
      get
      {
        return this.m_SelectOnDown;
      }
      set
      {
        this.m_SelectOnDown = value;
      }
    }

    /// <summary>
    ///   <para>Specify a Selectable UI GameObject to highlight when the left arrow key is pressed.</para>
    /// </summary>
    public Selectable selectOnLeft
    {
      get
      {
        return this.m_SelectOnLeft;
      }
      set
      {
        this.m_SelectOnLeft = value;
      }
    }

    /// <summary>
    ///   <para>Specify a Selectable UI GameObject to highlight when the right arrow key is pressed.</para>
    /// </summary>
    public Selectable selectOnRight
    {
      get
      {
        return this.m_SelectOnRight;
      }
      set
      {
        this.m_SelectOnRight = value;
      }
    }

    /// <summary>
    ///   <para>Return a Navigation with sensible default values.</para>
    /// </summary>
    public static Navigation defaultNavigation
    {
      get
      {
        return new Navigation() { m_Mode = Navigation.Mode.Automatic };
      }
    }

    public bool Equals(Navigation other)
    {
      return this.mode == other.mode && (UnityEngine.Object) this.selectOnUp == (UnityEngine.Object) other.selectOnUp && ((UnityEngine.Object) this.selectOnDown == (UnityEngine.Object) other.selectOnDown && (UnityEngine.Object) this.selectOnLeft == (UnityEngine.Object) other.selectOnLeft) && (UnityEngine.Object) this.selectOnRight == (UnityEngine.Object) other.selectOnRight;
    }

    /// <summary>
    ///   <para>Navigation mode. Used by Selectable.</para>
    /// </summary>
    [Flags]
    public enum Mode
    {
      None = 0,
      Horizontal = 1,
      Vertical = 2,
      Automatic = Vertical | Horizontal, // 0x00000003
      Explicit = 4,
    }
  }
}
