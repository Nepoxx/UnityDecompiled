// Decompiled with JetBrains decompiler
// Type: UnityEditor.ActiveEditorTracker
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [Serializable]
  public sealed class ActiveEditorTracker
  {
    private MonoReloadableIntPtrClear m_Property;

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern ActiveEditorTracker();

    public override bool Equals(object o)
    {
      return this.m_Property.m_IntPtr == (o as ActiveEditorTracker).m_Property.m_IntPtr;
    }

    public override int GetHashCode()
    {
      return this.m_Property.m_IntPtr.GetHashCode();
    }

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Dispose();

    ~ActiveEditorTracker()
    {
      this.Dispose();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Destroy();

    public extern Editor[] activeEditors { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetVisible(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetVisible(int index, int visible);

    public extern bool isDirty { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearDirty();

    public extern bool isLocked { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern InspectorMode inspectorMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool hasComponentsWhichCannotBeMultiEdited { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void RebuildIfNecessary();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ForceRebuild();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void VerifyModifiedMonoBehaviours();

    [Obsolete("Use Editor.CreateEditor instead")]
    public static Editor MakeCustomEditor(UnityEngine.Object obj)
    {
      return Editor.CreateEditor(obj);
    }

    public static bool HasCustomEditor(UnityEngine.Object obj)
    {
      return CustomEditorAttributes.FindCustomEditorType(obj, false) != null;
    }

    public static ActiveEditorTracker sharedTracker
    {
      get
      {
        ActiveEditorTracker sharedTracker = new ActiveEditorTracker();
        ActiveEditorTracker.SetupSharedTracker(sharedTracker);
        return sharedTracker;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetupSharedTracker(ActiveEditorTracker sharedTracker);
  }
}
