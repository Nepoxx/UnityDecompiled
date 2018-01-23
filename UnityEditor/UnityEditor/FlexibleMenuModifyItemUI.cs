// Decompiled with JetBrains decompiler
// Type: UnityEditor.FlexibleMenuModifyItemUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class FlexibleMenuModifyItemUI : PopupWindowContent
  {
    protected FlexibleMenuModifyItemUI.MenuType m_MenuType;
    public object m_Object;
    protected Action<object> m_AcceptedCallback;
    private bool m_IsInitialized;

    public override void OnClose()
    {
      this.m_Object = (object) null;
      this.m_AcceptedCallback = (Action<object>) null;
      this.m_IsInitialized = false;
      EditorApplication.RequestRepaintAllViews();
    }

    public void Init(FlexibleMenuModifyItemUI.MenuType menuType, object obj, Action<object> acceptedCallback)
    {
      this.m_MenuType = menuType;
      this.m_Object = obj;
      this.m_AcceptedCallback = acceptedCallback;
      this.m_IsInitialized = true;
    }

    public void Accepted()
    {
      if (this.m_AcceptedCallback != null)
        this.m_AcceptedCallback(this.m_Object);
      else
        Debug.LogError((object) "Missing callback. Did you remember to call Init ?");
    }

    public bool IsShowing()
    {
      return this.m_IsInitialized;
    }

    public enum MenuType
    {
      Add,
      Edit,
    }
  }
}
