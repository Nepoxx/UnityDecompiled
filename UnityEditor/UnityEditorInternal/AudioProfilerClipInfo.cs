// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerClipInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditorInternal
{
  [Serializable]
  public struct AudioProfilerClipInfo
  {
    public int assetInstanceId;
    public int assetNameOffset;
    public int loadState;
    public int internalLoadState;
    public int age;
    public int disposed;
    public int numChannelInstances;
  }
}
