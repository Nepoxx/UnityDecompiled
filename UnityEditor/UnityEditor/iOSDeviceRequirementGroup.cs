// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOSDeviceRequirementGroup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal sealed class iOSDeviceRequirementGroup
  {
    private string m_VariantName;

    internal iOSDeviceRequirementGroup(string variantName)
    {
      this.m_VariantName = variantName;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetDeviceRequirementForVariantNameImpl(string name, int index, out string[] keys, out string[] values);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetOrAddDeviceRequirementForVariantNameImpl(string name, int index, string[] keys, string[] values);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetCountForVariantImpl(string name);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void RemoveAtImpl(string name, int index);

    public int count
    {
      get
      {
        return iOSDeviceRequirementGroup.GetCountForVariantImpl(this.m_VariantName);
      }
    }

    public iOSDeviceRequirement this[int index]
    {
      get
      {
        string[] keys;
        string[] values;
        iOSDeviceRequirementGroup.GetDeviceRequirementForVariantNameImpl(this.m_VariantName, index, out keys, out values);
        iOSDeviceRequirement deviceRequirement = new iOSDeviceRequirement();
        for (int index1 = 0; index1 < keys.Length; ++index1)
          deviceRequirement.values.Add(keys[index1], values[index1]);
        return deviceRequirement;
      }
      set
      {
        iOSDeviceRequirementGroup.SetOrAddDeviceRequirementForVariantNameImpl(this.m_VariantName, index, value.values.Keys.ToArray<string>(), value.values.Values.ToArray<string>());
      }
    }

    public void RemoveAt(int index)
    {
      iOSDeviceRequirementGroup.RemoveAtImpl(this.m_VariantName, index);
    }

    public void Add(iOSDeviceRequirement requirement)
    {
      iOSDeviceRequirementGroup.SetOrAddDeviceRequirementForVariantNameImpl(this.m_VariantName, -1, requirement.values.Keys.ToArray<string>(), requirement.values.Values.ToArray<string>());
    }
  }
}
