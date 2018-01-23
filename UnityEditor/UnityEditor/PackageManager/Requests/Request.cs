// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.Requests.Request
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.PackageManager.Requests
{
  /// <summary>
  ///   <para>Tracks the state of an asynchronous Unity Package Manager (Upm) server operation.</para>
  /// </summary>
  public abstract class Request
  {
    [SerializeField]
    private NativeClient.StatusCode m_Status = NativeClient.StatusCode.NotFound;
    [SerializeField]
    private bool m_ErrorFetched;
    [SerializeField]
    private Error m_Error;
    [SerializeField]
    private long m_Id;

    internal Request()
    {
    }

    internal Request(long operationId, NativeClient.StatusCode initialStatus)
    {
      this.m_Id = operationId;
      this.m_Status = initialStatus;
    }

    private NativeClient.StatusCode NativeStatusCode
    {
      get
      {
        if (this.m_Status <= NativeClient.StatusCode.InProgress)
          this.m_Status = NativeClient.GetOperationStatus(this.Id);
        return this.m_Status;
      }
    }

    protected long Id
    {
      get
      {
        return this.m_Id;
      }
    }

    /// <summary>
    ///   <para>The request status.</para>
    /// </summary>
    public StatusCode Status
    {
      get
      {
        switch (this.NativeStatusCode)
        {
          case NativeClient.StatusCode.InQueue:
          case NativeClient.StatusCode.InProgress:
            return StatusCode.InProgress;
          case NativeClient.StatusCode.Done:
            return StatusCode.Success;
          case NativeClient.StatusCode.Error:
          case NativeClient.StatusCode.NotFound:
            return StatusCode.Failure;
          default:
            throw new NotSupportedException(string.Format("Unknown native status code {0}", (object) this.NativeStatusCode));
        }
      }
    }

    /// <summary>
    ///   <para>Whether the request is complete.</para>
    /// </summary>
    public bool IsCompleted
    {
      get
      {
        return this.Status > StatusCode.InProgress;
      }
    }

    /// <summary>
    ///   <para>The error returned by the request, if any.</para>
    /// </summary>
    public Error Error
    {
      get
      {
        if (!this.m_ErrorFetched && this.Status == StatusCode.Failure)
        {
          this.m_ErrorFetched = true;
          this.m_Error = NativeClient.GetOperationError(this.Id);
          if (this.m_Error == null)
            this.m_Error = this.NativeStatusCode != NativeClient.StatusCode.NotFound ? new Error(ErrorCode.Unknown, "Unknown error") : new Error(ErrorCode.NotFound, "Operation not found");
        }
        return this.m_Error;
      }
    }
  }
}
