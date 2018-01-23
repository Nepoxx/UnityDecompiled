// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRCustomOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.VR
{
  internal abstract class VRCustomOptions
  {
    private SerializedProperty editorSettings;
    private SerializedProperty playerSettings;

    internal SerializedProperty FindPropertyAssert(string name)
    {
      SerializedProperty serializedProperty = (SerializedProperty) null;
      if (this.editorSettings == null && this.playerSettings == null)
      {
        Debug.LogError((object) ("No existing VR settings. Failed to find:" + name));
      }
      else
      {
        bool flag = false;
        if (this.editorSettings != null)
        {
          serializedProperty = this.editorSettings.FindPropertyRelative(name);
          if (serializedProperty != null)
            flag = true;
        }
        if (!flag && this.playerSettings != null)
        {
          serializedProperty = this.playerSettings.FindPropertyRelative(name);
          if (serializedProperty != null)
            flag = true;
        }
        if (!flag)
          Debug.LogError((object) ("Failed to find property:" + name));
      }
      return serializedProperty;
    }

    public bool IsExpanded { get; set; }

    public virtual void Initialize(SerializedObject settings)
    {
      this.Initialize(settings, "");
    }

    public virtual void Initialize(SerializedObject settings, string propertyName)
    {
      this.editorSettings = settings.FindProperty("vrEditorSettings");
      if (this.editorSettings != null && !string.IsNullOrEmpty(propertyName))
        this.editorSettings = this.editorSettings.FindPropertyRelative(propertyName);
      this.playerSettings = settings.FindProperty("vrSettings");
      if (this.playerSettings == null || string.IsNullOrEmpty(propertyName))
        return;
      this.playerSettings = this.playerSettings.FindPropertyRelative(propertyName);
    }

    public abstract Rect Draw(Rect rect);

    public abstract float GetHeight();
  }
}
