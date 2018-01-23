// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.Error
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.PackageManager
{
  /// <summary>
  ///   <para>Structure describing the error of a package operation.</para>
  /// </summary>
  [RequiredByNativeCode]
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class Error
  {
    [SerializeField]
    private ErrorCode m_ErrorCode;
    [SerializeField]
    private string m_Message;

    private Error()
    {
    }

    internal Error(ErrorCode errorCode, string message)
    {
      this.m_ErrorCode = errorCode;
      this.m_Message = message;
    }

    /// <summary>
    ///   <para>Numerical error code.</para>
    /// </summary>
    public ErrorCode errorCode
    {
      get
      {
        return this.m_ErrorCode;
      }
    }

    /// <summary>
    ///   <para>Error message or description.</para>
    /// </summary>
    public string message
    {
      get
      {
        return this.m_Message;
      }
    }
  }
}
