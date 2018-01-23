// Decompiled with JetBrains decompiler
// Type: UnityEditor.RetainedMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal class RetainedMode : AssetPostprocessor
  {
    private static HashSet<UnityEngine.Object> s_TmpDirtySet = new HashSet<UnityEngine.Object>();

    static RetainedMode()
    {
      // ISSUE: reference to a compiler-generated field
      if (RetainedMode.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RetainedMode.\u003C\u003Ef__mg\u0024cache0 = new System.Action<IMGUIContainer>(RetainedMode.OnBeginContainer);
      }
      // ISSUE: reference to a compiler-generated field
      UIElementsUtility.s_BeginContainerCallback = RetainedMode.\u003C\u003Ef__mg\u0024cache0;
      // ISSUE: reference to a compiler-generated field
      if (RetainedMode.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RetainedMode.\u003C\u003Ef__mg\u0024cache1 = new System.Action<IMGUIContainer>(RetainedMode.OnEndContainer);
      }
      // ISSUE: reference to a compiler-generated field
      UIElementsUtility.s_EndContainerCallback = RetainedMode.\u003C\u003Ef__mg\u0024cache1;
    }

    private static void OnBeginContainer(IMGUIContainer c)
    {
      HandleUtility.BeginHandles();
    }

    private static void OnEndContainer(IMGUIContainer c)
    {
      HandleUtility.EndHandles();
    }

    [RequiredByNativeCode]
    private static void UpdateSchedulers()
    {
      try
      {
        RetainedMode.UpdateSchedulersInternal(RetainedMode.s_TmpDirtySet);
      }
      finally
      {
        RetainedMode.s_TmpDirtySet.Clear();
      }
    }

    private static void UpdateSchedulersInternal(HashSet<UnityEngine.Object> tmpDirtySet)
    {
      DataWatchService.sharedInstance.PollNativeData();
      Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
      while (panelsIterator.MoveNext())
      {
        Panel panel = panelsIterator.Current.Value;
        if (panel.contextType == ContextType.Editor)
        {
          IScheduler scheduler = panel.scheduler;
          panel.timerEventScheduler.UpdateScheduledEvents();
          if (panel.visualTree.IsDirty(ChangeType.Repaint))
          {
            GUIView ownerObject = panel.ownerObject as GUIView;
            if ((UnityEngine.Object) ownerObject != (UnityEngine.Object) null)
              ownerObject.Repaint();
          }
        }
      }
    }

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
      bool flag1 = false;
      bool flag2 = false;
      foreach (string importedAsset in importedAssets)
      {
        if (importedAsset.EndsWith("uss"))
        {
          flag2 = true;
          RetainedMode.FlagStyleSheetChange();
        }
        else if (importedAsset.EndsWith("uxml"))
        {
          flag1 = true;
          UIElementsViewImporter.logger.FinishImport();
          StyleSheetCache.ClearCaches();
        }
        if (flag1 && flag2)
          break;
      }
    }

    public static void FlagStyleSheetChange()
    {
      StyleSheetCache.ClearCaches();
      Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
      while (panelsIterator.MoveNext())
      {
        Panel panel = panelsIterator.Current.Value;
        if (panel.contextType == ContextType.Editor)
        {
          panel.styleContext.DirtyStyleSheets();
          panel.visualTree.Dirty(ChangeType.Styles);
          GUIView ownerObject = panel.ownerObject as GUIView;
          if ((UnityEngine.Object) ownerObject != (UnityEngine.Object) null)
            ownerObject.Repaint();
        }
      }
    }
  }
}
