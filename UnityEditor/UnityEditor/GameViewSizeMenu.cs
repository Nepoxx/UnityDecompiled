// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameViewSizeMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class GameViewSizeMenu : FlexibleMenu
  {
    private const float kTopMargin = 7f;
    private const float kMargin = 9f;
    private IGameViewSizeMenuUser m_GameView;

    public GameViewSizeMenu(IFlexibleMenuItemProvider itemProvider, int selectionIndex, FlexibleMenuModifyItemUI modifyItemUi, IGameViewSizeMenuUser gameView)
      : base(itemProvider, selectionIndex, modifyItemUi, new Action<int, object>(gameView.SizeSelectionCallback))
    {
      this.m_GameView = gameView;
    }

    private float frameHeight
    {
      get
      {
        return 30f;
      }
    }

    private float contentOffset
    {
      get
      {
        return this.frameHeight + 2f;
      }
    }

    public override Vector2 GetWindowSize()
    {
      Vector2 vector2 = this.CalcSize();
      if (!this.m_GameView.showLowResolutionToggle)
        return vector2;
      vector2.y += this.frameHeight + 2f;
      return vector2;
    }

    public override void OnGUI(Rect rect)
    {
      if (!this.m_GameView.showLowResolutionToggle)
      {
        base.OnGUI(rect);
      }
      else
      {
        GUI.Label(new Rect(rect.x, rect.y, rect.width, this.frameHeight), "", EditorStyles.inspectorBig);
        GUI.enabled = !this.m_GameView.forceLowResolutionAspectRatios;
        this.m_GameView.lowResolutionForAspectRatios = GUI.Toggle(new Rect(9f, 7f, rect.width, 16f), this.m_GameView.forceLowResolutionAspectRatios || this.m_GameView.lowResolutionForAspectRatios, GameView.Styles.lowResAspectRatiosContextMenuContent);
        GUI.enabled = true;
        rect.height -= this.contentOffset;
        rect.y += this.contentOffset;
        base.OnGUI(rect);
      }
    }
  }
}
