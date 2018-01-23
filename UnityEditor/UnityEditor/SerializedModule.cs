// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedModule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SerializedModule
  {
    protected string m_ModuleName;
    private SerializedObject m_Object;

    public SerializedModule(SerializedObject o, string name)
    {
      this.m_Object = o;
      this.m_ModuleName = name;
    }

    public SerializedProperty GetProperty0(string name)
    {
      SerializedProperty property = this.m_Object.FindProperty(name);
      if (property == null)
        Debug.LogError((object) ("GetProperty0: not found: " + name));
      return property;
    }

    public SerializedProperty GetProperty(string name)
    {
      SerializedProperty property = this.m_Object.FindProperty(SerializedModule.Concat(this.m_ModuleName, name));
      if (property == null)
        Debug.LogError((object) ("GetProperty: not found: " + SerializedModule.Concat(this.m_ModuleName, name)));
      return property;
    }

    public SerializedProperty GetProperty0(string structName, string propName)
    {
      SerializedProperty property = this.m_Object.FindProperty(SerializedModule.Concat(structName, propName));
      if (property == null)
        Debug.LogError((object) ("GetProperty: not found: " + SerializedModule.Concat(structName, propName)));
      return property;
    }

    public SerializedProperty GetProperty(string structName, string propName)
    {
      SerializedProperty property = this.m_Object.FindProperty(SerializedModule.Concat(SerializedModule.Concat(this.m_ModuleName, structName), propName));
      if (property == null)
        Debug.LogError((object) ("GetProperty: not found: " + SerializedModule.Concat(SerializedModule.Concat(this.m_ModuleName, structName), propName)));
      return property;
    }

    public static string Concat(string a, string b)
    {
      return a + "." + b;
    }

    public string GetUniqueModuleName(Object o)
    {
      return SerializedModule.Concat("" + (object) o.GetInstanceID(), this.m_ModuleName);
    }

    internal SerializedObject serializedObject
    {
      get
      {
        return this.m_Object;
      }
    }
  }
}
