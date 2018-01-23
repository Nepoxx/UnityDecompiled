// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ObjectMemoryInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditorInternal
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class ObjectMemoryInfo
  {
    public int instanceId;
    public long memorySize;
    public int count;
    public int reason;
    public string name;
    public string className;
  }
}
