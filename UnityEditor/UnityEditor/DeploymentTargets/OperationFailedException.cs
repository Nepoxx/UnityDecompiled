// Decompiled with JetBrains decompiler
// Type: UnityEditor.DeploymentTargets.OperationFailedException
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.DeploymentTargets
{
  internal class OperationFailedException : Exception
  {
    public readonly string title;

    public OperationFailedException(string title, string message)
      : base(message)
    {
      this.title = title;
    }
  }
}
