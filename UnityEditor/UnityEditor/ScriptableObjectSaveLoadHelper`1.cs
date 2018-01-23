// Decompiled with JetBrains decompiler
// Type: UnityEditor.ScriptableObjectSaveLoadHelper`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class ScriptableObjectSaveLoadHelper<T> where T : ScriptableObject
  {
    public ScriptableObjectSaveLoadHelper(string fileExtensionWithoutDot, SaveType saveType)
    {
      this.saveType = saveType;
      this.fileExtensionWithoutDot = fileExtensionWithoutDot.TrimStart('.');
    }

    public string fileExtensionWithoutDot { get; private set; }

    private SaveType saveType { get; set; }

    public T Load(string filePath)
    {
      filePath = this.AppendFileExtensionIfNeeded(filePath);
      if (!string.IsNullOrEmpty(filePath))
      {
        Object[] objectArray = InternalEditorUtility.LoadSerializedFileAndForget(filePath);
        if (objectArray != null && objectArray.Length > 0)
          return objectArray[0] as T;
      }
      return (T) null;
    }

    public T Create()
    {
      return ScriptableObject.CreateInstance<T>();
    }

    public void Save(T t, string filePath)
    {
      if ((Object) t == (Object) null)
        Debug.LogError((object) "Cannot save scriptableObject: its null!");
      else if (string.IsNullOrEmpty(filePath))
      {
        Debug.LogError((object) ("Invalid path: '" + filePath + "'"));
      }
      else
      {
        string directoryName = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        filePath = this.AppendFileExtensionIfNeeded(filePath);
        InternalEditorUtility.SaveToSerializedFileAndForget((Object[]) new T[1]
        {
          t
        }, filePath, this.saveType == SaveType.Text);
      }
    }

    public override string ToString()
    {
      return string.Format("{0}, {1}", (object) this.fileExtensionWithoutDot, (object) this.saveType);
    }

    private string AppendFileExtensionIfNeeded(string path)
    {
      if (!Path.HasExtension(path) && !string.IsNullOrEmpty(this.fileExtensionWithoutDot))
        return path + "." + this.fileExtensionWithoutDot;
      return path;
    }
  }
}
