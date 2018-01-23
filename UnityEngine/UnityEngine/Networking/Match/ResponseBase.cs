// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.ResponseBase
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
  internal abstract class ResponseBase
  {
    public abstract void Parse(object obj);

    public string ParseJSONString(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return obj as string;
      throw new FormatException(name + " not found in JSON dictionary");
    }

    public short ParseJSONInt16(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToInt16(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    public int ParseJSONInt32(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToInt32(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    public long ParseJSONInt64(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToInt64(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    public ushort ParseJSONUInt16(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToUInt16(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    public uint ParseJSONUInt32(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToUInt32(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    public ulong ParseJSONUInt64(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToUInt64(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    public bool ParseJSONBool(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToBoolean(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    public DateTime ParseJSONDateTime(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      throw new FormatException(name + " DateTime not yet supported");
    }

    public List<string> ParseJSONListOfStrings(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
      {
        List<object> objectList = obj as List<object>;
        if (objectList != null)
        {
          List<string> stringList = new List<string>();
          foreach (IEnumerable<KeyValuePair<string, object>> keyValuePairs in objectList)
          {
            foreach (KeyValuePair<string, object> keyValuePair in keyValuePairs)
            {
              string str = (string) keyValuePair.Value;
              stringList.Add(str);
            }
          }
          return stringList;
        }
      }
      throw new FormatException(name + " not found in JSON dictionary");
    }

    public List<T> ParseJSONList<T>(string name, object obj, IDictionary<string, object> dictJsonObj) where T : ResponseBase, new()
    {
      if (dictJsonObj.TryGetValue(name, out obj))
      {
        List<object> objectList = obj as List<object>;
        if (objectList != null)
        {
          List<T> objList = new List<T>();
          foreach (IDictionary<string, object> dictionary in objectList)
          {
            T instance = Activator.CreateInstance<T>();
            instance.Parse((object) dictionary);
            objList.Add(instance);
          }
          return objList;
        }
      }
      throw new FormatException(name + " not found in JSON dictionary");
    }
  }
}
