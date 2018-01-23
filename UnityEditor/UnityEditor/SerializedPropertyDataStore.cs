// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedPropertyDataStore
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Profiling;

namespace UnityEditor
{
  internal class SerializedPropertyDataStore
  {
    private Object[] m_Objects;
    private SerializedPropertyDataStore.Data[] m_Elements;
    private string[] m_PropNames;
    private SerializedPropertyDataStore.GatherDelegate m_GatherDel;

    public SerializedPropertyDataStore(string[] propNames, SerializedPropertyDataStore.GatherDelegate gatherDel)
    {
      this.m_PropNames = propNames;
      this.m_GatherDel = gatherDel;
      this.Repopulate();
    }

    public SerializedPropertyDataStore.Data[] GetElements()
    {
      return this.m_Elements;
    }

    ~SerializedPropertyDataStore()
    {
      this.Clear();
    }

    public bool Repopulate()
    {
      Profiler.BeginSample("SerializedPropertyDataStore.Repopulate.GatherDelegate");
      Object[] lhs = this.m_GatherDel();
      Profiler.EndSample();
      if (this.m_Objects != null)
      {
        if (lhs.Length == this.m_Objects.Length && ArrayUtility.ArrayReferenceEquals<Object>(lhs, this.m_Objects))
          return false;
        this.Clear();
      }
      this.m_Objects = lhs;
      this.m_Elements = new SerializedPropertyDataStore.Data[lhs.Length];
      for (int index = 0; index < lhs.Length; ++index)
        this.m_Elements[index] = new SerializedPropertyDataStore.Data(lhs[index], this.m_PropNames);
      return true;
    }

    private void Clear()
    {
      for (int index = 0; index < this.m_Elements.Length; ++index)
        this.m_Elements[index].Dispose();
      this.m_Objects = (Object[]) null;
      this.m_Elements = (SerializedPropertyDataStore.Data[]) null;
    }

    internal class Data
    {
      private Object m_Object;
      private SerializedObject m_SerializedObject;
      private SerializedProperty[] m_Properties;

      public Data(Object obj, string[] props)
      {
        this.m_Object = obj;
        this.m_SerializedObject = new SerializedObject(obj);
        this.m_Properties = new SerializedProperty[props.Length];
        for (int index = 0; index < props.Length; ++index)
          this.m_Properties[index] = this.m_SerializedObject.FindProperty(props[index]);
      }

      public void Dispose()
      {
        foreach (SerializedProperty property in this.m_Properties)
        {
          if (property != null)
            property.Dispose();
        }
        this.m_SerializedObject.Dispose();
        this.m_Object = (Object) null;
        this.m_SerializedObject = (SerializedObject) null;
        this.m_Properties = (SerializedProperty[]) null;
      }

      public string name
      {
        get
        {
          return !(this.m_Object != (Object) null) ? string.Empty : this.m_Object.name;
        }
      }

      public SerializedObject serializedObject
      {
        get
        {
          return this.m_SerializedObject;
        }
      }

      public SerializedProperty[] properties
      {
        get
        {
          return this.m_Properties;
        }
      }

      public int objectId
      {
        get
        {
          if (!(bool) this.m_Object)
            return 0;
          Component component = this.m_Object as Component;
          return !((Object) component != (Object) null) ? this.m_Object.GetInstanceID() : component.gameObject.GetInstanceID();
        }
      }

      public bool Update()
      {
        return this.m_Object != (Object) null && this.m_SerializedObject.UpdateIfRequiredOrScript();
      }

      public void Store()
      {
        if (!(this.m_Object != (Object) null))
          return;
        this.m_SerializedObject.ApplyModifiedProperties();
      }
    }

    internal delegate Object[] GatherDelegate();
  }
}
