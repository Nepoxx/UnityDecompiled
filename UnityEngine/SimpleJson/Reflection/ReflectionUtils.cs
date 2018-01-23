// Decompiled with JetBrains decompiler
// Type: SimpleJson.Reflection.ReflectionUtils
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SimpleJson.Reflection
{
  [GeneratedCode("reflection-utils", "1.0.0")]
  internal class ReflectionUtils
  {
    private static readonly object[] EmptyObjects = new object[0];

    public static Attribute GetAttribute(MemberInfo info, Type type)
    {
      if (info == null || type == null || !Attribute.IsDefined(info, type))
        return (Attribute) null;
      return Attribute.GetCustomAttribute(info, type);
    }

    public static Attribute GetAttribute(Type objectType, Type attributeType)
    {
      if (objectType == null || attributeType == null || !Attribute.IsDefined((MemberInfo) objectType, attributeType))
        return (Attribute) null;
      return Attribute.GetCustomAttribute((MemberInfo) objectType, attributeType);
    }

    public static Type[] GetGenericTypeArguments(Type type)
    {
      return type.GetGenericArguments();
    }

    public static bool IsTypeGenericeCollectionInterface(Type type)
    {
      if (!type.IsGenericType)
        return false;
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      return genericTypeDefinition == typeof (IList<>) || genericTypeDefinition == typeof (ICollection<>) || genericTypeDefinition == typeof (IEnumerable<>);
    }

    public static bool IsAssignableFrom(Type type1, Type type2)
    {
      return type1.IsAssignableFrom(type2);
    }

    public static bool IsTypeDictionary(Type type)
    {
      if (typeof (IDictionary).IsAssignableFrom(type))
        return true;
      if (!type.IsGenericType)
        return false;
      return type.GetGenericTypeDefinition() == typeof (IDictionary<,>);
    }

    public static bool IsNullableType(Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    public static object ToNullableType(object obj, Type nullableType)
    {
      return obj != null ? Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), (IFormatProvider) CultureInfo.InvariantCulture) : (object) null;
    }

    public static bool IsValueType(Type type)
    {
      return type.IsValueType;
    }

    public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
    {
      return (IEnumerable<ConstructorInfo>) type.GetConstructors();
    }

    public static ConstructorInfo GetConstructorInfo(Type type, params Type[] argsType)
    {
      foreach (ConstructorInfo constructor in ReflectionUtils.GetConstructors(type))
      {
        ParameterInfo[] parameters = constructor.GetParameters();
        if (argsType.Length == parameters.Length)
        {
          int index = 0;
          bool flag = true;
          foreach (ParameterInfo parameter in constructor.GetParameters())
          {
            if (parameter.ParameterType != argsType[index])
            {
              flag = false;
              break;
            }
          }
          if (flag)
            return constructor;
        }
      }
      return (ConstructorInfo) null;
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type type)
    {
      return (IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    public static IEnumerable<FieldInfo> GetFields(Type type)
    {
      return (IEnumerable<FieldInfo>) type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    public static MethodInfo GetGetterMethodInfo(PropertyInfo propertyInfo)
    {
      return propertyInfo.GetGetMethod(true);
    }

    public static MethodInfo GetSetterMethodInfo(PropertyInfo propertyInfo)
    {
      return propertyInfo.GetSetMethod(true);
    }

    public static ReflectionUtils.ConstructorDelegate GetContructor(ConstructorInfo constructorInfo)
    {
      return ReflectionUtils.GetConstructorByReflection(constructorInfo);
    }

    public static ReflectionUtils.ConstructorDelegate GetContructor(Type type, params Type[] argsType)
    {
      return ReflectionUtils.GetConstructorByReflection(type, argsType);
    }

    public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(ConstructorInfo constructorInfo)
    {
      return (ReflectionUtils.ConstructorDelegate) (args => constructorInfo.Invoke(args));
    }

    public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(Type type, params Type[] argsType)
    {
      ConstructorInfo constructorInfo = ReflectionUtils.GetConstructorInfo(type, argsType);
      return constructorInfo != null ? ReflectionUtils.GetConstructorByReflection(constructorInfo) : (ReflectionUtils.ConstructorDelegate) null;
    }

    public static ReflectionUtils.GetDelegate GetGetMethod(PropertyInfo propertyInfo)
    {
      return ReflectionUtils.GetGetMethodByReflection(propertyInfo);
    }

    public static ReflectionUtils.GetDelegate GetGetMethod(FieldInfo fieldInfo)
    {
      return ReflectionUtils.GetGetMethodByReflection(fieldInfo);
    }

    public static ReflectionUtils.GetDelegate GetGetMethodByReflection(PropertyInfo propertyInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new ReflectionUtils.GetDelegate(new ReflectionUtils.\u003CGetGetMethodByReflection\u003Ec__AnonStorey1() { methodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo) }.\u003C\u003Em__0);
    }

    public static ReflectionUtils.GetDelegate GetGetMethodByReflection(FieldInfo fieldInfo)
    {
      return (ReflectionUtils.GetDelegate) (source => fieldInfo.GetValue(source));
    }

    public static ReflectionUtils.SetDelegate GetSetMethod(PropertyInfo propertyInfo)
    {
      return ReflectionUtils.GetSetMethodByReflection(propertyInfo);
    }

    public static ReflectionUtils.SetDelegate GetSetMethod(FieldInfo fieldInfo)
    {
      return ReflectionUtils.GetSetMethodByReflection(fieldInfo);
    }

    public static ReflectionUtils.SetDelegate GetSetMethodByReflection(PropertyInfo propertyInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new ReflectionUtils.SetDelegate(new ReflectionUtils.\u003CGetSetMethodByReflection\u003Ec__AnonStorey3() { methodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo) }.\u003C\u003Em__0);
    }

    public static ReflectionUtils.SetDelegate GetSetMethodByReflection(FieldInfo fieldInfo)
    {
      return (ReflectionUtils.SetDelegate) ((source, value) => fieldInfo.SetValue(source, value));
    }

    public delegate object GetDelegate(object source);

    public delegate void SetDelegate(object source, object value);

    public delegate object ConstructorDelegate(params object[] args);

    public delegate TValue ThreadSafeDictionaryValueFactory<TKey, TValue>(TKey key);

    public sealed class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
      private readonly object _lock = new object();
      private readonly ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> _valueFactory;
      private Dictionary<TKey, TValue> _dictionary;

      public ThreadSafeDictionary(ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> valueFactory)
      {
        this._valueFactory = valueFactory;
      }

      private TValue Get(TKey key)
      {
        TValue obj;
        if (this._dictionary == null || !this._dictionary.TryGetValue(key, out obj))
          return this.AddValue(key);
        return obj;
      }

      private TValue AddValue(TKey key)
      {
        TValue obj1 = this._valueFactory(key);
        lock (this._lock)
        {
          if (this._dictionary == null)
          {
            this._dictionary = new Dictionary<TKey, TValue>();
            this._dictionary[key] = obj1;
          }
          else
          {
            TValue obj2;
            if (this._dictionary.TryGetValue(key, out obj2))
              return obj2;
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>) this._dictionary);
            dictionary[key] = obj1;
            this._dictionary = dictionary;
          }
        }
        return obj1;
      }

      public void Add(TKey key, TValue value)
      {
        throw new NotImplementedException();
      }

      public bool ContainsKey(TKey key)
      {
        return this._dictionary.ContainsKey(key);
      }

      public ICollection<TKey> Keys
      {
        get
        {
          return (ICollection<TKey>) this._dictionary.Keys;
        }
      }

      public bool Remove(TKey key)
      {
        throw new NotImplementedException();
      }

      public bool TryGetValue(TKey key, out TValue value)
      {
        value = this[key];
        return true;
      }

      public ICollection<TValue> Values
      {
        get
        {
          return (ICollection<TValue>) this._dictionary.Values;
        }
      }

      public TValue this[TKey key]
      {
        get
        {
          return this.Get(key);
        }
        set
        {
          throw new NotImplementedException();
        }
      }

      public void Add(KeyValuePair<TKey, TValue> item)
      {
        throw new NotImplementedException();
      }

      public void Clear()
      {
        throw new NotImplementedException();
      }

      public bool Contains(KeyValuePair<TKey, TValue> item)
      {
        throw new NotImplementedException();
      }

      public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
      {
        throw new NotImplementedException();
      }

      public int Count
      {
        get
        {
          return this._dictionary.Count;
        }
      }

      public bool IsReadOnly
      {
        get
        {
          throw new NotImplementedException();
        }
      }

      public bool Remove(KeyValuePair<TKey, TValue> item)
      {
        throw new NotImplementedException();
      }

      public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
      {
        return (IEnumerator<KeyValuePair<TKey, TValue>>) this._dictionary.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) this._dictionary.GetEnumerator();
      }
    }
  }
}
