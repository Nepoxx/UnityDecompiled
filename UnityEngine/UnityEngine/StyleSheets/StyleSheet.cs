// Decompiled with JetBrains decompiler
// Type: UnityEngine.StyleSheets.StyleSheet
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.StyleSheets
{
  [Serializable]
  internal class StyleSheet : ScriptableObject
  {
    [SerializeField]
    private StyleRule[] m_Rules;
    [SerializeField]
    private StyleComplexSelector[] m_ComplexSelectors;
    [SerializeField]
    internal float[] floats;
    [SerializeField]
    internal Color[] colors;
    [SerializeField]
    internal string[] strings;

    public StyleRule[] rules
    {
      get
      {
        return this.m_Rules;
      }
      internal set
      {
        this.m_Rules = value;
        this.SetupReferences();
      }
    }

    public StyleComplexSelector[] complexSelectors
    {
      get
      {
        return this.m_ComplexSelectors;
      }
      internal set
      {
        this.m_ComplexSelectors = value;
        this.SetupReferences();
      }
    }

    private static bool TryCheckAccess<T>(T[] list, StyleValueType type, StyleValueHandle handle, out T value)
    {
      bool flag = false;
      value = default (T);
      if (handle.valueType == type && handle.valueIndex >= 0 && handle.valueIndex < list.Length)
      {
        value = list[handle.valueIndex];
        flag = true;
      }
      return flag;
    }

    private static T CheckAccess<T>(T[] list, StyleValueType type, StyleValueHandle handle)
    {
      T obj = default (T);
      if (handle.valueType != type)
        Debug.LogErrorFormat("Trying to read value of type {0} while reading a value of type {1}", new object[2]
        {
          (object) type,
          (object) handle.valueType
        });
      else if (handle.valueIndex < 0 && handle.valueIndex >= list.Length)
        Debug.LogError((object) "Accessing invalid property");
      else
        obj = list[handle.valueIndex];
      return obj;
    }

    private void OnEnable()
    {
      this.SetupReferences();
    }

    private void SetupReferences()
    {
      if (this.complexSelectors == null || this.rules == null)
        return;
      for (int index = 0; index < this.complexSelectors.Length; ++index)
      {
        StyleComplexSelector complexSelector = this.complexSelectors[index];
        if (complexSelector.ruleIndex < this.rules.Length)
          complexSelector.rule = this.rules[complexSelector.ruleIndex];
      }
    }

    public StyleValueKeyword ReadKeyword(StyleValueHandle handle)
    {
      return (StyleValueKeyword) handle.valueIndex;
    }

    public float ReadFloat(StyleValueHandle handle)
    {
      return StyleSheet.CheckAccess<float>(this.floats, StyleValueType.Float, handle);
    }

    public bool TryReadFloat(StyleValueHandle handle, out float value)
    {
      return StyleSheet.TryCheckAccess<float>(this.floats, StyleValueType.Float, handle, out value);
    }

    public Color ReadColor(StyleValueHandle handle)
    {
      return StyleSheet.CheckAccess<Color>(this.colors, StyleValueType.Color, handle);
    }

    public bool TryReadColor(StyleValueHandle handle, out Color value)
    {
      return StyleSheet.TryCheckAccess<Color>(this.colors, StyleValueType.Color, handle, out value);
    }

    public string ReadString(StyleValueHandle handle)
    {
      return StyleSheet.CheckAccess<string>(this.strings, StyleValueType.String, handle);
    }

    public bool TryReadString(StyleValueHandle handle, out string value)
    {
      return StyleSheet.TryCheckAccess<string>(this.strings, StyleValueType.String, handle, out value);
    }

    public string ReadEnum(StyleValueHandle handle)
    {
      return StyleSheet.CheckAccess<string>(this.strings, StyleValueType.Enum, handle);
    }

    public bool TryReadEnum(StyleValueHandle handle, out string value)
    {
      return StyleSheet.TryCheckAccess<string>(this.strings, StyleValueType.Enum, handle, out value);
    }

    public string ReadResourcePath(StyleValueHandle handle)
    {
      return StyleSheet.CheckAccess<string>(this.strings, StyleValueType.ResourcePath, handle);
    }

    public bool TryReadResourcePath(StyleValueHandle handle, out string value)
    {
      return StyleSheet.TryCheckAccess<string>(this.strings, StyleValueType.ResourcePath, handle, out value);
    }
  }
}
