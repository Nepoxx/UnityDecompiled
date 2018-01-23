// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShaderGUIUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Reflection;

namespace UnityEditor
{
  internal static class ShaderGUIUtility
  {
    private static System.Type ExtractCustomEditorType(string customEditorName)
    {
      if (string.IsNullOrEmpty(customEditorName))
        return (System.Type) null;
      string str = "UnityEditor." + customEditorName;
      Assembly[] loadedAssemblies = EditorAssemblies.loadedAssemblies;
      for (int index = loadedAssemblies.Length - 1; index >= 0; --index)
      {
        foreach (System.Type c in AssemblyHelper.GetTypesFromAssembly(loadedAssemblies[index]))
        {
          if (c.FullName.Equals(customEditorName, StringComparison.Ordinal) || c.FullName.Equals(str, StringComparison.Ordinal))
            return !typeof (ShaderGUI).IsAssignableFrom(c) ? (System.Type) null : c;
        }
      }
      return (System.Type) null;
    }

    internal static ShaderGUI CreateShaderGUI(string customEditorName)
    {
      System.Type customEditorType = ShaderGUIUtility.ExtractCustomEditorType(customEditorName);
      return customEditorType == null ? (ShaderGUI) null : Activator.CreateInstance(customEditorType) as ShaderGUI;
    }
  }
}
