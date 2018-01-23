// Decompiled with JetBrains decompiler
// Type: SimpleJson.JsonArray
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleJson
{
  [GeneratedCode("simple-json", "1.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  internal class JsonArray : List<object>
  {
    public JsonArray()
    {
    }

    public JsonArray(int capacity)
      : base(capacity)
    {
    }

    public override string ToString()
    {
      return SimpleJson.SimpleJson.SerializeObject((object) this) ?? string.Empty;
    }
  }
}
