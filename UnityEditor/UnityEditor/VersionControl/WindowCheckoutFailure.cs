// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.WindowCheckoutFailure
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  internal class WindowCheckoutFailure : EditorWindow
  {
    private AssetList assetList = new AssetList();
    private ListControl checkoutSuccessList = new ListControl();
    private ListControl checkoutFailureList = new ListControl();

    public void OnEnable()
    {
      this.position = new Rect(100f, 100f, 700f, 230f);
      this.minSize = new Vector2(700f, 230f);
      this.checkoutSuccessList.ReadOnly = true;
      this.checkoutFailureList.ReadOnly = true;
    }

    public static void OpenIfCheckoutFailed(AssetList assets)
    {
      Object[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (WindowCheckoutFailure));
      bool alreadyOpen = (objectsOfTypeAll.Length <= 0 ? (Object) null : (Object) (objectsOfTypeAll[0] as WindowCheckoutFailure)) != (Object) null;
      bool flag = alreadyOpen;
      if (!flag)
      {
        foreach (Asset asset in (List<Asset>) assets)
        {
          if (!asset.IsState(Asset.States.CheckedOutLocal))
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return;
      WindowCheckoutFailure.GetWindow().DoOpen(assets, alreadyOpen);
    }

    private static WindowCheckoutFailure GetWindow()
    {
      return EditorWindow.GetWindow<WindowCheckoutFailure>(true, "Version Control Check Out Failed");
    }

    private void DoOpen(AssetList assets, bool alreadyOpen)
    {
      if (alreadyOpen)
      {
        foreach (Asset asset in (List<Asset>) assets)
        {
          bool flag = false;
          int count = this.assetList.Count;
          for (int index = 0; index < count; ++index)
          {
            if (this.assetList[index].path == asset.path)
            {
              flag = true;
              this.assetList[index] = asset;
              break;
            }
          }
          if (!flag)
            this.assetList.Add(asset);
        }
      }
      else
        this.assetList.AddRange((IEnumerable<Asset>) assets);
      this.RefreshList();
    }

    private void RefreshList()
    {
      this.checkoutSuccessList.Clear();
      this.checkoutFailureList.Clear();
      foreach (Asset asset in (List<Asset>) this.assetList)
      {
        if (asset.IsState(Asset.States.CheckedOutLocal))
          this.checkoutSuccessList.Add((ListItem) null, asset.prettyPath, asset);
        else
          this.checkoutFailureList.Add((ListItem) null, asset.prettyPath, asset);
      }
      this.checkoutSuccessList.Refresh();
      this.checkoutFailureList.Refresh();
      this.Repaint();
    }

    public void OnGUI()
    {
      float height = (float) (((double) this.position.height - 122.0) / 2.0);
      GUILayout.Label("Some files could not be checked out:", EditorStyles.boldLabel, new GUILayoutOption[0]);
      Rect screenRect1 = new Rect(6f, 40f, this.position.width - 12f, height);
      GUILayout.BeginArea(screenRect1);
      GUILayout.Box("", new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      GUILayout.EndArea();
      this.checkoutFailureList.OnGUI(new Rect(screenRect1.x + 2f, screenRect1.y + 2f, screenRect1.width - 4f, screenRect1.height - 4f), true);
      GUILayout.Space(20f + height);
      GUILayout.Label("The following files were successfully checked out:", EditorStyles.boldLabel, new GUILayoutOption[0]);
      Rect screenRect2 = new Rect(6f, (float) (40.0 + (double) height + 40.0), this.position.width - 12f, height);
      GUILayout.BeginArea(screenRect2);
      GUILayout.Box("", new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      GUILayout.EndArea();
      this.checkoutSuccessList.OnGUI(new Rect(screenRect2.x + 2f, screenRect2.y + 2f, screenRect2.width - 4f, screenRect2.height - 4f), true);
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      EditorUserSettings.showFailedCheckout = !GUILayout.Toggle(!EditorUserSettings.showFailedCheckout, "Don't show this window again.");
      GUILayout.FlexibleSpace();
      bool enabled = GUI.enabled;
      GUI.enabled = this.checkoutFailureList.Size > 0;
      if (GUILayout.Button("Retry Check Out"))
        Provider.Checkout(this.assetList, CheckoutMode.Exact);
      GUI.enabled = this.checkoutSuccessList.Size > 0;
      if (GUILayout.Button("Revert Unchanged"))
      {
        Provider.Revert(this.assetList, RevertMode.Unchanged).SetCompletionAction(CompletionAction.UpdatePendingWindow);
        Provider.Status(this.assetList);
        this.Close();
      }
      GUI.enabled = enabled;
      if (GUILayout.Button("OK"))
        this.Close();
      GUILayout.EndHorizontal();
      GUILayout.Space(12f);
    }
  }
}
