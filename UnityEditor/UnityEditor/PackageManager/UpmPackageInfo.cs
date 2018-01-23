// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.UpmPackageInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.PackageManager
{
  [RequiredByNativeCode]
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  internal class UpmPackageInfo
  {
    [SerializeField]
    private string m_PackageId;
    [SerializeField]
    private string m_Version;
    [SerializeField]
    private string m_ResolvedPath;
    [SerializeField]
    private string m_Name;
    [SerializeField]
    private string m_DisplayName;
    [SerializeField]
    private string m_Category;
    [SerializeField]
    private string m_Description;

    private UpmPackageInfo()
    {
    }

    internal UpmPackageInfo(string packageId, string displayName = "", string category = "", string description = "", string resolvedPath = "", string tag = "")
    {
      this.m_PackageId = packageId;
      this.m_DisplayName = displayName;
      this.m_Category = category;
      this.m_Description = description;
      this.m_ResolvedPath = resolvedPath;
      string[] strArray = packageId.Split('@');
      this.m_Name = strArray[0];
      this.m_Version = strArray[1];
    }

    public static implicit operator PackageInfo(UpmPackageInfo source)
    {
      return new PackageInfo(source);
    }

    public string packageId
    {
      get
      {
        return this.m_PackageId;
      }
    }

    public string version
    {
      get
      {
        return this.m_Version;
      }
    }

    public string resolvedPath
    {
      get
      {
        return this.m_ResolvedPath;
      }
    }

    public string name
    {
      get
      {
        return this.m_Name;
      }
    }

    public string displayName
    {
      get
      {
        return this.m_DisplayName;
      }
    }

    public string category
    {
      get
      {
        return this.m_Category;
      }
    }

    public string description
    {
      get
      {
        return this.m_Description;
      }
    }
  }
}
