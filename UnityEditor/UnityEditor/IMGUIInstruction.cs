// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUIInstruction
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [RequiredByNativeCode]
  internal struct IMGUIInstruction
  {
    public InstructionType type;
    public int level;
    public Rect unclippedRect;
    public StackFrame[] stack;
    public int typeInstructionIndex;
  }
}
