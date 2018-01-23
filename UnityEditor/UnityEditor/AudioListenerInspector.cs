// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioListenerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (AudioListener))]
  internal class AudioListenerInspector : Editor
  {
    private AudioListenerExtensionEditor m_SpatializerEditor = (AudioListenerExtensionEditor) null;
    private bool m_AddSpatializerExtension = false;
    private bool m_AddSpatializerExtensionMixedValues = false;
    private GUIContent addSpatializerExtensionLabel = new GUIContent("Override Spatializer Settings", "Override the Google spatializer's default settings.");

    private void OnEnable()
    {
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.UpdateSpatializerExtensionMixedValues();
      if (!this.m_AddSpatializerExtension)
        return;
      this.CreateExtensionEditors();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      if (AudioExtensionManager.IsListenerSpatializerExtensionRegistered() && (this.m_AddSpatializerExtension && !this.m_AddSpatializerExtensionMixedValues || !this.serializedObject.isEditingMultipleObjects))
      {
        EditorGUI.showMixedValue = this.m_AddSpatializerExtensionMixedValues;
        bool flag1 = EditorGUILayout.Toggle(this.addSpatializerExtensionLabel, this.m_AddSpatializerExtension, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        bool flag2 = false;
        if (this.m_AddSpatializerExtension != flag1)
        {
          this.m_AddSpatializerExtension = flag1;
          if (this.m_AddSpatializerExtension)
          {
            this.CreateExtensionEditors();
            if ((UnityEngine.Object) this.m_SpatializerEditor != (UnityEngine.Object) null)
              flag2 = this.m_SpatializerEditor.FindAudioExtensionProperties(this.serializedObject);
          }
          else
          {
            this.ClearExtensionProperties();
            this.DestroyExtensionEditors();
            flag2 = false;
          }
        }
        else if ((UnityEngine.Object) this.m_SpatializerEditor != (UnityEngine.Object) null)
        {
          flag2 = this.m_SpatializerEditor.FindAudioExtensionProperties(this.serializedObject);
          if (!flag2)
          {
            this.m_AddSpatializerExtension = false;
            this.ClearExtensionProperties();
            this.DestroyExtensionEditors();
          }
        }
        if ((UnityEngine.Object) this.m_SpatializerEditor != (UnityEngine.Object) null && flag2)
        {
          ++EditorGUI.indentLevel;
          this.m_SpatializerEditor.OnAudioListenerGUI();
          --EditorGUI.indentLevel;
          for (int index1 = 0; index1 < this.targets.Length; ++index1)
          {
            AudioListener target = this.targets[index1] as AudioListener;
            if ((UnityEngine.Object) target != (UnityEngine.Object) null)
            {
              AudioListenerExtension spatializerExtension = AudioExtensionManager.GetSpatializerExtension(target);
              if ((UnityEngine.Object) spatializerExtension != (UnityEngine.Object) null)
              {
                string name = AudioExtensionManager.GetListenerSpatializerExtensionType().Name;
                for (int index2 = 0; index2 < this.m_SpatializerEditor.GetNumExtensionProperties(); ++index2)
                {
                  PropertyName extensionPropertyName = this.m_SpatializerEditor.GetExtensionPropertyName(index2);
                  float propertyValue = 0.0f;
                  if (target.ReadExtensionProperty((PropertyName) name, extensionPropertyName, ref propertyValue))
                    spatializerExtension.WriteExtensionProperty(extensionPropertyName, propertyValue);
                }
              }
            }
          }
        }
      }
      this.serializedObject.ApplyModifiedProperties();
    }

    private void OnDisable()
    {
      this.DestroyExtensionEditors();
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    private void UpdateSpatializerExtensionMixedValues()
    {
      this.m_AddSpatializerExtension = false;
      int num = 0;
      for (int index = 0; index < this.targets.Length; ++index)
      {
        AudioListener target = this.targets[index] as AudioListener;
        if ((UnityEngine.Object) target != (UnityEngine.Object) null)
        {
          System.Type spatializerExtensionType = AudioExtensionManager.GetListenerSpatializerExtensionType();
          if (spatializerExtensionType != null && target.GetNumExtensionPropertiesForThisExtension((PropertyName) spatializerExtensionType.Name) > 0)
          {
            this.m_AddSpatializerExtension = true;
            ++num;
          }
        }
      }
      this.m_AddSpatializerExtensionMixedValues = num != 0 && num != this.targets.Length;
      if (!this.m_AddSpatializerExtensionMixedValues)
        return;
      this.m_AddSpatializerExtension = false;
    }

    private void CreateExtensionEditors()
    {
      if ((UnityEngine.Object) this.m_SpatializerEditor != (UnityEngine.Object) null)
        this.DestroyExtensionEditors();
      this.m_SpatializerEditor = ScriptableObject.CreateInstance(AudioExtensionManager.GetListenerSpatializerExtensionEditorType()) as AudioListenerExtensionEditor;
      if ((UnityEngine.Object) this.m_SpatializerEditor != (UnityEngine.Object) null)
      {
        for (int index1 = 0; index1 < this.targets.Length; ++index1)
        {
          AudioListener target = this.targets[index1] as AudioListener;
          if ((UnityEngine.Object) target != (UnityEngine.Object) null)
          {
            Undo.RecordObject((UnityEngine.Object) target, "Add AudioListener extension properties");
            PropertyName spatializerExtensionName = AudioExtensionManager.GetListenerSpatializerExtensionName();
            for (int index2 = 0; index2 < this.m_SpatializerEditor.GetNumExtensionProperties(); ++index2)
            {
              PropertyName extensionPropertyName = this.m_SpatializerEditor.GetExtensionPropertyName(index2);
              float propertyValue = 0.0f;
              if (!target.ReadExtensionProperty(spatializerExtensionName, extensionPropertyName, ref propertyValue))
              {
                propertyValue = this.m_SpatializerEditor.GetExtensionPropertyDefaultValue(index2);
                target.WriteExtensionProperty(AudioExtensionManager.GetSpatializerName(), spatializerExtensionName, extensionPropertyName, propertyValue);
              }
            }
          }
        }
      }
      this.m_AddSpatializerExtensionMixedValues = false;
    }

    private void DestroyExtensionEditors()
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_SpatializerEditor);
      this.m_SpatializerEditor = (AudioListenerExtensionEditor) null;
    }

    private void ClearExtensionProperties()
    {
      for (int index = 0; index < this.targets.Length; ++index)
      {
        AudioListener target = this.targets[index] as AudioListener;
        if ((UnityEngine.Object) target != (UnityEngine.Object) null)
        {
          Undo.RecordObject((UnityEngine.Object) target, "Remove AudioListener extension properties");
          target.ClearExtensionProperties(AudioExtensionManager.GetListenerSpatializerExtensionName());
        }
      }
      this.m_AddSpatializerExtensionMixedValues = false;
    }

    private void UndoRedoPerformed()
    {
      this.DestroyExtensionEditors();
      this.UpdateSpatializerExtensionMixedValues();
      if (!this.m_AddSpatializerExtension && !this.m_AddSpatializerExtensionMixedValues)
        this.ClearExtensionProperties();
      if (this.m_AddSpatializerExtension)
        this.CreateExtensionEditors();
      this.Repaint();
    }
  }
}
