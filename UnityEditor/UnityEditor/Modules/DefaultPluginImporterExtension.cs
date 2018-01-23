// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultPluginImporterExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityEditor.Modules
{
  internal class DefaultPluginImporterExtension : IPluginImporterExtension
  {
    protected bool hasModified = false;
    protected DefaultPluginImporterExtension.Property[] properties = (DefaultPluginImporterExtension.Property[]) null;
    internal bool propertiesRefreshed = false;

    public DefaultPluginImporterExtension(DefaultPluginImporterExtension.Property[] properties)
    {
      this.properties = properties;
    }

    public virtual void ResetValues(PluginImporterInspector inspector)
    {
      this.hasModified = false;
      this.RefreshProperties(inspector);
    }

    public virtual bool HasModified(PluginImporterInspector inspector)
    {
      return this.hasModified;
    }

    public virtual void Apply(PluginImporterInspector inspector)
    {
      if (!this.propertiesRefreshed)
        return;
      foreach (DefaultPluginImporterExtension.Property property in this.properties)
        property.Apply(inspector);
    }

    public virtual void OnEnable(PluginImporterInspector inspector)
    {
      this.RefreshProperties(inspector);
    }

    public virtual void OnDisable(PluginImporterInspector inspector)
    {
    }

    public virtual void OnPlatformSettingsGUI(PluginImporterInspector inspector)
    {
      if (!this.propertiesRefreshed)
        this.RefreshProperties(inspector);
      EditorGUI.BeginChangeCheck();
      foreach (DefaultPluginImporterExtension.Property property in this.properties)
        property.OnGUI(inspector);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.hasModified = true;
    }

    protected virtual void RefreshProperties(PluginImporterInspector inspector)
    {
      foreach (DefaultPluginImporterExtension.Property property in this.properties)
        property.Reset(inspector);
      this.propertiesRefreshed = true;
    }

    public virtual string CalculateFinalPluginPath(string platformName, PluginImporter imp)
    {
      string platformData = imp.GetPlatformData(platformName, "CPU");
      if (!string.IsNullOrEmpty(platformData) && string.Compare(platformData, "AnyCPU", true) != 0 && string.Compare(platformData, "None", true) != 0)
        return Path.Combine(platformData, Path.GetFileName(imp.assetPath));
      return Path.GetFileName(imp.assetPath);
    }

    protected Dictionary<string, List<PluginImporter>> GetCompatiblePlugins(string buildTargetName)
    {
      IEnumerable<PluginImporter> pluginImporters = ((IEnumerable<PluginImporter>) PluginImporter.GetAllImporters()).Where<PluginImporter>((Func<PluginImporter, bool>) (imp => imp.GetCompatibleWithPlatformOrAnyPlatformBuildTarget(buildTargetName)));
      Dictionary<string, List<PluginImporter>> dictionary = new Dictionary<string, List<PluginImporter>>();
      foreach (PluginImporter imp in pluginImporters)
      {
        if (!string.IsNullOrEmpty(imp.assetPath))
        {
          string finalPluginPath = this.CalculateFinalPluginPath(buildTargetName, imp);
          if (!string.IsNullOrEmpty(finalPluginPath))
          {
            List<PluginImporter> pluginImporterList = (List<PluginImporter>) null;
            if (!dictionary.TryGetValue(finalPluginPath, out pluginImporterList))
            {
              pluginImporterList = new List<PluginImporter>();
              dictionary[finalPluginPath] = pluginImporterList;
            }
            pluginImporterList.Add(imp);
          }
        }
      }
      return dictionary;
    }

    public virtual bool CheckFileCollisions(string buildTargetName)
    {
      Dictionary<string, List<PluginImporter>> compatiblePlugins = this.GetCompatiblePlugins(buildTargetName);
      bool flag = false;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, List<PluginImporter>> keyValuePair in compatiblePlugins)
      {
        List<PluginImporter> pluginImporterList = keyValuePair.Value;
        if (pluginImporterList.Count != 1)
        {
          int num = 0;
          foreach (PluginImporter pluginImporter in pluginImporterList)
          {
            if (!pluginImporter.GetIsOverridable())
              ++num;
          }
          if (num != 1)
          {
            flag = true;
            stringBuilder.AppendLine(string.Format("Plugin '{0}' is used from several locations:", (object) Path.GetFileName(keyValuePair.Key)));
            foreach (PluginImporter pluginImporter in pluginImporterList)
              stringBuilder.AppendLine(" " + pluginImporter.assetPath + " would be copied to <PluginPath>/" + keyValuePair.Key.Replace("\\", "/"));
          }
        }
      }
      if (flag)
      {
        stringBuilder.AppendLine("Please fix plugin settings and try again.");
        Debug.LogError((object) stringBuilder.ToString());
      }
      return flag;
    }

    internal class Property
    {
      internal Property(string name, string key, object defaultValue, string platformName)
        : this(new GUIContent(name), key, defaultValue, platformName)
      {
      }

      internal Property(GUIContent name, string key, object defaultValue, string platformName)
      {
        this.name = name;
        this.key = key;
        this.defaultValue = defaultValue;
        this.type = defaultValue.GetType();
        this.platformName = platformName;
      }

      internal GUIContent name { get; set; }

      internal string key { get; set; }

      internal object defaultValue { get; set; }

      internal System.Type type { get; set; }

      internal string platformName { get; set; }

      internal object value { get; set; }

      internal virtual void Reset(PluginImporterInspector inspector)
      {
        string platformData = inspector.importer.GetPlatformData(this.platformName, this.key);
        this.ParseStringValue(inspector, platformData, false);
      }

      protected void ParseStringValue(PluginImporterInspector inspector, string valueString, bool muteWarnings = false)
      {
        try
        {
          this.value = TypeDescriptor.GetConverter(this.type).ConvertFromString(valueString);
        }
        catch
        {
          this.value = this.defaultValue;
          if (muteWarnings || string.IsNullOrEmpty(valueString) || !inspector.importer.GetCompatibleWithPlatform(this.platformName))
            return;
          Debug.LogWarning((object) ("Failed to parse value ('" + valueString + "') for " + this.key + ", platform: " + this.platformName + ", type: " + (object) this.type + ". Default value will be set '" + this.defaultValue + "'"));
        }
      }

      internal virtual void Apply(PluginImporterInspector inspector)
      {
        inspector.importer.SetPlatformData(this.platformName, this.key, this.value.ToString());
      }

      internal virtual void OnGUI(PluginImporterInspector inspector)
      {
        if (this.type == typeof (bool))
          this.value = (object) EditorGUILayout.Toggle(this.name, (bool) this.value, new GUILayoutOption[0]);
        else if (this.type.IsEnum)
        {
          this.value = (object) EditorGUILayout.EnumPopup(this.name, (Enum) this.value, new GUILayoutOption[0]);
        }
        else
        {
          if (this.type != typeof (string))
            throw new NotImplementedException("Don't know how to display value.");
          this.value = (object) EditorGUILayout.TextField(this.name, (string) this.value, new GUILayoutOption[0]);
        }
      }
    }
  }
}
