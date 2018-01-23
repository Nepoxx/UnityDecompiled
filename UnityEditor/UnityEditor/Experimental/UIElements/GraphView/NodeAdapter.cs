// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.NodeAdapter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class NodeAdapter
  {
    private static List<MethodInfo> s_TypeAdapters;
    private static Dictionary<int, MethodInfo> s_NodeAdapterDictionary;

    public bool CanAdapt(object a, object b)
    {
      if (a == b || (a == null || b == null))
        return false;
      MethodInfo adapter = this.GetAdapter(a, b);
      if (adapter == null)
        Debug.Log((object) ("adapter node not found for: " + (object) a.GetType() + " -> " + (object) b.GetType()));
      return adapter != null;
    }

    public bool Connect(object a, object b)
    {
      MethodInfo adapter = this.GetAdapter(a, b);
      if (adapter == null)
      {
        Debug.LogError((object) ("Attempt to connect 2 unadaptable types: " + (object) a.GetType() + " -> " + (object) b.GetType()));
        return false;
      }
      return (bool) adapter.Invoke((object) this, new object[3]{ (object) this, a, b });
    }

    private IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly, System.Type extendedType)
    {
      return ((IEnumerable<System.Type>) assembly.GetTypes()).Where<System.Type>((Func<System.Type, bool>) (t => t.IsSealed && !t.IsGenericType && !t.IsNested)).SelectMany<System.Type, MethodInfo>((Func<System.Type, IEnumerable<MethodInfo>>) (t => (IEnumerable<MethodInfo>) t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.IsDefined(typeof (ExtensionAttribute), false) && m.GetParameters()[0].ParameterType == extendedType));
    }

    public MethodInfo GetAdapter(object a, object b)
    {
      if (a == null || b == null)
        return (MethodInfo) null;
      if (NodeAdapter.s_NodeAdapterDictionary == null)
      {
        NodeAdapter.s_NodeAdapterDictionary = new Dictionary<int, MethodInfo>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          foreach (MethodInfo extensionMethod in this.GetExtensionMethods(assembly, typeof (NodeAdapter)))
          {
            System.Reflection.ParameterInfo[] parameters = extensionMethod.GetParameters();
            if (parameters.Length == 3)
            {
              string str = parameters[1].ParameterType.ToString() + parameters[2].ParameterType.ToString();
              NodeAdapter.s_NodeAdapterDictionary.Add(str.GetHashCode(), extensionMethod);
            }
          }
        }
      }
      string str1 = a.GetType().ToString() + (object) b.GetType();
      MethodInfo methodInfo;
      return !NodeAdapter.s_NodeAdapterDictionary.TryGetValue(str1.GetHashCode(), out methodInfo) ? (MethodInfo) null : methodInfo;
    }

    public MethodInfo GetTypeAdapter(System.Type from, System.Type to)
    {
      if (NodeAdapter.s_TypeAdapters == null)
      {
        NodeAdapter.s_TypeAdapters = new List<MethodInfo>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          try
          {
            foreach (System.Type type in assembly.GetTypes())
            {
              foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
              {
                if (((IEnumerable<object>) method.GetCustomAttributes(typeof (TypeAdapter), false)).Any<object>())
                  NodeAdapter.s_TypeAdapters.Add(method);
              }
            }
          }
          catch (Exception ex)
          {
            Debug.Log((object) ex);
          }
        }
      }
      foreach (MethodInfo typeAdapter in NodeAdapter.s_TypeAdapters)
      {
        if (typeAdapter.ReturnType == to)
        {
          System.Reflection.ParameterInfo[] parameters = typeAdapter.GetParameters();
          if (parameters.Length == 1 && parameters[0].ParameterType == from)
            return typeAdapter;
        }
      }
      return (MethodInfo) null;
    }
  }
}
