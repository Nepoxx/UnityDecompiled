// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.MonoIsland
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Scripting
{
  internal struct MonoIsland
  {
    public readonly BuildTarget _target;
    public readonly bool _development_player;
    public readonly bool _editor;
    public readonly ApiCompatibilityLevel _api_compatibility_level;
    public readonly string[] _files;
    public readonly string[] _references;
    public readonly string[] _defines;
    public readonly string _output;

    public MonoIsland(BuildTarget target, ApiCompatibilityLevel api_compatibility_level, string[] files, string[] references, string[] defines, string output)
    {
      this._target = target;
      this._development_player = false;
      this._editor = false;
      this._api_compatibility_level = api_compatibility_level;
      this._files = files;
      this._references = references;
      this._defines = defines;
      this._output = output;
    }

    public MonoIsland(BuildTarget target, bool editor, bool development_player, ApiCompatibilityLevel api_compatibility_level, string[] files, string[] references, string[] defines, string output)
    {
      this._target = target;
      this._development_player = development_player;
      this._editor = editor;
      this._api_compatibility_level = api_compatibility_level;
      this._files = files;
      this._references = references;
      this._defines = defines;
      this._output = output;
    }

    public string GetExtensionOfSourceFiles()
    {
      return this._files.Length <= 0 ? "NA" : ScriptCompilers.GetExtensionOfSourceFile(this._files[0]);
    }
  }
}
