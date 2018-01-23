// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Persistence.WorldAnchorStore
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Persistence
{
  /// <summary>
  ///   <para>The storage object for persisted WorldAnchors.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.Persistence")]
  public sealed class WorldAnchorStore
  {
    private static WorldAnchorStore s_Instance = (WorldAnchorStore) null;
    private IntPtr m_NativePtr = IntPtr.Zero;

    private WorldAnchorStore(IntPtr nativePtr)
    {
      this.m_NativePtr = nativePtr;
    }

    public static void GetAsync(WorldAnchorStore.GetAsyncDelegate onCompleted)
    {
      if (WorldAnchorStore.s_Instance != null)
        onCompleted(WorldAnchorStore.s_Instance);
      else
        WorldAnchorStore.GetAsync_Internal(onCompleted);
    }

    /// <summary>
    ///   <para>Saves the provided WorldAnchor with the provided identifier. If the identifier is already in use, the method will return false.</para>
    /// </summary>
    /// <param name="id">The identifier to save the anchor with. This needs to be unique for your app.</param>
    /// <param name="anchor">The anchor to save.</param>
    /// <returns>
    ///   <para>Whether or not the save was successful. Will return false if the id conflicts with another already saved anchor's id.</para>
    /// </returns>
    public bool Save(string id, WorldAnchor anchor)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("id must not be null or empty", nameof (id));
      if ((UnityEngine.Object) anchor == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (anchor));
      return WorldAnchorStore.Save_Internal(this.m_NativePtr, id, anchor);
    }

    /// <summary>
    ///   <para>Loads a WorldAnchor from disk for given identifier and attaches it to the GameObject. If the GameObject has a WorldAnchor, that WorldAnchor will be updated. If the anchor is not found, null will be returned and the GameObject and any existing WorldAnchor attached to it will not be modified.</para>
    /// </summary>
    /// <param name="id">The identifier of the WorldAnchor to load.</param>
    /// <param name="go">The object to attach the WorldAnchor to if found.</param>
    /// <returns>
    ///   <para>The WorldAnchor loaded by the identifier or null if not found.</para>
    /// </returns>
    public WorldAnchor Load(string id, GameObject go)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("id must not be null or empty", nameof (id));
      if ((UnityEngine.Object) go == (UnityEngine.Object) null)
        throw new ArgumentNullException("anchor");
      WorldAnchor anchor = go.GetComponent<WorldAnchor>();
      bool flag = (UnityEngine.Object) anchor != (UnityEngine.Object) null;
      if ((UnityEngine.Object) anchor == (UnityEngine.Object) null)
        anchor = go.AddComponent<WorldAnchor>();
      if (WorldAnchorStore.Load_Internal(this.m_NativePtr, id, anchor))
        return go.GetComponent<WorldAnchor>();
      if (!flag)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) anchor);
      return (WorldAnchor) null;
    }

    /// <summary>
    ///   <para>Deletes a persisted WorldAnchor from the store.</para>
    /// </summary>
    /// <param name="id">The identifier of the WorldAnchor to delete.</param>
    /// <returns>
    ///   <para>Whether or not the WorldAnchor was found and deleted.</para>
    /// </returns>
    public bool Delete(string id)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("id must not be null or empty", nameof (id));
      return WorldAnchorStore.Delete_Internal(this.m_NativePtr, id);
    }

    /// <summary>
    ///   <para>Clears all persisted WorldAnchors.</para>
    /// </summary>
    public void Clear()
    {
      WorldAnchorStore.Clear_Internal(this.m_NativePtr);
    }

    /// <summary>
    ///   <para>(Read Only) Gets the number of persisted world anchors in this WorldAnchorStore.</para>
    /// </summary>
    public int anchorCount
    {
      get
      {
        return WorldAnchorStore.GetAnchorCount_Internal(this.m_NativePtr);
      }
    }

    /// <summary>
    ///   <para>Gets all of the identifiers of the currently persisted WorldAnchors.</para>
    /// </summary>
    /// <param name="ids">A target array to receive the identifiers of the currently persisted world anchors.</param>
    /// <returns>
    ///   <para>The number of identifiers stored in the target array.</para>
    /// </returns>
    public int GetAllIds(string[] ids)
    {
      if (ids == null)
        throw new ArgumentNullException(nameof (ids));
      if (ids.Length > 0)
        return WorldAnchorStore.GetAllIds_Internal(this.m_NativePtr, ids);
      return 0;
    }

    /// <summary>
    ///   <para>Gets all of the identifiers of the currently persisted WorldAnchors.</para>
    /// </summary>
    /// <returns>
    ///   <para>An array of string identifiers.</para>
    /// </returns>
    public string[] GetAllIds()
    {
      string[] ids = new string[this.anchorCount];
      this.GetAllIds(ids);
      return ids;
    }

    [RequiredByNativeCode]
    private static void InvokeGetAsyncDelegate(WorldAnchorStore.GetAsyncDelegate handler, IntPtr nativePtr)
    {
      WorldAnchorStore.s_Instance = new WorldAnchorStore(nativePtr);
      handler(WorldAnchorStore.s_Instance);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetAsync_Internal(WorldAnchorStore.GetAsyncDelegate onCompleted);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Save_Internal(IntPtr context, string id, WorldAnchor anchor);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Load_Internal(IntPtr context, string id, WorldAnchor anchor);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Delete_Internal(IntPtr context, string id);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Clear_Internal(IntPtr context);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetAnchorCount_Internal(IntPtr context);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetAllIds_Internal(IntPtr context, string[] ids);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Destruct_Internal(IntPtr context);

    /// <summary>
    ///   <para>The handler for when getting the WorldAnchorStore from GetAsync.</para>
    /// </summary>
    /// <param name="store">The instance of the WorldAnchorStore once loaded.</param>
    public delegate void GetAsyncDelegate(WorldAnchorStore store);
  }
}
