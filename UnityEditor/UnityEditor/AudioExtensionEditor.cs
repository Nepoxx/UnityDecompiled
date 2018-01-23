// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioExtensionEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioExtensionEditor : ScriptableObject
  {
    private bool foundAllExtensionProperties = false;
    protected AudioExtensionEditor.ExtensionPropertyInfo[] m_ExtensionProperties;

    public virtual void InitExtensionPropertyInfo()
    {
    }

    protected virtual int GetNumSerializedExtensionProperties(UnityEngine.Object obj)
    {
      return 0;
    }

    public void OnEnable()
    {
      this.InitExtensionPropertyInfo();
    }

    public int GetNumExtensionProperties()
    {
      return this.m_ExtensionProperties.Length;
    }

    public PropertyName GetExtensionPropertyName(int index)
    {
      return this.m_ExtensionProperties[index].propertyName;
    }

    public float GetExtensionPropertyDefaultValue(int index)
    {
      return this.m_ExtensionProperties[index].defaultValue;
    }

    public bool FindAudioExtensionProperties(SerializedObject serializedObject)
    {
      SerializedProperty serializedProperty = (SerializedProperty) null;
      if (serializedObject != null)
        serializedProperty = serializedObject.FindProperty("m_ExtensionPropertyValues");
      if (serializedProperty == null)
      {
        this.foundAllExtensionProperties = false;
        return false;
      }
      int num1 = serializedProperty.arraySize;
      if (serializedProperty.hasMultipleDifferentValues)
        num1 = this.GetMinNumSerializedExtensionProperties(serializedObject);
      if (serializedProperty == null || num1 == 0)
      {
        this.foundAllExtensionProperties = false;
        return false;
      }
      if (!this.foundAllExtensionProperties && serializedObject != null)
      {
        int num2 = 0;
        for (int index1 = 0; index1 < num1; ++index1)
        {
          SerializedProperty arrayElementAtIndex = serializedProperty.GetArrayElementAtIndex(index1);
          if (arrayElementAtIndex != null)
          {
            SerializedProperty propertyRelative = arrayElementAtIndex.FindPropertyRelative("propertyName");
            for (int index2 = 0; index2 < this.m_ExtensionProperties.Length; ++index2)
            {
              if (this.m_ExtensionProperties[index2].propertyName == (PropertyName) propertyRelative.stringValue && !propertyRelative.hasMultipleDifferentValues)
              {
                this.m_ExtensionProperties[index2].serializedProperty = arrayElementAtIndex.FindPropertyRelative("propertyValue");
                ++num2;
              }
            }
          }
        }
        this.foundAllExtensionProperties = num2 == this.m_ExtensionProperties.Length;
      }
      return this.foundAllExtensionProperties;
    }

    protected static void PropertyFieldAsBool(SerializedProperty property, GUIContent title)
    {
      Rect controlRect = EditorGUILayout.GetControlRect();
      title = EditorGUI.BeginProperty(controlRect, title, property);
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUI.Toggle(controlRect, title, (double) property.floatValue > 0.0);
      if (EditorGUI.EndChangeCheck())
        property.floatValue = !flag ? 0.0f : 1f;
      EditorGUI.EndProperty();
    }

    private int GetMinNumSerializedExtensionProperties(SerializedObject serializedObject)
    {
      UnityEngine.Object[] targetObjects = serializedObject.targetObjects;
      int val1 = targetObjects.Length <= 0 ? 0 : int.MaxValue;
      for (int index = 0; index < targetObjects.Length; ++index)
        val1 = Math.Min(val1, this.GetNumSerializedExtensionProperties(targetObjects[index]));
      return val1;
    }

    public struct ExtensionPropertyInfo
    {
      public PropertyName propertyName;
      public float defaultValue;
      public SerializedProperty serializedProperty;

      public ExtensionPropertyInfo(string nameIn, float defaultValueIn)
      {
        this.propertyName = new PropertyName(nameIn);
        this.defaultValue = defaultValueIn;
        this.serializedProperty = (SerializedProperty) null;
      }
    }
  }
}
