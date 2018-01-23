// Decompiled with JetBrains decompiler
// Type: UnityEditor.DragAndDrop
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Editor drag &amp; drop operations.</para>
  /// </summary>
  public sealed class DragAndDrop
  {
    private static Hashtable ms_GenericData;

    internal static bool HandleDelayedDrag(Rect position, int id, Object objectToDrag)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (position.Contains(current.mousePosition) && current.clickCount == 1 && current.button == 0 && (Application.platform != RuntimePlatform.OSXEditor || !current.control))
          {
            GUIUtility.hotControl = id;
            ((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), id)).mouseDownPosition = current.mousePosition;
            return true;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id && ((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), id)).CanStartDrag())
          {
            GUIUtility.hotControl = 0;
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.objectReferences = new Object[1]
            {
              objectToDrag
            };
            DragAndDrop.StartDrag(ObjectNames.GetDragAndDropTitle(objectToDrag));
            return true;
          }
          break;
      }
      return false;
    }

    /// <summary>
    ///   <para>Clears drag &amp; drop data.</para>
    /// </summary>
    public static void PrepareStartDrag()
    {
      DragAndDrop.ms_GenericData = (Hashtable) null;
      DragAndDrop.PrepareStartDrag_Internal();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void PrepareStartDrag_Internal();

    /// <summary>
    ///   <para>Start a drag operation.</para>
    /// </summary>
    /// <param name="title"></param>
    public static void StartDrag(string title)
    {
      if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
        DragAndDrop.StartDrag_Internal(title);
      else
        Debug.LogError((object) "Drags can only be started from MouseDown or MouseDrag events");
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void StartDrag_Internal(string title);

    /// <summary>
    ///   <para>References to Object|objects being dragged.</para>
    /// </summary>
    public static extern Object[] objectReferences { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The file names being dragged.</para>
    /// </summary>
    public static extern string[] paths { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The visual indication of the drag.</para>
    /// </summary>
    public static extern DragAndDropVisualMode visualMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Accept a drag operation.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void AcceptDrag();

    [RequiredByNativeCode]
    private static bool HasGenericDragData()
    {
      return DragAndDrop.ms_GenericData != null;
    }

    /// <summary>
    ///   <para>Get data associated with current drag and drop operation.</para>
    /// </summary>
    /// <param name="type"></param>
    public static object GetGenericData(string type)
    {
      if (DragAndDrop.ms_GenericData != null && DragAndDrop.ms_GenericData.Contains((object) type))
        return DragAndDrop.ms_GenericData[(object) type];
      return (object) null;
    }

    /// <summary>
    ///   <para>Set data associated with current drag and drop operation.</para>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    public static void SetGenericData(string type, object data)
    {
      if (DragAndDrop.ms_GenericData == null)
        DragAndDrop.ms_GenericData = new Hashtable();
      DragAndDrop.ms_GenericData[(object) type] = data;
    }

    /// <summary>
    ///   <para>Get or set ID of currently active drag and drop control.</para>
    /// </summary>
    public static extern int activeControlID { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
