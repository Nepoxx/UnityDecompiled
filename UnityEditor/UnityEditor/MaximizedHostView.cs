// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaximizedHostView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class MaximizedHostView : HostView
  {
    protected override void OldOnGUI()
    {
      this.ClearBackground();
      EditorGUIUtility.ResetGUIState();
      Rect rect1 = new Rect(-2f, 0.0f, this.position.width + 4f, this.position.height);
      this.background = (GUIStyle) "dockarea";
      Rect rect2 = this.background.margin.Remove(rect1);
      Rect position = new Rect(rect2.x + 1f, rect2.y, rect2.width - 2f, 17f);
      if (Event.current.type == EventType.Repaint)
      {
        this.background.Draw(rect2, GUIContent.none, false, false, false, false);
        (GUIStyle) "dragTab".Draw(position, this.actualView.titleContent, false, false, true, this.hasFocus);
      }
      if (Event.current.type == EventType.ContextClick && position.Contains(Event.current.mousePosition))
        this.PopupGenericMenu(this.actualView, new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 0.0f, 0.0f));
      this.ShowGenericMenu();
      if ((bool) ((UnityEngine.Object) this.actualView))
        this.actualView.m_Pos = this.borderSize.Remove(this.screenPosition);
      this.InvokeOnGUI(rect2);
    }

    protected override RectOffset GetBorderSize()
    {
      this.m_BorderSize.left = 0;
      this.m_BorderSize.right = 0;
      this.m_BorderSize.top = 17;
      this.m_BorderSize.bottom = 4;
      return this.m_BorderSize;
    }

    private void Unmaximize(object userData)
    {
      WindowLayout.Unmaximize((EditorWindow) userData);
    }

    protected override void AddDefaultItemsToMenu(GenericMenu menu, EditorWindow window)
    {
      base.AddDefaultItemsToMenu(menu, window);
      menu.AddItem(EditorGUIUtility.TextContent("Maximize"), !(this.parent is SplitView), new GenericMenu.MenuFunction2(this.Unmaximize), (object) window);
      menu.AddDisabledItem(EditorGUIUtility.TextContent("Close Tab"));
      menu.AddSeparator("");
      System.Type[] paneTypes = this.GetPaneTypes();
      GUIContent guiContent = EditorGUIUtility.TextContent("Add Tab");
      foreach (System.Type t in paneTypes)
      {
        if (t != null)
        {
          GUIContent content = new GUIContent(EditorWindow.GetLocalizedTitleContentFromType(t));
          content.text = guiContent.text + "/" + content.text;
          menu.AddDisabledItem(content);
        }
      }
    }
  }
}
