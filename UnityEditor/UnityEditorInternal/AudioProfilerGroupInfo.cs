// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerGroupInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditorInternal
{
  [Serializable]
  public struct AudioProfilerGroupInfo
  {
    public int assetInstanceId;
    public int objectInstanceId;
    public int assetNameOffset;
    public int objectNameOffset;
    public int parentId;
    public int uniqueId;
    public int flags;
    public int playCount;
    public float distanceToListener;
    public float volume;
    public float audibility;
    public float minDist;
    public float maxDist;
    public float time;
    public float duration;
    public float frequency;
  }
}
