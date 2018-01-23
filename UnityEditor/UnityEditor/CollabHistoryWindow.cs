// Decompiled with JetBrains decompiler
// Type: UnityEditor.CollabHistoryWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Collaboration;
using UnityEditor.Web;
using UnityEngine;

namespace UnityEditor
{
  internal class CollabHistoryWindow : WebViewEditorWindowTabs, IHasCustomMenu
  {
    private const string kServiceName = "Collab History";

    protected CollabHistoryWindow()
    {
      this.minSize = new Vector2(275f, 50f);
    }

    [MenuItem("Window/Collab History", false, 2011)]
    public static CollabHistoryWindow ShowHistoryWindow()
    {
      return EditorWindow.GetWindow<CollabHistoryWindow>("Collab History", new System.Type[1]{ typeof (InspectorWindow) });
    }

    [MenuItem("Window/Collab History", true)]
    public static bool ValidateShowHistoryWindow()
    {
      return Collab.instance.IsCollabEnabledForCurrentProject();
    }

    public void OnReceiveTitle(string title)
    {
      this.titleContent.text = title;
    }

    public new void OnInitScripting()
    {
      base.OnInitScripting();
    }

    public override void OnEnable()
    {
      Collab.instance.StateChanged += new StateChangedDelegate(this.OnCollabStateChanged);
      this.initialOpenUrl = "file:///" + EditorApplication.userJavascriptPackagesPath + "unityeditor-collab-history/dist/index.html";
      base.OnEnable();
    }

    public new void OnDestroy()
    {
      Collab.instance.StateChanged -= new StateChangedDelegate(this.OnCollabStateChanged);
      base.OnDestroy();
    }

    public void OnCollabStateChanged(CollabInfo info)
    {
      if (Collab.instance.IsCollabEnabledForCurrentProject())
        return;
      CollabHistoryWindow.CloseHistoryWindows();
    }

    public new void ToggleMaximize()
    {
      base.ToggleMaximize();
    }

    private static void CloseHistoryWindows()
    {
      CollabHistoryWindow[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (CollabHistoryWindow)) as CollabHistoryWindow[];
      if (objectsOfTypeAll == null)
        return;
      foreach (EditorWindow editorWindow in objectsOfTypeAll)
        editorWindow.Close();
    }
  }
}
