// Decompiled with JetBrains decompiler
// Type: UnityEditor.UnityType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal sealed class UnityType
  {
    private uint runtimeTypeIndex;
    private uint descendantCount;
    private static UnityType[] ms_types;
    private static ReadOnlyCollection<UnityType> ms_typesReadOnly;
    private static Dictionary<int, UnityType> ms_idToType;
    private static Dictionary<string, UnityType> ms_nameToType;

    static UnityType()
    {
      UnityType.UnityTypeTransport[] allTypes = UnityType.Internal_GetAllTypes();
      UnityType.ms_types = new UnityType[allTypes.Length];
      UnityType.ms_idToType = new Dictionary<int, UnityType>();
      UnityType.ms_nameToType = new Dictionary<string, UnityType>();
      for (int index = 0; index < allTypes.Length; ++index)
      {
        UnityType unityType1 = (UnityType) null;
        if ((long) allTypes[index].baseClassIndex < (long) allTypes.Length)
          unityType1 = UnityType.ms_types[(IntPtr) allTypes[index].baseClassIndex];
        UnityType unityType2 = new UnityType() { runtimeTypeIndex = allTypes[index].runtimeTypeIndex, descendantCount = allTypes[index].descendantCount, name = allTypes[index].className, nativeNamespace = allTypes[index].classNamespace, persistentTypeID = allTypes[index].persistentTypeID, baseClass = unityType1, flags = (UnityTypeFlags) allTypes[index].flags };
        UnityType.ms_types[index] = unityType2;
        UnityType.ms_typesReadOnly = new ReadOnlyCollection<UnityType>((IList<UnityType>) UnityType.ms_types);
        UnityType.ms_idToType[unityType2.persistentTypeID] = unityType2;
        UnityType.ms_nameToType[unityType2.name] = unityType2;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern UnityType.UnityTypeTransport[] Internal_GetAllTypes();

    public string name { get; private set; }

    public string nativeNamespace { get; private set; }

    public int persistentTypeID { get; private set; }

    public UnityType baseClass { get; private set; }

    public UnityTypeFlags flags { get; private set; }

    public bool isAbstract
    {
      get
      {
        return (this.flags & UnityTypeFlags.Abstract) != (UnityTypeFlags) 0;
      }
    }

    public bool isSealed
    {
      get
      {
        return (this.flags & UnityTypeFlags.Sealed) != (UnityTypeFlags) 0;
      }
    }

    public bool isEditorOnly
    {
      get
      {
        return (this.flags & UnityTypeFlags.EditorOnly) != (UnityTypeFlags) 0;
      }
    }

    public string qualifiedName
    {
      get
      {
        return !this.hasNativeNamespace ? this.name : this.nativeNamespace + "::" + this.name;
      }
    }

    public bool hasNativeNamespace
    {
      get
      {
        return this.nativeNamespace.Length > 0;
      }
    }

    public bool IsDerivedFrom(UnityType baseClass)
    {
      return this.runtimeTypeIndex - baseClass.runtimeTypeIndex < baseClass.descendantCount;
    }

    public static UnityType FindTypeByPersistentTypeID(int persistentTypeId)
    {
      UnityType unityType = (UnityType) null;
      UnityType.ms_idToType.TryGetValue(persistentTypeId, out unityType);
      return unityType;
    }

    public static uint TypeCount
    {
      get
      {
        return (uint) UnityType.ms_types.Length;
      }
    }

    public static UnityType GetTypeByRuntimeTypeIndex(uint index)
    {
      return UnityType.ms_types[(IntPtr) index];
    }

    public static UnityType FindTypeByName(string name)
    {
      UnityType unityType = (UnityType) null;
      UnityType.ms_nameToType.TryGetValue(name, out unityType);
      return unityType;
    }

    public static UnityType FindTypeByNameCaseInsensitive(string name)
    {
      return ((IEnumerable<UnityType>) UnityType.ms_types).FirstOrDefault<UnityType>((Func<UnityType, bool>) (t => string.Equals(name, t.name, StringComparison.OrdinalIgnoreCase)));
    }

    public static ReadOnlyCollection<UnityType> GetTypes()
    {
      return UnityType.ms_typesReadOnly;
    }

    [UsedByNativeCode]
    private struct UnityTypeTransport
    {
      public uint runtimeTypeIndex;
      public uint descendantCount;
      public uint baseClassIndex;
      public string className;
      public string classNamespace;
      public int persistentTypeID;
      public uint flags;
    }
  }
}
