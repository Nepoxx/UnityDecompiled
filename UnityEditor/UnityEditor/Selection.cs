// Decompiled with JetBrains decompiler
// Type: UnityEditor.Selection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Access to the selection in the editor.</para>
  /// </summary>
  public sealed class Selection
  {
    /// <summary>
    ///   <para>Delegate callback triggered when currently active/selected item has changed.</para>
    /// </summary>
    public static Action selectionChanged;

    /// <summary>
    ///   <para>Returns the top level selection, excluding prefabs.</para>
    /// </summary>
    public static extern Transform[] transforms { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the active transform. (The one shown in the inspector).</para>
    /// </summary>
    public static extern Transform activeTransform { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the actual game object selection. Includes prefabs, non-modifyable objects.</para>
    /// </summary>
    public static extern GameObject[] gameObjects { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the active game object. (The one shown in the inspector).</para>
    /// </summary>
    public static extern GameObject activeGameObject { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the actual object selection. Includes prefabs, non-modifyable objects.</para>
    /// </summary>
    public static extern UnityEngine.Object activeObject { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the current context object, as was set via SetActiveObjectWithContext.</para>
    /// </summary>
    public static extern UnityEngine.Object activeContext { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the instanceID of the actual object selection. Includes prefabs, non-modifyable objects.</para>
    /// </summary>
    public static extern int activeInstanceID { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The actual unfiltered selection from the Scene.</para>
    /// </summary>
    public static extern UnityEngine.Object[] objects { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The actual unfiltered selection from the Scene returned as instance ids instead of objects.</para>
    /// </summary>
    public static extern int[] instanceIDs { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns whether an object is contained in the current selection.</para>
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="obj"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Contains(int instanceID);

    /// <summary>
    ///   <para>Selects an object with a context.</para>
    /// </summary>
    /// <param name="obj">Object being selected (will be equal activeObject).</param>
    /// <param name="context">Context object.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetActiveObjectWithContext(UnityEngine.Object obj, UnityEngine.Object context);

    /// <summary>
    ///   <para>Allows for fine grained control of the selection type using the SelectionMode bitmask.</para>
    /// </summary>
    /// <param name="mode">Options for refining the selection.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Transform[] GetTransforms(SelectionMode mode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern UnityEngine.Object[] GetObjectsMode(SelectionMode mode);

    internal static extern string[] assetGUIDsDeepSelection { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the guids of the selected assets.</para>
    /// </summary>
    public static extern string[] assetGUIDs { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private static void Internal_CallSelectionChanged()
    {
      if (Selection.selectionChanged == null)
        return;
      Selection.selectionChanged();
    }

    /// <summary>
    ///   <para>Returns whether an object is contained in the current selection.</para>
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="obj"></param>
    public static bool Contains(UnityEngine.Object obj)
    {
      return Selection.Contains(obj.GetInstanceID());
    }

    internal static void Add(int instanceID)
    {
      List<int> intList = new List<int>((IEnumerable<int>) Selection.instanceIDs);
      if (intList.IndexOf(instanceID) >= 0)
        return;
      intList.Add(instanceID);
      Selection.instanceIDs = intList.ToArray();
    }

    internal static void Add(UnityEngine.Object obj)
    {
      if (!(obj != (UnityEngine.Object) null))
        return;
      Selection.Add(obj.GetInstanceID());
    }

    internal static void Remove(int instanceID)
    {
      List<int> intList = new List<int>((IEnumerable<int>) Selection.instanceIDs);
      intList.Remove(instanceID);
      Selection.instanceIDs = intList.ToArray();
    }

    internal static void Remove(UnityEngine.Object obj)
    {
      if (!(obj != (UnityEngine.Object) null))
        return;
      Selection.Remove(obj.GetInstanceID());
    }

    private static IEnumerable GetFilteredInternal(System.Type type, SelectionMode mode)
    {
      if (typeof (Component).IsAssignableFrom(type) || type.IsInterface)
        return (IEnumerable) ((IEnumerable<Transform>) Selection.GetTransforms(mode)).Select<Transform, Component>((Func<Transform, Component>) (t => t.GetComponent(type))).Where<Component>((Func<Component, bool>) (c => (UnityEngine.Object) c != (UnityEngine.Object) null));
      if (typeof (GameObject).IsAssignableFrom(type))
        return (IEnumerable) ((IEnumerable<Transform>) Selection.GetTransforms(mode)).Select<Transform, GameObject>((Func<Transform, GameObject>) (t => t.gameObject));
      return (IEnumerable) ((IEnumerable<UnityEngine.Object>) Selection.GetObjectsMode(mode)).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (o => o != (UnityEngine.Object) null && type.IsAssignableFrom(o.GetType())));
    }

    public static T[] GetFiltered<T>(SelectionMode mode)
    {
      return Selection.GetFilteredInternal(typeof (T), mode).Cast<T>().ToArray<T>();
    }

    /// <summary>
    ///   <para>Returns the current selection filtered by type and mode.</para>
    /// </summary>
    /// <param name="type">Only objects of this type will be retrieved.</param>
    /// <param name="mode">Further options to refine the selection.</param>
    public static UnityEngine.Object[] GetFiltered(System.Type type, SelectionMode mode)
    {
      return Selection.GetFilteredInternal(type, mode).Cast<UnityEngine.Object>().ToArray<UnityEngine.Object>();
    }
  }
}
