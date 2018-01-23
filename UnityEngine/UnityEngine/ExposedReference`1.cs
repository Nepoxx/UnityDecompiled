// Decompiled with JetBrains decompiler
// Type: UnityEngine.ExposedReference`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode(Name = "ExposedReference")]
  [Serializable]
  public struct ExposedReference<T> where T : Object
  {
    [SerializeField]
    public PropertyName exposedName;
    [SerializeField]
    public Object defaultValue;

    public T Resolve(IExposedPropertyTable resolver)
    {
      if (resolver != null)
      {
        bool idValid;
        Object referenceValue = resolver.GetReferenceValue(this.exposedName, out idValid);
        if (idValid)
          return referenceValue as T;
      }
      return this.defaultValue as T;
    }
  }
}
