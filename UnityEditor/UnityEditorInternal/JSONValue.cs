// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.JSONValue
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditorInternal
{
  internal struct JSONValue
  {
    private object data;

    public JSONValue(object o)
    {
      this.data = o;
    }

    public bool IsString()
    {
      return this.data is string;
    }

    public bool IsFloat()
    {
      return this.data is float;
    }

    public bool IsList()
    {
      return this.data is List<JSONValue>;
    }

    public bool IsDict()
    {
      return this.data is Dictionary<string, JSONValue>;
    }

    public bool IsBool()
    {
      return this.data is bool;
    }

    public bool IsNull()
    {
      return this.data == null;
    }

    public static implicit operator JSONValue(string s)
    {
      return new JSONValue((object) s);
    }

    public static implicit operator JSONValue(float s)
    {
      return new JSONValue((object) s);
    }

    public static implicit operator JSONValue(bool s)
    {
      return new JSONValue((object) s);
    }

    public static implicit operator JSONValue(int s)
    {
      return new JSONValue((object) (float) s);
    }

    public object AsObject()
    {
      return this.data;
    }

    public string AsString(bool nothrow)
    {
      if (this.data is string)
        return (string) this.data;
      if (!nothrow)
        throw new JSONTypeException("Tried to read non-string json value as string");
      return "";
    }

    public string AsString()
    {
      return this.AsString(false);
    }

    public float AsFloat(bool nothrow)
    {
      if (this.data is float)
        return (float) this.data;
      if (!nothrow)
        throw new JSONTypeException("Tried to read non-float json value as float");
      return 0.0f;
    }

    public float AsFloat()
    {
      return this.AsFloat(false);
    }

    public bool AsBool(bool nothrow)
    {
      if (this.data is bool)
        return (bool) this.data;
      if (!nothrow)
        throw new JSONTypeException("Tried to read non-bool json value as bool");
      return false;
    }

    public bool AsBool()
    {
      return this.AsBool(false);
    }

    public List<JSONValue> AsList(bool nothrow)
    {
      if (this.data is List<JSONValue>)
        return (List<JSONValue>) this.data;
      if (!nothrow)
        throw new JSONTypeException("Tried to read " + this.data.GetType().Name + " json value as list");
      return (List<JSONValue>) null;
    }

    public List<JSONValue> AsList()
    {
      return this.AsList(false);
    }

    public Dictionary<string, JSONValue> AsDict(bool nothrow)
    {
      if (this.data is Dictionary<string, JSONValue>)
        return (Dictionary<string, JSONValue>) this.data;
      if (!nothrow)
        throw new JSONTypeException("Tried to read non-dictionary json value as dictionary");
      return (Dictionary<string, JSONValue>) null;
    }

    public Dictionary<string, JSONValue> AsDict()
    {
      return this.AsDict(false);
    }

    public static JSONValue NewString(string val)
    {
      return new JSONValue((object) val);
    }

    public static JSONValue NewFloat(float val)
    {
      return new JSONValue((object) val);
    }

    public static JSONValue NewDict()
    {
      return new JSONValue((object) new Dictionary<string, JSONValue>());
    }

    public static JSONValue NewList()
    {
      return new JSONValue((object) new List<JSONValue>());
    }

    public static JSONValue NewBool(bool val)
    {
      return new JSONValue((object) val);
    }

    public static JSONValue NewNull()
    {
      return new JSONValue((object) null);
    }

    public JSONValue this[string index]
    {
      get
      {
        return this.AsDict()[index];
      }
      set
      {
        if (this.data == null)
          this.data = (object) new Dictionary<string, JSONValue>();
        this.AsDict()[index] = value;
      }
    }

    public bool ContainsKey(string index)
    {
      if (!this.IsDict())
        return false;
      return this.AsDict().ContainsKey(index);
    }

    public JSONValue Get(string key)
    {
      if (!this.IsDict())
        return new JSONValue((object) null);
      JSONValue jsonValue = this;
      string str = key;
      char[] chArray = new char[1]{ '.' };
      foreach (string index in str.Split(chArray))
      {
        if (!jsonValue.ContainsKey(index))
          return new JSONValue((object) null);
        jsonValue = jsonValue[index];
      }
      return jsonValue;
    }

    public void Set(string key, string value)
    {
      if (value == null)
        this[key] = JSONValue.NewNull();
      else
        this[key] = JSONValue.NewString(value);
    }

    public void Set(string key, float value)
    {
      this[key] = JSONValue.NewFloat(value);
    }

    public void Set(string key, bool value)
    {
      this[key] = JSONValue.NewBool(value);
    }

    public void Add(string value)
    {
      List<JSONValue> jsonValueList = this.AsList();
      if (value == null)
        jsonValueList.Add(JSONValue.NewNull());
      else
        jsonValueList.Add(JSONValue.NewString(value));
    }

    public void Add(float value)
    {
      this.AsList().Add(JSONValue.NewFloat(value));
    }

    public void Add(bool value)
    {
      this.AsList().Add(JSONValue.NewBool(value));
    }

    public override string ToString()
    {
      if (this.IsString())
        return "\"" + JSONValue.EncodeString(this.AsString()) + "\"";
      if (this.IsFloat())
        return this.AsFloat().ToString();
      if (this.IsList())
      {
        string str1 = "[";
        string str2 = "";
        foreach (JSONValue jsonValue in this.AsList())
        {
          str1 = str1 + str2 + jsonValue.ToString();
          str2 = ", ";
        }
        return str1 + "]";
      }
      if (this.IsDict())
      {
        string str1 = "{";
        string str2 = "";
        foreach (KeyValuePair<string, JSONValue> keyValuePair in this.AsDict())
        {
          str1 = str1 + str2 + (object) '"' + JSONValue.EncodeString(keyValuePair.Key) + "\" : " + keyValuePair.Value.ToString();
          str2 = ", ";
        }
        return str1 + "}";
      }
      if (this.IsBool())
        return !this.AsBool() ? "false" : "true";
      if (this.IsNull())
        return "null";
      throw new JSONTypeException("Cannot serialize json value of unknown type");
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
  }
}
