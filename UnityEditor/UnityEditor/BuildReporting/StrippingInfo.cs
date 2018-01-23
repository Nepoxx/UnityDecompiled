// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildReporting.StrippingInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.BuildReporting
{
  internal class StrippingInfo : ScriptableObject, ISerializationCallbackReceiver
  {
    public List<string> modules = new List<string>();
    public List<int> serializedSizes = new List<int>();
    public Dictionary<string, HashSet<string>> dependencies = new Dictionary<string, HashSet<string>>();
    public Dictionary<string, int> sizes = new Dictionary<string, int>();
    public Dictionary<string, string> icons = new Dictionary<string, string>();
    public int totalSize = 0;
    public const string RequiredByScripts = "Required by Scripts";
    public List<StrippingInfo.SerializedDependency> serializedDependencies;

    private void OnEnable()
    {
      this.SetIcon("Required by Scripts", "class/MonoScript");
      this.SetIcon(StrippingInfo.ModuleName("AI"), "class/NavMeshAgent");
      this.SetIcon(StrippingInfo.ModuleName("Animation"), "class/Animation");
      this.SetIcon(StrippingInfo.ModuleName("Audio"), "class/AudioSource");
      this.SetIcon(StrippingInfo.ModuleName("Core"), "class/GameManager");
      this.SetIcon(StrippingInfo.ModuleName("IMGUI"), "class/GUILayer");
      this.SetIcon(StrippingInfo.ModuleName("ParticleSystem"), "class/ParticleSystem");
      this.SetIcon(StrippingInfo.ModuleName("ParticlesLegacy"), "class/EllipsoidParticleEmitter");
      this.SetIcon(StrippingInfo.ModuleName("Physics"), "class/PhysicMaterial");
      this.SetIcon(StrippingInfo.ModuleName("Physics2D"), "class/PhysicsMaterial2D");
      this.SetIcon(StrippingInfo.ModuleName("TextRendering"), "class/Font");
      this.SetIcon(StrippingInfo.ModuleName("UI"), "class/CanvasGroup");
      this.SetIcon(StrippingInfo.ModuleName("Umbra"), "class/OcclusionCullingSettings");
      this.SetIcon(StrippingInfo.ModuleName("UNET"), "class/NetworkTransform");
      this.SetIcon(StrippingInfo.ModuleName("Vehicles"), "class/WheelCollider");
      this.SetIcon(StrippingInfo.ModuleName("Cloth"), "class/Cloth");
      this.SetIcon(StrippingInfo.ModuleName("ImageConversion"), "class/Texture");
      this.SetIcon(StrippingInfo.ModuleName("ScreenCapture"), "class/RenderTexture");
      this.SetIcon(StrippingInfo.ModuleName("Wind"), "class/WindZone");
    }

    public void OnBeforeSerialize()
    {
      this.serializedDependencies = new List<StrippingInfo.SerializedDependency>();
      foreach (KeyValuePair<string, HashSet<string>> dependency in this.dependencies)
      {
        List<string> stringList = new List<string>();
        foreach (string str in dependency.Value)
          stringList.Add(str);
        StrippingInfo.SerializedDependency serializedDependency;
        serializedDependency.key = dependency.Key;
        serializedDependency.value = stringList;
        serializedDependency.icon = !this.icons.ContainsKey(dependency.Key) ? "class/DefaultAsset" : this.icons[dependency.Key];
        serializedDependency.size = !this.sizes.ContainsKey(dependency.Key) ? 0 : this.sizes[dependency.Key];
        this.serializedDependencies.Add(serializedDependency);
      }
      this.serializedSizes = new List<int>();
      foreach (string module in this.modules)
        this.serializedSizes.Add(!this.sizes.ContainsKey(module) ? 0 : this.sizes[module]);
    }

    public void OnAfterDeserialize()
    {
      this.dependencies = new Dictionary<string, HashSet<string>>();
      this.icons = new Dictionary<string, string>();
      for (int index = 0; index < this.serializedDependencies.Count; ++index)
      {
        HashSet<string> stringSet = new HashSet<string>();
        foreach (string str in this.serializedDependencies[index].value)
          stringSet.Add(str);
        this.dependencies.Add(this.serializedDependencies[index].key, stringSet);
        this.icons[this.serializedDependencies[index].key] = this.serializedDependencies[index].icon;
        this.sizes[this.serializedDependencies[index].key] = this.serializedDependencies[index].size;
      }
      this.sizes = new Dictionary<string, int>();
      for (int index = 0; index < this.serializedSizes.Count; ++index)
        this.sizes[this.modules[index]] = this.serializedSizes[index];
    }

    public void RegisterDependency(string obj, string depends)
    {
      if (!this.dependencies.ContainsKey(obj))
        this.dependencies[obj] = new HashSet<string>();
      this.dependencies[obj].Add(depends);
      if (this.icons.ContainsKey(depends))
        return;
      this.SetIcon(depends, "class/" + depends);
    }

    public void AddModule(string module)
    {
      if (!this.modules.Contains(module))
        this.modules.Add(module);
      if (!this.sizes.ContainsKey(module))
        this.sizes[module] = 0;
      if (this.icons.ContainsKey(module))
        return;
      this.SetIcon(module, "class/DefaultAsset");
    }

    public void SetIcon(string dependency, string icon)
    {
      this.icons[dependency] = icon;
      if (this.dependencies.ContainsKey(dependency))
        return;
      this.dependencies[dependency] = new HashSet<string>();
    }

    public void AddModuleSize(string module, int size)
    {
      if (!this.modules.Contains(module))
        return;
      this.sizes[module] = size;
    }

    public static StrippingInfo GetBuildReportData(BuildReport report)
    {
      if ((UnityEngine.Object) report == (UnityEngine.Object) null)
        return (StrippingInfo) null;
      StrippingInfo[] appendices = (StrippingInfo[]) report.GetAppendices(typeof (StrippingInfo));
      if (appendices.Length > 0)
        return appendices[0];
      StrippingInfo instance = ScriptableObject.CreateInstance<StrippingInfo>();
      report.AddAppendix((UnityEngine.Object) instance);
      return instance;
    }

    public static string ModuleName(string module)
    {
      return module + " Module";
    }

    [Serializable]
    public struct SerializedDependency
    {
      [SerializeField]
      public string key;
      [SerializeField]
      public List<string> value;
      [SerializeField]
      public string icon;
      [SerializeField]
      public int size;
    }
  }
}
