// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedGUIState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal struct SavedGUIState
  {
    internal GUILayoutUtility.LayoutCache layoutCache;
    internal IntPtr guiState;
    internal Vector2 screenManagerSize;
    internal Rect renderManagerRect;
    internal GUISkin skin;
    internal int instanceID;

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetupSavedGUIState(out IntPtr state, out Vector2 screenManagerSize);

    private static void Internal_ApplySavedGUIState(IntPtr state, Vector2 screenManagerSize)
    {
      SavedGUIState.INTERNAL_CALL_Internal_ApplySavedGUIState(state, ref screenManagerSize);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_ApplySavedGUIState(IntPtr state, ref Vector2 screenManagerSize);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int Internal_GetGUIDepth();

    internal static SavedGUIState Create()
    {
      SavedGUIState savedGuiState = new SavedGUIState();
      if (SavedGUIState.Internal_GetGUIDepth() > 0)
      {
        savedGuiState.skin = GUI.skin;
        savedGuiState.layoutCache = new GUILayoutUtility.LayoutCache(GUILayoutUtility.current);
        savedGuiState.instanceID = GUIUtility.s_OriginalID;
        SavedGUIState.Internal_SetupSavedGUIState(out savedGuiState.guiState, out savedGuiState.screenManagerSize);
      }
      return savedGuiState;
    }

    internal void ApplyAndForget()
    {
      if (this.layoutCache == null)
        return;
      GUILayoutUtility.current = this.layoutCache;
      GUI.skin = this.skin;
      GUIUtility.s_OriginalID = this.instanceID;
      SavedGUIState.Internal_ApplySavedGUIState(this.guiState, this.screenManagerSize);
      GUIClip.Reapply();
    }
  }
}
