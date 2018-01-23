// Decompiled with JetBrains decompiler
// Type: SimpleJson.JsonObject
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleJson
{
  [GeneratedCode("simple-json", "1.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  internal class JsonObject : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
  {
    private readonly Dictionary<string, object> _members;

    public JsonObject()
    {
      this._members = new Dictionary<string, object>();
    }

    public JsonObject(IEqualityComparer<string> comparer)
    {
      this._members = new Dictionary<string, object>(comparer);
    }

    public object this[int index]
    {
      get
      {
        return JsonObject.GetAtIndex((IDictionary<string, object>) this._members, index);
      }
    }

    internal static object GetAtIndex(IDictionary<string, object> obj, int index)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      if (index >= obj.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      int num = 0;
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) obj)
      {
        if (num++ == index)
          return keyValuePair.Value;
      }
      return (object) null;
    }

    public void Add(string key, object value)
    {
      this._members.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
      return this._members.ContainsKey(key);
    }

    public ICollection<string> Keys
    {
      get
      {
        return (ICollection<string>) this._members.Keys;
      }
    }

    public bool Remove(string key)
    {
      return this._members.Remove(key);
    }

    public bool TryGetValue(string key, out object value)
    {
      return this._members.TryGetValue(key, out value);
    }

    public ICollection<object> Values
    {
      get
      {
        return (ICollection<object>) this._members.Values;
      }
    }

    public object this[string key]
    {
      get
      {
        return this._members[key];
      }
      set
      {
        this._members[key] = value;
      }
    }

    public void Add(KeyValuePair<string, object> item)
    {
      this._members.Add(item.Key, item.Value);
    }

    public void Clear()
    {
      this._members.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
      return this._members.ContainsKey(item.Key) && this._members[item.Key] == item.Value;
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      int count = this.Count;
      foreach (KeyValuePair<string, object> keyValuePair in this)
      {
        array[arrayIndex++] = keyValuePair;
        if (--count <= 0)
          break;
      }
    }

    public int Count
    {
      get
      {
        return this._members.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
      return this._members.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<string, object>>) this._members.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this._members.GetEnumerator();
    }

    public override string ToString()
    {
      return SimpleJson.SimpleJson.SerializeObject((object) this);
    }
  }
}
