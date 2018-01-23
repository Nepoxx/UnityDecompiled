// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.WSAHelpers
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Scripting.ScriptCompilation
{
  internal static class WSAHelpers
  {
    public static bool IsCSharpAssembly(ScriptAssembly scriptAssembly)
    {
      if (scriptAssembly.Filename.ToLower().Contains("firstpass"))
        return false;
      return scriptAssembly.Language == ScriptCompilers.CSharpSupportedLanguage;
    }

    public static bool IsCSharpFirstPassAssembly(ScriptAssembly scriptAssembly)
    {
      if (!scriptAssembly.Filename.ToLower().Contains("firstpass"))
        return false;
      return scriptAssembly.Language == ScriptCompilers.CSharpSupportedLanguage;
    }

    public static bool UseDotNetCore(ScriptAssembly scriptAssembly)
    {
      PlayerSettings.WSACompilationOverrides compilationOverrides = PlayerSettings.WSA.compilationOverrides;
      return scriptAssembly.BuildTarget == BuildTarget.WSAPlayer && compilationOverrides != PlayerSettings.WSACompilationOverrides.None && (WSAHelpers.IsCSharpAssembly(scriptAssembly) || compilationOverrides != PlayerSettings.WSACompilationOverrides.UseNetCorePartially && WSAHelpers.IsCSharpFirstPassAssembly(scriptAssembly));
    }
  }
}
