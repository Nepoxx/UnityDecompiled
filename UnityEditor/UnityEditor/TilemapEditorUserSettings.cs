// Decompiled with JetBrains decompiler
// Type: UnityEditor.TilemapEditorUserSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor
{
  [NativeType(Header = "Modules/TilemapEditor/Editor/TilemapEditorUserSettings.h")]
  internal sealed class TilemapEditorUserSettings
  {
    public static extern GameObject lastUsedPalette { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern TilemapEditorUserSettings.FocusMode focusMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public enum FocusMode
    {
      None,
      Tilemap,
      Grid,
    }
  }
}
