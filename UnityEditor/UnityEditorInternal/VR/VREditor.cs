// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VREditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace UnityEditorInternal.VR
{
  public sealed class VREditor
  {
    private static Dictionary<BuildTargetGroup, bool> dirtyDeviceLists = new Dictionary<BuildTargetGroup, bool>();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern VRDeviceInfoEditor[] GetAllVRDeviceInfo(BuildTargetGroup targetGroup);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern VRDeviceInfoEditor[] GetAllVRDeviceInfoByTarget(BuildTarget target);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetVREnabledOnTargetGroup(BuildTargetGroup targetGroup);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetVREnabledOnTargetGroup(BuildTargetGroup targetGroup, bool value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetVREnabledDevicesOnTargetGroup(BuildTargetGroup targetGroup);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetVREnabledDevicesOnTarget(BuildTarget target);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetVREnabledDevicesOnTargetGroup(BuildTargetGroup targetGroup, string[] devices);

    public static bool IsDeviceListDirty(BuildTargetGroup targetGroup)
    {
      if (VREditor.dirtyDeviceLists.ContainsKey(targetGroup))
        return VREditor.dirtyDeviceLists[targetGroup];
      return false;
    }

    private static void SetDeviceListDirty(BuildTargetGroup targetGroup)
    {
      if (VREditor.dirtyDeviceLists.ContainsKey(targetGroup))
        VREditor.dirtyDeviceLists[targetGroup] = true;
      else
        VREditor.dirtyDeviceLists.Add(targetGroup, true);
    }

    public static void ClearDeviceListDirty(BuildTargetGroup targetGroup)
    {
      if (!VREditor.dirtyDeviceLists.ContainsKey(targetGroup))
        return;
      VREditor.dirtyDeviceLists[targetGroup] = false;
    }

    public static VRDeviceInfoEditor[] GetEnabledVRDeviceInfo(BuildTargetGroup targetGroup)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<VRDeviceInfoEditor>) VREditor.GetAllVRDeviceInfo(targetGroup)).Where<VRDeviceInfoEditor>(new Func<VRDeviceInfoEditor, bool>(new VREditor.\u003CGetEnabledVRDeviceInfo\u003Ec__AnonStorey0() { enabledVRDevices = VREditor.GetVREnabledDevicesOnTargetGroup(targetGroup) }.\u003C\u003Em__0)).ToArray<VRDeviceInfoEditor>();
    }

    public static VRDeviceInfoEditor[] GetEnabledVRDeviceInfo(BuildTarget target)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<VRDeviceInfoEditor>) VREditor.GetAllVRDeviceInfoByTarget(target)).Where<VRDeviceInfoEditor>(new Func<VRDeviceInfoEditor, bool>(new VREditor.\u003CGetEnabledVRDeviceInfo\u003Ec__AnonStorey1() { enabledVRDevices = VREditor.GetVREnabledDevicesOnTarget(target) }.\u003C\u003Em__0)).ToArray<VRDeviceInfoEditor>();
    }

    public static bool IsVRDeviceEnabledForBuildTarget(BuildTarget target, string deviceName)
    {
      foreach (string str in VREditor.GetVREnabledDevicesOnTarget(target))
      {
        if (str == deviceName)
          return true;
      }
      return false;
    }

    public static string[] GetAvailableVirtualRealitySDKs(BuildTargetGroup targetGroup)
    {
      VRDeviceInfoEditor[] allVrDeviceInfo = VREditor.GetAllVRDeviceInfo(targetGroup);
      string[] strArray = new string[allVrDeviceInfo.Length];
      for (int index = 0; index < allVrDeviceInfo.Length; ++index)
        strArray[index] = allVrDeviceInfo[index].deviceNameKey;
      return strArray;
    }

    public static string[] GetVirtualRealitySDKs(BuildTargetGroup targetGroup)
    {
      return VREditor.GetVREnabledDevicesOnTargetGroup(targetGroup);
    }

    public static void SetVirtualRealitySDKs(BuildTargetGroup targetGroup, string[] sdks)
    {
      VREditor.SetVREnabledDevicesOnTargetGroup(targetGroup, sdks);
      VREditor.SetDeviceListDirty(targetGroup);
    }
  }
}
