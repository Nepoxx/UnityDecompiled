// Decompiled with JetBrains decompiler
// Type: SimpleJson.IJsonSerializerStrategy
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.CodeDom.Compiler;

namespace SimpleJson
{
  [GeneratedCode("simple-json", "1.0.0")]
  internal interface IJsonSerializerStrategy
  {
    bool TrySerializeNonPrimitiveObject(object input, out object output);

    object DeserializeObject(object value, Type type);
  }
}
