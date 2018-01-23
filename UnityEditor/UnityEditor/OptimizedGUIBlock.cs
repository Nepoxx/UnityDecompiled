// Decompiled with JetBrains decompiler
// Type: UnityEditor.OptimizedGUIBlock
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal sealed class OptimizedGUIBlock
  {
    private bool m_Valid = false;
    private bool m_Recording = false;
    private bool m_WatchForUsed = false;
    [NonSerialized]
    private IntPtr m_Ptr;
    private int m_KeyboardControl;
    private int m_LastSearchIndex;
    private int m_ActiveDragControl;
    private Color m_GUIColor;
    private Rect m_Rect;

    public OptimizedGUIBlock()
    {
      this.Init();
    }

    ~OptimizedGUIBlock()
    {
      if (!(this.m_Ptr != IntPtr.Zero))
        return;
      Debug.Log((object) "Failed cleaning up Optimized GUI Block");
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Init();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Dispose();

    public bool Begin(bool hasChanged, Rect position)
    {
      if (hasChanged)
        this.m_Valid = false;
      if (Event.current.type == EventType.Repaint)
      {
        if (GUIUtility.keyboardControl != this.m_KeyboardControl)
        {
          this.m_Valid = false;
          this.m_KeyboardControl = GUIUtility.keyboardControl;
        }
        if (DragAndDrop.activeControlID != this.m_ActiveDragControl)
        {
          this.m_Valid = false;
          this.m_ActiveDragControl = DragAndDrop.activeControlID;
        }
        if (GUI.color != this.m_GUIColor)
        {
          this.m_Valid = false;
          this.m_GUIColor = GUI.color;
        }
        position = GUIClip.Unclip(position);
        if (this.m_Valid && position != this.m_Rect)
        {
          this.m_Rect = position;
          this.m_Valid = false;
        }
        if (EditorGUI.isCollectingTooltips)
          return true;
        if (this.m_Valid)
          return false;
        this.m_Recording = true;
        this.BeginRecording();
        return true;
      }
      if (Event.current.type == EventType.Used)
        return false;
      if (Event.current.type != EventType.Used)
        this.m_WatchForUsed = true;
      return true;
    }

    public void End()
    {
      bool recording = this.m_Recording;
      if (this.m_Recording)
      {
        this.EndRecording();
        this.m_Recording = false;
        this.m_Valid = true;
        this.m_LastSearchIndex = EditorGUIUtility.GetSearchIndexOfControlIDList();
      }
      if (Event.current == null)
        Debug.LogError((object) "Event.current is null");
      if (Event.current.type == EventType.Repaint && !EditorGUI.isCollectingTooltips)
      {
        this.Execute();
        if (!recording)
          EditorGUIUtility.SetSearchIndexOfControlIDList(this.m_LastSearchIndex);
      }
      if (this.m_WatchForUsed && Event.current.type == EventType.Used)
        this.m_Valid = false;
      this.m_WatchForUsed = false;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void BeginRecording();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void EndRecording();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Execute();

    public bool valid
    {
      get
      {
        return this.m_Valid;
      }
      set
      {
        this.m_Valid = value;
      }
    }
  }
}
