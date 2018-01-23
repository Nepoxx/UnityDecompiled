// Decompiled with JetBrains decompiler
// Type: UnityEngine.ResourceRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Asynchronous load request from the Resources bundle.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public class ResourceRequest : AsyncOperation
  {
    internal string m_Path;
    internal System.Type m_Type;

    /// <summary>
    ///   <para>Asset object being loaded (Read Only).</para>
    /// </summary>
    public Object asset
    {
      get
      {
        return Resources.Load(this.m_Path, this.m_Type);
      }
    }
  }
}
