// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUIViewDebuggerHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal class GUIViewDebuggerHelper
  {
    internal static Action onViewInstructionsChanged;

    internal static void GetViews(List<GUIView> views)
    {
      GUIViewDebuggerHelper.GetViewsInternal((object) views);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetViewsInternal(object views);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DebugWindow(GUIView view);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void StopDebugging();

    private static GUIContent CreateGUIContent(string text, Texture image, string tooltip)
    {
      return new GUIContent(text, image, tooltip);
    }

    internal static void GetDrawInstructions(List<IMGUIDrawInstruction> drawInstructions)
    {
      GUIViewDebuggerHelper.GetDrawInstructionsInternal((object) drawInstructions);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetDrawInstructionsInternal(object drawInstructions);

    internal static void GetClipInstructions(List<IMGUIClipInstruction> clipInstructions)
    {
      GUIViewDebuggerHelper.GetClipInstructionsInternal((object) clipInstructions);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetClipInstructionsInternal(object clipInstructions);

    internal static void GetNamedControlInstructions(List<IMGUINamedControlInstruction> namedControlInstructions)
    {
      GUIViewDebuggerHelper.GetNamedControlInstructionsInternal((object) namedControlInstructions);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetNamedControlInstructionsInternal(object namedControlInstructions);

    internal static void GetPropertyInstructions(List<IMGUIPropertyInstruction> namedControlInstructions)
    {
      GUIViewDebuggerHelper.GetPropertyInstructionsInternal((object) namedControlInstructions);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetPropertyInstructionsInternal(object propertyInstructions);

    internal static void GetLayoutInstructions(List<IMGUILayoutInstruction> layoutInstructions)
    {
      GUIViewDebuggerHelper.GetLayoutInstructionsInternal((object) layoutInstructions);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetLayoutInstructionsInternal(object layoutInstructions);

    internal static void GetUnifiedInstructions(List<IMGUIInstruction> layoutInstructions)
    {
      GUIViewDebuggerHelper.GetUnifiedInstructionsInternal((object) layoutInstructions);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetUnifiedInstructionsInternal(object instructions);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ClearInstructions();

    [RequiredByNativeCode]
    private static void CallOnViewInstructionsChanged()
    {
      if (GUIViewDebuggerHelper.onViewInstructionsChanged == null)
        return;
      GUIViewDebuggerHelper.onViewInstructionsChanged();
    }
  }
}
