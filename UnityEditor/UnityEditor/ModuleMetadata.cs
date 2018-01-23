// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModuleMetadata
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal sealed class ModuleMetadata
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetModuleNames();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetModuleDependencies(string moduleName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetModuleStrippable(string moduleName);

    public static UnityType[] GetModuleTypes(string moduleName)
    {
      return ((IEnumerable<uint>) ModuleMetadata.GetModuleTypeIndices(moduleName)).Select<uint, UnityType>((Func<uint, UnityType>) (index => UnityType.GetTypeByRuntimeTypeIndex(index))).ToArray<UnityType>();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ModuleIncludeSetting GetModuleIncludeSettingForModule(string module);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetModuleIncludeSettingForModule(string module, ModuleIncludeSetting setting);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern ModuleIncludeSetting GetModuleIncludeSettingForObject(UnityEngine.Object o);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern uint[] GetModuleTypeIndices(string moduleName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetICallModule(string icall);
  }
}
