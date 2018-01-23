// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.OutdatedPackage
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
  internal class OutdatedPackage
  {
    [SerializeField]
    private UpmPackageInfo m_Current;
    [SerializeField]
    private UpmPackageInfo m_Latest;

    private OutdatedPackage()
    {
    }

    public OutdatedPackage(UpmPackageInfo current, UpmPackageInfo latest)
    {
      this.m_Current = current;
      this.m_Latest = latest;
    }

    public UpmPackageInfo current
    {
      get
      {
        return this.m_Current;
      }
    }

    public UpmPackageInfo latest
    {
      get
      {
        return this.m_Latest;
      }
    }
  }
}
