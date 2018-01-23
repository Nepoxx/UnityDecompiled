// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.PackageUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

namespace UnityEditor.Connect
{
  internal class PackageUtils
  {
    private bool m_outdatedOperationRunning = false;
    private long m_outdatedOperationId = 0;
    private bool m_listOperationRunning = false;
    private long m_listOperationId = 0;
    private Dictionary<string, UpmPackageInfo> m_currentPackages = new Dictionary<string, UpmPackageInfo>();
    private Dictionary<string, UpmPackageInfo> m_outdatedPackages = new Dictionary<string, UpmPackageInfo>();
    private static readonly PackageUtils s_Instance = new PackageUtils();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool WaitForPackageManagerOperation(long operationId, string progressBarText);

    public static PackageUtils instance
    {
      get
      {
        return PackageUtils.s_Instance;
      }
    }

    public void RetrievePackageInfo()
    {
      if (NativeClient.List(out this.m_listOperationId) == NativeClient.StatusCode.Error)
      {
        Debug.LogWarning((object) "Failed to call list packages!");
      }
      else
      {
        this.m_listOperationRunning = true;
        if (NativeClient.Outdated(out this.m_outdatedOperationId) == NativeClient.StatusCode.Error)
          Debug.LogWarning((object) "Failed to call outdated package!");
        else
          this.m_outdatedOperationRunning = true;
      }
    }

    public string GetCurrentVersion(string packageName)
    {
      this.CheckRunningOperations();
      if (this.m_currentPackages.ContainsKey(packageName))
        return this.m_currentPackages[packageName].version;
      return string.Empty;
    }

    public string GetLatestVersion(string packageName)
    {
      this.CheckRunningOperations();
      if (this.m_outdatedPackages.ContainsKey(packageName))
        return this.m_outdatedPackages[packageName].version;
      return this.GetCurrentVersion(packageName);
    }

    public bool UpdateLatest(string packageName)
    {
      if (!this.m_outdatedPackages.ContainsKey(packageName))
        return false;
      long operationId = 0;
      if (NativeClient.Add(out operationId, this.m_outdatedPackages[packageName].packageId) == NativeClient.StatusCode.Error)
      {
        Debug.LogWarningFormat("Failed to update outdated package {0}!", (object) packageName);
        return false;
      }
      if (!PackageUtils.WaitForPackageManagerOperation(operationId, string.Format("Updating Package {0} to version {1}", (object) packageName, (object) this.m_outdatedPackages[packageName].version)))
        return false;
      UpmPackageInfo addOperationData = NativeClient.GetAddOperationData(operationId);
      this.m_currentPackages[this.GetPackageRootName(addOperationData)] = addOperationData;
      return true;
    }

    private void CheckRunningOperations()
    {
      if (this.m_outdatedOperationRunning)
      {
        switch (NativeClient.GetOperationStatus(this.m_outdatedOperationId))
        {
          case NativeClient.StatusCode.Done:
            this.m_outdatedPackages.Clear();
            Dictionary<string, OutdatedPackage> outdatedOperationData = NativeClient.GetOutdatedOperationData(this.m_outdatedOperationId);
            foreach (string key in outdatedOperationData.Keys)
              this.m_outdatedPackages[key] = outdatedOperationData[key].latest;
            this.m_outdatedOperationRunning = false;
            break;
          case NativeClient.StatusCode.Error:
          case NativeClient.StatusCode.NotFound:
            this.m_outdatedOperationRunning = false;
            Debug.LogWarning((object) "Failed to retrieve outdated package list!");
            break;
        }
      }
      if (!this.m_listOperationRunning)
        return;
      switch (NativeClient.GetOperationStatus(this.m_listOperationId))
      {
        case NativeClient.StatusCode.Done:
          this.m_currentPackages.Clear();
          OperationStatus listOperationData = NativeClient.GetListOperationData(this.m_listOperationId);
          for (int index = 0; index < listOperationData.packageList.Length; ++index)
            this.m_currentPackages[this.GetPackageRootName(listOperationData.packageList[index])] = listOperationData.packageList[index];
          this.m_listOperationRunning = false;
          break;
        case NativeClient.StatusCode.Error:
        case NativeClient.StatusCode.NotFound:
          this.m_listOperationRunning = false;
          Debug.LogWarning((object) "Failed to retrieve package list!");
          break;
      }
    }

    private string GetPackageRootName(UpmPackageInfo packageInfo)
    {
      return packageInfo.packageId.Substring(0, packageInfo.packageId.Length - packageInfo.version.Length - 1);
    }
  }
}
