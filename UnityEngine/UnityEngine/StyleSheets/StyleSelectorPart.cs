// Decompiled with JetBrains decompiler
// Type: UnityEngine.StyleSheets.StyleSelectorPart
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.StyleSheets
{
  [Serializable]
  internal struct StyleSelectorPart
  {
    [SerializeField]
    private string m_Value;
    [SerializeField]
    private StyleSelectorType m_Type;
    internal object tempData;

    public string value
    {
      get
      {
        return this.m_Value;
      }
      internal set
      {
        this.m_Value = value;
      }
    }

    public StyleSelectorType type
    {
      get
      {
        return this.m_Type;
      }
      internal set
      {
        this.m_Type = value;
      }
    }

    public override string ToString()
    {
      return string.Format("[StyleSelectorPart: value={0}, type={1}]", (object) this.value, (object) this.type);
    }

    public static StyleSelectorPart CreateClass(string className)
    {
      return new StyleSelectorPart() { m_Type = StyleSelectorType.Class, m_Value = className };
    }

    public static StyleSelectorPart CreateId(string Id)
    {
      return new StyleSelectorPart() { m_Type = StyleSelectorType.ID, m_Value = Id };
    }

    public static StyleSelectorPart CreateType(System.Type t)
    {
      return new StyleSelectorPart() { m_Type = StyleSelectorType.Type, m_Value = t.Name };
    }

    public static StyleSelectorPart CreatePredicate(object predicate)
    {
      return new StyleSelectorPart() { m_Type = StyleSelectorType.Predicate, tempData = predicate };
    }

    public static StyleSelectorPart CreateWildCard()
    {
      return new StyleSelectorPart() { m_Type = StyleSelectorType.Wildcard };
    }
  }
}
