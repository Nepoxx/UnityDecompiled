// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreResponse
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class AssetStoreResponse
  {
    internal AsyncHTTPClient job;
    public Dictionary<string, JSONValue> dict;
    public bool ok;

    public bool failed
    {
      get
      {
        return !this.ok;
      }
    }

    public string message
    {
      get
      {
        if (this.dict == null || !this.dict.ContainsKey(nameof (message)))
          return (string) null;
        return this.dict[nameof (message)].AsString(true);
      }
    }

    private static string EncodeString(string str)
    {
      str = str.Replace("\"", "\\\"");
      str = str.Replace("\\", "\\\\");
      str = str.Replace("\b", "\\b");
      str = str.Replace("\f", "\\f");
      str = str.Replace("\n", "\\n");
      str = str.Replace("\r", "\\r");
      str = str.Replace("\t", "\\t");
      return str;
    }

    public override string ToString()
    {
      string str1 = "{";
      string str2 = "";
      foreach (KeyValuePair<string, JSONValue> keyValuePair in this.dict)
      {
        str1 = str1 + str2 + (object) '"' + AssetStoreResponse.EncodeString(keyValuePair.Key) + "\" : " + keyValuePair.Value.ToString();
        str2 = ", ";
      }
      return str1 + "}";
    }
  }
}
