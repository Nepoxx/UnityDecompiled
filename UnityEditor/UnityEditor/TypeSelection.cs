// Decompiled with JetBrains decompiler
// Type: UnityEditor.TypeSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class TypeSelection : IComparable
  {
    public GUIContent label;
    public UnityEngine.Object[] objects;

    public TypeSelection(string typeName, UnityEngine.Object[] objects)
    {
      this.objects = objects;
      this.label = new GUIContent(objects.Length.ToString() + " " + ObjectNames.NicifyVariableName(typeName) + (objects.Length <= 1 ? (object) "" : (object) "s"));
      this.label.image = (Texture) AssetPreview.GetMiniTypeThumbnail(objects[0]);
    }

    public int CompareTo(object o)
    {
      TypeSelection typeSelection = (TypeSelection) o;
      if (typeSelection.objects.Length != this.objects.Length)
        return typeSelection.objects.Length.CompareTo(this.objects.Length);
      return this.label.text.CompareTo(typeSelection.label.text);
    }
  }
}
