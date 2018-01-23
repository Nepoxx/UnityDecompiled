// Decompiled with JetBrains decompiler
// Type: UnityEditor.ScriptableSingletonDictionary`2
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class ScriptableSingletonDictionary<TDerived, TValue> : ScriptableObject where TDerived : ScriptableObject where TValue : ScriptableObject
  {
    private static readonly string k_Extension = ".pref";
    private static TDerived s_Instance;
    protected string m_PreferencesFileName;

    public static TDerived instance
    {
      get
      {
        if ((UnityEngine.Object) ScriptableSingletonDictionary<TDerived, TValue>.s_Instance == (UnityEngine.Object) null)
        {
          ScriptableSingletonDictionary<TDerived, TValue>.s_Instance = ScriptableObject.CreateInstance<TDerived>();
          ScriptableSingletonDictionary<TDerived, TValue>.s_Instance.hideFlags = HideFlags.HideAndDontSave;
        }
        return ScriptableSingletonDictionary<TDerived, TValue>.s_Instance;
      }
    }

    public TValue this[string preferencesFileName]
    {
      get
      {
        return this.Load(preferencesFileName);
      }
    }

    private TValue CreateNewValue()
    {
      TValue instance = ScriptableObject.CreateInstance<TValue>();
      instance.hideFlags |= HideFlags.HideAndDontSave;
      return instance;
    }

    private string GetProjectRelativePath(string file)
    {
      return this.GetFolderPath() + "/" + file + ScriptableSingletonDictionary<TDerived, TValue>.k_Extension;
    }

    public void Save(string preferencesFileName, TValue value)
    {
      if (string.IsNullOrEmpty(preferencesFileName) || (UnityEngine.Object) value == (UnityEngine.Object) null)
        return;
      string file = preferencesFileName;
      if (string.IsNullOrEmpty(file))
        return;
      string path = Application.dataPath + "/../" + this.GetFolderPath();
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      InternalEditorUtility.SaveToSerializedFileAndForget((UnityEngine.Object[]) new TValue[1]
      {
        value
      }, this.GetProjectRelativePath(file), true);
    }

    public void Clear(string preferencesFileName)
    {
      string path = Application.dataPath + "/../" + this.GetProjectRelativePath(preferencesFileName);
      if (!File.Exists(path))
        return;
      File.Delete(path);
    }

    private TValue Load(string preferencesFileName)
    {
      TValue obj1 = (TValue) null;
      string file = preferencesFileName;
      if (!string.IsNullOrEmpty(file))
      {
        UnityEngine.Object[] objectArray = InternalEditorUtility.LoadSerializedFileAndForget(this.GetProjectRelativePath(file));
        if (objectArray != null && objectArray.Length > 0)
        {
          obj1 = objectArray[0] as TValue;
          if ((UnityEngine.Object) obj1 != (UnityEngine.Object) null)
            obj1.hideFlags |= HideFlags.HideAndDontSave;
        }
      }
      this.m_PreferencesFileName = preferencesFileName;
      TValue obj2 = obj1;
      if ((object) obj2 == null)
        obj2 = this.CreateNewValue();
      return obj2;
    }

    private string GetFolderPath()
    {
      foreach (object customAttribute in this.GetType().GetCustomAttributes(true))
      {
        if (customAttribute is LibraryFolderPathAttribute)
          return (customAttribute as LibraryFolderPathAttribute).folderPath;
      }
      throw new ArgumentException("The LibraryFolderPathAttribute[] attribute is required for this class.");
    }
  }
}
