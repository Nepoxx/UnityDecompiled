// Decompiled with JetBrains decompiler
// Type: UnityEditor.LibraryFolderPathAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [AttributeUsage(AttributeTargets.Class)]
  internal class LibraryFolderPathAttribute : Attribute
  {
    public LibraryFolderPathAttribute(string relativePath)
    {
      if (string.IsNullOrEmpty(relativePath))
      {
        Debug.LogError((object) "Invalid relative path! (its null or empty)");
      }
      else
      {
        if ((int) relativePath[0] == 47)
          throw new ArgumentException("Folder relative path cannot start with a slash.");
        this.folderPath = "Library/" + relativePath;
      }
    }

    public string folderPath { get; set; }
  }
}
