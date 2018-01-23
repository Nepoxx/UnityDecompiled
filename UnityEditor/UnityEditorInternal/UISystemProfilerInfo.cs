// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.UISystemProfilerInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Scripting;

namespace UnityEditorInternal
{
  [RequiredByNativeCode]
  [Serializable]
  public struct UISystemProfilerInfo
  {
    public int objectInstanceId;
    public int objectNameOffset;
    public int parentId;
    public int batchCount;
    public int totalBatchCount;
    public int vertexCount;
    public int totalVertexCount;
    public bool isBatch;
    public BatchBreakingReason batchBreakingReason;
    public int instanceIDsIndex;
    public int instanceIDsCount;
    public int renderDataIndex;
    public int renderDataCount;
  }
}
