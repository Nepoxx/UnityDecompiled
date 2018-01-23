// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUIClipInstruction
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [RequiredByNativeCode]
  internal struct IMGUIClipInstruction
  {
    public Rect screenRect;
    public Rect unclippedScreenRect;
    public Vector2 scrollOffset;
    public Vector2 renderOffset;
    public bool resetOffset;
    public int level;
    public StackFrame[] pushStacktrace;
    public StackFrame[] popStacktrace;
  }
}
