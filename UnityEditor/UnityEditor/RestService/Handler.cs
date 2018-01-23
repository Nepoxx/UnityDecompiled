// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.Handler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.RestService
{
  [RequiredByNativeCode]
  internal abstract class Handler
  {
    protected abstract void InvokeGet(Request request, string payload, Response writeResponse);

    protected abstract void InvokePost(Request request, string payload, Response writeResponse);

    protected abstract void InvokeDelete(Request request, string payload, Response writeResponse);
  }
}
