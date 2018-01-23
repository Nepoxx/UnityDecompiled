// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.Player.PlayerBuildInterface
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;

namespace UnityEditor.Experimental.Build.Player
{
  public class PlayerBuildInterface
  {
    private static ScriptCompilationResult CompilePlayerScriptsNative(ScriptCompilationSettings input, string outputFolder, bool editorScripts)
    {
      ScriptCompilationResult ret;
      PlayerBuildInterface.CompilePlayerScriptsNative_Injected(ref input, outputFolder, editorScripts, out ret);
      return ret;
    }

    public static ScriptCompilationResult CompilePlayerScripts(ScriptCompilationSettings input, string outputFolder)
    {
      return PlayerBuildInterface.CompilePlayerScriptsInternal(input, outputFolder, false);
    }

    internal static ScriptCompilationResult CompilePlayerScriptsInternal(ScriptCompilationSettings input, string outputFolder, bool editorScripts)
    {
      input.m_ResultTypeDB = new TypeDB();
      ScriptCompilationResult compilationResult = PlayerBuildInterface.CompilePlayerScriptsNative(input, outputFolder, editorScripts);
      compilationResult.m_TypeDB = compilationResult.m_Assemblies.Length == 0 ? (TypeDB) null : input.m_ResultTypeDB;
      return compilationResult;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void CompilePlayerScriptsNative_Injected(ref ScriptCompilationSettings input, string outputFolder, bool editorScripts, out ScriptCompilationResult ret);
  }
}
