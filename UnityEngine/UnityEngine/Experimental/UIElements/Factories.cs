// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Factories
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Experimental.UIElements
{
  internal static class Factories
  {
    private static Dictionary<string, Func<IUxmlAttributes, CreationContext, VisualElement>> s_Factories;

    internal static void RegisterFactory(string fullTypeName, Func<IUxmlAttributes, CreationContext, VisualElement> factory)
    {
      Factories.DiscoverFactories();
      Factories.s_Factories.Add(fullTypeName, factory);
    }

    internal static void RegisterFactory<T>(Func<IUxmlAttributes, CreationContext, VisualElement> factory) where T : VisualElement
    {
      Factories.RegisterFactory(typeof (T).FullName, factory);
    }

    private static void DiscoverFactories()
    {
      if (Factories.s_Factories != null)
        return;
      Factories.s_Factories = new Dictionary<string, Func<IUxmlAttributes, CreationContext, VisualElement>>();
      CoreFactories.RegisterAll();
      AppDomain currentDomain = AppDomain.CurrentDomain;
      HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) ScriptingRuntime.GetAllUserAssemblies());
      foreach (Assembly assembly in currentDomain.GetAssemblies())
      {
        if (stringSet.Contains(assembly.GetName().Name + ".dll"))
        {
          try
          {
            foreach (System.Type type in assembly.GetTypes())
            {
              if (typeof (IUxmlFactory).IsAssignableFrom(type))
              {
                IUxmlFactory instance = (IUxmlFactory) Activator.CreateInstance(type);
                Factories.RegisterFactory(instance.CreatesType.FullName, new Func<IUxmlAttributes, CreationContext, VisualElement>(instance.Create));
              }
            }
          }
          catch (TypeLoadException ex)
          {
            Debug.LogWarningFormat("Error while loading types from assembly {0}: {1}", new object[2]
            {
              (object) assembly.FullName,
              (object) ex
            });
          }
        }
      }
    }

    internal static bool TryGetValue(string fullTypeName, out Func<IUxmlAttributes, CreationContext, VisualElement> factory)
    {
      Factories.DiscoverFactories();
      factory = (Func<IUxmlAttributes, CreationContext, VisualElement>) null;
      return Factories.s_Factories != null && Factories.s_Factories.TryGetValue(fullTypeName, out factory);
    }
  }
}
