// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.APIUpdaterRuntimeServices
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEngineInternal
{
  public sealed class APIUpdaterRuntimeServices
  {
    private static IList<System.Type> ComponentsFromUnityEngine;

    static APIUpdaterRuntimeServices()
    {
      System.Type type = typeof (Component);
      APIUpdaterRuntimeServices.ComponentsFromUnityEngine = (IList<System.Type>) ((IEnumerable<System.Type>) type.Assembly.GetTypes()).Where<System.Type>(new Func<System.Type, bool>(type.IsAssignableFrom)).ToList<System.Type>();
    }

    [Obsolete("AddComponent(string) has been deprecated. Use GameObject.AddComponent<T>() / GameObject.AddComponent(Type) instead.\nAPI Updater could not automatically update the original call to AddComponent(string name), because it was unable to resolve the type specified in parameter 'name'.\nInstead, this call has been replaced with a call to APIUpdaterRuntimeServices.AddComponent() so you can try to test your game in the editor.\nIn order to be able to build the game, replace this call (APIUpdaterRuntimeServices.AddComponent()) with a call to GameObject.AddComponent<T>() / GameObject.AddComponent(Type).")]
    public static Component AddComponent(GameObject go, string sourceInfo, string name)
    {
      Debug.LogWarningFormat("Performing a potentially slow search for component {0}.", (object) name);
      System.Type componentType = APIUpdaterRuntimeServices.ResolveType(name, Assembly.GetCallingAssembly(), sourceInfo);
      return componentType != null ? go.AddComponent(componentType) : (Component) null;
    }

    private static System.Type ResolveType(string name, Assembly callingAssembly, string sourceInfo)
    {
      System.Type type1 = APIUpdaterRuntimeServices.ComponentsFromUnityEngine.FirstOrDefault<System.Type>((Func<System.Type, bool>) (t => (t.Name == name || t.FullName == name) && !APIUpdaterRuntimeServices.IsMarkedAsObsolete(t)));
      if (type1 != null)
      {
        Debug.LogWarningFormat("[{1}] Component type '{0}' found in UnityEngine, consider replacing with go.AddComponent<{0}>()", new object[2]
        {
          (object) name,
          (object) sourceInfo
        });
        return type1;
      }
      System.Type type2 = callingAssembly.GetType(name);
      if (type2 != null)
      {
        Debug.LogWarningFormat("[{1}] Component type '{0}' found on caller assembly, consider replacing with go.AddComponent<{0}>()", new object[2]
        {
          (object) name,
          (object) sourceInfo
        });
        return type2;
      }
      System.Type type3 = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (a => (IEnumerable<System.Type>) a.GetTypes())).SingleOrDefault<System.Type>((Func<System.Type, bool>) (t => (t.Name == name || t.FullName == name) && typeof (Component).IsAssignableFrom(t)));
      if (type3 != null)
      {
        Debug.LogWarningFormat("[{2}] Component type '{0}' found on assembly {1}, consider replacing with go.AddComponent<{0}>()", (object) name, (object) new AssemblyName(type3.Assembly.FullName).Name, (object) sourceInfo);
        return type3;
      }
      Debug.LogErrorFormat("[{1}] Component Type '{0}' not found.", new object[2]
      {
        (object) name,
        (object) sourceInfo
      });
      return (System.Type) null;
    }

    private static bool IsMarkedAsObsolete(System.Type t)
    {
      return ((IEnumerable<object>) t.GetCustomAttributes(typeof (ObsoleteAttribute), false)).Any<object>();
    }
  }
}
