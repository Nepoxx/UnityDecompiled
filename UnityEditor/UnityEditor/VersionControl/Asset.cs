// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.Asset
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>This class containes information about the version control state of an asset.</para>
  /// </summary>
  public sealed class Asset
  {
    private GUID m_guid;

    public Asset(string clientPath)
    {
      this.InternalCreateFromString(clientPath);
    }

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void InternalCreateFromString(string clientPath);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Dispose();

    ~Asset()
    {
      this.Dispose();
    }

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool IsChildOf(Asset other);

    /// <summary>
    ///   <para>Gets the version control state of the asset.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern Asset.States state { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets the path of the asset.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern string path { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the asset is a folder.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern bool isFolder { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true is the asset is read only.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern bool readOnly { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the instance of the Asset class actually refers to a .meta file.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern bool isMeta { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the asset is locked by the version control system.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern bool locked { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get the name of the asset.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern string name { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets the full name of the asset including extension.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern string fullName { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the assets is in the current project.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern bool isInCurrentProject { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static bool IsState(Asset.States isThisState, Asset.States partOfThisState)
    {
      return (isThisState & partOfThisState) != Asset.States.None;
    }

    public bool IsState(Asset.States state)
    {
      return Asset.IsState(this.state, state);
    }

    public bool IsOneOfStates(Asset.States[] states)
    {
      foreach (Asset.States state in states)
      {
        if ((this.state & state) != Asset.States.None)
          return true;
      }
      return false;
    }

    internal bool IsUnderVersionControl
    {
      get
      {
        return this.IsState(Asset.States.Synced) || this.IsState(Asset.States.OutOfSync) || this.IsState(Asset.States.AddedLocal);
      }
    }

    /// <summary>
    ///   <para>Opens the assets in an associated editor.</para>
    /// </summary>
    public void Edit()
    {
      UnityEngine.Object target = this.Load();
      if (!(target != (UnityEngine.Object) null))
        return;
      AssetDatabase.OpenAsset(target);
    }

    /// <summary>
    ///   <para>Loads the asset to memory.</para>
    /// </summary>
    public UnityEngine.Object Load()
    {
      if (this.state == Asset.States.DeletedLocal || this.isMeta)
        return (UnityEngine.Object) null;
      return AssetDatabase.LoadAssetAtPath(this.path, typeof (UnityEngine.Object));
    }

    internal static string StateToString(Asset.States state)
    {
      if (Asset.IsState(state, Asset.States.AddedLocal))
        return "Added Local";
      if (Asset.IsState(state, Asset.States.AddedRemote))
        return "Added Remote";
      if (Asset.IsState(state, Asset.States.CheckedOutLocal) && !Asset.IsState(state, Asset.States.LockedLocal))
        return "Checked Out Local";
      if (Asset.IsState(state, Asset.States.CheckedOutRemote) && !Asset.IsState(state, Asset.States.LockedRemote))
        return "Checked Out Remote";
      if (Asset.IsState(state, Asset.States.Conflicted))
        return "Conflicted";
      if (Asset.IsState(state, Asset.States.DeletedLocal))
        return "Deleted Local";
      if (Asset.IsState(state, Asset.States.DeletedRemote))
        return "Deleted Remote";
      if (Asset.IsState(state, Asset.States.Local))
        return "Local";
      if (Asset.IsState(state, Asset.States.LockedLocal))
        return "Locked Local";
      if (Asset.IsState(state, Asset.States.LockedRemote))
        return "Locked Remote";
      if (Asset.IsState(state, Asset.States.OutOfSync))
        return "Out Of Sync";
      return Asset.IsState(state, Asset.States.Updating) ? "Updating Status" : "";
    }

    internal static string AllStateToString(Asset.States state)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (Asset.IsState(state, Asset.States.AddedLocal))
        stringBuilder.AppendLine("Added Local");
      if (Asset.IsState(state, Asset.States.AddedRemote))
        stringBuilder.AppendLine("Added Remote");
      if (Asset.IsState(state, Asset.States.CheckedOutLocal))
        stringBuilder.AppendLine("Checked Out Local");
      if (Asset.IsState(state, Asset.States.CheckedOutRemote))
        stringBuilder.AppendLine("Checked Out Remote");
      if (Asset.IsState(state, Asset.States.Conflicted))
        stringBuilder.AppendLine("Conflicted");
      if (Asset.IsState(state, Asset.States.DeletedLocal))
        stringBuilder.AppendLine("Deleted Local");
      if (Asset.IsState(state, Asset.States.DeletedRemote))
        stringBuilder.AppendLine("Deleted Remote");
      if (Asset.IsState(state, Asset.States.Local))
        stringBuilder.AppendLine("Local");
      if (Asset.IsState(state, Asset.States.LockedLocal))
        stringBuilder.AppendLine("Locked Local");
      if (Asset.IsState(state, Asset.States.LockedRemote))
        stringBuilder.AppendLine("Locked Remote");
      if (Asset.IsState(state, Asset.States.OutOfSync))
        stringBuilder.AppendLine("Out Of Sync");
      if (Asset.IsState(state, Asset.States.Synced))
        stringBuilder.AppendLine("Synced");
      if (Asset.IsState(state, Asset.States.Missing))
        stringBuilder.AppendLine("Missing");
      if (Asset.IsState(state, Asset.States.ReadOnly))
        stringBuilder.AppendLine("ReadOnly");
      return stringBuilder.ToString();
    }

    internal string AllStateToString()
    {
      return Asset.AllStateToString(this.state);
    }

    internal string StateToString()
    {
      return Asset.StateToString(this.state);
    }

    public string prettyPath
    {
      get
      {
        return this.path;
      }
    }

    /// <summary>
    ///   <para>Describes the various version control states an asset can have.</para>
    /// </summary>
    [System.Flags]
    public enum States
    {
      None = 0,
      Local = 1,
      Synced = 2,
      OutOfSync = 4,
      Missing = 8,
      CheckedOutLocal = 16, // 0x00000010
      CheckedOutRemote = 32, // 0x00000020
      DeletedLocal = 64, // 0x00000040
      DeletedRemote = 128, // 0x00000080
      AddedLocal = 256, // 0x00000100
      AddedRemote = 512, // 0x00000200
      Conflicted = 1024, // 0x00000400
      LockedLocal = 2048, // 0x00000800
      LockedRemote = 4096, // 0x00001000
      Updating = 8192, // 0x00002000
      ReadOnly = 16384, // 0x00004000
      MetaFile = 32768, // 0x00008000
    }
  }
}
