// Decompiled with JetBrains decompiler
// Type: UnityEditor.DefaultAssetInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  [CustomEditor(typeof (DefaultAsset), isFallback = true)]
  internal class DefaultAssetInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      DefaultAsset target = (DefaultAsset) this.target;
      if (target.message.Length <= 0)
        return;
      EditorGUILayout.HelpBox(target.message, !target.isWarning ? MessageType.Info : MessageType.Warning);
    }
  }
}
