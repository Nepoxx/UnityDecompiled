// Decompiled with JetBrains decompiler
// Type: PatchImportSettingRecycleID
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using UnityEditor;

internal class PatchImportSettingRecycleID
{
  public static void Patch(SerializedObject serializedObject, int classID, string oldName, string newName)
  {
    PatchImportSettingRecycleID.PatchMultiple(serializedObject, classID, new string[1]
    {
      oldName
    }, new string[1]{ newName });
  }

  public static void PatchMultiple(SerializedObject serializedObject, int classID, string[] oldNames, string[] newNames)
  {
    int length = oldNames.Length;
    IEnumerator enumerator = serializedObject.FindProperty("m_FileIDToRecycleName").GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        SerializedProperty current = (SerializedProperty) enumerator.Current;
        if (AssetImporter.LocalFileIDToClassID(current.FindPropertyRelative("first").longValue) == classID)
        {
          SerializedProperty propertyRelative = current.FindPropertyRelative("second");
          int index = Array.IndexOf<string>(oldNames, propertyRelative.stringValue);
          if (index >= 0)
          {
            propertyRelative.stringValue = newNames[index];
            if (--length == 0)
              break;
          }
        }
      }
    }
    finally
    {
      IDisposable disposable;
      if ((disposable = enumerator as IDisposable) != null)
        disposable.Dispose();
    }
  }
}
