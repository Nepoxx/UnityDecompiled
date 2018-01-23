// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ObjectMemoryStackInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEditorInternal
{
  [RequiredByNativeCode]
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class ObjectMemoryStackInfo
  {
    public bool expanded;
    public bool sorted;
    public int allocated;
    public int ownedAllocated;
    public ObjectMemoryStackInfo[] callerSites;
    public string name;
  }
}
