// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.SerializableJsonDictionary
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
  internal class SerializableJsonDictionary : ScriptableObject, ISerializationCallbackReceiver, ISerializableJsonDictionary
  {
    [SerializeField]
    private List<string> m_Keys = new List<string>();
    [SerializeField]
    private List<string> m_Values = new List<string>();
    [NonSerialized]
    private Dictionary<string, object> m_Dict = new Dictionary<string, object>();

    public void Set<T>(string key, T value) where T : class
    {
      this.m_Dict[key] = (object) value;
    }

    public T Get<T>(string key) where T : class
    {
      if (!this.ContainsKey(key))
        return (T) null;
      if (this.m_Dict[key] is string)
      {
        T instance = Activator.CreateInstance<T>();
        EditorJsonUtility.FromJsonOverwrite((string) this.m_Dict[key], (object) instance);
        this.m_Dict[key] = (object) instance;
      }
      return this.m_Dict[key] as T;
    }

    public T GetScriptable<T>(string key) where T : ScriptableObject
    {
      if (!this.ContainsKey(key))
        return (T) null;
      if (this.m_Dict[key] is string)
      {
        T instance = ScriptableObject.CreateInstance<T>();
        EditorJsonUtility.FromJsonOverwrite((string) this.m_Dict[key], (object) instance);
        this.m_Dict[key] = (object) instance;
      }
      return this.m_Dict[key] as T;
    }

    public void Overwrite(object obj, string key)
    {
      if (!this.ContainsKey(key))
        return;
      if (this.m_Dict[key] is string)
      {
        EditorJsonUtility.FromJsonOverwrite((string) this.m_Dict[key], obj);
        this.m_Dict[key] = obj;
      }
      else
      {
        if (this.m_Dict[key] == obj)
          return;
        EditorJsonUtility.FromJsonOverwrite(EditorJsonUtility.ToJson(this.m_Dict[key]), obj);
        this.m_Dict[key] = obj;
      }
    }

    public bool ContainsKey(string key)
    {
      return this.m_Dict.ContainsKey(key);
    }

    public void OnBeforeSerialize()
    {
      this.m_Keys.Clear();
      this.m_Values.Clear();
      foreach (KeyValuePair<string, object> keyValuePair in this.m_Dict)
      {
        if (keyValuePair.Key != null && keyValuePair.Value != null)
        {
          this.m_Keys.Add(keyValuePair.Key);
          this.m_Values.Add(EditorJsonUtility.ToJson(keyValuePair.Value));
        }
      }
    }

    public void OnAfterDeserialize()
    {
      if (this.m_Keys.Count == this.m_Values.Count)
        this.m_Dict = Enumerable.Range(0, this.m_Keys.Count).ToDictionary<int, string, object>((Func<int, string>) (i => this.m_Keys[i]), (Func<int, object>) (i => (object) this.m_Values[i]));
      this.m_Keys.Clear();
      this.m_Values.Clear();
    }
  }
}
