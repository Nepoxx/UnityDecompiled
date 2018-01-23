// Decompiled with JetBrains decompiler
// Type: UnityEditor.TypeSelectionList
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class TypeSelectionList
  {
    private List<TypeSelection> m_TypeSelections;

    public TypeSelectionList(Object[] objects)
    {
      Dictionary<string, List<Object>> dictionary = new Dictionary<string, List<Object>>();
      foreach (Object @object in objects)
      {
        string typeName = ObjectNames.GetTypeName(@object);
        if (!dictionary.ContainsKey(typeName))
          dictionary[typeName] = new List<Object>();
        dictionary[typeName].Add(@object);
      }
      this.m_TypeSelections = new List<TypeSelection>();
      foreach (KeyValuePair<string, List<Object>> keyValuePair in dictionary)
        this.m_TypeSelections.Add(new TypeSelection(keyValuePair.Key, keyValuePair.Value.ToArray()));
      this.m_TypeSelections.Sort();
    }

    public List<TypeSelection> typeSelections
    {
      get
      {
        return this.m_TypeSelections;
      }
    }
  }
}
