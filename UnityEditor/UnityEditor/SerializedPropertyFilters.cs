// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedPropertyFilters
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class SerializedPropertyFilters
  {
    internal static readonly SerializedPropertyFilters.None s_FilterNone = new SerializedPropertyFilters.None();

    internal interface IFilter
    {
      bool Active();

      bool Filter(SerializedProperty prop);

      void OnGUI(Rect r);

      string SerializeState();

      void DeserializeState(string state);
    }

    internal abstract class SerializableFilter : SerializedPropertyFilters.IFilter
    {
      public abstract bool Active();

      public abstract bool Filter(SerializedProperty prop);

      public abstract void OnGUI(Rect r);

      public string SerializeState()
      {
        return JsonUtility.ToJson((object) this);
      }

      public void DeserializeState(string state)
      {
        JsonUtility.FromJsonOverwrite(state, (object) this);
      }
    }

    internal class String : SerializedPropertyFilters.SerializableFilter
    {
      [SerializeField]
      protected string m_Text = "";

      public override bool Active()
      {
        return !string.IsNullOrEmpty(this.m_Text);
      }

      public override bool Filter(SerializedProperty prop)
      {
        return prop.stringValue.IndexOf(this.m_Text, 0, StringComparison.OrdinalIgnoreCase) >= 0;
      }

      public override void OnGUI(Rect r)
      {
        r.width -= 15f;
        this.m_Text = EditorGUI.TextField(r, GUIContent.none, this.m_Text, SerializedPropertyFilters.String.Styles.searchField);
        r.x += r.width;
        r.width = 15f;
        bool flag = this.m_Text != "";
        if (!GUI.Button(r, GUIContent.none, !flag ? SerializedPropertyFilters.String.Styles.searchFieldCancelButtonEmpty : SerializedPropertyFilters.String.Styles.searchFieldCancelButton) || !flag)
          return;
        this.m_Text = "";
        GUIUtility.keyboardControl = 0;
      }

      private static class Styles
      {
        public static readonly GUIStyle searchField = (GUIStyle) "SearchTextField";
        public static readonly GUIStyle searchFieldCancelButton = (GUIStyle) "SearchCancelButton";
        public static readonly GUIStyle searchFieldCancelButtonEmpty = (GUIStyle) "SearchCancelButtonEmpty";
      }
    }

    internal sealed class Name : SerializedPropertyFilters.String
    {
      public bool Filter(string str)
      {
        return str.IndexOf(this.m_Text, 0, StringComparison.OrdinalIgnoreCase) >= 0;
      }
    }

    internal sealed class None : SerializedPropertyFilters.IFilter
    {
      public bool Active()
      {
        return false;
      }

      public bool Filter(SerializedProperty prop)
      {
        return true;
      }

      public void OnGUI(Rect r)
      {
      }

      public string SerializeState()
      {
        return (string) null;
      }

      public void DeserializeState(string state)
      {
      }
    }
  }
}
