// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.Change
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEditor.Collaboration
{
  [StructLayout(LayoutKind.Sequential)]
  internal class Change
  {
    private string m_Path;
    private Collab.CollabStates m_State;
    private Change.RevertableStates m_RevertableState;
    private string m_RelatedTo;
    private string m_LocalStatus;
    private string m_RemoteStatus;
    private string m_ResolveStatus;

    private Change()
    {
    }

    public string path
    {
      get
      {
        return this.m_Path;
      }
    }

    public ulong state
    {
      get
      {
        return (ulong) this.m_State;
      }
    }

    public bool isRevertable
    {
      get
      {
        return (this.m_RevertableState & Change.RevertableStates.Revertable) == Change.RevertableStates.Revertable;
      }
    }

    public ulong revertableState
    {
      get
      {
        return (ulong) this.m_RevertableState;
      }
    }

    public string relatedTo
    {
      get
      {
        return this.m_RelatedTo;
      }
    }

    public bool isMeta
    {
      get
      {
        return (this.m_State & Collab.CollabStates.kCollabMetaFile) == Collab.CollabStates.kCollabMetaFile;
      }
    }

    public bool isConflict
    {
      get
      {
        return (this.m_State & Collab.CollabStates.kCollabConflicted) == Collab.CollabStates.kCollabConflicted || (this.m_State & Collab.CollabStates.kCollabPendingMerge) == Collab.CollabStates.kCollabPendingMerge;
      }
    }

    public bool isFolderMeta
    {
      get
      {
        return (this.m_State & Collab.CollabStates.kCollabFolderMetaFile) == Collab.CollabStates.kCollabFolderMetaFile;
      }
    }

    public bool isResolved
    {
      get
      {
        return (this.m_State & Collab.CollabStates.kCollabUseMine) == Collab.CollabStates.kCollabUseMine || (this.m_State & Collab.CollabStates.kCollabUseTheir) == Collab.CollabStates.kCollabUseTheir || (this.m_State & Collab.CollabStates.kCollabMerged) == Collab.CollabStates.kCollabMerged;
      }
    }

    public string localStatus
    {
      get
      {
        return this.m_LocalStatus;
      }
    }

    public string remoteStatus
    {
      get
      {
        return this.m_RemoteStatus;
      }
    }

    public string resolveStatus
    {
      get
      {
        return this.m_ResolveStatus;
      }
    }

    [System.Flags]
    public enum RevertableStates : ulong
    {
      Revertable = 1,
      NotRevertable = 2,
      Revertable_File = 4,
      Revertable_Folder = 8,
      Revertable_EmptyFolder = 16, // 0x0000000000000010
      NotRevertable_File = 32, // 0x0000000000000020
      NotRevertable_Folder = 64, // 0x0000000000000040
      NotRevertable_FileAdded = 128, // 0x0000000000000080
      NotRevertable_FolderAdded = 256, // 0x0000000000000100
      NotRevertable_FolderContainsAdd = 512, // 0x0000000000000200
      InvalidRevertableState = 2147483648, // 0x0000000080000000
    }
  }
}
