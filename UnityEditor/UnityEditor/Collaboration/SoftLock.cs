// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.SoftLock
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEditor.Collaboration
{
  [StructLayout(LayoutKind.Sequential)]
  internal class SoftLock
  {
    private string m_UserID;
    private string m_MachineID;
    private string m_DisplayName;
    private ulong m_TimeStamp;
    private string m_Hash;

    private SoftLock()
    {
    }

    public string userID
    {
      get
      {
        return this.m_UserID;
      }
    }

    public string machineID
    {
      get
      {
        return this.m_MachineID;
      }
    }

    public string displayName
    {
      get
      {
        return this.m_DisplayName;
      }
    }

    public ulong timeStamp
    {
      get
      {
        return this.m_TimeStamp;
      }
    }

    public string hash
    {
      get
      {
        return this.m_Hash;
      }
    }
  }
}
