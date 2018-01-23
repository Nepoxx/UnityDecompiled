// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoScriptInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Compilation;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (MonoScript))]
  internal class MonoScriptInspector : TextAssetInspector
  {
    public override void OnInspectorGUI()
    {
      if (this.targets.Length == 1)
      {
        string assetPath = AssetDatabase.GetAssetPath(this.target);
        string nameFromScriptPath = CompilationPipeline.GetAssemblyNameFromScriptPath(assetPath);
        if (nameFromScriptPath != null)
        {
          GUILayout.Label("Assembly Information", EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.LabelField("Filename", nameFromScriptPath, new GUILayoutOption[0]);
          string pathFromScriptPath = CompilationPipeline.GetAssemblyDefinitionFilePathFromScriptPath(assetPath);
          if (pathFromScriptPath != null)
          {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(pathFromScriptPath);
            using (new EditorGUI.DisabledScope(true))
              EditorGUILayout.ObjectField("Definition File", (Object) textAsset, typeof (TextAsset), false, new GUILayoutOption[0]);
          }
          EditorGUILayout.Space();
        }
      }
      base.OnInspectorGUI();
    }
  }
}
