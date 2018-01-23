// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrefabInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PrefabInspector
  {
    public static void OnOverridenPrefabsInspector(GameObject gameObject)
    {
      GUI.enabled = true;
      Object prefabObject = PrefabUtility.GetPrefabObject((Object) gameObject);
      if (prefabObject == (Object) null)
        return;
      EditorGUIUtility.labelWidth = 200f;
      if (PrefabUtility.GetPrefabType((Object) gameObject) == PrefabType.PrefabInstance)
      {
        PropertyModification[] propertyModifications = PrefabUtility.GetPropertyModifications((Object) gameObject);
        if (propertyModifications != null && propertyModifications.Length != 0)
        {
          GUI.changed = false;
          for (int index = 0; index < propertyModifications.Length; ++index)
            propertyModifications[index].value = EditorGUILayout.TextField(propertyModifications[index].propertyPath, propertyModifications[index].value, new GUILayoutOption[0]);
          if (GUI.changed)
            PrefabUtility.SetPropertyModifications((Object) gameObject, propertyModifications);
        }
      }
      PrefabInspector.AddComponentGUI(prefabObject);
    }

    private static void AddComponentGUI(Object prefab)
    {
      SerializedObject serializedObject = new SerializedObject(prefab);
      SerializedProperty property = serializedObject.FindProperty("m_Modification");
      SerializedProperty endProperty = property.GetEndProperty();
      bool enterChildren;
      do
      {
        enterChildren = EditorGUILayout.PropertyField(property);
      }
      while (property.NextVisible(enterChildren) && !SerializedProperty.EqualContents(property, endProperty));
      serializedObject.ApplyModifiedProperties();
    }
  }
}
