// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Sharing.WorldAnchorTransferBatch
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Sharing
{
  /// <summary>
  ///   <para>A batch of WorldAnchors which can be exported and imported between apps.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.Sharing")]
  public sealed class WorldAnchorTransferBatch : IDisposable
  {
    private IntPtr m_NativePtr = IntPtr.Zero;

    public WorldAnchorTransferBatch()
    {
      this.m_NativePtr = WorldAnchorTransferBatch.Create_Internal();
    }

    private WorldAnchorTransferBatch(IntPtr nativePtr)
    {
      this.m_NativePtr = nativePtr;
    }

    public static void ExportAsync(WorldAnchorTransferBatch transferBatch, WorldAnchorTransferBatch.SerializationDataAvailableDelegate onDataAvailable, WorldAnchorTransferBatch.SerializationCompleteDelegate onCompleted)
    {
      if (transferBatch == null)
        throw new ArgumentNullException(nameof (transferBatch));
      if (onDataAvailable == null)
        throw new ArgumentNullException(nameof (onDataAvailable));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      WorldAnchorTransferBatch.ExportAsync_Internal(transferBatch.m_NativePtr, onDataAvailable, onCompleted);
    }

    public static void ImportAsync(byte[] serializedData, WorldAnchorTransferBatch.DeserializationCompleteDelegate onComplete)
    {
      WorldAnchorTransferBatch.ImportAsync(serializedData, 0, serializedData.Length, onComplete);
    }

    public static void ImportAsync(byte[] serializedData, int offset, int length, WorldAnchorTransferBatch.DeserializationCompleteDelegate onComplete)
    {
      if (serializedData == null)
        throw new ArgumentNullException(nameof (serializedData));
      if (serializedData.Length < 1)
        throw new ArgumentException("serializedData is empty!", nameof (serializedData));
      if (offset + length > serializedData.Length)
        throw new ArgumentException("offset + length is greater that serializedData.Length!");
      if (onComplete == null)
        throw new ArgumentNullException(nameof (onComplete));
      WorldAnchorTransferBatch.ImportAsync_Internal(serializedData, offset, length, onComplete);
    }

    /// <summary>
    ///   <para>Adds a WorldAnchor to the batch with the specified identifier.</para>
    /// </summary>
    /// <param name="id">The identifier associated with this anchor in the batch. This must be unique per batch.</param>
    /// <param name="anchor">The anchor to add to the batch.</param>
    /// <returns>
    ///   <para>Whether or not the anchor was added successfully.</para>
    /// </returns>
    public bool AddWorldAnchor(string id, WorldAnchor anchor)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("id is null or empty!", nameof (id));
      if ((UnityEngine.Object) anchor == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (anchor));
      return WorldAnchorTransferBatch.AddWorldAnchor_Internal(this.m_NativePtr, id, anchor);
    }

    /// <summary>
    ///   <para>(Read Only) Gets the number of world anchors in this WorldAnchorTransferBatch.</para>
    /// </summary>
    public int anchorCount
    {
      get
      {
        return WorldAnchorTransferBatch.GetAnchorCount_Internal(this.m_NativePtr);
      }
    }

    /// <summary>
    ///   <para>Gets all of the identifiers currently mapped in this WorldAnchorTransferBatch. If the target array is not large enough to contain all the identifiers, then only those identifiers that fit within the array will be stored and the return value will equal the size of the array. You can detect this condition by checking for a return value less than WorldAnchorTransferBatch.anchorCount.</para>
    /// </summary>
    /// <param name="ids">A target array to receive the identifiers of the currently mapped world anchors.</param>
    /// <returns>
    ///   <para>The number of identifiers stored in the target array.</para>
    /// </returns>
    public int GetAllIds(string[] ids)
    {
      if (ids == null)
        throw new ArgumentNullException(nameof (ids));
      if (ids.Length > 0)
        return WorldAnchorTransferBatch.GetAllIds_Internal(this.m_NativePtr, ids);
      return 0;
    }

    /// <summary>
    ///   <para>Gets all of the identifiers currently mapped in this WorldAnchorTransferBatch.</para>
    /// </summary>
    /// <returns>
    ///   <para>The identifiers of all of the WorldAnchors in this WorldAnchorTransferBatch.</para>
    /// </returns>
    public string[] GetAllIds()
    {
      string[] ids = new string[this.anchorCount];
      this.GetAllIds(ids);
      return ids;
    }

    /// <summary>
    ///   <para>Locks the provided GameObject to the world by loading and applying the WorldAnchor from the TransferBatch for the provided id.</para>
    /// </summary>
    /// <param name="id">The identifier for the WorldAnchor to load and apply to the GameObject.</param>
    /// <param name="go">The GameObject to apply the WorldAnchor to. If the GameObject already has a WorldAnchor, it will be updated.</param>
    /// <returns>
    ///   <para>The loaded WorldAnchor or null if the id does not map to a WorldAnchor.</para>
    /// </returns>
    public WorldAnchor LockObject(string id, GameObject go)
    {
      WorldAnchor anchor = go.GetComponent<WorldAnchor>();
      bool flag = (UnityEngine.Object) anchor != (UnityEngine.Object) null;
      if ((UnityEngine.Object) anchor == (UnityEngine.Object) null)
        anchor = go.AddComponent<WorldAnchor>();
      if (WorldAnchorTransferBatch.LoadAnchor_Internal(this.m_NativePtr, id, anchor))
        return anchor;
      if (!flag)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) anchor);
      return (WorldAnchor) null;
    }

    ~WorldAnchorTransferBatch()
    {
      if (!(this.m_NativePtr != IntPtr.Zero))
        return;
      WorldAnchorTransferBatch.DisposeThreaded_Internal(this.m_NativePtr);
      this.m_NativePtr = IntPtr.Zero;
    }

    /// <summary>
    ///   <para>Cleans up the WorldAnchorTransferBatch and releases memory.</para>
    /// </summary>
    public void Dispose()
    {
      if (this.m_NativePtr != IntPtr.Zero)
      {
        WorldAnchorTransferBatch.Dispose_Internal(this.m_NativePtr);
        this.m_NativePtr = IntPtr.Zero;
      }
      GC.SuppressFinalize((object) this);
    }

    [RequiredByNativeCode]
    private static void InvokeWorldAnchorSerializationDataAvailableDelegate(WorldAnchorTransferBatch.SerializationDataAvailableDelegate onSerializationDataAvailable, byte[] data)
    {
      onSerializationDataAvailable(data);
    }

    [RequiredByNativeCode]
    private static void InvokeWorldAnchorSerializationCompleteDelegate(WorldAnchorTransferBatch.SerializationCompleteDelegate onSerializationComplete, SerializationCompletionReason completionReason)
    {
      onSerializationComplete(completionReason);
    }

    [RequiredByNativeCode]
    private static void InvokeWorldAnchorDeserializationCompleteDelegate(WorldAnchorTransferBatch.DeserializationCompleteDelegate onDeserializationComplete, SerializationCompletionReason completionReason, IntPtr nativePtr)
    {
      WorldAnchorTransferBatch deserializedTransferBatch = new WorldAnchorTransferBatch(nativePtr);
      onDeserializationComplete(completionReason, deserializedTransferBatch);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ExportAsync_Internal(IntPtr transferBatch, WorldAnchorTransferBatch.SerializationDataAvailableDelegate onDataAvailable, WorldAnchorTransferBatch.SerializationCompleteDelegate onComplete);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ImportAsync_Internal(byte[] serializedData, int offset, int length, WorldAnchorTransferBatch.DeserializationCompleteDelegate onComplete);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool AddWorldAnchor_Internal(IntPtr context, string id, WorldAnchor anchor);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetAnchorCount_Internal(IntPtr context);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetAllIds_Internal(IntPtr context, string[] ids);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool LoadAnchor_Internal(IntPtr context, string id, WorldAnchor anchor);

    private static IntPtr Create_Internal()
    {
      IntPtr num;
      WorldAnchorTransferBatch.INTERNAL_CALL_Create_Internal(out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Create_Internal(out IntPtr value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Dispose_Internal(IntPtr context);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void DisposeThreaded_Internal(IntPtr context);

    /// <summary>
    ///   <para>The handler for when some data is available from serialization.</para>
    /// </summary>
    /// <param name="data">A set of bytes from the exported transfer batch.</param>
    public delegate void SerializationDataAvailableDelegate(byte[] data);

    /// <summary>
    ///   <para>The handler for when serialization is completed.</para>
    /// </summary>
    /// <param name="completionReason">Why the serialization completed (success or failure reason).</param>
    public delegate void SerializationCompleteDelegate(SerializationCompletionReason completionReason);

    /// <summary>
    ///   <para>The handler for when deserialization has completed.</para>
    /// </summary>
    /// <param name="completionReason">The reason the deserialization completed (success or failure reason).</param>
    /// <param name="deserializedTransferBatch">The resulting transfer batch which is empty on error.</param>
    public delegate void DeserializationCompleteDelegate(SerializationCompletionReason completionReason, WorldAnchorTransferBatch deserializedTransferBatch);
  }
}
