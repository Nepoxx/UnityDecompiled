// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoScript
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Representation of Script assets.</para>
  /// </summary>
  public sealed class MonoScript : TextAsset
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern MonoScript();

    /// <summary>
    ///   <para>Returns the System.Type object of the class implemented by this script.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern System.Type GetClass();

    /// <summary>
    ///   <para>Returns the MonoScript object containing specified MonoBehaviour.</para>
    /// </summary>
    /// <param name="behaviour">The MonoBehaviour whose MonoScript should be returned.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern MonoScript FromMonoBehaviour(MonoBehaviour behaviour);

    /// <summary>
    ///   <para>Returns the MonoScript object containing specified ScriptableObject.</para>
    /// </summary>
    /// <param name="scriptableObject">The ScriptableObject whose MonoScript should be returned.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern MonoScript FromScriptableObject(ScriptableObject scriptableObject);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool GetScriptTypeWasJustCreatedFromComponentMenu();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetScriptTypeWasJustCreatedFromComponentMenu();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void Init(string scriptContents, string className, string nameSpace, string assemblyName, bool isEditorScript);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string GetNamespace();
  }
}
