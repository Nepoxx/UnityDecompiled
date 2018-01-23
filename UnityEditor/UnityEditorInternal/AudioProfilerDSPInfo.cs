// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerDSPInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditorInternal
{
  [Serializable]
  public struct AudioProfilerDSPInfo
  {
    public int id;
    public int target;
    public int targetPort;
    public int numChannels;
    public int nameOffset;
    public float weight;
    public float cpuLoad;
    public float level1;
    public float level2;
    public int numLevels;
    public int flags;
  }
}
