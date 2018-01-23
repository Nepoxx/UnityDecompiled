// Decompiled with JetBrains decompiler
// Type: UnityEditor.FilePathAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [AttributeUsage(AttributeTargets.Class)]
  internal class FilePathAttribute : Attribute
  {
    public FilePathAttribute(string relativePath, FilePathAttribute.Location location)
    {
      if (string.IsNullOrEmpty(relativePath))
      {
        Debug.LogError((object) "Invalid relative path! (its null or empty)");
      }
      else
      {
        if ((int) relativePath[0] == 47)
          relativePath = relativePath.Substring(1);
        if (location == FilePathAttribute.Location.PreferencesFolder)
          this.filepath = InternalEditorUtility.unityPreferencesFolder + "/" + relativePath;
        else
          this.filepath = relativePath;
      }
    }

    public string filepath { get; set; }

    public enum Location
    {
      PreferencesFolder,
      ProjectFolder,
    }
  }
}
