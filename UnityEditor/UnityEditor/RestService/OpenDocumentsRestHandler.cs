// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.OpenDocumentsRestHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;

namespace UnityEditor.RestService
{
  internal class OpenDocumentsRestHandler : JSONHandler
  {
    protected override JSONValue HandlePost(Request request, JSONValue payload)
    {
      ScriptEditorSettings.OpenDocuments = !payload.ContainsKey("documents") ? new List<string>() : payload["documents"].AsList().Select<JSONValue, string>((Func<JSONValue, string>) (d => d.AsString())).ToList<string>();
      ScriptEditorSettings.Save();
      return new JSONValue();
    }

    protected override JSONValue HandleGet(Request request, JSONValue payload)
    {
      JSONValue jsonValue = new JSONValue();
      jsonValue["documents"] = JSONHandler.ToJSON((IEnumerable<string>) ScriptEditorSettings.OpenDocuments);
      return jsonValue;
    }

    internal static void Register()
    {
      Router.RegisterHandler("/unity/opendocuments", (Handler) new OpenDocumentsRestHandler());
    }
  }
}
