// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.BaseTypeFactory`2
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal abstract class BaseTypeFactory<TKey, TValue>
  {
    private readonly Dictionary<System.Type, System.Type> m_Mappings = new Dictionary<System.Type, System.Type>();
    private static readonly System.Type k_KeyType = typeof (TKey);
    private static readonly System.Type k_ValueType = typeof (TValue);
    private readonly System.Type m_FallbackType;

    protected BaseTypeFactory()
      : this(typeof (TValue))
    {
    }

    protected BaseTypeFactory(System.Type fallbackType)
    {
      this.m_FallbackType = fallbackType;
    }

    public System.Type this[System.Type t]
    {
      get
      {
        try
        {
          return this.m_Mappings[t];
        }
        catch (KeyNotFoundException ex)
        {
          throw new KeyNotFoundException("Type " + t.Name + " is not registered in the factory.", (Exception) ex);
        }
      }
      set
      {
        if (!t.IsSubclassOf(BaseTypeFactory<TKey, TValue>.k_KeyType) && !((IEnumerable<System.Type>) t.GetInterfaces()).Contains<System.Type>(BaseTypeFactory<TKey, TValue>.k_KeyType))
          throw new ArgumentException("The type passed as key (" + t.Name + ") does not implement or derive from " + BaseTypeFactory<TKey, TValue>.k_KeyType.Name + ".");
        if (!value.IsSubclassOf(BaseTypeFactory<TKey, TValue>.k_ValueType))
          throw new ArgumentException("The type passed as value (" + value.Name + ") does not derive from " + BaseTypeFactory<TKey, TValue>.k_ValueType.Name + ".");
        this.m_Mappings[t] = value;
      }
    }

    public virtual TValue Create(TKey key)
    {
      System.Type valueType = (System.Type) null;
      System.Type key1 = key.GetType();
      while (valueType == null && key1 != null && key1 != typeof (TKey))
      {
        if (!this.m_Mappings.TryGetValue(key1, out valueType))
          key1 = key1.BaseType;
      }
      if (valueType == null)
        valueType = this.m_FallbackType;
      return this.InternalCreate(valueType);
    }

    protected virtual TValue InternalCreate(System.Type valueType)
    {
      return (TValue) Activator.CreateInstance(valueType);
    }
  }
}
