// Decompiled with JetBrains decompiler
// Type: UnityEditor.CustomEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Tells an Editor class which run-time type it's an editor for.</para>
  /// </summary>
  public class CustomEditor : Attribute
  {
    internal System.Type m_InspectedType;
    internal bool m_EditorForChildClasses;

    /// <summary>
    ///   <para>Defines which object type the custom editor class can edit.</para>
    /// </summary>
    /// <param name="inspectedType">Type that this editor can edit.</param>
    /// <param name="editorForChildClasses">If true, child classes of inspectedType will also show this editor. Defaults to false.</param>
    public CustomEditor(System.Type inspectedType)
    {
      if (inspectedType == null)
        Debug.LogError((object) "Failed to load CustomEditor inspected type");
      this.m_InspectedType = inspectedType;
      this.m_EditorForChildClasses = false;
    }

    /// <summary>
    ///   <para>Defines which object type the custom editor class can edit.</para>
    /// </summary>
    /// <param name="inspectedType">Type that this editor can edit.</param>
    /// <param name="editorForChildClasses">If true, child classes of inspectedType will also show this editor. Defaults to false.</param>
    public CustomEditor(System.Type inspectedType, bool editorForChildClasses)
    {
      if (inspectedType == null)
        Debug.LogError((object) "Failed to load CustomEditor inspected type");
      this.m_InspectedType = inspectedType;
      this.m_EditorForChildClasses = editorForChildClasses;
    }

    /// <summary>
    ///   <para>If true, match this editor only if all non-fallback editors do not match. Defaults to false.</para>
    /// </summary>
    public bool isFallback { get; set; }
  }
}
