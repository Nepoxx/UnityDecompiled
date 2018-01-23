// Decompiled with JetBrains decompiler
// Type: UnityEngine.Assertions.AssertionException
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Assertions
{
  public class AssertionException : Exception
  {
    private string m_UserMessage;

    public AssertionException(string message, string userMessage)
      : base(message)
    {
      this.m_UserMessage = userMessage;
    }

    public override string Message
    {
      get
      {
        string str = base.Message;
        if (this.m_UserMessage != null)
          str = str + (object) '\n' + this.m_UserMessage;
        return str;
      }
    }
  }
}
