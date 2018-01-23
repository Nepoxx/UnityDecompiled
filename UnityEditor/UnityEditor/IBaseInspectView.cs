// Decompiled with JetBrains decompiler
// Type: UnityEditor.IBaseInspectView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal interface IBaseInspectView
  {
    void UpdateInstructions();

    void DrawInstructionList();

    void DrawSelectedInstructionDetails();

    void ShowOverlay();

    void SelectRow(int index);

    void ClearRowSelection();
  }
}
