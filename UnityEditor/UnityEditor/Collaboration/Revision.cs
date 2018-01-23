// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.Revision
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEditor.Collaboration
{
  [StructLayout(LayoutKind.Sequential)]
  internal class Revision
  {
    private string m_AuthorName;
    private string m_Author;
    private string m_Comment;
    private string m_RevisionID;
    private string m_Reference;
    private ulong m_TimeStamp;

    private Revision()
    {
    }

    public string authorName
    {
      get
      {
        return this.m_AuthorName;
      }
    }

    public string author
    {
      get
      {
        return this.m_Author;
      }
    }

    public string comment
    {
      get
      {
        return this.m_Comment;
      }
    }

    public string revisionID
    {
      get
      {
        return this.m_RevisionID;
      }
    }

    public string reference
    {
      get
      {
        return this.m_Reference;
      }
    }

    public ulong timeStamp
    {
      get
      {
        return this.m_TimeStamp;
      }
    }
  }
}
