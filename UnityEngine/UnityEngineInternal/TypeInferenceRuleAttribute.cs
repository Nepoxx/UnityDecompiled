// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.TypeInferenceRuleAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngineInternal
{
  [AttributeUsage(AttributeTargets.Method)]
  [Serializable]
  public class TypeInferenceRuleAttribute : Attribute
  {
    private readonly string _rule;

    public TypeInferenceRuleAttribute(TypeInferenceRules rule)
      : this(rule.ToString())
    {
    }

    public TypeInferenceRuleAttribute(string rule)
    {
      this._rule = rule;
    }

    public override string ToString()
    {
      return this._rule;
    }
  }
}
