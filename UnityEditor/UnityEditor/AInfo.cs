// Decompiled with JetBrains decompiler
// Type: UnityEditor.AInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  internal class AInfo : IComparable, IEquatable<AInfo>
  {
    public bool m_IconEnabled;
    public bool m_GizmoEnabled;
    public int m_ClassID;
    public string m_ScriptClass;
    public string m_DisplayText;
    public int m_Flags;

    public AInfo(bool gizmoEnabled, bool iconEnabled, int flags, int classID, string scriptClass)
    {
      this.m_GizmoEnabled = gizmoEnabled;
      this.m_IconEnabled = iconEnabled;
      this.m_ClassID = classID;
      this.m_ScriptClass = scriptClass;
      this.m_Flags = flags;
      if (this.m_ScriptClass == "")
        this.m_DisplayText = UnityType.FindTypeByPersistentTypeID(this.m_ClassID).name;
      else
        this.m_DisplayText = this.m_ScriptClass;
    }

    private bool IsBitSet(byte b, int pos)
    {
      return ((int) b & 1 << pos) != 0;
    }

    public bool HasGizmo()
    {
      return (this.m_Flags & 2) > 0;
    }

    public bool HasIcon()
    {
      return (this.m_Flags & 1) > 0;
    }

    public int CompareTo(object obj)
    {
      AInfo ainfo = obj as AInfo;
      if (ainfo != null)
        return this.m_DisplayText.CompareTo(ainfo.m_DisplayText);
      throw new ArgumentException("Object is not an AInfo");
    }

    public bool Equals(AInfo other)
    {
      return this.m_ClassID == other.m_ClassID && this.m_ScriptClass == other.m_ScriptClass;
    }

    public enum Flags
    {
      kHasIcon = 1,
      kHasGizmo = 2,
    }
  }
}
