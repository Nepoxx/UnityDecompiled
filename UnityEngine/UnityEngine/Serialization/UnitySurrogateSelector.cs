// Decompiled with JetBrains decompiler
// Type: UnityEngine.Serialization.UnitySurrogateSelector
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UnityEngine.Serialization
{
  public class UnitySurrogateSelector : ISurrogateSelector
  {
    public ISerializationSurrogate GetSurrogate(System.Type type, StreamingContext context, out ISurrogateSelector selector)
    {
      if (type.IsGenericType)
      {
        System.Type genericTypeDefinition = type.GetGenericTypeDefinition();
        if (genericTypeDefinition == typeof (List<>))
        {
          selector = (ISurrogateSelector) this;
          return ListSerializationSurrogate.Default;
        }
        if (genericTypeDefinition == typeof (Dictionary<,>))
        {
          selector = (ISurrogateSelector) this;
          return (ISerializationSurrogate) Activator.CreateInstance(typeof (DictionarySerializationSurrogate<,>).MakeGenericType(type.GetGenericArguments()));
        }
      }
      selector = (ISurrogateSelector) null;
      return (ISerializationSurrogate) null;
    }

    public void ChainSelector(ISurrogateSelector selector)
    {
      throw new NotImplementedException();
    }

    public ISurrogateSelector GetNextSelector()
    {
      throw new NotImplementedException();
    }
  }
}
