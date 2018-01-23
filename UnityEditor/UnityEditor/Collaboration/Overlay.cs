// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.Overlay
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Collaboration
{
  internal class Overlay
  {
    private static readonly Dictionary<Collab.CollabStates, GUIContent> s_Overlays = new Dictionary<Collab.CollabStates, GUIContent>();
    public const double k_OverlaySizeOnSmallIcon = 0.6;
    public const double k_OverlaySizeOnLargeIcon = 0.35;

    protected static void LoadOverlays()
    {
      Overlay.s_Overlays.Clear();
      Overlay.s_Overlays.Add(Collab.CollabStates.kCollabIgnored, EditorGUIUtility.IconContent("CollabExclude Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.kCollabConflicted, EditorGUIUtility.IconContent("CollabConflict Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.kCollabPendingMerge, EditorGUIUtility.IconContent("CollabConflict Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.kCollabMovedLocal, EditorGUIUtility.IconContent("CollabMoved Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.kCollabCheckedOutLocal | Collab.CollabStates.kCollabMovedLocal, EditorGUIUtility.IconContent("CollabMoved Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.kCollabCheckedOutLocal, EditorGUIUtility.IconContent("CollabEdit Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.kCollabAddedLocal, EditorGUIUtility.IconContent("CollabCreate Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.kCollabDeletedLocal, EditorGUIUtility.IconContent("CollabDeleted Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.KCollabContentConflicted, EditorGUIUtility.IconContent("CollabChangesConflict Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.KCollabContentChanged, EditorGUIUtility.IconContent("CollabChanges Icon"));
      Overlay.s_Overlays.Add(Collab.CollabStates.KCollabContentDeleted, EditorGUIUtility.IconContent("CollabChangesDeleted Icon"));
    }

    protected static bool AreOverlaysLoaded()
    {
      if (Overlay.s_Overlays.Count == 0)
        return false;
      foreach (GUIContent guiContent in Overlay.s_Overlays.Values)
      {
        if (guiContent == null)
          return false;
      }
      return true;
    }

    protected static Collab.CollabStates GetOverlayStateForAsset(Collab.CollabStates assetStates)
    {
      foreach (Collab.CollabStates key in Overlay.s_Overlays.Keys)
      {
        if (Overlay.HasState(assetStates, key))
          return key;
      }
      return Collab.CollabStates.kCollabNone;
    }

    protected static void DrawOverlayElement(Collab.CollabStates singleState, Rect itemRect)
    {
      GUIContent guiContent;
      if (!Overlay.s_Overlays.TryGetValue(singleState, out guiContent))
        return;
      Texture image = guiContent.image;
      if ((UnityEngine.Object) image != (UnityEngine.Object) null)
        GUI.DrawTexture(itemRect, image, ScaleMode.ScaleToFit);
    }

    protected static bool HasState(Collab.CollabStates assetStates, Collab.CollabStates includesState)
    {
      return (assetStates & includesState) == includesState;
    }

    public static void DrawOverlays(Collab.CollabStates assetState, Rect itemRect, bool isListMode)
    {
      if (assetState == Collab.CollabStates.kCollabInvalidState || assetState == Collab.CollabStates.kCollabNone || Event.current.type != EventType.Repaint)
        return;
      if (!Overlay.AreOverlaysLoaded())
        Overlay.LoadOverlays();
      Overlay.DrawOverlayElement(Overlay.GetOverlayStateForAsset(assetState), Overlay.GetRectForTopRight(itemRect, Overlay.GetScale(itemRect, isListMode)));
    }

    public static Rect ScaleRect(Rect rect, double scale)
    {
      return new Rect(rect) { width = (float) Convert.ToInt32(Math.Ceiling((double) rect.width * scale)), height = (float) Convert.ToInt32(Math.Ceiling((double) rect.height * scale)) };
    }

    public static double GetScale(Rect rect, bool isListMode)
    {
      double num = 0.35;
      if (isListMode)
        num = 0.6;
      return num;
    }

    public static Rect GetRectForTopRight(Rect projectBrowserDrawRect, double scale)
    {
      Rect rect = Overlay.ScaleRect(projectBrowserDrawRect, scale);
      rect.x += projectBrowserDrawRect.width - rect.width;
      return rect;
    }

    public static Rect GetRectForBottomRight(Rect projectBrowserDrawRect, double scale)
    {
      Rect rect = Overlay.ScaleRect(projectBrowserDrawRect, scale);
      rect.x += projectBrowserDrawRect.width - rect.width;
      rect.y += projectBrowserDrawRect.height - rect.height;
      return rect;
    }
  }
}
