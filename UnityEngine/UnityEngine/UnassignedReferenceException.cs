// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnassignedReferenceException
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.Serialization;

namespace UnityEngine
{
  [Serializable]
  public class UnassignedReferenceException : SystemException
  {
    private const int Result = -2147467261;
    private string unityStackTrace;

    public UnassignedReferenceException()
      : base("A Unity Runtime error occurred!")
    {
      this.HResult = -2147467261;
    }

    public UnassignedReferenceException(string message)
      : base(message)
    {
      this.HResult = -2147467261;
    }

    public UnassignedReferenceException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.HResult = -2147467261;
    }

    protected UnassignedReferenceException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
