// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.EditorWindowPersistentViewData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Experimental.UIElements
{
  [LibraryFolderPath("UIElements/EditorWindows")]
  internal class EditorWindowPersistentViewData : ScriptableSingletonDictionary<EditorWindowPersistentViewData, SerializableJsonDictionary>
  {
    public static SerializableJsonDictionary GetEditorData(EditorWindow window)
    {
      return ScriptableSingletonDictionary<EditorWindowPersistentViewData, SerializableJsonDictionary>.instance[window.GetType().ToString()];
    }
  }
}
