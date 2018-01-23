// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.Debugger.PanelPickerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Experimental.UIElements.Debugger
{
  internal class PanelPickerWindow : EditorWindow
  {
    private System.Action<UIElementsDebugger.ViewPanel?> m_Callback;
    private PickingData m_Data;

    internal static PanelPickerWindow Show(PickingData data, System.Action<UIElementsDebugger.ViewPanel?> callback)
    {
      PanelPickerWindow instance = ScriptableObject.CreateInstance<PanelPickerWindow>();
      instance.m_Data = data;
      instance.m_Pos = data.screenRect;
      instance.m_Callback = callback;
      instance.ShowPopup();
      instance.Focus();
      return instance;
    }

    public void OnGUI()
    {
      UIElementsDebugger.ViewPanel? selectedPanel = new UIElementsDebugger.ViewPanel?();
      if (this.m_Data.Draw(ref selectedPanel, this.m_Data.screenRect))
      {
        this.Close();
        this.m_Callback(selectedPanel);
      }
      else
      {
        if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
          return;
        this.Close();
        this.m_Callback(new UIElementsDebugger.ViewPanel?());
      }
    }
  }
}
