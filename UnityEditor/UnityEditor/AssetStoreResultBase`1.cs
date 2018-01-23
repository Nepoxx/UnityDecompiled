// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreResultBase`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal abstract class AssetStoreResultBase<Derived> where Derived : class
  {
    private AssetStoreResultBase<Derived>.Callback callback;
    public string error;
    public string warnings;

    public AssetStoreResultBase(AssetStoreResultBase<Derived>.Callback cb)
    {
      this.callback = cb;
      this.warnings = "";
    }

    public void Parse(AssetStoreResponse response)
    {
      if (response.job.IsSuccess())
      {
        if (response.job.responseCode >= 300)
          this.error = string.Format("HTTP status code {0}", (object) response.job.responseCode);
        else if (response.dict.ContainsKey("error"))
          this.error = response.dict["error"].AsString(true);
        else
          this.Parse(response.dict);
      }
      else
        this.error = "Error receiving response from server on url '" + (response.job != null ? (response.job.url != null ? response.job.url : "null") : "nulljob") + "': " + (response.job.text ?? "n/a");
      this.callback((object) this as Derived);
    }

    protected abstract void Parse(Dictionary<string, JSONValue> dict);

    public delegate void Callback(Derived res) where Derived : class;
  }
}
