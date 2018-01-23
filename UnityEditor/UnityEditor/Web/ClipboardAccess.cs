// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.ClipboardAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class ClipboardAccess
  {
    private ClipboardAccess()
    {
    }

    static ClipboardAccess()
    {
      JSProxyMgr.GetInstance().AddGlobalObject("unity/ClipboardAccess", (object) new ClipboardAccess());
    }

    public void CopyToClipboard(string value)
    {
      TextEditor textEditor = new TextEditor();
      textEditor.text = value;
      textEditor.SelectAll();
      textEditor.Copy();
    }

    public string PasteFromClipboard()
    {
      TextEditor textEditor = new TextEditor();
      textEditor.Paste();
      return textEditor.text;
    }
  }
}
