// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.PackageInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.PackageManager
{
  /// <summary>
  ///   <para>Structure describing a Unity Package.</para>
  /// </summary>
  [Serializable]
  public class PackageInfo
  {
    [SerializeField]
    private UpmPackageInfo m_UpmPackageInfo;

    private PackageInfo()
    {
    }

    internal PackageInfo(UpmPackageInfo upmPackageInfo)
    {
      this.m_UpmPackageInfo = upmPackageInfo;
    }

    /// <summary>
    ///   <para>Identifier of the package.</para>
    /// </summary>
    public string packageId
    {
      get
      {
        return this.m_UpmPackageInfo.packageId;
      }
    }

    /// <summary>
    ///   <para>Version of the package.</para>
    /// </summary>
    public string version
    {
      get
      {
        return this.m_UpmPackageInfo.version;
      }
    }

    /// <summary>
    ///   <para>The local path of the project on disk.</para>
    /// </summary>
    public string resolvedPath
    {
      get
      {
        return this.m_UpmPackageInfo.resolvedPath;
      }
    }

    /// <summary>
    ///   <para>Unique name of the package.</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_UpmPackageInfo.name;
      }
    }

    /// <summary>
    ///   <para>Friendly display name of the package.</para>
    /// </summary>
    public string displayName
    {
      get
      {
        return this.m_UpmPackageInfo.displayName;
      }
    }

    /// <summary>
    ///   <para>Category of the package.</para>
    /// </summary>
    public string category
    {
      get
      {
        return this.m_UpmPackageInfo.category;
      }
    }

    /// <summary>
    ///   <para>Detailed description of the package.</para>
    /// </summary>
    public string description
    {
      get
      {
        return this.m_UpmPackageInfo.description;
      }
    }
  }
}
