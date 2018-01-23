// Decompiled with JetBrains decompiler
// Type: UnityEditor.RuntimeClassRegistry
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class RuntimeClassRegistry
  {
    protected HashSet<string> monoBaseClasses = new HashSet<string>();
    protected Dictionary<string, string[]> m_UsedTypesPerUserAssembly = new Dictionary<string, string[]>();
    protected Dictionary<int, List<string>> classScenes = new Dictionary<int, List<string>>();
    protected UnityType objectUnityType = (UnityType) null;
    protected Dictionary<int, string> allNativeClasses = new Dictionary<int, string>();
    internal List<RuntimeClassRegistry.MethodDescription> m_MethodsToPreserve = new List<RuntimeClassRegistry.MethodDescription>();
    internal List<string> m_UserAssemblies = new List<string>();
    protected BuildTarget buildTarget;

    public Dictionary<string, string[]> UsedTypePerUserAssembly
    {
      get
      {
        return this.m_UsedTypesPerUserAssembly;
      }
    }

    public List<string> GetScenesForClass(int ID)
    {
      if (!this.classScenes.ContainsKey(ID))
        return (List<string>) null;
      return this.classScenes[ID];
    }

    public void AddNativeClassID(int ID)
    {
      string name = UnityType.FindTypeByPersistentTypeID(ID).name;
      if (name.Length <= 0)
        return;
      this.allNativeClasses[ID] = name;
    }

    public void SetUsedTypesInUserAssembly(string[] typeNames, string assemblyName)
    {
      this.m_UsedTypesPerUserAssembly[assemblyName] = typeNames;
    }

    public bool IsDLLUsed(string dll)
    {
      if (this.m_UsedTypesPerUserAssembly == null || Array.IndexOf<string>(CodeStrippingUtils.UserAssemblies, dll) != -1)
        return true;
      return this.m_UsedTypesPerUserAssembly.ContainsKey(dll);
    }

    protected void AddManagedBaseClass(string className)
    {
      this.monoBaseClasses.Add(className);
    }

    protected void AddNativeClassFromName(string className)
    {
      if (this.objectUnityType == null)
        this.objectUnityType = UnityType.FindTypeByName("Object");
      UnityType typeByName = UnityType.FindTypeByName(className);
      if (typeByName == null || typeByName.persistentTypeID == this.objectUnityType.persistentTypeID)
        return;
      this.allNativeClasses[typeByName.persistentTypeID] = className;
    }

    public List<string> GetAllNativeClassesIncludingManagersAsString()
    {
      return new List<string>((IEnumerable<string>) this.allNativeClasses.Values);
    }

    public List<string> GetAllManagedBaseClassesAsString()
    {
      return new List<string>((IEnumerable<string>) this.monoBaseClasses);
    }

    public static RuntimeClassRegistry Create()
    {
      return new RuntimeClassRegistry();
    }

    public void Initialize(int[] nativeClassIDs, BuildTarget buildTarget)
    {
      this.buildTarget = buildTarget;
      this.InitRuntimeClassRegistry();
      foreach (int nativeClassId in nativeClassIDs)
        this.AddNativeClassID(nativeClassId);
    }

    public void SetSceneClasses(int[] nativeClassIDs, string scene)
    {
      foreach (int nativeClassId in nativeClassIDs)
      {
        this.AddNativeClassID(nativeClassId);
        if (!this.classScenes.ContainsKey(nativeClassId))
          this.classScenes[nativeClassId] = new List<string>();
        this.classScenes[nativeClassId].Add(scene);
      }
    }

    internal void AddMethodToPreserve(string assembly, string @namespace, string klassName, string methodName)
    {
      this.m_MethodsToPreserve.Add(new RuntimeClassRegistry.MethodDescription()
      {
        assembly = assembly,
        fullTypeName = @namespace + (@namespace.Length <= 0 ? "" : ".") + klassName,
        methodName = methodName
      });
    }

    internal List<RuntimeClassRegistry.MethodDescription> GetMethodsToPreserve()
    {
      return this.m_MethodsToPreserve;
    }

    internal void AddUserAssembly(string assembly)
    {
      if (this.m_UserAssemblies.Contains(assembly))
        return;
      this.m_UserAssemblies.Add(assembly);
    }

    internal string[] GetUserAssemblies()
    {
      return this.m_UserAssemblies.ToArray();
    }

    protected void InitRuntimeClassRegistry()
    {
      BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(this.buildTarget);
      this.AddManagedBaseClass("UnityEngine.MonoBehaviour");
      this.AddManagedBaseClass("UnityEngine.ScriptableObject");
      if (buildTargetGroup == BuildTargetGroup.Android)
        this.AddManagedBaseClass("UnityEngine.AndroidJavaProxy");
      foreach (string dontStripClassName in RuntimeInitializeOnLoadManager.dontStripClassNames)
        this.AddManagedBaseClass(dontStripClassName);
    }

    internal class MethodDescription
    {
      public string assembly;
      public string fullTypeName;
      public string methodName;
    }
  }
}
