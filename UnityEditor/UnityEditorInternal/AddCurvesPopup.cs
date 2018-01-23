// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AddCurvesPopup : EditorWindow
  {
    private static Vector2 windowSize = new Vector2(240f, 250f);
    private const float k_WindowPadding = 3f;
    internal static AnimationWindowState s_State;
    private static AddCurvesPopup s_AddCurvesPopup;
    private static long s_LastClosedTime;
    private static AddCurvesPopupHierarchy s_Hierarchy;
    private static AddCurvesPopup.OnNewCurveAdded NewCurveAddedCallback;

    internal static AnimationWindowSelection selection { get; set; }

    private void Init(Rect buttonRect)
    {
      buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
      this.ShowAsDropDown(buttonRect, AddCurvesPopup.windowSize, new PopupLocationHelper.PopupLocation[1]
      {
        PopupLocationHelper.PopupLocation.Right
      });
    }

    private void OnEnable()
    {
      AssemblyReloadEvents.beforeAssemblyReload += new AssemblyReloadEvents.AssemblyReloadCallback(((EditorWindow) this).Close);
    }

    private void OnDisable()
    {
      AssemblyReloadEvents.beforeAssemblyReload -= new AssemblyReloadEvents.AssemblyReloadCallback(((EditorWindow) this).Close);
      AddCurvesPopup.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
      AddCurvesPopup.s_AddCurvesPopup = (AddCurvesPopup) null;
      AddCurvesPopup.s_Hierarchy = (AddCurvesPopupHierarchy) null;
    }

    internal static void AddNewCurve(AddCurvesPopupPropertyNode node)
    {
      AnimationWindowUtility.CreateDefaultCurves(AddCurvesPopup.s_State, node.selectionItem, node.curveBindings);
      if (AddCurvesPopup.NewCurveAddedCallback == null)
        return;
      AddCurvesPopup.NewCurveAddedCallback(node);
    }

    internal static bool ShowAtPosition(Rect buttonRect, AnimationWindowState state, AddCurvesPopup.OnNewCurveAdded newCurveCallback)
    {
      if (DateTime.Now.Ticks / 10000L < AddCurvesPopup.s_LastClosedTime + 50L)
        return false;
      Event.current.Use();
      if ((UnityEngine.Object) AddCurvesPopup.s_AddCurvesPopup == (UnityEngine.Object) null)
        AddCurvesPopup.s_AddCurvesPopup = ScriptableObject.CreateInstance<AddCurvesPopup>();
      AddCurvesPopup.NewCurveAddedCallback = newCurveCallback;
      AddCurvesPopup.s_State = state;
      AddCurvesPopup.s_AddCurvesPopup.Init(buttonRect);
      return true;
    }

    internal void OnGUI()
    {
      if (Event.current.type == EventType.Layout)
        return;
      if (AddCurvesPopup.s_Hierarchy == null)
        AddCurvesPopup.s_Hierarchy = new AddCurvesPopupHierarchy();
      Rect position = new Rect(1f, 1f, AddCurvesPopup.windowSize.x - 3f, AddCurvesPopup.windowSize.y - 3f);
      GUI.Box(new Rect(0.0f, 0.0f, AddCurvesPopup.windowSize.x, AddCurvesPopup.windowSize.y), GUIContent.none, new GUIStyle((GUIStyle) "grey_border"));
      AddCurvesPopup.s_Hierarchy.OnGUI(position, (EditorWindow) this);
    }

    public delegate void OnNewCurveAdded(AddCurvesPopupPropertyNode node);
  }
}
