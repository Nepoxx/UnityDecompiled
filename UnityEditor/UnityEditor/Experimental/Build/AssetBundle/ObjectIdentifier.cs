// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.AssetBundle.ObjectIdentifier
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
  [UsedByNativeCode]
  [Serializable]
  public struct ObjectIdentifier
  {
    [NativeName("guid")]
    internal GUID m_GUID;
    [NativeName("localIdentifierInFile")]
    internal long m_LocalIdentifierInFile;
    [NativeName("fileType")]
    internal FileType m_FileType;
    [NativeName("filePath")]
    internal string m_FilePath;

    public GUID guid
    {
      get
      {
        return this.m_GUID;
      }
    }

    public long localIdentifierInFile
    {
      get
      {
        return this.m_LocalIdentifierInFile;
      }
    }

    public FileType fileType
    {
      get
      {
        return this.m_FileType;
      }
    }

    public string filePath
    {
      get
      {
        return this.m_FilePath;
      }
    }

    public override string ToString()
    {
      return UnityString.Format("{{guid: {0}, fileID: {1}, type: {2}, path: {3}}}", (object) this.m_GUID, (object) this.m_LocalIdentifierInFile, (object) this.m_FileType, (object) this.m_FilePath);
    }

    public static bool operator ==(ObjectIdentifier a, ObjectIdentifier b)
    {
      return !(a.m_GUID != b.m_GUID) && a.m_LocalIdentifierInFile == b.m_LocalIdentifierInFile && (a.m_FileType == b.m_FileType && !(a.m_FilePath != b.m_FilePath));
    }

    public static bool operator !=(ObjectIdentifier a, ObjectIdentifier b)
    {
      return !(a == b);
    }

    public static bool operator <(ObjectIdentifier a, ObjectIdentifier b)
    {
      if (a.m_GUID == b.m_GUID)
        return a.m_LocalIdentifierInFile < b.m_LocalIdentifierInFile;
      return a.m_GUID < b.m_GUID;
    }

    public static bool operator >(ObjectIdentifier a, ObjectIdentifier b)
    {
      if (a.m_GUID == b.m_GUID)
        return a.m_LocalIdentifierInFile > b.m_LocalIdentifierInFile;
      return a.m_GUID > b.m_GUID;
    }

    public override bool Equals(object obj)
    {
      return obj is ObjectIdentifier && this == (ObjectIdentifier) obj;
    }

    public override int GetHashCode()
    {
      int num = (int) ((FileType) ((this.m_GUID.GetHashCode() * 397 ^ this.m_LocalIdentifierInFile.GetHashCode()) * 397) ^ this.m_FileType);
      if (!string.IsNullOrEmpty(this.m_FilePath))
        num = num * 397 ^ this.m_FilePath.GetHashCode();
      return num;
    }
  }
}
