// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.Requests.Request`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.PackageManager.Requests
{
  /// <summary>
  ///   <para>Tracks the state of an asynchronous Unity Package Manager (Upm) server operation that returns a non-empty response.</para>
  /// </summary>
  public abstract class Request<T> : Request
  {
    [SerializeField]
    private bool m_ResultFetched = false;
    [SerializeField]
    private T m_Result = default (T);

    internal Request()
    {
    }

    internal Request(long operationId, NativeClient.StatusCode initialStatus)
      : base(operationId, initialStatus)
    {
    }

    protected abstract T GetResult();

    public T Result
    {
      get
      {
        if (!this.m_ResultFetched && this.Status == StatusCode.Success)
        {
          this.m_ResultFetched = true;
          this.m_Result = this.GetResult();
        }
        return this.m_Result;
      }
    }
  }
}
