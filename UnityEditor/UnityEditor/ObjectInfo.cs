// Decompiled with JetBrains decompiler
// Type: UnityEditor.ObjectInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor
{
  internal class ObjectInfo
  {
    public int instanceId;
    public long memorySize;
    public int reason;
    public List<ObjectInfo> referencedBy;
    public string name;
    public string className;
  }
}
