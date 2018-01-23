// Decompiled with JetBrains decompiler
// Type: UnityEditor.WebTemplate
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class WebTemplate
  {
    public string m_Path;
    public string m_Name;
    public Texture2D m_Thumbnail;
    public string[] m_CustomKeys;

    public string[] CustomKeys
    {
      get
      {
        return this.m_CustomKeys;
      }
    }

    public override bool Equals(object other)
    {
      return other is WebTemplate && other.ToString().Equals(this.ToString());
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ this.m_Path.GetHashCode();
    }

    public override string ToString()
    {
      return this.m_Path;
    }

    public GUIContent ToGUIContent(Texture2D defaultIcon)
    {
      return new GUIContent(this.m_Name, !((Object) this.m_Thumbnail == (Object) null) ? (Texture) this.m_Thumbnail : (Texture) defaultIcon);
    }
  }
}
