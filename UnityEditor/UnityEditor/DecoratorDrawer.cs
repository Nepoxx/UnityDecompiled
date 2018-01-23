// Decompiled with JetBrains decompiler
// Type: UnityEditor.DecoratorDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Base class to derive custom decorator drawers from.</para>
  /// </summary>
  public abstract class DecoratorDrawer : GUIDrawer
  {
    internal PropertyAttribute m_Attribute;

    /// <summary>
    ///   <para>The PropertyAttribute for the decorator. (Read Only)</para>
    /// </summary>
    public PropertyAttribute attribute
    {
      get
      {
        return this.m_Attribute;
      }
    }

    /// <summary>
    ///         <para>Override this method to make your own GUI for the decorator.
    /// See DecoratorDrawer for an example of how to use this.</para>
    ///       </summary>
    /// <param name="position">Rectangle on the screen to use for the decorator GUI.</param>
    public virtual void OnGUI(Rect position)
    {
    }

    /// <summary>
    ///   <para>Override this method to specify how tall the GUI for this decorator is in pixels.</para>
    /// </summary>
    public virtual float GetHeight()
    {
      return 16f;
    }

    /// <summary>
    ///   <para>Override this method to determine whether the inspector GUI for your decorator can be cached.</para>
    /// </summary>
    /// <returns>
    ///   <para>Whether the inspector GUI for your decorator can be cached.</para>
    /// </returns>
    public virtual bool CanCacheInspectorGUI()
    {
      return true;
    }
  }
}
