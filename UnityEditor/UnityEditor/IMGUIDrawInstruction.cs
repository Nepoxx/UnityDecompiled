// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUIDrawInstruction
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [RequiredByNativeCode]
  internal struct IMGUIDrawInstruction
  {
    public Rect rect;
    public Rect visibleRect;
    public GUIStyle usedGUIStyle;
    public GUIContent usedGUIContent;
    public StackFrame[] stackframes;

    public void Reset()
    {
      this.rect = new Rect();
      this.visibleRect = new Rect();
      this.usedGUIStyle = GUIStyle.none;
      this.usedGUIContent = GUIContent.none;
    }
  }
}
