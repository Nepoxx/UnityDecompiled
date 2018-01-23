// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.OperationStatus
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEditor.PackageManager
{
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  internal class OperationStatus
  {
    private StatusCode m_Status;
    private string m_Id;
    private string m_Type;
    private UpmPackageInfo[] m_PackageList;
    private float m_Progress;

    private OperationStatus()
    {
    }

    public string id
    {
      get
      {
        return this.m_Id;
      }
    }

    public StatusCode status
    {
      get
      {
        return this.m_Status;
      }
    }

    public string type
    {
      get
      {
        return this.m_Type;
      }
    }

    public UpmPackageInfo[] packageList
    {
      get
      {
        return this.m_PackageList;
      }
    }

    public float progress
    {
      get
      {
        return this.m_Progress;
      }
    }
  }
}
