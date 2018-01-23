// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.Task
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>A UnityEditor.VersionControl.Task is created almost everytime UnityEditor.VersionControl.Provider is ask to perform an action.</para>
  /// </summary>
  public sealed class Task
  {
    private IntPtr m_thisDummy;

    internal Task()
    {
    }

    /// <summary>
    ///   <para>A blocking wait for the task to complete.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Wait();

    /// <summary>
    ///   <para>Upon completion of a task a completion task will be performed if it is set.</para>
    /// </summary>
    /// <param name="action">Which completion action to perform.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetCompletionAction(CompletionAction action);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Asset[] Internal_GetAssetList();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern ChangeSet[] Internal_GetChangeSets();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Message[] Internal_GetMessages();

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Dispose();

    ~Task()
    {
      this.Dispose();
    }

    public extern int userIdentifier { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Will contain the result of the Provider.ChangeSetDescription task.</para>
    /// </summary>
    public extern string text { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>A short description of the current task.</para>
    /// </summary>
    public extern string description { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get whether or not the task was completed succesfully.</para>
    /// </summary>
    public extern bool success { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Total time spent in task since the task was started.</para>
    /// </summary>
    public extern int secondsSpent { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Progress of current task in precent.</para>
    /// </summary>
    public extern int progressPct { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern string progressMessage { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Some task return result codes, these are stored here.</para>
    /// </summary>
    public extern int resultCode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>May contain messages from the version control plugins.</para>
    /// </summary>
    public extern Message[] messages { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The result of some types of tasks.</para>
    /// </summary>
    public AssetList assetList
    {
      get
      {
        AssetList assetList = new AssetList();
        foreach (Asset asset in this.Internal_GetAssetList())
          assetList.Add(asset);
        return assetList;
      }
    }

    /// <summary>
    ///   <para>List of changesets returned by some tasks.</para>
    /// </summary>
    public ChangeSets changeSets
    {
      get
      {
        ChangeSets changeSets = new ChangeSets();
        foreach (ChangeSet changeSet in this.Internal_GetChangeSets())
          changeSets.Add(changeSet);
        return changeSets;
      }
    }
  }
}
