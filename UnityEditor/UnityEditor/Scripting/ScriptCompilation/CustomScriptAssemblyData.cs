// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.CustomScriptAssemblyData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Scripting.ScriptCompilation
{
  [Serializable]
  internal class CustomScriptAssemblyData
  {
    public string name;
    public string[] references;
    public string[] includePlatforms;
    public string[] excludePlatforms;

    public static CustomScriptAssemblyData FromJson(string json)
    {
      CustomScriptAssemblyData scriptAssemblyData = JsonUtility.FromJson<CustomScriptAssemblyData>(json);
      if (scriptAssemblyData == null)
        throw new Exception("Json file does not contain an assembly definition");
      if (string.IsNullOrEmpty(scriptAssemblyData.name))
        throw new Exception("Required property 'name' not set");
      if (scriptAssemblyData.excludePlatforms != null && scriptAssemblyData.excludePlatforms.Length > 0 && (scriptAssemblyData.includePlatforms != null && scriptAssemblyData.includePlatforms.Length > 0))
        throw new Exception("Both 'excludePlatforms' and 'includePlatforms' are set.");
      return scriptAssemblyData;
    }

    public static string ToJson(CustomScriptAssemblyData data)
    {
      return JsonUtility.ToJson((object) data, true);
    }
  }
}
