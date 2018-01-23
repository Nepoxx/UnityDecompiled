// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.AssetImporters.ScriptedImporterEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Experimental.AssetImporters
{
  /// <summary>
  ///   <para>Default editor for source assets handled by Scripted Importers.</para>
  /// </summary>
  [CustomEditor(typeof (ScriptedImporter), true)]
  public class ScriptedImporterEditor : AssetImporterEditor
  {
    internal override string targetTitle
    {
      get
      {
        return base.targetTitle + " (" + ObjectNames.NicifyVariableName(this.GetType().Name) + ")";
      }
    }

    /// <summary>
    ///   <para>Implement this method to customize how Unity's Asset inspector is drawn for an Asset managed by a ScriptedImporter.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      SerializedProperty iterator = this.serializedObject.GetIterator();
      for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
        EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
      this.ApplyRevertGUI();
    }

    protected override bool OnApplyRevertGUI()
    {
      bool flag = base.OnApplyRevertGUI();
      if (flag)
        ActiveEditorTracker.sharedTracker.ForceRebuild();
      return flag;
    }
  }
}
