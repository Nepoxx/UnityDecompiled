// Decompiled with JetBrains decompiler
// Type: UnityEditor.PaneDragTab
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PaneDragTab : GUIView
  {
    [SerializeField]
    private Vector2 m_FullWindowSize = new Vector2(80f, 60f);
    private float m_TargetAlpha = 1f;
    private DropInfo.Type m_Type = ~DropInfo.Type.Tab;
    [SerializeField]
    private ContainerWindow m_InFrontOfWindow = (ContainerWindow) null;
    private const float kMaxArea = 50000f;
    [SerializeField]
    private bool m_Shadow;
    private static PaneDragTab s_Get;
    private const float kTopThumbnailOffset = 10f;
    [SerializeField]
    private Rect m_TargetRect;
    [SerializeField]
    private static GUIStyle s_PaneStyle;
    [SerializeField]
    private static GUIStyle s_TabStyle;
    private bool m_TabVisible;
    private GUIContent m_Content;
    [SerializeField]
    internal ContainerWindow m_Window;

    public static PaneDragTab get
    {
      get
      {
        if (!(bool) ((Object) PaneDragTab.s_Get))
        {
          Object[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (PaneDragTab));
          if (objectsOfTypeAll.Length != 0)
            PaneDragTab.s_Get = (PaneDragTab) objectsOfTypeAll[0];
          if ((bool) ((Object) PaneDragTab.s_Get))
            return PaneDragTab.s_Get;
          PaneDragTab.s_Get = ScriptableObject.CreateInstance<PaneDragTab>();
        }
        return PaneDragTab.s_Get;
      }
    }

    public void SetDropInfo(DropInfo di, Vector2 mouseScreenPos, ContainerWindow inFrontOf)
    {
      if (this.m_Type != di.type || di.type == DropInfo.Type.Pane && di.rect != this.m_TargetRect)
      {
        this.m_Type = di.type;
        switch (di.type)
        {
          case DropInfo.Type.Tab:
          case DropInfo.Type.Pane:
            this.m_TargetAlpha = 1f;
            break;
          case DropInfo.Type.Window:
            this.m_TargetAlpha = 0.6f;
            break;
        }
      }
      switch (di.type)
      {
        case DropInfo.Type.Tab:
        case DropInfo.Type.Pane:
          this.m_TargetRect = di.rect;
          break;
        case DropInfo.Type.Window:
          this.m_TargetRect = new Rect(mouseScreenPos.x - this.m_FullWindowSize.x / 2f, mouseScreenPos.y - this.m_FullWindowSize.y / 2f, this.m_FullWindowSize.x, this.m_FullWindowSize.y);
          break;
      }
      this.m_TabVisible = di.type == DropInfo.Type.Tab;
      this.m_TargetRect.x = Mathf.Round(this.m_TargetRect.x);
      this.m_TargetRect.y = Mathf.Round(this.m_TargetRect.y);
      this.m_TargetRect.width = Mathf.Round(this.m_TargetRect.width);
      this.m_TargetRect.height = Mathf.Round(this.m_TargetRect.height);
      this.m_InFrontOfWindow = inFrontOf;
      this.m_Window.MoveInFrontOf(this.m_InFrontOfWindow);
      this.SetWindowPos(this.m_TargetRect);
      this.Repaint();
    }

    public void Close()
    {
      if ((bool) ((Object) this.m_Window))
        this.m_Window.Close();
      Object.DestroyImmediate((Object) this, true);
      PaneDragTab.s_Get = (PaneDragTab) null;
    }

    public void Show(Rect pixelPos, GUIContent content, Vector2 viewSize, Vector2 mouseScreenPosition)
    {
      this.m_Content = content;
      float num = viewSize.x * viewSize.y;
      this.m_FullWindowSize = viewSize * Mathf.Sqrt(Mathf.Clamp01(50000f / num));
      if (!(bool) ((Object) this.m_Window))
      {
        this.m_Window = ScriptableObject.CreateInstance<ContainerWindow>();
        this.m_Window.m_DontSaveToLayout = true;
        this.SetMinMaxSizes(Vector2.zero, new Vector2(10000f, 10000f));
        this.SetWindowPos(pixelPos);
        this.m_Window.rootView = (View) this;
      }
      else
        this.SetWindowPos(pixelPos);
      this.m_Window.Show(ShowMode.NoShadow, true, false);
      this.m_TargetRect = pixelPos;
    }

    private void SetWindowPos(Rect screenPosition)
    {
      this.m_Window.position = screenPosition;
    }

    protected override void OldOnGUI()
    {
      if (PaneDragTab.s_PaneStyle == null)
      {
        PaneDragTab.s_PaneStyle = (GUIStyle) "dragtabdropwindow";
        PaneDragTab.s_TabStyle = (GUIStyle) "dragtab";
      }
      if (Event.current.type != EventType.Repaint)
        return;
      Color color = GUI.color;
      GUI.color = Color.white;
      PaneDragTab.s_PaneStyle.Draw(new Rect(0.0f, 0.0f, this.position.width, this.position.height), this.m_Content, false, false, true, true);
      if (this.m_TabVisible)
        PaneDragTab.s_TabStyle.Draw(new Rect(0.0f, 0.0f, this.position.width, this.position.height), this.m_Content, false, false, true, true);
      GUI.color = color;
      this.m_Window.SetAlpha(this.m_TargetAlpha);
    }
  }
}
